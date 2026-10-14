using Application.Addresses.Commands;
using Application.Addresses.Models;

namespace Application.Addresses.Repositories
{
    public interface IAddressWithPlaceRepository
    {
        Task<AddressDto> CreateAsync(
            CreateAddressWithPlaceCommand command);

        Task<AddressDto?> UpdateAsync(
            UpdateAddressWithPlaceCommand command);
    }
}