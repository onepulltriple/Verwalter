using Application.Addresses;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DAC
{
    public sealed class AddressWithPlaceDAC
        : IAddressWithPlaceRepository
    {
        private readonly AddressDatabaseContext _context;

        public AddressWithPlaceDAC(AddressDatabaseContext context)
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

            var postcode = await FindPostcodeAsync(
                country.CountryId,
                postcodeValue);

            if (postcode is null)
            {
                postcode = new Postcode
                {
                    PostcodeId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    Postcode1 = postcodeValue
                };

                await _context.Postcodes.AddAsync(postcode);
                await _context.SaveChangesAsync();
            }

            var city = await FindCityAsync(
                country.CountryId,
                cityName);

            if (city is null)
            {
                city = new City
                {
                    CityId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    NameEnGb = cityName
                };

                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();
            }

            var place = await _context.Places
                .SingleOrDefaultAsync(x =>
                    x.CountryId == country.CountryId &&
                    x.PostcodeId == postcode.PostcodeId &&
                    x.CityId == city.CityId);

            if (place is null)
            {
                place = new Place
                {
                    PlaceId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    PostcodeId = postcode.PostcodeId,
                    CityId = city.CityId,
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

            var postcode = await FindPostcodeAsync(
                country.CountryId,
                postcodeValue);

            if (postcode is null)
            {
                postcode = new Postcode
                {
                    PostcodeId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    Postcode1 = postcodeValue
                };

                await _context.Postcodes.AddAsync(postcode);
                await _context.SaveChangesAsync();
            }

            var city = await FindCityAsync(
                country.CountryId,
                cityName);

            if (city is null)
            {
                city = new City
                {
                    CityId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    NameEnGb = cityName
                };

                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();
            }

            var place = await _context.Places
                .SingleOrDefaultAsync(x =>
                    x.CountryId == country.CountryId &&
                    x.PostcodeId == postcode.PostcodeId &&
                    x.CityId == city.CityId);

            if (place is null)
            {
                place = new Place
                {
                    PlaceId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    PostcodeId = postcode.PostcodeId,
                    CityId = city.CityId,
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

        private async Task<Postcode?> FindPostcodeAsync(
            Guid countryId,
            string postcode)
        {
            var postcodes = await _context.Postcodes
                .AsTracking()
                .Where(x => x.CountryId == countryId)
                .ToListAsync();

            return postcodes.FirstOrDefault(existingPostcode =>
                string.Equals(
                    existingPostcode.Postcode1.Trim(),
                    postcode,
                    StringComparison.OrdinalIgnoreCase));
        }

        private async Task<City?> FindCityAsync(
            Guid countryId,
            string cityName)
        {
            var cities = await _context.Cities
                .AsTracking()
                .Where(x => x.CountryId == countryId)
                .ToListAsync();

            return cities.FirstOrDefault(city =>
                string.Equals(
                    city.NameEnGb.Trim(),
                    cityName,
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