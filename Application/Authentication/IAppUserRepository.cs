namespace Application.Authentication
{
    public interface IAppUserRepository
    {
        Task<AppUserAuthenticationData?> GetByUsernameAsync(
            string username);
    }

    public sealed class AppUserAuthenticationData
    {
        public Guid AppUserId { get; init; }

        public string Username { get; init; } = string.Empty;

        public string? EmailAddress { get; init; }

        public string PasswordHash { get; init; } = string.Empty;

        public bool Active { get; init; }

        public DateTime? LastLoginUtc { get; init; }
    }
}