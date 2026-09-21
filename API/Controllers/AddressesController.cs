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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AddressDto>> GetById(Guid id)
        {
            var address = await _addressService.GetByIdAsync(id);

            if (address is null)
            {
                return NotFound();
            }

            return Ok(address);
        }
    }
}
