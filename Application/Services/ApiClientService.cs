using System.Text.Json;

namespace PruebaEurofirms.Application.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;

        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<dynamic> GetAsync(string endpoint)
        {
            var fullUrl = new Uri(_httpClient.BaseAddress, endpoint.TrimStart('/'));
            var response = await _httpClient.GetAsync(fullUrl);

            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<dynamic>(jsonResponse);
        }
    }
}