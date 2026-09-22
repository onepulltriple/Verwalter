using AnwendungWPF.Models;

namespace AnwendungWPF.Clients
{
    public interface ICountryApiClient
    {
        Task<IReadOnlyList<CountryModel>> GetAllAsync();

        Task<CountryModel?> GetByIdAsync(Guid countryId);

        Task<CountryModel> CreateAsync(
            string? isoAlpha2,
            string? isoAlpha3,
            string nameEnGb,
            bool active);

        Task UpdateAsync(
            Guid countryId,
            string? isoAlpha2,
            string? isoAlpha3,
            string nameEnGb,
            bool active);

        Task DeleteAsync(Guid countryId);

        Task<bool> HasReferencesAsync(Guid countryId);
    }
}