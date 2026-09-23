using Application.Common;

namespace Application.Addresses
{
    public interface IAddressService
    {
        Task<IReadOnlyList<AddressDto>> GetAllAsync();

        Task<AddressDto?> GetByIdAsync(Guid addressId);

        Task<AddressDto> CreateAsync(AddressDto address);

        Task<bool> UpdateAsync(AddressDto address);

        Task<bool> DeleteAsync(Guid addressId);

        Task<AddressDto> CreateWithPlaceAsync(CreateAddressWithPlaceCommand command);

        Task<AddressDto?> UpdateWithPlaceAsync(UpdateAddressWithPlaceCommand command);

        Task<PagedResult<AddressDto>> SearchAsync(AddressSearchCriteria criteria);
    }
}