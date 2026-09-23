using AnwendungWPF.Configuration;
using AnwendungWPF.Models;
using APIContracts.Addresses;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;

namespace AnwendungWPF.Clients
{
    public sealed class AddressApiClient : IAddressApiClient
    {
        private readonly HttpClient _httpClient;

        public AddressApiClient(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress =
                new Uri(apiSettings.Value.BaseUrl);
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
            var responses =
                await _httpClient.GetFromJsonAsync<List<AddressResponse>>(
                    "api/addresses")
                ?? [];

            return responses
                .Select(Map)
                .ToList();
        }

        public async Task<AddressModel> CreateAsync(
            Guid placeId,
            string addressLine1,
            string? addressLine2,
            string? addressLine3,
            string? name1,
            string? name2)
        {
            var request = new CreateAddressRequest
            {
                PlaceId = placeId,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                AddressLine3 = addressLine3,
                Name1 = name1,
                Name2 = name2
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/addresses",
                request);

            response.EnsureSuccessStatusCode();

            var address =
                await response.Content.ReadFromJsonAsync<AddressResponse>()
                ?? throw new InvalidOperationException(
                    "The API returned an empty response when creating an address.");

            return Map(address);
        }

        public async Task UpdateAsync(
            Guid addressId,
            Guid placeId,
            string addressLine1,
            string? addressLine2,
            string? addressLine3,
            string? name1,
            string? name2)
        {
            var request = new UpdateAddressRequest
            {
                PlaceId = placeId,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                AddressLine3 = addressLine3,
                Name1 = name1,
                Name2 = name2
            };

            var response = await _httpClient.PutAsJsonAsync(
                $"api/addresses/{addressId}",
                request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid addressId)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/addresses/{addressId}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<AddressModel> CreateWithPlaceAsync(
            string countryName,
            string postcode,
            string cityName,
            string addressLine1,
            string? addressLine2,
            string? addressLine3,
            string? name1,
            string? name2)
        {
            var request = new CreateAddressWithPlaceRequest
            {
                CountryName = countryName,
                Postcode = postcode,
                CityName = cityName,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                AddressLine3 = addressLine3,
                Name1 = name1,
                Name2 = name2
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/addresses/with-place",
                request);

            response.EnsureSuccessStatusCode();

            var address =
                await response.Content.ReadFromJsonAsync<AddressResponse>()
                ?? throw new InvalidOperationException(
                    "The API returned an empty response when creating an address and place.");

            return Map(address);
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
                Name2 = response.Name2,
                ModifiedUtc = response.ModifiedUtc,
                //City = response.City,
                //Postcode = response.Postcode,
                //Country = response.Country
            };
        }
    }
}