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

        protected override async void OnStartup(StartupEventArgs e)
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
                    services.AddHttpClient<IPlaceApiClient, PlaceApiClient>();
                    services.AddHttpClient<IAppUserApiClient, AppUserApiClient>();


                    // ViewModels and Windows
                    services.AddTransient<AppUserLoginViewModel>();
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<ManageCountriesViewModel>();
                    services.AddTransient<ManageAddressesViewModel>();

                    services.AddTransient<AppUserLoginWindow>();
                    services.AddTransient<MainWindow>();
                    services.AddTransient<ManageCountriesWindow>();
                    services.AddTransient<ManageAddressesWindow>();


                })
                .Build();

            _host.Start();

            var loginWindow =
                _host.Services.GetRequiredService<AppUserLoginWindow>();

            loginWindow.Show();
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
