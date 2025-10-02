using System.Diagnostics;
using System.Text;
using System.Threading.RateLimiting;
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

var jwtSection = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSection["SecretKey"];
var issuer = jwtSection["Issuer"];
var audience = jwtSection["Audience"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

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

// Swagger com JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Laquila API", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira o token JWT no formato **Bearer {seu_token}**",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", jwtScheme);

    var requirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    };

    c.AddSecurityRequirement(requirement);
});


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
            .SetIsOriginAllowed(origin => origin.StartsWith("https://localhostssssssss"))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// DB Context
builder.Services.AddDbContext<LaquilaHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LaquilaHubConnection")));

builder.Services.AddDbContext<Everest30Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Everest30Connection")));

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
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
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
