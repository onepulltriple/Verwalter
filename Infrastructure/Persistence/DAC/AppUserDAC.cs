using Application.Authentication.Repositories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public sealed class AppUserDAC : IAppUserRepository
    {
        private readonly AddressDatabaseContext _context;

        public AppUserDAC(AddressDatabaseContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<AppUserAuthenticationData?> GetByUsernameAsync(
            string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            var entity = await _context.AppUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Username == username);

            if (entity is null)
            {
                return null;
            }

            return new AppUserAuthenticationData
            {
                AppUserId = entity.AppUserId,
                Username = entity.Username,
                EmailAddress = entity.EmailAddress,
                PasswordHash = entity.PasswordHash,
                Active = entity.Active,
                LastLoginUtc = entity.LastLoginUtc
            };
        }
    }
}