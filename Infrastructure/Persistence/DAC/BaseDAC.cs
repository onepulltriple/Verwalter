using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Common;

namespace Infrastructure.Persistence.DAC
{
    public abstract class BaseDAC<TEntity, TDto> : IBaseRepository<TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly AddressDatabaseContext _context;

        protected BaseDAC(AddressDatabaseContext context)
        {
            _context = context;
        }

        protected abstract TDto MapToDto(TEntity entity);

        protected abstract TEntity MapToEntity(TDto dto);

        protected abstract void UpdateEntity(TEntity entity, TDto dto);

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            var entities = await _context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

            return entities
                .Select(MapToDto)
                .ToList();
        }

        public virtual async Task<TDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<TEntity>()
                .FindAsync(id);

            return entity is null
                ? null
                : MapToDto(entity);
        }

        public virtual async Task AddAsync(TDto dto)
        {
            var entity = MapToEntity(dto);

            await _context.Set<TEntity>()
                .AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TDto dto)
        {
            var id = GetEntityId(dto);

            var entity = await _context.Set<TEntity>()
                .FindAsync(id);

            if (entity is null)
            {
                throw new KeyNotFoundException(
                    $"{typeof(TEntity).Name} with ID {id} was not found.");
            }

            UpdateEntity(entity, dto);

            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Set<TEntity>()
                .FindAsync(id);

            if (entity is null)
            {
                return;
            }

            _context.Set<TEntity>()
                .Remove(entity);

            await _context.SaveChangesAsync();
        }

        protected abstract Guid GetEntityId(TDto dto);
    }
}
