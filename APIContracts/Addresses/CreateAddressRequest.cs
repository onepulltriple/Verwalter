using System.ComponentModel.DataAnnotations;

namespace ApiContracts.Addresses
{
    /// <summary>
    /// Data required to create an address associated with an existing place.
    /// </summary>
    public sealed class CreateAddressRequest
    {
        /// <summary>
        /// The unique identifier of the place associated with the address.
        /// </summary>
        [Required]
        public Guid PlaceId { get; init; }

        /// <summary>
        /// The first address line.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string AddressLine1 { get; init; } = string.Empty;

        /// <summary>
        /// The optional second address line.
        /// </summary>
        [StringLength(200)]
        public string? AddressLine2 { get; init; }

        /// <summary>
        /// The optional third address line.
        /// </summary>
        [StringLength(200)]
        public string? AddressLine3 { get; init; }

        /// <summary>
        /// The optional first person or organization name.
        /// </summary>
        [StringLength(200)]
        public string? Name1 { get; init; }

        /// <summary>
        /// The optional second person or organization name.
        /// </summary>
        [StringLength(200)]
        public string? Name2 { get; init; }
    }
}