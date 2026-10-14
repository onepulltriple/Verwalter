using Application.Authentication.Repositories;

namespace Application.Authentication.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IAppUserRepository _appUserRepository;

        public AuthenticationService(
            IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository
                ?? throw new ArgumentNullException(nameof(appUserRepository));
        }

        public async Task<AuthenticationResult> LoginAsync(
            LoginRequest request)
        {
            if (request is null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return InvalidCredentials();
            }

            var appUser = await _appUserRepository
                .GetByUsernameAsync(request.Username);

            if (appUser is null || !appUser.Active)
            {
                return InvalidCredentials();
            }

            bool passwordIsValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                appUser.PasswordHash);

            if (!passwordIsValid)
            {
                return InvalidCredentials();
            }

            return new AuthenticationResult
            {
                Success = true,
                AppUserId = appUser.AppUserId,
                Username = appUser.Username,
                EmailAddress = appUser.EmailAddress,
                Active = appUser.Active,
                LastLoginUtc = appUser.LastLoginUtc
            };
        }

        private static AuthenticationResult InvalidCredentials()
        {
            return new AuthenticationResult
            {
                Success = false,
                ErrorMessage = "Invalid credentials."
            };
        }
    }
}