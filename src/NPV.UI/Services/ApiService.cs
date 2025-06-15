
using System.Net.Http.Json;

namespace NPV.UI.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
        }

        public async ValueTask<decimal> CalculateNetPresentValue(decimal discountRate, List<decimal> cashflow)
        {
            var payload = new
            {
                discountRate,
                cashflow
            };
            var response = await _http.PostAsJsonAsync("npv", payload);

            if (response.IsSuccessStatusCode)
            {
                decimal npv = await response.Content.ReadFromJsonAsync<decimal>();

                Console.WriteLine($"Calculated NPV from API: {npv}");
                return npv;
            }
            else
            {
                Console.WriteLine($"API call failed with status code: {response.StatusCode}");
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error message: {errorMessage}");

                throw new Exception($"Failed to calculate NPV: {response.StatusCode} - {errorMessage}");
            }
        }

        public async ValueTask<List<decimal>> GetAvailableDiscountRates()
        {
            return (await _http.GetFromJsonAsync<IEnumerable<decimal>>("npv/discounts")).ToList();
        }
    }
}
