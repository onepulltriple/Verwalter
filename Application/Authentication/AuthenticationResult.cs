namespace Application.Authentication;

public sealed class AuthenticationResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public Guid? AppUserId { get; init; }
    public string? Username { get; init; }
    public string? EmailAddress { get; init; }
    public bool Active { get; init; }
    public DateTime? LastLoginUtc { get; init; }
    public string? JwtToken { get; init; }
}