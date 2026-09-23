using AnwendungWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnwendungWPF.Windows
{
    /// <summary>
    /// Interaction logic for ManageAddressesWindow.xaml
    /// </summary>
    public partial class ManageAddressesWindow : Window
    {
        public ManageAddressesWindow(ManageAddressesViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel
                ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
