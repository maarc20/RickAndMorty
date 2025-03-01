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

        public async Task<dynamic> GetAsync(string endpoint, Boolean useBaseAddress=true)
        {
            Uri fullUrl;
            if (useBaseAddress){
                fullUrl = new Uri(_httpClient.BaseAddress, endpoint.TrimStart('/'));
            }
            else{
                fullUrl = new Uri(endpoint);
            }
            var response = await _httpClient.GetAsync(fullUrl);

            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<dynamic>(jsonResponse);
        }
    }
}