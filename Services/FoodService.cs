using Acau_Playground.Models;
using System.Net.Http.Json;

namespace Acau_Playground.Services
{
    public interface IFoodService
    {
        public Task<IReadOnlyList<Food>?> GetFoodAsync();
    }

    public class FoodService : IFoodService
    {
        private readonly HttpClient _httpClient;

        public FoodService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IReadOnlyList<Food>?> GetFoodAsync()
        {
            return _httpClient.GetFromJsonAsync<IReadOnlyList<Food>?>("sample-data/datas.json");
        }

    }
}
