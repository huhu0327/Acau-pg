using Acau_Playground;
using Acau_Playground.Services;
using Acau_Playground.Viewmodels;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IClipboardService, ClipboardService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<FoodViewModel>();
builder.Services.AddScoped<LocalStorageViewModel>();
builder.Services.AddScoped<CalculatorViewModel>();

var host = builder.Build();
await host.Services.GetRequiredService<FoodViewModel>().LoadDataAsync();
await host.Services.GetRequiredService<LocalStorageViewModel>().OnInitializedAsync();
await host.Services.GetRequiredService<CalculatorViewModel>().OnInitializedAsync();
await host.RunAsync();
