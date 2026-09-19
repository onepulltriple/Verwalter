using FluentAssertions;
using Infrastructure;
using Infrastructure.Persistence.Entities;
using Microsoft.Extensions.DependencyInjection;


namespace Verwalter.Infrastructure.Tests
{
    public class AddressDbContextTests
    {
        [Test]
        public async Task CanConnectToDatabase()
        {
            // arrange
            var services = new ServiceCollection();

            services.AddInfrastructure();

            await using var provider = services.BuildServiceProvider();
            await using var context = provider.GetRequiredService<AddressDatabaseContext>();

            // act
            var canConnect = await context.Database.CanConnectAsync();

            // assert
            canConnect.Should().BeTrue();
        }
    }
}

