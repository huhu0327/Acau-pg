using Acau_Playground.Services;
using Acau_Playground.Viewmodels;

namespace Acau_Playground.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFoodService, FoodService>();

            services.AddScoped<IClipboardService, ClipboardService>();
            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddScoped<LocalStorageViewModel>();

            services.AddScoped<FoodViewModel>();
            services.AddScoped<CalculatorViewModel>();

            return services;
        }

        public static Task InitServices(this IServiceProvider serviceProvider)
        {
            var tasks = new Task[]{
                serviceProvider.GetRequiredService<LocalStorageViewModel>().OnInitializedAsync(),

                serviceProvider.GetRequiredService<FoodViewModel>().OnInitializedAsync(),
                serviceProvider.GetRequiredService<CalculatorViewModel>().OnInitializedAsync(),
            };

            return Task.WhenAll(tasks);
        }

    }
}
