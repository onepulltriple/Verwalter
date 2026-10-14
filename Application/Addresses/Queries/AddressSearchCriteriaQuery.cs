namespace Application.Addresses.Queries
{
    public sealed class AddressSearchCriteriaQuery
    {
        public string? AddressLine1 { get; init; }

        public string? AddressLine2 { get; init; }

        public string? AddressLine3 { get; init; }

        public string? Name1 { get; init; }

        public string? Name2 { get; init; }

        public string? City { get; init; }

        public string? Postcode { get; init; }

        public string? Country { get; init; }

        public Guid? CountryId { get; init; }

        public Guid? PostcodeId { get; init; }

        public Guid? CityId { get; init; }

        public Guid? PlaceId { get; init; }

        public int PageNumber { get; init; } = 1;

        public int PageSize { get; init; } = 500;

        public string? SortBy { get; init; }

        public bool SortDescending { get; init; }
    }
}
