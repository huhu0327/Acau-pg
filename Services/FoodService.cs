using Acau_Playground.Models;
using Newtonsoft.Json;

namespace Acau_Playground.Services
{
    public interface IFoodService
    {
        public Task<IReadOnlyDictionary<string, IEnumerable<Food>>> GetFoodAsync();
    }

    public class FoodService : IFoodService
    {
        private readonly HttpClient _httpClient;

        public FoodService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyDictionary<string, IEnumerable<Food>>> GetFoodAsync()
        {
            var json = await _httpClient.GetStringAsync("sample-data/datas.json");

            var result = JsonConvert.DeserializeObject<IEnumerable<Job>>(json)?.ToDictionary(k => k.Name, v => v.Foods);

            return result;
        }
    }
}
