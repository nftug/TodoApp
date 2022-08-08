using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Services.Authentication;
using Blazored.LocalStorage;
using Client.Services.Api;
using MatBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5011/api/") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMatBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, SpaAuthenticateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<TodoApiService>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
