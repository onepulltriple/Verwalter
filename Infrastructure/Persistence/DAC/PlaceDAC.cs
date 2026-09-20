using Application.Places;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public class PlaceDAC
        : BaseDAC<Place, PlaceDto>, IPlaceRepository
    {
        public PlaceDAC(AddressDatabaseContext context)
            : base(context)
        {
        }

        protected override PlaceDto MapToDto(Place entity)
        {
            return new PlaceDto
            {
                PlaceId = entity.PlaceId,
                CountryId = entity.CountryId,
                PostcodeId = entity.PostcodeId,
                CityId = entity.CityId,
                NameEnGb = entity.NameEnGb
            };
        }

        protected override Place MapToEntity(PlaceDto dto)
        {
            return new Place
            {
                PlaceId = dto.PlaceId,
                CountryId = dto.CountryId,
                PostcodeId = dto.PostcodeId,
                CityId = dto.CityId,
                NameEnGb = dto.NameEnGb
            };
        }

        protected override void UpdateEntity(
            Place entity,
            PlaceDto dto)
        {
            entity.CountryId = dto.CountryId;
            entity.PostcodeId = dto.PostcodeId;
            entity.CityId = dto.CityId;
            entity.NameEnGb = dto.NameEnGb;
        }

        protected override Guid GetEntityId(PlaceDto dto)
        {
            return dto.PlaceId;
        }

        public async Task<List<PlaceDto>> GetByPostcodeIdAsync(
            Guid postcodeId)
        {
            var entities = await _context.Set<Place>()
                .AsNoTracking()
                .Where(x => x.PostcodeId == postcodeId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }

        public async Task<List<PlaceDto>> GetByCityIdAsync(
            Guid cityId)
        {
            var entities = await _context.Set<Place>()
                .AsNoTracking()
                .Where(x => x.CityId == cityId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }

        public async Task<List<PlaceDto>> GetByPostcodeAndCityAsync(
            Guid postcodeId,
            Guid cityId)
        {
            var entities = await _context.Set<Place>()
                .AsNoTracking()
                .Where(x =>
                    x.PostcodeId == postcodeId &&
                    x.CityId == cityId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }
    }
}
