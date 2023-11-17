using Botticelli.Server.Analytics;
using Botticelli.Server.Analytics.Cache;
using Botticelli.Server.Analytics.Extensions;
using Botticelli.Server.Analytics.Settings;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

var analyticsSettings = builder.Configuration
    .GetSection(nameof(AnalyticsSettings))
    .Get<AnalyticsSettings>();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        });
    });

builder.Services
    .AddSingleton<IMapper, Mapper>()
    .AddScoped<MetricsReaderWriter>()
    .AddDbContext<MetricsContext>(c => c.UseNpgsql(analyticsSettings.ConnectionString))
    .AddMetrics()
    .AddSingleton<ICache, Cache>();

builder.Services.AddControllers();

builder.ApplyMigrations<MetricsContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseCors(builder => builder.AllowAnyMethod()
    .AllowAnyOrigin()
    .AllowAnyHeader());
app.Run();