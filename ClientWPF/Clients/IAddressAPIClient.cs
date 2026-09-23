using AnwendungWPF.Models;
using ApiContracts.Addresses;

namespace AnwendungWPF.Clients
{
    public interface IAddressApiClient
    {
        Task<AddressModel?> GetByIdAsync(Guid addressId);

        Task<IReadOnlyList<AddressModel>> GetAllAsync();

        Task<AddressModel> CreateAsync(
            Guid placeId,
            string addressLine1,
            string? addressLine2,
            string? addressLine3,
            string? name1,
            string? name2);

        Task<AddressModel> CreateWithPlaceAsync(
            string countryName,
            string postcode,
            string cityName,
            string addressLine1,
            string? addressLine2,
            string? addressLine3,
            string? name1,
            string? name2);

        Task UpdateAsync(
            Guid addressId,
            Guid placeId,
            string addressLine1,
            string? addressLine2,
            string? addressLine3,
            string? name1,
            string? name2);

        Task UpdateWithPlaceAsync(
            Guid addressId,
            string countryName,
            string postcode,
            string cityName,
            string addressLine1,
            string? addressLine2,
            string? addressLine3,
            string? name1,
            string? name2);

        Task DeleteAsync(Guid addressId);

        Task<IReadOnlyList<AddressModel>> SearchAsync(
            AddressSearchRequest request);
    }
}