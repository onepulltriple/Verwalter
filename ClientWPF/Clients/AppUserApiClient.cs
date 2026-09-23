using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiContracts.Authentication;
using AnwendungWPF.Models;

namespace AnwendungWPF.Clients
{
    public class AppUserApiClient : IAppUserApiClient
    {
        private readonly HttpClient _httpClient;

        public AppUserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<(bool Success, string? ErrorMessage, AppUserModel? User, string? JwtToken)> LoginAsync(string username, string password)
        {
            var request = new ApiContracts.Authentication.LoginRequest
            {
                Username = username,
                Password = password
            };

            using var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                // Try to read API result if present to capture error message
                var maybe = await response.Content.ReadFromJsonAsync<ApiContracts.Authentication.AuthenticationResult>();
                return (false, maybe?.ErrorMessage ?? $"HTTP {(int)response.StatusCode}", null, null);
            }

            var result = await response.Content.ReadFromJsonAsync<ApiContracts.Authentication.AuthenticationResult>();
            if (result is null)
            {
                return (false, "Empty response from server", null, null);
            }

            if (!result.Success || result.User is null)
            {
                return (false, result.ErrorMessage, null, result.JwtToken);
            }

            var user = new AppUserModel
            {
                AppUserId = result.User.AppUserId,
                Username = result.User.Username,
                EmailAddress = result.User.EmailAddress,
                Active = result.User.Active,
                LastLoginUtc = result.User.LastLoginUtc
            };

            return (true, null, user, result.JwtToken);
        }
    }
}
