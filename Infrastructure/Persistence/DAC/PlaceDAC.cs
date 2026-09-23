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
                NameEnGb = entity.NameEnGb,
                City = entity.City?.NameEnGb ?? string.Empty,
                Postcode = entity.Postcode?.Postcode1 ?? string.Empty,
                Country = entity.City?.Country?.NameEnGb ?? string.Empty
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

        public override async Task<List<PlaceDto>> GetAllAsync()
        {
            var entities = await QueryPlaces()
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }

        public override async Task<PlaceDto?> GetByIdAsync(Guid id)
        {
            var entity = await QueryPlaces()
                .SingleOrDefaultAsync(x => x.PlaceId == id);

            return entity is null
                ? null
                : MapToDto(entity);
        }

        public async Task<List<PlaceDto>> GetByPostcodeIdAsync(
            Guid postcodeId)
        {
            var entities = await QueryPlaces()
                .Where(x => x.PostcodeId == postcodeId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }

        public async Task<List<PlaceDto>> GetByCityIdAsync(
            Guid cityId)
        {
            var entities = await QueryPlaces()
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
            var entities = await QueryPlaces()
                .Where(x =>
                    x.PostcodeId == postcodeId &&
                    x.CityId == cityId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }

        private IQueryable<Place> QueryPlaces()
        {
            return _context.Set<Place>()
                .AsNoTracking()
                .Include(x => x.City)
                    .ThenInclude(x => x.Country)
                .Include(x => x.Postcode);
        }
    }
}