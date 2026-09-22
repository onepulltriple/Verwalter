namespace Application.Countries
{
    public interface ICountryService
    {
        Task<IReadOnlyList<CountryDto>> GetAllAsync();

        Task<CountryDto?> GetByIdAsync(Guid countryId);

        Task<CountryDto> CreateAsync(CountryDto country);

        Task<bool> UpdateAsync(CountryDto country);

        Task<bool> DeleteAsync(Guid countryId);

        Task<bool> HasReferencesAsync(Guid countryId);
    }
}