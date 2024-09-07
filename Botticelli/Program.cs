using System.Text;
using Botticelli.SecureStorage;
using Botticelli.SecureStorage.Settings;
using Botticelli.Server.Data;
using Botticelli.Server.Extensions;
using Botticelli.Server.Services;
using Botticelli.Server.Services.Auth;
using Botticelli.Server.Settings;
using Botticelli.Server.Utils;
using FluentEmail.Core.Interfaces;
using FluentEmail.MailKitSmtp;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddDefaultPolicy(cors =>
{
    cors.SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Content-Disposition");
}));

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
    .AddEnvironmentVariables();

var secureStorageSettings = builder.Configuration
    .GetSection(nameof(SecureStorageSettings))
    .Get<SecureStorageSettings>();

var serverSettings = builder.Configuration
    .GetSection(nameof(ServerSettings))
    .Get<ServerSettings>();

builder.Services.AddSingleton(serverSettings);

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Example: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

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

builder.Services.Configure<SmtpClientOptions>(
    builder.Configuration.GetSection($"{nameof(ServerSettings)}:{nameof(SmtpClientOptions)}"));

builder.Services
    .Configure<ServerSettings>(nameof(ServerSettings), builder.Configuration.GetSection(nameof(ServerSettings)))
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authorization:Issuer"],
        ValidAudience = builder.Configuration["Authorization:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authorization:Key"] ?? string.Empty))
    });

builder.Services
    .AddLogging(cfg => cfg.AddNLog())
    .AddScoped<IBotManagementService, BotManagementService>()
    .AddScoped<IBotStatusDataService, BotStatusDataService>()
    .AddSingleton(new SecureStorage(secureStorageSettings))
    .AddScoped<IAdminAuthService, AdminAuthService>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IConfirmationService, ConfirmationService>()
    .AddScoped<IPasswordSender, PasswordSender>()
    .AddSingleton<IMapper, Mapper>()
    .AddScoped<ISender, SslMailKitSender>()
    .AddDbContext<BotInfoContext>(c => c.UseSqlite($"Data source={serverSettings.BotInfoDb}"))
    .AddDefaultIdentity<IdentityUser<string>>(options => options
        .SignIn
        .RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BotInfoContext>();

if (serverSettings.UseSsl)
    builder.WebHost.AddSsl(builder.Configuration);

#if !DEBUG
builder.Services.AddIdentity();
#endif

builder.Services.AddControllers();


builder.ApplyMigrations<BotInfoContext>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (OperatingSystem.IsWindows()) app.UseHttpsRedirection();

app.UseHsts();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();