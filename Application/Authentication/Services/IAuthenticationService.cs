namespace Application.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(
            LoginRequest request);
    }
}
