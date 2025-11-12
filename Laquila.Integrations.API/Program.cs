using System.Diagnostics;
using System.Text;
using System.Threading.RateLimiting;
using DotNetEnv;
using Laquila.Integrations.API.Configurations;
using Laquila.Integrations.API.Middlewares;
using Laquila.Integrations.Core.Infra.Interfaces;
using Laquila.Integrations.Core.Infra.Repositories;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
var issuer = Environment.GetEnvironmentVariable("ISSUER");
var audience = Environment.GetEnvironmentVariable("AUDIENCE");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

// Services
builder.Services.AddDependencyInjection();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(1)
            }));

    options.RejectionStatusCode = 429;
});


// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    context.Token = authHeader.Substring("Bearer ".Length).Trim();
                }
                else if (context.Request.Cookies.ContainsKey("jwt"))
                {
                    context.Token = context.Request.Cookies["jwt"];
                }

                Console.WriteLine("Token recebido: " + context.Token);

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Falha na validação: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });


SwaggerConfig.AddSwagger(builder.Services);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DynamicCors", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => origin.StartsWith("https://localhost"))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// DB Context
builder.Services.AddDbContext<LaquilaHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LaquilaHubConnection"))
#if DEBUG
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
#endif
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddDbContext<Everest30Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Everest30Connection"))
#if DEBUG
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
#endif
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(builder.Configuration.GetConnectionString("Everest30Connection") ?? ""));

// HTTPS redirect
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001;
});


// App
var app = builder.Build();

if (app.Environment.IsDevelopment())
{

    app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/swagger"), subApp =>
    {
        subApp.Use(async (context, next) =>
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await next();
        });
    });

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.IndexStream = () => File.OpenRead("wwwroot/swagger/index.html"); // custom UI
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laquila API v1");
        c.RoutePrefix = string.Empty;
    });

    try
    {
        var url = "https://localhost:5001";
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao abrir o navegador: {ex.Message}");
    }
}

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseCors("DynamicCors");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<Middleware>();

app.MapControllers();

app.Run();
