using System.Windows;
using AnwendungWPF.ViewModels;

namespace AnwendungWPF.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel
                ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}