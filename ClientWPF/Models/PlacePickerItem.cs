namespace AnwendungWPF.Models
{
    public sealed class PlacePickerItem
    {
        public Guid Id { get; init; }

        public string City { get; init; } = string.Empty;

        public string Postcode { get; init; } = string.Empty;

        public string Country { get; init; } = string.Empty;
    }
}