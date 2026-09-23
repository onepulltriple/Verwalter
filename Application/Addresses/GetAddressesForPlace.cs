namespace Application.Addresses
{
    public class GetAddressesForPlace
    {
        private readonly IAddressRepository _addressRepository;

        public GetAddressesForPlace(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<List<AddressDto>> ExecuteAsync(Guid placeId)
        {
            var addresses = await _addressRepository.GetByPlaceIdAsync(placeId);

            return addresses
                .Select(x => new AddressDto
                {
                    AddressId = x.AddressId,
                    PlaceId = x.PlaceId,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    AddressLine3 = x.AddressLine3,
                    Name1 = x.Name1,
                    Name2 = x.Name2
                })
                .ToList();
        }
    }
}