namespace ApiContracts.Addresses
{
    public sealed class AddressSearchResponse
    {
        public IReadOnlyList<AddressResponse> Items { get; init; } = [];

        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        public int TotalCount { get; init; }

        public int TotalPages { get; init; }
    }
}