using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddDbContext<AddressDbContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=.\SQLEXPRESS;Database=AddressDatabase;Integrated Security=True;TrustServerCertificate=True");
            });

            return services;
        }
    }
}
