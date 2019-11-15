
using System.Threading.Tasks;
using AuthApi.Models;
using AuthApi.Providers;
using AuthApi.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/v1/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IGetUserProvider _getUserProvider;
        private readonly ICreateTokenService _createTokenService;
        private readonly ITelemetryService _telemetryService;

        public LoginController(IGetUserProvider getUserProvider, ICreateTokenService createTokenService,
            ITelemetryService telemetryService)
        {
            _getUserProvider = getUserProvider;
            _createTokenService = createTokenService;
            _telemetryService = telemetryService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = await _getUserProvider.GetUserByAuthenticationCredentials(request.EmailAddress, request.Password);
            if (user == null)
                {_telemetryService.TrackEvent("Login Failed");
                return Unauthorized("Credentials were not valid");
            }

            _telemetryService.TrackEvent("Login Successful");
            var webToken = _createTokenService.CreateToken(user);
            return Accepted(string.Empty, webToken);
        }
    }
}