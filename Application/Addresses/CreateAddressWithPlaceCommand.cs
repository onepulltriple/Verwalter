namespace Application.Addresses
{
    public sealed class CreateAddressWithPlaceCommand
    {
        public string CountryName { get; init; } = string.Empty;

        public string Postcode { get; init; } = string.Empty;

        public string CityName { get; init; } = string.Empty;

        public string AddressLine1 { get; init; } = string.Empty;

        public string? AddressLine2 { get; init; }

        public string? AddressLine3 { get; init; }

        public string? Name1 { get; init; }

        public string? Name2 { get; init; }
    }
}