using AnwendungWPF.Infrastructure;
using AnwendungWPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace AnwendungWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;

        #region menu commands
        public ICommand ManageCountriesCommand { get; }

        public ICommand ExitCommand { get; }
        #endregion

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(nameof(serviceProvider));

            ManageCountriesCommand =
                new RelayCommand(ManageCountries);

            ExitCommand =
                new RelayCommand(Exit);
        }

        #region menu actions
        private void ManageCountries()
        {
            var window = _serviceProvider.GetRequiredService<ManageCountriesWindow>();
            window.ShowDialog();
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}
