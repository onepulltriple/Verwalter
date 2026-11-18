using FluentAssertions;
using Infrastructure;
using Infrastructure.Persistence.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Verwalter.Infrastructure.Tests
{
    public class AddressDbContextTests
    {
        [Test]
        public void AddInfrastructure_RegistersVerwalterContext()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddInfrastructure();

            using var provider = services.BuildServiceProvider();

            // Act
            var context = provider.GetService<VerwalterContext>();

            // Assert
            // should work even if no SQL server instance is running
            context.Should().NotBeNull();
        }

        [Test]
        public async Task VerwalterContext_CanConnectToDatabase()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddInfrastructure();

            await using var provider = services.BuildServiceProvider();
            await using var context =
                provider.GetRequiredService<VerwalterContext>();

            // Act
            var canConnect = await context.Database.CanConnectAsync();

            // Assert
            // requires an SQL server instance to be running
            canConnect.Should().BeTrue();
        }
    }
}
