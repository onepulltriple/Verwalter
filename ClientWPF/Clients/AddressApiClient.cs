using Application.Addresses;
using System.Net.Http;
using System.Net.Http.Json;

namespace AnwendungWPF.Clients
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

        public async Task<IReadOnlyList<AddressDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AddressDto>>(
                "api/addresses")
                ?? [];
        }

        //public async Task<AddressDto> CreateAsync(
        //    CreateAddressRequest request)
        //{
        //    var response = await _httpClient.PostAsJsonAsync(
        //        "api/addresses",
        //        request);

        //    response.EnsureSuccessStatusCode();

        //    return await response.Content
        //        .ReadFromJsonAsync<AddressDto>()
        //        ?? throw new InvalidOperationException();
        //}

        //public async Task UpdateAsync(
        //    Guid id,
        //    UpdateAddressRequest request)
        //{
        //    var response = await _httpClient.PutAsJsonAsync(
        //        $"api/addresses/{id}",
        //        request);

        //    response.EnsureSuccessStatusCode();
        //}

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/addresses/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
