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

        public RelayCommand ShowFullListCommand { get; }

        public SearchAddressesViewModel(IAddressApiClient addressApiClient)
        {
            ArgumentNullException.ThrowIfNull(addressApiClient);

            _addressApiClient = addressApiClient;

            Editor = new AddressSearchEditorViewModel();

            SearchCommand =
                new RelayCommand(_ => SearchAddresses());

            ShowFullListCommand =
                new RelayCommand(_ => ShowFullList());
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

            IEnumerable<AddressModel> matchingAddresses =
                _allAddresses;

            if (!string.IsNullOrWhiteSpace(Editor.AddressLine1))
            {
                string searchText = Editor.AddressLine1.Trim();

                matchingAddresses = matchingAddresses.Where(address =>
                    ContainsText(address.AddressLine1, searchText));
            }

            if (!string.IsNullOrWhiteSpace(Editor.Name1))
            {
                string searchText = Editor.Name1.Trim();

                matchingAddresses = matchingAddresses.Where(address =>
                    ContainsText(address.Name1, searchText));
            }

            if (!string.IsNullOrWhiteSpace(Editor.CityName))
            {
                string searchText = Editor.CityName.Trim();

                matchingAddresses = matchingAddresses.Where(address =>
                    ContainsText(address.City, searchText));
            }

            if (!string.IsNullOrWhiteSpace(Editor.CountryName))
            {
                string searchText = Editor.CountryName.Trim();

                matchingAddresses = matchingAddresses.Where(address =>
                    ContainsText(address.Country, searchText));
            }

            if (!string.IsNullOrWhiteSpace(Editor.Postcode))
            {
                string searchText = Editor.Postcode.Trim();

                matchingAddresses = matchingAddresses.Where(address =>
                    ContainsText(address.Postcode, searchText));
            }

            ListOfAddresses.Clear();

            foreach (var address in matchingAddresses)
            {
                ListOfAddresses.Add(address);
            }

            SelectedAddress = null;
        }

        private void ShowFullList()
        {
            ErrorMessage = string.Empty;

            Editor.AddressLine1 = string.Empty;
            Editor.Name1 = string.Empty;
            Editor.CityName = string.Empty;
            Editor.CountryName = string.Empty;
            Editor.Postcode = string.Empty;

            ListOfAddresses.Clear();

            foreach (var address in _allAddresses)
            {
                ListOfAddresses.Add(address);
            }

            SelectedAddress = null;
        }

        private static bool ContainsText(
            string? value,
            string searchText)
        {
            return value?.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}