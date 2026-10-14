using Application.Countries.Models;
using Application.Countries.Repositories;

namespace Application.Countries.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IReadOnlyList<CountryDto>> GetAllAsync()
        {
            return await _countryRepository.GetAllAsync();
        }

        public async Task<CountryDto?> GetByIdAsync(Guid countryId)
        {
            return await _countryRepository.GetByIdAsync(countryId);
        }

        public async Task<CountryDto> CreateAsync(CountryDto country)
        {
            await _countryRepository.AddAsync(country);

            return country;
        }

        public async Task<bool> UpdateAsync(CountryDto country)
        {
            var existing = await _countryRepository.GetByIdAsync(
                country.CountryId);

            if (existing is null)
            {
                return false;
            }

            await _countryRepository.UpdateAsync(country);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid countryId)
        {
            var existing = await _countryRepository.GetByIdAsync(countryId);

            if (existing is null)
            {
                return false;
            }

            await _countryRepository.DeleteAsync(countryId);

            return true;
        }

        public async Task<bool> HasReferencesAsync(Guid countryId)
        {
            return await _countryRepository.HasReferencesAsync(countryId);
        }
    }
}