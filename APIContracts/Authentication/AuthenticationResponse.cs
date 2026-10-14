using ApiContracts.AppUsers;

namespace ApiContracts.Authentication;

public sealed class AuthenticationResponse
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public string? JwtToken { get; init; }
    public AppUserResponse? User { get; init; }
}