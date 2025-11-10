using DotNetEnv;
using Laquila.Integrations.Application.Services;
using Laquila.Integrations.Worker;
using Laquila.Integrations.Worker.Context;
using Laquila.Integrations.Worker.Querys;
using Laquila.Integrations.Worker.Querys.Interfaces;
using Laquila.Integrations.Worker.Services;
using Laquila.Integrations.Worker.Services.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

Env.Load();

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IEverest30Query, Everest30Query>();
builder.Services.AddScoped<IProcessService, ProcessService>();

builder.Services.AddSingleton<AuthContext>();

var host = builder.Build();
host.Run();


