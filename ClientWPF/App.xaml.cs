using AnwendungWPF.Clients;
using AnwendungWPF.ViewModels;
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
                .ConfigureServices((_, services) =>
                {
                    services.AddHttpClient<IAddressApiClient, AddressApiClient>(client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:7171/");
                    });

                    services.AddTransient<MainViewModel>();
                    services.AddTransient<AddressListViewModel>();

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
