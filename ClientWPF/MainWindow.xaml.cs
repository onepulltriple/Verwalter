using AnwendungWPF.ViewModels;
using System.Windows;

namespace AnwendungWPF
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _viewModel = mainViewModel;
            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await _viewModel.Addresses.LoadCommand.ExecuteAsync(null);
        }
    }
}
