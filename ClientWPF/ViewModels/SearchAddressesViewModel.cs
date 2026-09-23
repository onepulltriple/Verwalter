using AnwendungWPF.Clients;
using AnwendungWPF.Models;
using AnwendungWPF.Utilities;
using AnwendungWPF.ViewModels.EditorViewModels;
using System.Collections.ObjectModel;

namespace AnwendungWPF.ViewModels
{
    public class SearchAddressesViewModel : ViewModelBase
    {
        private readonly IAddressApiClient _addressApiClient;

        private IReadOnlyList<AddressModel> _allAddresses =
            Array.Empty<AddressModel>();

        private AddressModel? _selectedAddress;

        private string _errorMessage = string.Empty;

        public ObservableCollection<AddressModel> ListOfAddresses { get; } = new();

        public AddressSearchEditorViewModel Editor { get; }

        public AddressModel? SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                if (_selectedAddress == value)
                    return;

                _selectedAddress = value;

                OnPropertyChanged(nameof(SelectedAddress));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                if (_errorMessage == value)
                    return;

                _errorMessage = value;

                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public RelayCommand SearchCommand { get; }

        public SearchAddressesViewModel(IAddressApiClient addressApiClient)
        {
            ArgumentNullException.ThrowIfNull(addressApiClient);

            _addressApiClient = addressApiClient;

            Editor = new AddressSearchEditorViewModel();

            SearchCommand =
                new RelayCommand(_ => SearchAddresses());
        }

        public async Task InitialiseAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                _allAddresses = await _addressApiClient.GetAllAsync();

                ListOfAddresses.Clear();

                foreach (var address in _allAddresses)
                {
                    ListOfAddresses.Add(address);
                }
            }
            catch (Exception)
            {
                ErrorMessage = "The addresses could not be loaded.";
            }
        }

        private void SearchAddresses()
        {
            ErrorMessage = string.Empty;

            string searchText = Editor.AddressLine1.Trim();

            IEnumerable<AddressModel> matchingAddresses = _allAddresses;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                matchingAddresses = _allAddresses.Where(address =>
                    address.AddressLine1.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase));
            }

            ListOfAddresses.Clear();

            foreach (var address in matchingAddresses)
            {
                ListOfAddresses.Add(address);
            }

            SelectedAddress = null;
        }
    }
}