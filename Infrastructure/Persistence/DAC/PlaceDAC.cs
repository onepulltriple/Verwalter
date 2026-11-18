using Application.Places.Models;
using Application.Places.Repositories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public class PlaceDAC
        : BaseDAC<Place, PlaceDto>, IPlaceRepository
    {
        public PlaceDAC(VerwalterContext context)
            : base(context)
        {
        }

        protected override PlaceDto MapToDto(Place entity)
        {
            return new PlaceDto
            {
                PlaceId = entity.PlaceId,
                CountryId = entity.CountryId,
                NameEnGb = entity.NameEnGb,
                City = entity.CityNameEnGb,
                Postcode = entity.Postcode,
                Country = entity.Country?.NameEnGb ?? string.Empty
            };
        }

        protected override Place MapToEntity(PlaceDto dto)
        {
            return new Place
            {
                PlaceId = dto.PlaceId,
                CountryId = dto.CountryId,
                CityNameEnGb = dto.City,
                Postcode = dto.Postcode,
                NameEnGb = dto.NameEnGb
            };
        }

        protected override void UpdateEntity(
            Place entity,
            PlaceDto dto)
        {
            entity.CountryId = dto.CountryId;
            entity.CityNameEnGb = dto.City;
            entity.Postcode = dto.Postcode;
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

        private IQueryable<Place> QueryPlaces()
        {
            return _context.Set<Place>()
                .AsNoTracking()
                .Include(x => x.Country);
        }
    }
}