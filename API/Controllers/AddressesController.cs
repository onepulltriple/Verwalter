using APIContracts.Addresses;
using Application.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AddressResponse>>> GetAll()
        {
            var addresses = await _addressService.GetAllAsync();

            var response = addresses
                .Select(Map)
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AddressResponse>> GetById(Guid id)
        {
            var address = await _addressService.GetByIdAsync(id);

            if (address is null)
            {
                return NotFound();
            }

            return Ok(Map(address));
        }

        [HttpPost]
        public async Task<ActionResult<AddressResponse>> Create(
            CreateAddressRequest request)
        {
            var address = new AddressDto
            {
                AddressId = Guid.NewGuid(),
                PlaceId = request.PlaceId,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                AddressLine3 = request.AddressLine3,
                Name1 = request.Name1,
                Name2 = request.Name2
            };

            var created = await _addressService.CreateAsync(address);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.AddressId },
                Map(created));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateAddressRequest request)
        {
            var address = new AddressDto
            {
                AddressId = id,
                PlaceId = request.PlaceId,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                AddressLine3 = request.AddressLine3,
                Name1 = request.Name1,
                Name2 = request.Name2
            };

            var updated = await _addressService.UpdateAsync(address);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _addressService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        private static AddressResponse Map(AddressDto address)
        {
            return new AddressResponse
            {
                AddressId = address.AddressId,
                PlaceId = address.PlaceId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                Name1 = address.Name1,
                Name2 = address.Name2
            };
        }
    }
}