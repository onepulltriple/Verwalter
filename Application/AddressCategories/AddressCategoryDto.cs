namespace Application.AddressCategories
{
    public sealed class AddressCategoryDto
    {
        public Guid AddressCategoryId { get; init; }

        public string Code { get; init; } = string.Empty;

        public string NameEnGb { get; init; } = string.Empty;

        public string? Description { get; init; }

        public bool Active { get; init; }
    }
}
