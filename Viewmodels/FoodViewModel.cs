using Acau_Playground.Models;
using Acau_Playground.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Acau_Playground.Viewmodels
{
    public class FoodViewModel : ObservableObject
    {
        private readonly IFoodService _foodService;
        private IReadOnlyDictionary<string, IEnumerable<Food>> _foodList;

        public FoodViewModel(IFoodService foodService)
        {
            _foodService = foodService;
        }
        public async Task OnInitializedAsync()
        {
            _foodList = await _foodService.GetFoodAsync();
        }

        public Food? GetFood(string foodName)
        {
            var list = _foodList.Values.SelectMany(food => food);

            var food = list.FirstOrDefault(food => food.Name.Contains(foodName));

            return food;
        }

        public IEnumerable<Food>? GetFoodList(string job)
        {
            _foodList.TryGetValue(job, out var result);

            return result;
        }

        public IEnumerable<Food>? GetFoodListAtIndex(int index)
        {
            return _foodList.ElementAt(index).Value;
        }

        public IEnumerable<string>? GetJobList() => _foodList.Keys;
    }
}
