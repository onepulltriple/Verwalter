namespace AnwendungWPF.Models
{
    public sealed class AppUserModel
    {
        public Guid AppUserId { get; init; }
        public string Username { get; init; } = null!;
        public string? EmailAddress { get; init; }
        public bool Active { get; init; }
        public DateTime? LastLoginUtc { get; init; }
    }
}
