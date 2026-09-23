namespace Application.Addresses
{
    public interface IAddressWithPlaceRepository
    {
        Task<AddressDto> CreateAsync(
            CreateAddressWithPlaceCommand command);

        Task<AddressDto?> UpdateAsync(
            UpdateAddressWithPlaceCommand command);
    }
}