using AnwendungWPF.Models;
using APIContracts.Addresses;
using System.Net.Http;
using System.Net.Http.Json;

namespace AnwendungWPF.Clients
{
    public sealed class AddressApiClient : IAddressApiClient
    {
        private readonly HttpClient _httpClient;

        public AddressApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AddressModel?> GetByIdAsync(Guid addressId)
        {
            var response = await _httpClient.GetFromJsonAsync<AddressResponse>(
                $"api/addresses/{addressId}");

            return response is null
                ? null
                : Map(response);
        }

        public async Task<IReadOnlyList<AddressModel>> GetAllAsync()
        {
            var responses = await _httpClient.GetFromJsonAsync<List<AddressResponse>>(
                "api/addresses")
                ?? [];

            return responses
                .Select(Map)
                .ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/addresses/{id}");

            response.EnsureSuccessStatusCode();
        }

        private static AddressModel Map(AddressResponse response)
        {
            return new AddressModel
            {
                AddressId = response.AddressId,
                PlaceId = response.PlaceId,
                AddressLine1 = response.AddressLine1,
                AddressLine2 = response.AddressLine2,
                AddressLine3 = response.AddressLine3,
                Name1 = response.Name1,
                Name2 = response.Name2
            };
        }
    }
}
