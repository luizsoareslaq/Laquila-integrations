using DotNetEnv;
using Laquila.Integrations.Worker;
using Laquila.Integrations.Worker.Configurations;
using Laquila.Integrations.Worker.Context;

var builder = Host.CreateApplicationBuilder(args);

Env.Load();

builder.Services.AddHostedService<Worker>();

//Injeção de dependencias
builder.Services.AddClients();
builder.Services.AddQuerys();
builder.Services.AddServices();

builder.Services.AddSingleton<AuthContext>();

var host = builder.Build();
host.Run();


