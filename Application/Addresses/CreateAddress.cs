//namespace Application.Addresses
//{
//    public class CreateAddress
//    {
//        private readonly IAddressRepository _addressRepository;

//        public CreateAddress(IAddressRepository addressRepository)
//        {
//            _addressRepository = addressRepository;
//        }

//        public async Task<Guid> ExecuteAsync(CreateAddressCommand command)
//        {
//            if (string.IsNullOrWhiteSpace(command.AddressLine1))
//            {
//                throw new ArgumentException(
//                    "AddressLine1 is required.",
//                    nameof(command.AddressLine1));
//            }

//            // Application decides what should happen.
//            // Persistence implementation is delegated to the repository.

//            return await _addressRepository.CreateAsync(
//                command.PlaceId,
//                command.AddressLine1,
//                command.AddressLine2,
//                command.AddressLine3,
//                command.Name1,
//                command.Name2);
//        }
//    }
//}
