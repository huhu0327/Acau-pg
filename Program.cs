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

builder.Services.AddServices();
builder.Services.AddViewModels();

var host = builder.Build();

var tasks = new Task[]
{
    host.Services.GetRequiredService<FoodViewModel>().OnInitializedAsync(),
    host.Services.GetRequiredService<LocalStorageViewModel>().OnInitializedAsync(),
    host.Services.GetRequiredService<CalculatorViewModel>().OnInitializedAsync(),
};
await Task.WhenAll(tasks);
await host.RunAsync();

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddScoped<IFoodService, FoodService>();

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<FoodViewModel>();
        services.AddScoped<LocalStorageViewModel>();
        services.AddScoped<CalculatorViewModel>();

        return services;
    }
}