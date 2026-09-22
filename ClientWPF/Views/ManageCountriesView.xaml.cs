using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AnwendungWPF.Utilities;
using AnwendungWPF.ViewModels;

namespace AnwendungWPF.Views
{
    /// <summary>
    /// Interaction logic for ManageCountriesView.xaml
    /// </summary>
    public partial class ManageCountriesView : UserControl
    {
        public ManageCountriesView()
        {
            InitializeComponent();
        }

        private async void ManageCountriesView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContextChanged += ManageCountriesView_DataContextChanged;

            SubscribeToViewModel();

            if (DataContext is ManageCountriesViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManageCountriesView: ListOfCountries.Count = " +
                        $"{viewModel.ListOfCountries?.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ManageCountriesView.InitialiseAsync threw: {ex}");
                    System.Windows.MessageBox.Show($"Initialise error: {ex.Message}", "Debug");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ManageCountriesView: DataContext is not a ManageCountriesViewModel");
            }
        }

        private void ManageCountriesView_Unloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribeFromViewModel();

            DataContextChanged -= ManageCountriesView_DataContextChanged;
        }

        private void SubscribeToViewModel()
        {
            if (DataContext is ManageCountriesViewModel viewModel)
            {
                viewModel.DeleteCountryConfirmationRequested +=
                    ManageCountriesView_DeleteCountryConfirmationRequested;
            }
        }

        private void UnsubscribeFromViewModel()
        {
            if (DataContext is ManageCountriesViewModel viewModel)
            {
                viewModel.DeleteCountryConfirmationRequested -=
                    ManageCountriesView_DeleteCountryConfirmationRequested;
            }
        }

        private void ManageCountriesView_DataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ManageCountriesViewModel oldViewModel)
            {
                oldViewModel.DeleteCountryConfirmationRequested -=
                    ManageCountriesView_DeleteCountryConfirmationRequested;
            }

            if (e.NewValue is ManageCountriesViewModel newViewModel)
            {
                newViewModel.DeleteCountryConfirmationRequested +=
                    ManageCountriesView_DeleteCountryConfirmationRequested;
            }
        }

        private void ManageCountriesView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (DataContext is ManageCountriesViewModel viewModel &&
                viewModel.SaveCommand.CanExecute(null))
            {
                viewModel.SaveCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void ManageCountriesView_DeleteCountryConfirmationRequested(
            object? sender,
            DeleteCountryConfirmationEventArguments e)
        {
            if (e.HasReferences)
            {
                var message =
                    $"The country '{e.Country.NameEnGb}' is referenced by " +
                    "one or more cities or postcodes.\n\n" +
                    "Remove those references before deleting this country.";

                MessageBox.Show(
                    message,
                    "Cannot delete Country",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                e.Confirmed = false;
                return;
            }

            var confirmationMessage =
                $"The country '{e.Country.NameEnGb}' is not referenced by any " +
                "cities or postcodes.\n\n" +
                "Are you sure you want to delete this country?";

            var result = MessageBox.Show(
                confirmationMessage,
                "Delete Country?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            e.Confirmed = result == MessageBoxResult.Yes;
        }

    }
}
