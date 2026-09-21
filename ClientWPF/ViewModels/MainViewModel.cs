using Application.Addresses;
using ClientWPF.Clients;

namespace ClientWPF.ViewModels
{
    public class MainViewModel
    {
        private readonly AddressApiClient _addressApiClient;

        public AddressDto? Address { get; private set; }

        public MainViewModel()
        {
            _addressApiClient = new AddressApiClient();
        }

        public async Task LoadAddressAsync()
        {
            Address = await _addressApiClient.GetByIdAsync(
                Guid.Parse("60000000-0000-0000-0000-000000000001")
            );
            ;
        }
    }
}
