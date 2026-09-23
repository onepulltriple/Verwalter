using System.Windows;
using AnwendungWPF.ViewModels;

namespace AnwendungWPF.Windows
{
    /// <summary>
    /// Interaction logic for AppUserLoginWindow.xaml
    /// </summary>
    public partial class AppUserLoginWindow : Window
    {
        private readonly AppUserLoginViewModel _viewModel;
        public AppUserLoginWindow(AppUserLoginViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel
                ?? throw new ArgumentNullException(nameof(viewModel));

            this.DataContext = _viewModel;

            _viewModel.LoginSucceeded += ViewModel_LoginSucceeded;

            this.Loaded += (_, _) =>
            {
                MinWidth = ActualWidth;
                MinHeight = ActualHeight;
            };
        }

        private void ViewModel_LoginSucceeded(object? sender, EventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.LoginSucceeded -= ViewModel_LoginSucceeded;
            base.OnClosed(e);
        }
    }
}
