using Botticelli.Server.FrontNew;
using Botticelli.Server.FrontNew.Clients;
using Botticelli.Server.FrontNew.Handler;
using Botticelli.Server.FrontNew.Pages;
using Botticelli.Server.FrontNew.Settings;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<SessionClient>();
builder.Services.Configure<BackSettings>(nameof(BackSettings), builder.Configuration.GetSection(nameof(BackSettings)));
builder.Services.AddScoped<AuthDelegatingHandler>();
builder.Services.AddScoped<CookieStorageAccessor>();
builder.Services.AddHttpClient<YourBots>(c =>
    {
        c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        c.DefaultRequestHeaders.Clear();
    })
    .AddHttpMessageHandler<AuthDelegatingHandler>();

var app = builder.Build();
await app.RunAsync();