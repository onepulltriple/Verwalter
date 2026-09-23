using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Addresses;
using Application.Common;

namespace Infrastructure.Persistence.DAC
{
    public class AddressDAC : BaseDAC<Address, AddressDto>, IAddressRepository
    {
        public AddressDAC(AddressDatabaseContext context)
            : base(context)
        {
        }

        protected override AddressDto MapToDto(Address entity)
        {
            return new AddressDto
            {
                AddressId = entity.AddressId,
                PlaceId = entity.PlaceId,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                AddressLine3 = entity.AddressLine3,
                Name1 = entity.Name1,
                Name2 = entity.Name2,
                ModifiedUtc = entity.ModifiedUtc
            };
        }

        protected override Address MapToEntity(AddressDto dto)
        {
            return new Address
            {
                AddressId = dto.AddressId,
                PlaceId = dto.PlaceId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                AddressLine3 = dto.AddressLine3,
                Name1 = dto.Name1,
                Name2 = dto.Name2
            };
        }

        protected override void UpdateEntity(
            Address entity,
            AddressDto dto)
        {
            entity.PlaceId = dto.PlaceId;
            entity.AddressLine1 = dto.AddressLine1;
            entity.AddressLine2 = dto.AddressLine2;
            entity.AddressLine3 = dto.AddressLine3;
            entity.Name1 = dto.Name1;
            entity.Name2 = dto.Name2;
            entity.ModifiedUtc = DateTime.UtcNow;
        }

        protected override Guid GetEntityId(AddressDto dto)
        {
            return dto.AddressId;
        }

        public async Task<PagedResult<AddressDto>> SearchAsync(
            AddressSearchCriteria criteria)
        {
            IQueryable<Address> query = _context.Set<Address>()
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.AddressLine1))
            {
                query = query.Where(address =>
                    address.AddressLine1.Contains(
                        criteria.AddressLine1));
            }

            if (!string.IsNullOrWhiteSpace(criteria.AddressLine2))
            {
                query = query.Where(address =>
                    address.AddressLine2 != null &&
                    address.AddressLine2.Contains(
                        criteria.AddressLine2));
            }

            if (!string.IsNullOrWhiteSpace(criteria.AddressLine3))
            {
                query = query.Where(address =>
                    address.AddressLine3 != null &&
                    address.AddressLine3.Contains(
                        criteria.AddressLine3));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Name1))
            {
                query = query.Where(address =>
                    address.Name1 != null &&
                    address.Name1.Contains(criteria.Name1));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Name2))
            {
                query = query.Where(address =>
                    address.Name2 != null &&
                    address.Name2.Contains(criteria.Name2));
            }

            var requiresPlaceJoin =
                !string.IsNullOrWhiteSpace(criteria.City) ||
                !string.IsNullOrWhiteSpace(criteria.Postcode) ||
                !string.IsNullOrWhiteSpace(criteria.Country) ||
                criteria.CountryId.HasValue ||
                criteria.PostcodeId.HasValue ||
                criteria.CityId.HasValue;

            if (requiresPlaceJoin)
            {
                query =
                    from address in query
                    join place in _context.Set<Place>()
                        on address.PlaceId equals place.PlaceId
                    join city in _context.Set<City>()
                        on place.CityId equals city.CityId
                    join country in _context.Set<Country>()
                        on place.CountryId equals country.CountryId
                    join postcode in _context.Set<Postcode>()
                        on place.PostcodeId equals postcode.PostcodeId
                    where
                        (!string.IsNullOrWhiteSpace(criteria.City) &&
                         city.NameEnGb.Contains(criteria.City) ||
                         string.IsNullOrWhiteSpace(criteria.City))
                        &&
                        (!string.IsNullOrWhiteSpace(criteria.Postcode) &&
                         postcode.Postcode1.Contains(criteria.Postcode) ||
                         string.IsNullOrWhiteSpace(criteria.Postcode))
                        &&
                        (!string.IsNullOrWhiteSpace(criteria.Country) &&
                         country.NameEnGb.Contains(criteria.Country) ||
                         string.IsNullOrWhiteSpace(criteria.Country))
                        &&
                        (!criteria.CountryId.HasValue ||
                         place.CountryId == criteria.CountryId.Value)
                        &&
                        (!criteria.PostcodeId.HasValue ||
                         place.PostcodeId == criteria.PostcodeId.Value)
                        &&
                        (!criteria.CityId.HasValue ||
                         place.CityId == criteria.CityId.Value)
                    select address;
            }

            if (criteria.PlaceId.HasValue)
            {
                query = query.Where(address =>
                    address.PlaceId == criteria.PlaceId.Value);
            }

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, criteria);

            var pageNumber = Math.Max(1, criteria.PageNumber);

            var pageSize = Math.Clamp(
                criteria.PageSize,
                1,
                500);

            var entities = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<AddressDto>
            {
                Items = entities
                    .Select(MapToDto)
                    .ToList(),

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<List<AddressDto>> GetByPlaceIdAsync(
            Guid placeId)
        {
            var entities = await _context.Set<Address>()
                .AsNoTracking()
                .Where(x => x.PlaceId == placeId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }

        private static IQueryable<Address> ApplySorting(
            IQueryable<Address> query,
            AddressSearchCriteria criteria)
        {
            if (string.Equals(
                    criteria.SortBy,
                    "AddressLine1",
                    StringComparison.OrdinalIgnoreCase))
            {
                return criteria.SortDescending
                    ? query.OrderByDescending(x => x.AddressLine1)
                    : query.OrderBy(x => x.AddressLine1);
            }

            if (string.Equals(
                    criteria.SortBy,
                    "Name1",
                    StringComparison.OrdinalIgnoreCase))
            {
                return criteria.SortDescending
                    ? query.OrderByDescending(x => x.Name1)
                    : query.OrderBy(x => x.Name1);
            }

            return criteria.SortDescending
                ? query.OrderByDescending(x => x.AddressId)
                : query.OrderBy(x => x.AddressId);
        }
    }
}
