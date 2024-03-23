using CatTime.Frontend;
using CatTime.Frontend.Infrastructure.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Abstractions;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7143") });

// LocalStorage
builder.Services.AddSingleton<LocalStorageService>();

// ClientService
builder.Services.AddSingleton<ClientService>();

// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddMudServices();

await builder.Build().RunAsync();
