namespace Application.Addresses
{
    public sealed class AddressSearchCriteria
    {
        public string? Street { get; init; }

        public string? Town { get; init; }

        public Guid? CountryId { get; init; }

        public Guid? PostcodeId { get; init; }

        public Guid? CityId { get; init; }

        public Guid? PlaceId { get; init; }

        public string? Name { get; init; }

        public string? SearchText { get; init; }

        public int PageNumber { get; init; } = 1;

        public int PageSize { get; init; } = 50;

        public string? SortBy { get; init; }

        public bool SortDescending { get; init; }
    }
}
