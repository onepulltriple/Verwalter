namespace Application.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<AddressDto?> GetByIdAsync(Guid addressId)
        {
            return await _addressRepository.GetByIdAsync(addressId);
        }
    }
}