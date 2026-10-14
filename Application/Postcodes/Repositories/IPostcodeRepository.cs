using Application.Common.Repositories;
using Application.Postcodes.Models;

namespace Application.Postcodes.Repositories
{
    public interface IPostcodeRepository : IBaseRepository<PostcodeDto>
    {
        Task<List<PostcodeDto>> GetByCountryIdAsync(
            Guid countryId);
    }
}
