using AnwendungWPF.Clients;
using AnwendungWPF.Models;
using AnwendungWPF.Utilities;
using AnwendungWPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AnwendungWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IAddressApiClient _addressApiClient;

        public ObservableCollection<AddressModel> ListOfAddresses { get; } = new();

        #region menu commands
        public ICommand ManageCountriesCommand { get; }
        public ICommand ManageAddressesCommand { get; }

        public ICommand ExitCommand { get; }
        #endregion

        public MainWindowViewModel(IServiceProvider serviceProvider, IAddressApiClient addressApiClient)
        {
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(nameof(serviceProvider));
            _addressApiClient = addressApiClient
                ?? throw new ArgumentNullException(nameof(serviceProvider));

            ManageCountriesCommand =
                new RelayCommand(ManageCountries);

            ManageAddressesCommand =
                new RelayCommand(ManageAddresses);

            ExitCommand =
                new RelayCommand(Exit);
        }

        // initialization
        public async Task InitializeAsync()
        {
            var addresses = await _addressApiClient.GetAllAsync();

            ListOfAddresses.Clear();

            foreach (AddressModel address in addresses.OrderBy(a => a.Name1))
            {
                ListOfAddresses.Add(address);
            }
        }


        #region menu actions
        private void ManageCountries()
        {
            var window = _serviceProvider.GetRequiredService<ManageCountriesWindow>();
            window.ShowDialog();
        }

        private void ManageAddresses()
        {
            var window = _serviceProvider.GetRequiredService<ManageAddressesWindow>();
            window.ShowDialog();
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}
