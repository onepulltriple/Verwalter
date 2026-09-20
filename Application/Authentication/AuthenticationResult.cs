namespace Application.Authentication
{
    public sealed class AuthenticationResult
    {
        public bool Succeeded { get; init; }

        public string? ErrorMessage { get; init; }

        public string? AccessToken { get; init; }
    }
}
