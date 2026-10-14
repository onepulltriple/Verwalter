using Infrastructure.Persistence.DAC;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.Addresses.Repositories;
using Application.AddressCategories.Repositories;
using Application.Authentication.Repositories;
using Application.Cities.Repositories;
using Application.Countries.Repositories;
using Application.Places.Repositories;
using Application.Postcodes.Repositories;

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
            services.AddScoped<IAddressWithPlaceRepository, AddressWithPlaceDAC>();

            return services;
        }
    }
}
