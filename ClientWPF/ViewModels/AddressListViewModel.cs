using AnwendungWPF.Clients;
using AnwendungWPF.Infrastructure;
using System.Collections.ObjectModel;

namespace AnwendungWPF.ViewModels
{
    public class AddressListViewModel : ViewModelBase
    {
        private readonly IAddressApiClient _addressApiClient;

        public ObservableCollection<AddressViewModel> Addresses { get; }
            = new();

        public AsyncRelayCommand LoadCommand { get; }

        public AddressListViewModel(IAddressApiClient addressApiClient)
        {
            _addressApiClient = addressApiClient;

            LoadCommand = new AsyncRelayCommand(LoadAsync);
        }

        private async Task LoadAsync()
        {
            var addresses = await _addressApiClient.GetAllAsync();

            Addresses.Clear();

            foreach (var address in addresses)
            {
                Addresses.Add(new AddressViewModel(address));
            }
        }
    }
}
