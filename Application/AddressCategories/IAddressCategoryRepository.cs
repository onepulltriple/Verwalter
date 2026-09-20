using Application.Common;

namespace Application.AddressCategories
{
    public interface IAddressCategoryRepository
        : IBaseRepository<AddressCategoryDto>
    {
        Task<AddressCategoryDto?> GetByCodeAsync(
            string code);

        Task<List<AddressCategoryDto>> GetActiveAsync();
    }
}
