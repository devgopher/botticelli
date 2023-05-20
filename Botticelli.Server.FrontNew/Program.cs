using Botticelli.Server.FrontNew;
using Botticelli.Server.FrontNew.Clients;
using Botticelli.Server.FrontNew.Handler;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient<SessionClient>();
       //.AddHttpMessageHandler<AuthDelegatingHandler>();
builder.Services.AddScoped<SessionClient>();
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});


var app = builder.Build();
await app.RunAsync();