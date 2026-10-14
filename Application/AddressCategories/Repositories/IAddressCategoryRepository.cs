using Application.AddressCategories.Models;
using Application.Common.Repositories;

namespace Application.AddressCategories.Repositories
{
    public interface IAddressCategoryRepository
        : IBaseRepository<AddressCategoryDto>
    {
        Task<AddressCategoryDto?> GetByCodeAsync(
            string code);

        Task<List<AddressCategoryDto>> GetActiveAsync();
    }
}
