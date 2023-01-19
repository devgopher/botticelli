using Botticelli.Server.Data;
using Botticelli.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddScoped<IBotManagementService, BotManagementService>()
       .AddScoped<IBotStatusDataService, BotStatusDataService>()
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