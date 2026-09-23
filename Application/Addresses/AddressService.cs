namespace Application.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IReadOnlyList<AddressDto>> GetAllAsync()
        {
            return await _addressRepository.GetAllAsync();
        }

        public async Task<AddressDto?> GetByIdAsync(Guid addressId)
        {
            return await _addressRepository.GetByIdAsync(addressId);
        }

        public async Task<AddressDto> CreateAsync(AddressDto address)
        {
            await _addressRepository.AddAsync(address);

            return address;
        }

        public async Task<bool> UpdateAsync(AddressDto address)
        {
            var existing = await _addressRepository.GetByIdAsync(
                address.AddressId);

            if (existing is null)
            {
                return false;
            }

            await _addressRepository.UpdateAsync(address);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid addressId)
        {
            var existing = await _addressRepository.GetByIdAsync(addressId);

            if (existing is null)
            {
                return false;
            }

            await _addressRepository.DeleteAsync(addressId);

            return true;
        }
    }
}