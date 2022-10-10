using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Services.Authentication;
using Blazored.LocalStorage;
using Client.Services.Api;
using MudBlazor.Services;
using Client;
using MudBlazor;
using Client.Services.UserPreferences;
using Client.Services;
using Domain.Todos.Queries;
using Domain.Todos.DTOs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;

if (builder.HostEnvironment.Environment == "Development")
{
    builder.Services.AddScoped(sp =>
        new HttpClient { BaseAddress = new Uri("http://localhost:5011/api/") }
    );
}

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, SpaAuthenticateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<AuthStoreService>();
builder.Services.AddScoped<UserPreferencesService>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddSingleton<PageInfoService>();

builder.Services.AddScoped<IApiService<TodoResultDTO, TodoCommand, TodoQueryParameter>, TodoApiService>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 200;
    config.SnackbarConfiguration.ShowTransitionDuration = 200;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
