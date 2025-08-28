using System.Diagnostics;
using Laquila.Integrations.API.Configurations;
using Laquila.Integrations.API.Middlewares;
using Laquila.Integrations.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDependencyInjection();

builder.Services.AddDbContext<LaquilaHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LaquilaHubConnection")));

//Swagger
builder.Services.AddSwaggerGen();

//App
var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laquila API v1");
        c.RoutePrefix = string.Empty; // abre direto na raiz (http://localhost:5000)
    });

    try
    {
        var url = "http://localhost:5259";
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

app.UseHttpsRedirection();
app.UseMiddleware<Middleware>();
app.UseAuthorization();

app.Run();
