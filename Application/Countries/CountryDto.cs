namespace Application.Countries
{
    public sealed class CountryDto
    {
        public Guid CountryId { get; init; }

        public string? IsoAlpha2 { get; init; }

        public string? IsoAlpha3 { get; init; }

        public string NameEnGb { get; init; } = string.Empty;

        public bool Active { get; init; }
    }
}
