using FluentAssertions;
using Infrastructure;
using Infrastructure.Persistence.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Verwalter.Infrastructure.Tests
{
    public class AddressDbContextTests
    {
        [Test]
        public void AddInfrastructure_RegistersAddressDatabaseContext()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddInfrastructure();

            using var provider = services.BuildServiceProvider();

            // Act
            var context = provider.GetService<AddressDatabaseContext>();

            // Assert
            // should work even if no SQL server instance is running
            context.Should().NotBeNull();
        }

        [Test]
        public async Task AddressDatabaseContext_CanConnectToDatabase()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddInfrastructure();

            await using var provider = services.BuildServiceProvider();
            await using var context =
                provider.GetRequiredService<AddressDatabaseContext>();

            // Act
            var canConnect = await context.Database.CanConnectAsync();

            // Assert
            // requires an SQL server instance to be running
            canConnect.Should().BeTrue();
        }
    }
}
