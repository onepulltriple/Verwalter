using Application.AddressCategories;
using Application.Addresses;
using Application.Authentication;
using Application.Cities;
using Application.Countries;
using Application.Places;
using Application.Postcodes;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}