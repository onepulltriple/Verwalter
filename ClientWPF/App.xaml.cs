using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using System.Windows;
using AnwendungWPF.Clients;
using AnwendungWPF.ViewModels;
//using ClientWPF.Windows;

namespace AnwendungWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    // API clients
                    services.AddHttpClient<AddressApiClient>(client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:7171/");
                    });

                    // ViewModels
                    services.AddTransient<MainViewModel>();

                    // Windows
                    services.AddTransient<MainWindow>();
                })
                .Build();

            _host.Start();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }
}
