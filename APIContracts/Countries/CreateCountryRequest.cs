namespace APIContracts.Countries
{
    public sealed class CreateCountryRequest
    {
        public string? IsoAlpha2 { get; init; }

        public string? IsoAlpha3 { get; init; }

        public string NameEnGb { get; init; } = string.Empty;

        public bool Active { get; init; }
    }
}