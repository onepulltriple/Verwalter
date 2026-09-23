namespace Application.Addresses
{
    public interface IAddressWithPlaceRepository
    {
        Task<AddressDto> CreateAsync(
            CreateAddressWithPlaceCommand command);
    }
}