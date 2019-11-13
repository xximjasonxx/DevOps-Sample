
using System.Threading.Tasks;
using AuthApi.Models;
using AuthApi.Providers;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/v1/login")]
    public class LoginController : Controller
    {
        private readonly IGetUserProvider _getUserProvider;
        private readonly ICreateTokenService _createTokenService;

        public LoginController(IGetUserProvider getUserProvider, ICreateTokenService createTokenService)
        {
            _getUserProvider = getUserProvider;
            _createTokenService = createTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = await _getUserProvider.GetUserByAuthenticationCredentials(request.EmailAddress, request.Password);
            if (user == null)
            {
                return Unauthorized("Credentials were not valid");
            }

            var webToken = _createTokenService.CreateToken(user);
            return Ok(webToken);
        }
    }
}