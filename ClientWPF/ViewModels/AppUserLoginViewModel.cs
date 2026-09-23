using AnwendungWPF.Clients;
using AnwendungWPF.Models;
using AnwendungWPF.Utilities;
using AnwendungWPF.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace AnwendungWPF.ViewModels
{
    public class AppUserLoginViewModel : ViewModelBase
    {
        private readonly IAppUserApiClient _appUserApiClient;

        private readonly IServiceProvider _serviceProvider;

        private string _enteredUsername = string.Empty;
        public string EnteredUsername
        {
            get => _enteredUsername ?? string.Empty;
            set
            {
                if (_enteredUsername == value)
                    return;

                _enteredUsername = value;
                OnPropertyChanged(nameof(EnteredUsername));
            }
        }

        private string _enteredPassword = string.Empty;
        public string EnteredPassword
        {
            get => _enteredPassword ?? string.Empty;
            set
            {
                if (_enteredPassword == value)
                    return;

                _enteredPassword = value;
                OnPropertyChanged(nameof(EnteredPassword));
            }
        }

        private string _displayedErrorMessage = string.Empty;
        public string DisplayedErrorMessage
        {
            get => _displayedErrorMessage ?? string.Empty;
            set
            {
                if (_displayedErrorMessage == value)
                    return;

                _displayedErrorMessage = value;
                OnPropertyChanged(nameof(DisplayedErrorMessage));
            }
        }

        public AsyncRelayCommand LoginCommand { get; }

        public event EventHandler? LoginSucceeded;

        // constructor receives required services from DI
        public AppUserLoginViewModel(IAppUserApiClient appUserApiClient, IServiceProvider serviceProvider)
        {
            _appUserApiClient = appUserApiClient ?? throw new ArgumentNullException(nameof(appUserApiClient));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            LoginCommand = new AsyncRelayCommand(CheckAppUserAndLogin);

            // string temp = BCrypt.Net.BCrypt.HashPassword("doyoureallythinkiwouldleavethishere?");
        }

        private async Task CheckAppUserAndLogin()
        {
            DisplayedErrorMessage = string.Empty;

            // check that all fields are filled out
            if (string.IsNullOrWhiteSpace(EnteredUsername) ||
                string.IsNullOrWhiteSpace(EnteredPassword))
            {
                DisplayedErrorMessage = "Please fill out all fields.";
                return;
            }

            var loginResult = await _appUserApiClient.LoginAsync(
                EnteredUsername,
                EnteredPassword);

            if (!loginResult.Success)
            {
                DisplayedErrorMessage = loginResult.ErrorMessage
                    ?? "Invalid credentials.";

                return;
            }

            // if all checks pass, resolve the main window/viewmodel using DI
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            await mainWindowViewModel.InitializeAsync();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();

            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }
    }
}
