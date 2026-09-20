using Microsoft.EntityFrameworkCore;
using Application.Postcodes;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.DAC
{
    public class PostcodeDAC
        : BaseDAC<Postcode, PostcodeDto>, IPostcodeRepository
    {
        public PostcodeDAC(AddressDatabaseContext context)
            : base(context)
        {
        }

        protected override PostcodeDto MapToDto(Postcode entity)
        {
            return new PostcodeDto
            {
                PostcodeId = entity.PostcodeId,
                CountryId = entity.CountryId,
                Postcode = entity.Postcode1
            };
        }

        protected override Postcode MapToEntity(PostcodeDto dto)
        {
            return new Postcode
            {
                PostcodeId = dto.PostcodeId,
                CountryId = dto.CountryId,
                Postcode1 = dto.Postcode
            };
        }

        protected override void UpdateEntity(
            Postcode entity,
            PostcodeDto dto)
        {
            entity.CountryId = dto.CountryId;
            entity.Postcode1 = dto.Postcode;
        }

        protected override Guid GetEntityId(PostcodeDto dto)
        {
            return dto.PostcodeId;
        }

        public async Task<List<PostcodeDto>> GetByCountryIdAsync(
            Guid countryId)
        {
            var entities = await _context.Set<Postcode>()
                .AsNoTracking()
                .Where(x => x.CountryId == countryId)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }
    }
}
