using System.Text;
using BotDataSecureStorage;
using BotDataSecureStorage.Settings;
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

builder.Configuration
    .AddJsonFile("appsettings.json")
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
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authorization:Key"]))
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
    .AddDbContext<BotInfoContext>(c => c.UseSqlite(@"Data source=botInfo.Db"))
    .AddDefaultIdentity<IdentityUser<string>>(options => options
        .SignIn
        .RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BotInfoContext>();

builder.Services.AddRazorPages();

#if !DEBUG
builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        options.User.RequireUniqueEmail = true;
    });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        // Cookie settings
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    });
#endif

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.ApplyMigrations<BotInfoContext>();

var app = builder.Build();

app.UseCors("AllowCorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseCors(builder => builder.AllowAnyMethod()
    .AllowAnyOrigin()
    .AllowAnyHeader());
app.Run();