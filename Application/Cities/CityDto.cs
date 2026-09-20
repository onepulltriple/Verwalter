namespace Application.Cities
{
    public sealed class CityDto
    {
        public Guid CityId { get; init; }

        public Guid CountryId { get; init; }

        public string NameEnGb { get; init; } = string.Empty;
    }
}
