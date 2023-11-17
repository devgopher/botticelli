using Botticelli.Client.Analytics.Extensions;
using MetricsRandomGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMetrics(builder.Configuration);
builder.Services.AddHostedService(sp => new MetricsSender(sp));

var app = builder.Build();

app.Run();