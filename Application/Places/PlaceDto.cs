namespace Application.Places
{
    public sealed class PlaceDto
    {
        public Guid PlaceId { get; init; }

        public Guid CountryId { get; init; }

        public Guid PostcodeId { get; init; }

        public Guid CityId { get; init; }

        public string? NameEnGb { get; init; }
    }
}
