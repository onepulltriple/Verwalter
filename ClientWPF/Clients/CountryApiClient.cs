using AnwendungWPF.Configuration;
using AnwendungWPF.Models;
using ApiContracts.Countries;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;

namespace AnwendungWPF.Clients
{
    public class CountryApiClient : ICountryApiClient
    {
        private readonly HttpClient _httpClient;

        public CountryApiClient(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress =
                new Uri(apiSettings.Value.BaseUrl);
        }

        public async Task<IReadOnlyList<CountryModel>> GetAllAsync()
        {
            var responses =
                await _httpClient.GetFromJsonAsync<List<CountryResponse>>(
                    "api/countries")
                ?? [];

            return responses
                .Select(Map)
                .ToList();
        }

        public async Task<CountryModel?> GetByIdAsync(Guid countryId)
        {
            var response =
                await _httpClient.GetFromJsonAsync<CountryResponse>(
                    $"api/countries/{countryId}");

            return response is null
                ? null
                : Map(response);
        }

        public async Task<CountryModel> CreateAsync(
            string? isoAlpha2,
            string? isoAlpha3,
            string nameEnGb,
            bool active)
        {
            var request = new CreateCountryRequest
            {
                IsoAlpha2 = isoAlpha2,
                IsoAlpha3 = isoAlpha3,
                NameEnGb = nameEnGb,
                Active = active
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/countries",
                request);

            response.EnsureSuccessStatusCode();

            var country =
                await response.Content.ReadFromJsonAsync<CountryResponse>()
                ?? throw new InvalidOperationException(
                    "The API returned an empty response when creating a country.");

            return Map(country);
        }

        public async Task UpdateAsync(
            Guid countryId,
            string? isoAlpha2,
            string? isoAlpha3,
            string nameEnGb,
            bool active)
        {
            var request = new UpdateCountryRequest
            {
                IsoAlpha2 = isoAlpha2,
                IsoAlpha3 = isoAlpha3,
                NameEnGb = nameEnGb,
                Active = active
            };

            var response = await _httpClient.PutAsJsonAsync(
                $"api/countries/{countryId}",
                request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid countryId)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/countries/{countryId}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> HasReferencesAsync(Guid countryId)
        {
            var response =
                await _httpClient.GetFromJsonAsync<CountryReferencesResponse>(
                    $"api/countries/{countryId}/references");

            return response?.HasReferences ?? false;
        }


        private static CountryModel Map(CountryResponse response)
        {
            return new CountryModel
            {
                CountryId = response.CountryId,
                IsoAlpha2 = response.IsoAlpha2,
                IsoAlpha3 = response.IsoAlpha3,
                NameEnGb = response.NameEnGb,
                Active = response.Active
            };
        }
    }
}