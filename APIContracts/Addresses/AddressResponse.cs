namespace APIContracts.Addresses
{
    public sealed class AddressResponse
    {
        public Guid AddressId { get; init; }

        public Guid PlaceId { get; init; }

        public string AddressLine1 { get; init; } = string.Empty;

        public string? AddressLine2 { get; init; }

        public string? AddressLine3 { get; init; }

        public string? Name1 { get; init; }

        public string? Name2 { get; init; }

        public DateTime ModifiedUtc { get; init; }
    }
}
