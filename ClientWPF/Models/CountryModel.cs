namespace AnwendungWPF.Models
{
    public sealed class CountryModel
    {
        public Guid CountryId { get; set; }

        public string? IsoAlpha2 { get; set; }

        public string? IsoAlpha3 { get; set; }

        public string NameEnGb { get; set; } = string.Empty;

        public bool Active { get; set; }
    }
}