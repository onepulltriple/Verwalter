using AnwendungWPF.Clients;
using AnwendungWPF.Utilities;
using AnwendungWPF.Models;
using AnwendungWPF.ViewModels;
using AnwendungWPF.ViewModels.EditorViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnwendungWPF.ViewModels
{
    public class ManageCountriesViewModel : ViewModelBase
    {
        private readonly ICountryApiClient _countryApiClient;

        public ObservableCollection<CountryModel> ListOfCountries { get; } = new();

        private CountryModel? _selectedCountry;
        public CountryModel? SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry == value)
                    return;

                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));

                // changing the selected country cancels any changes currently being made
                Editor.Cancel();

                UpdateCommandStates();
            }
        }


        private string _countryErrorMessage = string.Empty;
        public string CountryErrorMessage
        {
            get => _countryErrorMessage;
            private set
            {
                if (_countryErrorMessage == value)
                    return;

                _countryErrorMessage = value;

                OnPropertyChanged(nameof(CountryErrorMessage));
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // combined error message from this and editor
        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(CountryErrorMessage))
                    return CountryErrorMessage;

                return Editor.ErrorMessage;
            }
        }

        public CountryEditorViewModel Editor { get; }

        public RelayCommand CreateCommand { get; }
        public RelayCommand EditCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }
        public RelayCommand CancelCommand { get; }

        public event EventHandler<DeleteCountryConfirmationEventArguments>? DeleteCountryConfirmationRequested;

        public ManageCountriesViewModel(ICountryApiClient countryApiClient)
        {
            ArgumentNullException.ThrowIfNull(countryApiClient);
            _countryApiClient = countryApiClient;

            Editor = new CountryEditorViewModel();

            // listen for editor changes so command states update when validation state changes
            Editor.PropertyChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(ErrorMessage));
                UpdateCommandStates();
            };

            CreateCommand
                = new RelayCommand(_ => CreateCountry(), CanCreateCountry);
            EditCommand
                = new RelayCommand(_ => EditCountry(), CanEditCountry);
            SaveCommand
                = new AsyncRelayCommand(_ => SaveCountry(), CanSaveCountry);
            DeleteCommand
                = new AsyncRelayCommand(_ => DeleteCountry(), CanDeleteCountry);
            CancelCommand
                = new RelayCommand(_ => CancelEdit(), CanCancelEdit);
        }


        // loads the list
        public async Task InitialiseAsync()
        {
            var countries = await _countryApiClient.GetAllAsync();

            ListOfCountries.Clear();

            foreach (CountryModel country in countries.OrderBy(c => c.NameEnGb))
            {
                ListOfCountries.Add(country);
            }
        }

        private void CreateCountry()
        {
            CountryErrorMessage = string.Empty;

            Editor.BeginCreate();

            UpdateCommandStates();
        }

        private void EditCountry()
        {
            if (SelectedCountry is null)
                return;

            CountryErrorMessage = string.Empty;

            Editor.BeginEdit(SelectedCountry);

            UpdateCommandStates();
        }

        private async Task SaveCountry()
        {
            CountryErrorMessage = string.Empty;

            if (!Editor.IsEditing)
                return;

            if (Editor.WorkingCopyCountry is null || !Editor.IsValid)
                return;

            if (Editor.WorkingCopyCountry.CountryId == Guid.Empty)
            {
                // new country
                await _countryApiClient.CreateAsync(
                    Editor.WorkingCopyCountry.IsoAlpha2,
                    Editor.WorkingCopyCountry.IsoAlpha3,
                    Editor.WorkingCopyCountry.NameEnGb,
                    Editor.WorkingCopyCountry.Active
                );
            }
            else
            {
                // existing country
                await _countryApiClient.UpdateAsync(
                    Editor.WorkingCopyCountry.CountryId,
                    Editor.WorkingCopyCountry.IsoAlpha2,
                    Editor.WorkingCopyCountry.IsoAlpha3,
                    Editor.WorkingCopyCountry.NameEnGb,
                    Editor.WorkingCopyCountry.Active
                );
            }

            await InitialiseAsync();

            Editor.Cancel();

            SelectedCountry = null;

            UpdateCommandStates();
        }

        private async Task DeleteCountry()
        {
            if (SelectedCountry is null)
                return;

            CountryErrorMessage = string.Empty;

            var country = SelectedCountry;

            var hasReferences =
                await _countryApiClient.HasReferencesAsync(
                    country.CountryId);

            var args = new DeleteCountryConfirmationEventArguments(
                country,
                hasReferences);

            DeleteCountryConfirmationRequested?.Invoke(this, args);

            if (!args.Confirmed)
                return;

            await _countryApiClient.DeleteAsync(country.CountryId);

            await InitialiseAsync();

            Editor.Cancel();

            SelectedCountry = null;
        }

        private void CancelEdit()
        {
            CountryErrorMessage = string.Empty;

            Editor.Cancel();

            UpdateCommandStates();
        }

        private bool CanCreateCountry(object? arg)
        {
            return !Editor.IsEditing;
        }

        private bool CanEditCountry(object? parameter)
        {
            return SelectedCountry is not null;
        }

        private bool CanSaveCountry(object? parameter)
        {
            return Editor.WorkingCopyCountry is not null &&
                Editor.IsValid;
        }

        private bool CanDeleteCountry(object? parameter)
        {
            return SelectedCountry is not null;
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

