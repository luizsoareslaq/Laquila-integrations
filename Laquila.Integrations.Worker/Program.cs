using Laquila.Integrations.Worker;
using Laquila.Integrations.Worker.Querys;
using Laquila.Integrations.Worker.Querys.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IEverest30Query, Everest30Query>();

var host = builder.Build();
host.Run();


