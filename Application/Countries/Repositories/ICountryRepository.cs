using Application.Common.Repositories;
using Application.Countries.Models;

namespace Application.Countries.Repositories
{
    public interface ICountryRepository : IBaseRepository<CountryDto>
    {
        Task<bool> HasReferencesAsync(Guid countryId);

        //GetByIsoAlpha2Async()
        //GetByIsoAlpha3Async()
    }
}
