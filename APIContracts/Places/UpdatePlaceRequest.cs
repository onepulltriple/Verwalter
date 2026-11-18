namespace ApiContracts.Places
{
    public sealed class UpdatePlaceRequest
    {
        public Guid CountryId { get; init; }

        public string City { get; init; } = string.Empty;

        public string Postcode { get; init; } = string.Empty;

        public string? NameEnGb { get; init; }
    }
}