using Application.Addresses.Commands;
using Application.Addresses.Models;
using Application.Addresses.Queries;
using Application.Common;

namespace Application.Addresses.Services
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

        Task<PagedResult<AddressDto>> SearchAsync(AddressSearchCriteriaQuery criteria);
    }
}