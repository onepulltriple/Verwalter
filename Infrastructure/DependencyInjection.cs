using Application.AddressCategories;
using Application.Addresses;
using Application.Cities;
using Application.Countries;
using Application.Places;
using Application.Postcodes;
using Application.Authentication;
using Infrastructure.Persistence.DAC;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services
            )
        {
            services.AddDbContext<AddressDatabaseContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=.\SQLEXPRESS;Database=AddressDatabase;Integrated Security=True;TrustServerCertificate=True");
            });

            services.AddScoped<IAddressRepository, AddressDAC>();
            services.AddScoped<ICountryRepository, CountryDAC>();
            services.AddScoped<IPostcodeRepository, PostcodeDAC>();
            services.AddScoped<ICityRepository, CityDAC>();
            services.AddScoped<IPlaceRepository, PlaceDAC>();
            services.AddScoped<IAppUserRepository, AppUserDAC>();
            services.AddScoped<IAddressCategoryRepository, AddressCategoryDAC>();

            return services;
        }
    }
}
