using AnwendungWPF.Clients;
using AnwendungWPF.Models;
using AnwendungWPF.Utilities;
using AnwendungWPF.ViewModels.EditorViewModels;
using System.Collections.ObjectModel;

namespace AnwendungWPF.ViewModels
{
    public class ManageAddressesViewModel : ViewModelBase
    {
        private readonly IAddressApiClient _addressApiClient;
        private readonly IPlaceApiClient _placeApiClient;

        public ObservableCollection<AddressModel> ListOfAddresses { get; } = new();
        public ObservableCollection<PlacePickerItem> PlacePickerItems { get; } = new();

        private AddressModel? _selectedAddress;
        public AddressModel? SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                if (_selectedAddress == value)
                    return;

                _selectedAddress = value;

                OnPropertyChanged(nameof(SelectedAddress));

                // changing the selected address cancels any changes currently being made
                Editor.Cancel();

                UpdateCommandStates();
            }
        }

        private Guid _selectedPlaceId;

        public Guid SelectedPlaceId
        {
            get => _selectedPlaceId;
            set
            {
                if (_selectedPlaceId == value)
                    return;

                _selectedPlaceId = value;

                OnPropertyChanged(nameof(SelectedPlaceId));

                if (Editor.IsEditing && Editor.PlaceId != value)
                {
                    Editor.PlaceId = value;
                }
            }
        }

        private string _addressErrorMessage = string.Empty;
        public string AddressErrorMessage
        {
            get => _addressErrorMessage;
            private set
            {
                if (_addressErrorMessage == value)
                    return;

                _addressErrorMessage = value;

                OnPropertyChanged(nameof(AddressErrorMessage));
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // combined error message from this view model and the editor
        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(AddressErrorMessage))
                    return AddressErrorMessage;

                return Editor.ErrorMessage;
            }
        }

        public AddressEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }

        public RelayCommand EditCommand { get; }

        public AsyncRelayCommand SaveCommand { get; }

        public AsyncRelayCommand DeleteCommand { get; }

        public RelayCommand CancelCommand { get; }

        public event EventHandler<DeleteAddressConfirmationEventArguments>? DeleteAddressConfirmationRequested;

        public ManageAddressesViewModel(IAddressApiClient addressApiClient, IPlaceApiClient placeApiClient)
        {
            ArgumentNullException.ThrowIfNull(addressApiClient);
            _addressApiClient = addressApiClient;
            ArgumentNullException.ThrowIfNull(placeApiClient);
            _placeApiClient = placeApiClient;

            Editor = new AddressEditorViewModel();

            // listen for editor changes so command states update when validation changes
            Editor.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(AddressEditorViewModel.PlaceId))
                {
                    _selectedPlaceId = Editor.PlaceId;
                    OnPropertyChanged(nameof(SelectedPlaceId));
                }

                OnPropertyChanged(nameof(ErrorMessage));
                UpdateCommandStates();
            };

            CreateCommand =
                new RelayCommand(_ => CreateAddress(), CanCreateAddress);

            EditCommand =
                new RelayCommand(_ => EditAddress(), CanEditAddress);

            SaveCommand =
                new AsyncRelayCommand(_ => SaveAddress(), CanSaveAddress);

            DeleteCommand =
                new AsyncRelayCommand(_ => DeleteAddress(), CanDeleteAddress);

            CancelCommand =
                new RelayCommand(_ => CancelEdit(), CanCancelEdit);
        }

        public async Task InitialiseAsync()
        {
            var places = await _placeApiClient.GetAllAsync();

            PlacePickerItems.Clear();

            foreach (var place in places
                .OrderBy(p => p.City)
                .ThenBy(p => p.Postcode)
                .ThenBy(p => p.Country))
            {
                PlacePickerItems.Add(new PlacePickerItem
                {
                    Id = place.PlaceId,
                    City = place.City,
                    Postcode = place.Postcode,
                    Country = place.Country
                });
            }

            var addresses = await _addressApiClient.GetAllAsync();

            ListOfAddresses.Clear();

            foreach (var address in addresses
                .OrderByDescending(a => a.ModifiedUtc)
                .ThenBy(a => a.PlaceId))
            {
                var place = places.FirstOrDefault(
                    p => p.PlaceId == address.PlaceId);

                if (place is not null)
                {
                    address.City = place.City;
                    address.Postcode = place.Postcode;
                    address.Country = place.Country;
                }

                ListOfAddresses.Add(address);
            }
        }

        private void ApplyPlaceDetails(AddressModel address)
        {
            var place = PlacePickerItems.FirstOrDefault(
                item => item.Id == address.PlaceId);

            if (place is null)
            {
                address.City = string.Empty;
                address.Postcode = string.Empty;
                address.Country = string.Empty;
                return;
            }

            address.City = place.City;
            address.Postcode = place.Postcode;
            address.Country = place.Country;
        }

        private void CreateAddress()
        {
            AddressErrorMessage = string.Empty;

            Editor.BeginCreate();
            SelectedPlaceId = Guid.Empty;

            UpdateCommandStates();
        }

        private void EditAddress()
        {
            if (SelectedAddress is null)
                return;

            AddressErrorMessage = string.Empty;

            Editor.BeginEdit(SelectedAddress);
            SelectedPlaceId = SelectedAddress.PlaceId;

            UpdateCommandStates();
        }

        private async Task SaveAddress()
        {
            AddressErrorMessage = string.Empty;

            if (!Editor.IsEditing)
                return;

            if (Editor.WorkingCopyAddress is null || !Editor.IsValid)
                return;

            var address = Editor.WorkingCopyAddress;

            if (address.AddressId == Guid.Empty)
            {
                // new address
                await _addressApiClient.CreateAsync(
                    address.PlaceId,
                    address.AddressLine1,
                    address.AddressLine2,
                    address.AddressLine3,
                    address.Name1,
                    address.Name2);
            }
            else
            {
                // existing address
                await _addressApiClient.UpdateAsync(
                    address.AddressId,
                    address.PlaceId,
                    address.AddressLine1,
                    address.AddressLine2,
                    address.AddressLine3,
                    address.Name1,
                    address.Name2);
            }

            await InitialiseAsync();

            Editor.Cancel();

            SelectedAddress = null;

            UpdateCommandStates();
        }

        private async Task DeleteAddress()
        {
            if (SelectedAddress is null)
                return;

            var address = SelectedAddress;

            var hasReferences = false;
            // TODO implement HasReferences for addresses
            //var hasReferences =
            //    await _addressApiClient.HasReferencesAsync(
            //        address.AddressId);

            var args = new DeleteAddressConfirmationEventArguments(
                address,
                hasReferences);

            DeleteAddressConfirmationRequested?.Invoke(this, args);

            if (!args.Confirmed)
                return;

            AddressErrorMessage = string.Empty;

            await _addressApiClient.DeleteAsync(
                SelectedAddress.AddressId);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedAddress = null;

            UpdateCommandStates();
        }

        private void CancelEdit()
        {
            AddressErrorMessage = string.Empty;

            Editor.Cancel();
            SelectedPlaceId = Guid.Empty;

            UpdateCommandStates();
        }

        private bool CanCreateAddress(object? parameter)
        {
            return !Editor.IsEditing;
        }

        private bool CanEditAddress(object? parameter)
        {
            return SelectedAddress is not null;
        }

        private bool CanSaveAddress(object? parameter)
        {
            return Editor.WorkingCopyAddress is not null &&
                Editor.IsValid;
        }

        private bool CanDeleteAddress(object? parameter)
        {
            return SelectedAddress is not null;
        }

        private bool CanCancelEdit(object? parameter)
        {
            return Editor.IsEditing;
        }

        private void UpdateCommandStates()
        {
            CreateCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }
    }
}