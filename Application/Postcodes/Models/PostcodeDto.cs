namespace Application.Postcodes.Models
{
    public sealed class PostcodeDto
    {
        public Guid PostcodeId { get; init; }

        public Guid CountryId { get; init; }

        public string Postcode { get; init; } = string.Empty;
    }
}
