using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AnwendungWPF.Utilities;
using AnwendungWPF.ViewModels;

namespace AnwendungWPF.Views
{
    /// <summary>
    /// Interaction logic for ManageAddressesView.xaml
    /// </summary>
    public partial class ManageAddressesView : UserControl
    {
        public ManageAddressesView()
        {
            InitializeComponent();
        }
        private async void ManageAddressesView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContextChanged += ManageAddressesView_DataContextChanged;

            SubscribeToViewModel();

            if (DataContext is ManageAddressesViewModel viewModel)
            {
                try
                {
                    await viewModel.InitialiseAsync();

                    // diagnostics: report list counts
                    System.Diagnostics.Debug.WriteLine($"ManageAddressesView: ListOfAddresses.Count = " +
                        $"{viewModel.ListOfAddresses?.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ManageAddressesView.InitialiseAsync threw: {ex}");
                    System.Windows.MessageBox.Show($"Initialise error: {ex.Message}", "Debug");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ManageAddressesView: DataContext is not a ManageAddressesViewModel");
            }
        }

        private void ManageAddressesView_Unloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribeFromViewModel();

            DataContextChanged -= ManageAddressesView_DataContextChanged;
        }

        private void SubscribeToViewModel()
        {
            if (DataContext is ManageAddressesViewModel viewModel)
            {
                viewModel.DeleteAddressConfirmationRequested +=
                    ManageAddressesView_DeleteAddressConfirmationRequested;
            }
        }

        private void UnsubscribeFromViewModel()
        {
            if (DataContext is ManageAddressesViewModel viewModel)
            {
                viewModel.DeleteAddressConfirmationRequested -=
                    ManageAddressesView_DeleteAddressConfirmationRequested;
            }
        }

        private void ManageAddressesView_DataContextChanged(
            object sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ManageAddressesViewModel oldViewModel)
            {
                oldViewModel.DeleteAddressConfirmationRequested -=
                    ManageAddressesView_DeleteAddressConfirmationRequested;
            }

            if (e.NewValue is ManageAddressesViewModel newViewModel)
            {
                newViewModel.DeleteAddressConfirmationRequested +=
                    ManageAddressesView_DeleteAddressConfirmationRequested;
            }
        }

        private void ManageAddressesView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (DataContext is ManageAddressesViewModel viewModel &&
                viewModel.SaveCommand.CanExecute(null))
            {
                viewModel.SaveCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void ManageAddressesView_DeleteAddressConfirmationRequested(
            object? sender,
            DeleteAddressConfirmationEventArguments e)
        {
            if (e.HasReferences)
            {
                var message =
                    $"The address '{e.Address.Name1}' is referenced by " +
                    "one or more AppUsers.\n\n" +
                    "Remove those references before deleting this address.";

                MessageBox.Show(
                    message,
                    "Cannot delete Address",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                e.Confirmed = false;
                return;
            }

            var confirmationMessage =
                $"The address '{e.Address.Name1}' is not referenced by any " +
                "AppUsers.\n\n" +
                "Are you sure you want to delete this address?";

            var result = MessageBox.Show(
                confirmationMessage,
                "Delete Address?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            e.Confirmed = result == MessageBoxResult.Yes;
        }
    }
}
