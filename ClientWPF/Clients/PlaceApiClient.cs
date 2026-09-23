using AnwendungWPF.Configuration;
using ApiContracts.Places;
using AnwendungWPF.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;

namespace AnwendungWPF.Clients
{
    public sealed class PlaceApiClient : IPlaceApiClient
    {
        private readonly HttpClient _httpClient;

        public PlaceApiClient(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress =
                new Uri(apiSettings.Value.BaseUrl);
        }

        public async Task<IReadOnlyList<PlaceModel>> GetAllAsync()
        {
            var responses =
                await _httpClient.GetFromJsonAsync<List<PlaceResponse>>(
                    "api/places")
                ?? [];

            return responses
                .Select(Map)
                .ToList();
        }

        public async Task<PlaceModel?> GetByIdAsync(Guid placeId)
        {
            var response =
                await _httpClient.GetFromJsonAsync<PlaceResponse>(
                    $"api/places/{placeId}");

            return response is null
                ? null
                : Map(response);
        }

        public async Task<PlaceModel> CreateAsync(
            Guid countryId,
            Guid postcodeId,
            Guid cityId,
            string? nameEnGb)
        {
            var request = new CreatePlaceRequest
            {
                CountryId = countryId,
                PostcodeId = postcodeId,
                CityId = cityId,
                NameEnGb = nameEnGb
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/places",
                request);

            response.EnsureSuccessStatusCode();

            var place =
                await response.Content.ReadFromJsonAsync<PlaceResponse>()
                ?? throw new InvalidOperationException(
                    "The API returned an empty response when creating a place.");

            return Map(place);
        }

        public async Task UpdateAsync(
            Guid placeId,
            Guid countryId,
            Guid postcodeId,
            Guid cityId,
            string? nameEnGb)
        {
            var request = new UpdatePlaceRequest
            {
                CountryId = countryId,
                PostcodeId = postcodeId,
                CityId = cityId,
                NameEnGb = nameEnGb
            };

            var response = await _httpClient.PutAsJsonAsync(
                $"api/places/{placeId}",
                request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid placeId)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/places/{placeId}");

            response.EnsureSuccessStatusCode();
        }

        private static PlaceModel Map(PlaceResponse response)
        {
            return new PlaceModel
            {
                PlaceId = response.PlaceId,
                CountryId = response.CountryId,
                PostcodeId = response.PostcodeId,
                CityId = response.CityId,
                NameEnGb = response.NameEnGb,
                City = response.City,
                Postcode = response.Postcode,
                Country = response.Country
            };
        }
    }
}