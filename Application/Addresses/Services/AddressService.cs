using Application.Addresses.Commands;
using Application.Addresses.Models;
using Application.Addresses.Queries;
using Application.Addresses.Repositories;
using Application.Common;

namespace Application.Addresses.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IAddressWithPlaceRepository _addressWithPlaceRepository;

        public AddressService(
            IAddressRepository addressRepository,
            IAddressWithPlaceRepository addressWithPlaceRepository)
        {
            _addressRepository = addressRepository;
            _addressWithPlaceRepository = addressWithPlaceRepository;
        }

        public async Task<IReadOnlyList<AddressDto>> GetAllAsync()
        {
            return await _addressRepository.GetAllAsync();
        }

        public async Task<AddressDto?> GetByIdAsync(Guid addressId)
        {
            return await _addressRepository.GetByIdAsync(addressId);
        }

        public async Task<AddressDto> CreateAsync(AddressDto address)
        {
            await _addressRepository.AddAsync(address);

            return address;
        }

        public async Task<bool> UpdateAsync(AddressDto address)
        {
            var existing = await _addressRepository.GetByIdAsync(
                address.AddressId);

            if (existing is null)
            {
                return false;
            }

            await _addressRepository.UpdateAsync(address);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid addressId)
        {
            var existing = await _addressRepository.GetByIdAsync(addressId);

            if (existing is null)
            {
                return false;
            }

            await _addressRepository.DeleteAsync(addressId);

            return true;
        }

        public async Task<AddressDto> CreateWithPlaceAsync(
            CreateAddressWithPlaceCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.CountryName))
                throw new ArgumentException(
                    "Country name is required.",
                    nameof(command));

            if (string.IsNullOrWhiteSpace(command.Postcode))
                throw new ArgumentException(
                    "Postcode is required.",
                    nameof(command));

            if (string.IsNullOrWhiteSpace(command.CityName))
                throw new ArgumentException(
                    "City name is required.",
                    nameof(command));

            if (string.IsNullOrWhiteSpace(command.AddressLine1))
                throw new ArgumentException(
                    "Address line 1 is required.",
                    nameof(command));

            return await _addressWithPlaceRepository.CreateAsync(command);
        }

        public async Task<AddressDto?> UpdateWithPlaceAsync(
            UpdateAddressWithPlaceCommand command)
        {
            if (command.AddressId == Guid.Empty)
                throw new ArgumentException(
                    "Address ID is required.",
                    nameof(command));

            if (string.IsNullOrWhiteSpace(command.CountryName))
                throw new ArgumentException(
                    "Country name is required.",
                    nameof(command));

            if (string.IsNullOrWhiteSpace(command.Postcode))
                throw new ArgumentException(
                    "Postcode is required.",
                    nameof(command));

            if (string.IsNullOrWhiteSpace(command.CityName))
                throw new ArgumentException(
                    "City name is required.",
                    nameof(command));

            if (string.IsNullOrWhiteSpace(command.AddressLine1))
                throw new ArgumentException(
                    "Address line 1 is required.",
                    nameof(command));

            return await _addressWithPlaceRepository.UpdateAsync(command);
        }

        public async Task<PagedResult<AddressDto>> SearchAsync(
            AddressSearchCriteriaQuery criteria)
        {
            return await _addressRepository.SearchAsync(criteria);
        }
    }
}