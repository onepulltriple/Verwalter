using System.Windows;
using AnwendungWPF.ViewModels;

namespace AnwendungWPF.Windows
{
    /// <summary>
    /// Interaction logic for ManageCountriesWindow.xaml
    /// </summary>
    public partial class ManageCountriesWindow : Window
    {
        public ManageCountriesWindow(ManageCountriesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel
                ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
