using APIContracts.Addresses;
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
        private readonly IPlaceApiClient _placeApiClient;

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

        public AsyncRelayCommand SearchCommand { get; }

        public AsyncRelayCommand ShowFullListCommand { get; }

        public SearchAddressesViewModel(
            IAddressApiClient addressApiClient,
            IPlaceApiClient placeApiClient)
        {
            ArgumentNullException.ThrowIfNull(addressApiClient);
            ArgumentNullException.ThrowIfNull(placeApiClient);

            _addressApiClient = addressApiClient;
            _placeApiClient = placeApiClient;

            Editor = new AddressSearchEditorViewModel();

            SearchCommand =
                new AsyncRelayCommand(_ => SearchAddressesAsync());

            ShowFullListCommand =
                new AsyncRelayCommand(_ => ShowFullListAsync());
        }

        public async Task InitialiseAsync()
        {
            await SearchAddressesAsync();
        }

        private async Task SearchAddressesAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                var request = new AddressSearchRequest
                {
                    AddressLine1 =
                        GetSearchValue(Editor.AddressLine1),

                    Name1 =
                        GetSearchValue(Editor.Name1),

                    City =
                        GetSearchValue(Editor.CityName),

                    Country =
                        GetSearchValue(Editor.CountryName),

                    Postcode =
                        GetSearchValue(Editor.Postcode),

                    PageNumber = 1,
                    PageSize = 500
                };

                var addresses =
                    await _addressApiClient.SearchAsync(request);

                await DisplayAddressesAsync(addresses);
            }
            catch (Exception)
            {
                ErrorMessage = "The addresses could not be searched.";
            }
        }

        private async Task ShowFullListAsync()
        {
            Editor.AddressLine1 = string.Empty;
            Editor.Name1 = string.Empty;
            Editor.CityName = string.Empty;
            Editor.CountryName = string.Empty;
            Editor.Postcode = string.Empty;

            await SearchAddressesAsync();
        }

        private async Task DisplayAddressesAsync(
            IReadOnlyList<AddressModel> addresses)
        {
            var places = await _placeApiClient.GetAllAsync();

            var placesById = places.ToDictionary(
                place => place.PlaceId);

            ListOfAddresses.Clear();

            foreach (var address in addresses)
            {
                if (placesById.TryGetValue(
                        address.PlaceId,
                        out var place))
                {
                    address.City = place.City;
                    address.Postcode = place.Postcode;
                    address.Country = place.Country;
                }

                ListOfAddresses.Add(address);
            }

            SelectedAddress = null;
        }

        private static string? GetSearchValue(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }
    }
}