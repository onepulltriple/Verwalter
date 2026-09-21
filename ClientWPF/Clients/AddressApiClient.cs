using Application.Addresses;
using System.Net.Http;
using System.Net.Http.Json;

namespace ClientWPF.Clients
{
    public class AddressApiClient
    {
        private readonly HttpClient _httpClient;

        public AddressApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AddressDto?> GetByIdAsync(Guid addressId)
        {
            return await _httpClient.GetFromJsonAsync<AddressDto>(
                $"api/addresses/{addressId}"
                );
        }
    }
}
