using System.Net.Http;

namespace ClientWPF.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7171/")
            };
        }

        public async Task<string> GetAddressesAsync()
        {
            var response = await _httpClient.GetAsync("api/addresses");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
