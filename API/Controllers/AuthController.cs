using Microsoft.AspNetCore.Mvc;
using ApiContracts.Authentication;
using Application.Authentication.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApiContracts.Authentication.LoginRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            // Map API contract to application request
            var appRequest = new Application.Authentication.LoginRequest
            {
                Username = request.Username,
                Password = request.Password
            };

            var appResult = await _authenticationService.LoginAsync(appRequest);

            // Map application result to API contract
            var apiResult = new ApiContracts.Authentication.AuthenticationResponse
            {
                Success = appResult.Success,
                ErrorMessage = appResult.ErrorMessage,
                JwtToken = appResult.JwtToken,
                User = appResult.Success && appResult.AppUserId.HasValue
                    ? new ApiContracts.AppUsers.AppUserResponse
                    {
                        AppUserId = appResult.AppUserId.Value,
                        Username = appResult.Username ?? string.Empty,
                        EmailAddress = appResult.EmailAddress,
                        Active = appResult.Active,
                        LastLoginUtc = appResult.LastLoginUtc
                    }
                    : null
            };

            return appResult.Success
                ? Ok(apiResult)
                : Unauthorized(apiResult);
        }
    }
}
