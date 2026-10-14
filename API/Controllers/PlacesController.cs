using ApiContracts.Places;
using Application.Places.Models;
using Application.Places.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/places")]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PlaceResponse>>> GetAll()
        {
            var places = await _placeService.GetAllAsync();

            var response = places
                .Select(Map)
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PlaceResponse>> GetById(Guid id)
        {
            var place = await _placeService.GetByIdAsync(id);

            if (place is null)
            {
                return NotFound();
            }

            return Ok(Map(place));
        }

        [HttpPost]
        public async Task<ActionResult<PlaceResponse>> Create(
            CreatePlaceRequest request)
        {
            var place = new PlaceDto
            {
                PlaceId = Guid.NewGuid(),
                CountryId = request.CountryId,
                PostcodeId = request.PostcodeId,
                CityId = request.CityId,
                NameEnGb = request.NameEnGb
            };

            var created = await _placeService.CreateAsync(place);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.PlaceId },
                Map(created));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdatePlaceRequest request)
        {
            var place = new PlaceDto
            {
                PlaceId = id,
                CountryId = request.CountryId,
                PostcodeId = request.PostcodeId,
                CityId = request.CityId,
                NameEnGb = request.NameEnGb
            };

            var updated = await _placeService.UpdateAsync(place);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _placeService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        private static PlaceResponse Map(PlaceDto place)
        {
            return new PlaceResponse
            {
                PlaceId = place.PlaceId,
                CountryId = place.CountryId,
                PostcodeId = place.PostcodeId,
                CityId = place.CityId,
                NameEnGb = place.NameEnGb,
                City = place.City,
                Postcode = place.Postcode,
                Country = place.Country
            };
        }
    }
}