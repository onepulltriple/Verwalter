using Application.AddressCategories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public class AddressCategoryDAC
        : BaseDAC<AddressCategory, AddressCategoryDto>,
          IAddressCategoryRepository
    {
        public AddressCategoryDAC(AddressDatabaseContext context)
            : base(context)
        {
        }

        protected override AddressCategoryDto MapToDto(
            AddressCategory entity)
        {
            return new AddressCategoryDto
            {
                AddressCategoryId = entity.AddressCategoryId,
                Code = entity.Code,
                NameEnGb = entity.NameEnGb,
                Description = entity.Description,
                Active = entity.Active
            };
        }

        protected override AddressCategory MapToEntity(
            AddressCategoryDto dto)
        {
            return new AddressCategory
            {
                AddressCategoryId = dto.AddressCategoryId,
                Code = dto.Code,
                NameEnGb = dto.NameEnGb,
                Description = dto.Description,
                Active = dto.Active
            };
        }

        protected override void UpdateEntity(
            AddressCategory entity,
            AddressCategoryDto dto)
        {
            entity.Code = dto.Code;
            entity.NameEnGb = dto.NameEnGb;
            entity.Description = dto.Description;
            entity.Active = dto.Active;
        }

        protected override Guid GetEntityId(
            AddressCategoryDto dto)
        {
            return dto.AddressCategoryId;
        }

        public async Task<AddressCategoryDto?> GetByCodeAsync(
            string code)
        {
            var entity = await _context.Set<AddressCategory>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Code == code);

            return entity is null
                ? null
                : MapToDto(entity);
        }

        public async Task<List<AddressCategoryDto>> GetActiveAsync()
        {
            var entities = await _context.Set<AddressCategory>()
                .AsNoTracking()
                .Where(x => x.Active)
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }
    }
}
