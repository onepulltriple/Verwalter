using Application.AddressCategories;
using Application.Addresses.Services;
using Application.Authentication.Services;
using Application.Cities;
using Application.Countries.Services;
using Application.Places.Services;
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
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}