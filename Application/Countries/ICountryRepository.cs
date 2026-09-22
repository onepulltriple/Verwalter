using Application.Common;

namespace Application.Countries
{
    public interface ICountryRepository : IBaseRepository<CountryDto>
    {
        Task<bool> HasReferencesAsync(Guid countryId);

        //GetByIsoAlpha2Async()
        //GetByIsoAlpha3Async()
    }
}
