using Acau_Playground.Models;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace Acau_Playground.Services
{
    public interface IFoodService
    {
        public Task<ImmutableDictionary<string, IEnumerable<Food>>> GetFoodAsync();
    }

    public class FoodService : IFoodService
    {
        private readonly HttpClient _httpClient;

        public FoodService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ImmutableDictionary<string, IEnumerable<Food>>> GetFoodAsync()
        {
            var json = await _httpClient.GetStringAsync("sample-data/datas.json");

            var result = JsonConvert.DeserializeObject<IEnumerable<Job>>(json)?.ToImmutableDictionary(k => k.Name, v => v.Foods);

            return result;
        }
    }
}
