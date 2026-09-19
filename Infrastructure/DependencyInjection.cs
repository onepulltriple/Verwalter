using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Entities;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddDbContext<AddressDatabaseContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=.\SQLEXPRESS;Database=AddressDatabase;Integrated Security=True;TrustServerCertificate=True");
            });

            return services;
        }
    }
}
