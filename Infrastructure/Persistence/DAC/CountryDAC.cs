using Application.Countries.Models;
using Application.Countries.Repositories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public class CountryDAC
        : BaseDAC<Country, CountryDto>, ICountryRepository
    {
        public CountryDAC(VerwalterContext context)
            : base(context)
        {
        }

        protected override CountryDto MapToDto(Country entity)
        {
            return new CountryDto
            {
                CountryId = entity.CountryId,
                IsoAlpha2 = entity.IsoAlpha2,
                IsoAlpha3 = entity.IsoAlpha3,
                NameEnGb = entity.NameEnGb,
                Active = entity.Active
            };
        }

        protected override Country MapToEntity(CountryDto dto)
        {
            return new Country
            {
                CountryId = dto.CountryId,
                IsoAlpha2 = dto.IsoAlpha2,
                IsoAlpha3 = dto.IsoAlpha3,
                NameEnGb = dto.NameEnGb,
                Active = dto.Active
            };
        }

        protected override void UpdateEntity(
            Country entity,
            CountryDto dto)
        {
            entity.IsoAlpha2 = dto.IsoAlpha2;
            entity.IsoAlpha3 = dto.IsoAlpha3;
            entity.NameEnGb = dto.NameEnGb;
            entity.Active = dto.Active;
        }

        protected override Guid GetEntityId(CountryDto dto)
        {
            return dto.CountryId;
        }

        public async Task<bool> HasReferencesAsync(Guid countryId)
        {
            return await _context.Places
                .AnyAsync(p => p.CountryId == countryId);
        }
    }
}
