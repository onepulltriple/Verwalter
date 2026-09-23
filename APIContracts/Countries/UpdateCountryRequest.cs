namespace ApiContracts.Countries
{
    public sealed class UpdateCountryRequest
    {
        public string? IsoAlpha2 { get; init; }

        public string? IsoAlpha3 { get; init; }

        public string NameEnGb { get; init; } = string.Empty;

        public bool Active { get; init; }
    }
}