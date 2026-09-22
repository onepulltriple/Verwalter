using System.Windows;
using AnwendungWPF.ViewModels;

namespace AnwendungWPF.Windows
{
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
