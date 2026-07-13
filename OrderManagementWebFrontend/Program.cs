using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OrderManagementWebFrontend;
using OrderManagementWebFrontend.Authentication;
using OrderManagementWebFrontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<ApiClientUser>();
builder.Services.AddScoped<ApiClientProdotti>();
builder.Services.AddScoped<ApiClientDelivery>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
