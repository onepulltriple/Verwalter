using Application.Common;

namespace Application.Cities
{
    public interface ICityRepository : IBaseRepository<CityDto>
    {
        Task<List<CityDto>> GetByCountryIdAsync(
            Guid countryId);
    }
}
