namespace Application.Cities.Models
{
    public sealed class CityDto
    {
        public Guid CityId { get; init; }

        public Guid CountryId { get; init; }

        public string NameEnGb { get; init; } = string.Empty;
    }
}
