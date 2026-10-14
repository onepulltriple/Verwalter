using Application.Cities.Models;
using Application.Common.Repositories;

namespace Application.Cities.Repositories
{
    public interface ICityRepository : IBaseRepository<CityDto>
    {
        Task<List<CityDto>> GetByCountryIdAsync(
            Guid countryId);
    }
}
