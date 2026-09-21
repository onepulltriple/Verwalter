using Application.Addresses;
using AnwendungWPF.Clients;

namespace AnwendungWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly AddressApiClient _addressApiClient;

        public AddressDto? Address { get; private set; }

        public MainViewModel(AddressApiClient addressApiClient)
        {
            _addressApiClient = addressApiClient;
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
