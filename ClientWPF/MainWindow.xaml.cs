//using System.Windows;

//namespace ClientWPF
//{
//    /// <summary>
//    /// Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            InitializeComponent();
//        }
//    }
//}
using ClientWPF.ViewModels;
using System.Windows;

namespace ClientWPF
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

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAddressAsync();
        }
    }
}
