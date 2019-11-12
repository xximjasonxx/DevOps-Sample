
using System.Threading.Tasks;
using AuthApi.Models;
using AuthApi.Providers;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/v1/login")]
    public class LoginController : Controller
    {
        private readonly IGetUserProvider _getUserProvider;

        public LoginController(IGetUserProvider getUserProvider)
        {
            _getUserProvider = getUserProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = await _getUserProvider.GetUserByAuthenticationCredentials(request.EmailAddress, request.Password);
            if (user == null)
            {
                return Unauthorized("Credentials were not valid");
            }

            return Ok(user);
        }
    }
}