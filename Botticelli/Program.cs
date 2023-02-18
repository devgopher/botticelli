using BotDataSecureStorage;
using BotDataSecureStorage.Settings;
using Botticelli.Server.Data;
using Botticelli.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var secureStorageSettings = new SecureStorageSettings();
builder.Configuration.Bind(nameof(SecureStorageSettings), secureStorageSettings);

builder.Services.AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddScoped<IBotManagementService, BotManagementService>()
       .AddScoped<IBotStatusDataService, BotStatusDataService>()
       .AddSingleton(new SecureStorage(secureStorageSettings))
       .AddDbContext<BotInfoContext>(c
                                             => c.UseSqlite(@"Data source=botInfo.Db"));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();