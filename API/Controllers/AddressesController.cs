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

            var response = new AddressResponse
            {
                AddressId = address.AddressId,
                PlaceId = address.PlaceId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                Name1 = address.Name1,
                Name2 = address.Name2
            };

            return Ok(response);
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
