namespace AnwendungWPF.Models
{
    public sealed class AddressModel
    {
        public Guid AddressId { get; set; }

        public Guid PlaceId { get; set; }

        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public string? Name1 { get; set; }

        public string? Name2 { get; set; }

        public string City { get; set; } = string.Empty;

        public string Postcode { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }
}