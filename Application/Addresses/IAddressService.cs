namespace Application.Addresses
{
    public interface IAddressService
    {
        Task<AddressDto?> GetByIdAsync(Guid addressId);

        Task<IReadOnlyList<AddressDto>> GetAllAsync();
    }
}