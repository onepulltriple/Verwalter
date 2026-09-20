namespace Application.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(
            LoginRequest request);
    }
}
