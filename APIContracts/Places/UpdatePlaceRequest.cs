namespace ApiContracts.Places
{
    public sealed class UpdatePlaceRequest
    {
        public Guid CountryId { get; init; }

        public Guid PostcodeId { get; init; }

        public Guid CityId { get; init; }

        public string? NameEnGb { get; init; }
    }
}