using Application.Addresses.Commands;
using Application.Addresses.Models;
using Application.Addresses.Repositories;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public sealed class AddressWithPlaceDAC
        : IAddressWithPlaceRepository
    {
        private readonly VerwalterContext _context;

        public AddressWithPlaceDAC(VerwalterContext context)
        {
            _context = context;
        }

        public async Task<AddressDto> CreateAsync(
            CreateAddressWithPlaceCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var countryName = command.CountryName.Trim();
            var postcodeValue = command.Postcode.Trim();
            var cityName = command.CityName.Trim();

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var country = await FindCountryAsync(countryName);

            if (country is null)
            {
                country = new Country
                {
                    CountryId = Guid.NewGuid(),
                    NameEnGb = countryName,
                    Active = true
                };

                await _context.Countries.AddAsync(country);
                await _context.SaveChangesAsync();
            }

            var place = await FindPlaceAsync(
                country.CountryId,
                cityName,
                postcodeValue);

            if (place is null)
            {
                place = new Place
                {
                    PlaceId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    CityNameEnGb = cityName,
                    Postcode = postcodeValue,
                    NameEnGb = CreatePlaceName(
                        cityName,
                        postcodeValue,
                        countryName)
                };

                await _context.Places.AddAsync(place);
                await _context.SaveChangesAsync();
            }
            else if (string.IsNullOrWhiteSpace(place.NameEnGb))
            {
                place.NameEnGb = CreatePlaceName(
                    cityName,
                    postcodeValue,
                    countryName);

                await _context.SaveChangesAsync();
            }

            var address = new Address
            {
                AddressId = Guid.NewGuid(),
                PlaceId = place.PlaceId,
                AddressLine1 = command.AddressLine1.Trim(),
                AddressLine2 = NormalizeOptional(command.AddressLine2),
                AddressLine3 = NormalizeOptional(command.AddressLine3),
                Name1 = NormalizeOptional(command.Name1),
                Name2 = NormalizeOptional(command.Name2)
            };

            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new AddressDto
            {
                AddressId = address.AddressId,
                PlaceId = address.PlaceId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                Name1 = address.Name1,
                Name2 = address.Name2,
                ModifiedUtc = address.ModifiedUtc
            };
        }

        public async Task<AddressDto?> UpdateAsync(
            UpdateAddressWithPlaceCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var countryName = command.CountryName.Trim();
            var postcodeValue = command.Postcode.Trim();
            var cityName = command.CityName.Trim();

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var address = await _context.Addresses
                .SingleOrDefaultAsync(x => x.AddressId == command.AddressId);

            if (address is null)
            {
                return null;
            }

            var country = await FindCountryAsync(countryName);

            if (country is null)
            {
                country = new Country
                {
                    CountryId = Guid.NewGuid(),
                    NameEnGb = countryName,
                    Active = true
                };

                await _context.Countries.AddAsync(country);
                await _context.SaveChangesAsync();
            }

            var place = await FindPlaceAsync(
                country.CountryId,
                cityName,
                postcodeValue);

            if (place is null)
            {
                place = new Place
                {
                    PlaceId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    CityNameEnGb = cityName,
                    Postcode = postcodeValue,
                    NameEnGb = CreatePlaceName(
                        cityName,
                        postcodeValue,
                        countryName)
                };

                await _context.Places.AddAsync(place);
                await _context.SaveChangesAsync();
            }
            else if (string.IsNullOrWhiteSpace(place.NameEnGb))
            {
                place.NameEnGb = CreatePlaceName(
                    cityName,
                    postcodeValue,
                    countryName);

                await _context.SaveChangesAsync();
            }

            address.PlaceId = place.PlaceId;
            address.AddressLine1 = command.AddressLine1.Trim();
            address.AddressLine2 = NormalizeOptional(command.AddressLine2);
            address.AddressLine3 = NormalizeOptional(command.AddressLine3);
            address.Name1 = NormalizeOptional(command.Name1);
            address.Name2 = NormalizeOptional(command.Name2);
            address.ModifiedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new AddressDto
            {
                AddressId = address.AddressId,
                PlaceId = address.PlaceId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                Name1 = address.Name1,
                Name2 = address.Name2,
                ModifiedUtc = address.ModifiedUtc
            };
        }

        private async Task<Country?> FindCountryAsync(
            string countryName)
        {
            var countries = await _context.Countries
                .AsTracking()
                .ToListAsync();

            return countries.FirstOrDefault(country =>
                string.Equals(
                    country.NameEnGb.Trim(),
                    countryName,
                    StringComparison.OrdinalIgnoreCase));
        }
        
        private async Task<Place?> FindPlaceAsync(
            Guid countryId,
            string cityName,
            string postcode)
        {
            var places = await _context.Places
                .AsTracking()
                .Where(x => x.CountryId == countryId)
                .ToListAsync();

            return places.FirstOrDefault(place =>
                string.Equals(
                    place.CityNameEnGb.Trim(),
                    cityName,
                    StringComparison.OrdinalIgnoreCase) &&
                string.Equals(
                    place.Postcode.Trim(),
                    postcode,
                    StringComparison.OrdinalIgnoreCase));
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private static string CreatePlaceName(
            string cityName,
            string postcode,
            string countryName)
        {
            return $"{cityName}, {postcode} {countryName}";
        }
    }
}