using ApiContracts.Countries;
using Application.Countries.Models;
using Application.Countries.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CountryResponse>>> GetAll()
        {
            var countries = await _countryService.GetAllAsync();

            var response = countries
                .Select(Map)
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CountryResponse>> GetById(Guid id)
        {
            var country = await _countryService.GetByIdAsync(id);

            if (country is null)
            {
                return NotFound();
            }

            return Ok(Map(country));
        }

        [HttpPost]
        public async Task<ActionResult<CountryResponse>> Create(
            CreateCountryRequest request)
        {
            var country = new CountryDto
            {
                CountryId = Guid.NewGuid(),
                IsoAlpha2 = request.IsoAlpha2,
                IsoAlpha3 = request.IsoAlpha3,
                NameEnGb = request.NameEnGb,
                Active = request.Active
            };

            var created = await _countryService.CreateAsync(country);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.CountryId },
                Map(created));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateCountryRequest request)
        {
            var country = new CountryDto
            {
                CountryId = id,
                IsoAlpha2 = request.IsoAlpha2,
                IsoAlpha3 = request.IsoAlpha3,
                NameEnGb = request.NameEnGb,
                Active = request.Active
            };

            var updated = await _countryService.UpdateAsync(country);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _countryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id:guid}/references")]
        public async Task<ActionResult<CountryReferencesResponse>> HasReferences(
            Guid id)
        {
            var hasReferences =
                await _countryService.HasReferencesAsync(id);

            return Ok(new CountryReferencesResponse
            {
                HasReferences = hasReferences
            });
        }

        private static CountryResponse Map(CountryDto country)
        {
            return new CountryResponse
            {
                CountryId = country.CountryId,
                IsoAlpha2 = country.IsoAlpha2,
                IsoAlpha3 = country.IsoAlpha3,
                NameEnGb = country.NameEnGb,
                Active = country.Active
            };
        }
    }
}