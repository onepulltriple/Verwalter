using Application.Cities.Models;
using Application.Cities.Repositories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public class CityDAC
        : BaseDAC<City, CityDto>, ICityRepository
    {
        public CityDAC(AddressDatabaseContext context)
            : base(context)
        {
        }

        protected override CityDto MapToDto(City entity)
        {
            return new CityDto
            {
                CityId = entity.CityId,
                CountryId = entity.CountryId,
                NameEnGb = entity.NameEnGb
            };
        }

        protected override City MapToEntity(CityDto dto)
        {
            return new City
            {
                CityId = dto.CityId,
                CountryId = dto.CountryId,
                NameEnGb = dto.NameEnGb
            };
        }

        protected override void UpdateEntity(
            City entity,
            CityDto dto)
        {
            entity.CountryId = dto.CountryId;
            entity.NameEnGb = dto.NameEnGb;
        }

        protected override Guid GetEntityId(CityDto dto)
        {
            return dto.CityId;
        }

        public async Task<List<CityDto>> GetByCountryIdAsync(
            Guid countryId)
        {
            var entities = await _context.Set<City>()
                .AsNoTracking()
                .Where(x => x.CountryId == countryId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }
    }
}
