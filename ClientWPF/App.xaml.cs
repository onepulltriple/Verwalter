using AnwendungWPF.Clients;
using AnwendungWPF.Configuration;
using AnwendungWPF.ViewModels;
using AnwendungWPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace AnwendungWPF
{
    public partial class App : System.Windows.Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // API Clients
                    services.Configure<ApiSettings>(
                        context.Configuration.GetSection("ApiSettings")
                    );

                    services.AddHttpClient<IAddressApiClient, AddressApiClient>();
                    services.AddHttpClient<ICountryApiClient, CountryApiClient>();
                    
                    // ViewModels and Windows
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<ManageCountriesViewModel>();
                    services.AddTransient<AddressListViewModel>();

                    services.AddTransient<MainWindow>();
                    services.AddTransient<ManageCountriesWindow>();


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
