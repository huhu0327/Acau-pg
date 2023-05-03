using Acau_Playground.Models;
using Acau_Playground.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Acau_Playground.Viewmodels
{
    public class FoodViewModel : ObservableObject
    {
        private readonly IFoodService _foodService;

        public IReadOnlyList<Food>? Foods;

        public FoodViewModel(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public async Task LoadDataAsync()
        {
            Foods = await _foodService.GetFoodAsync();
        }

        public IEnumerable<Content> GetContents(int index)
        {
            if (Foods is null) return Enumerable.Empty<Content>();

            return Foods[index].Contents;
        }

    }
}
