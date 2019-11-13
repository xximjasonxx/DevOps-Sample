
using System.Threading.Tasks;
using AuthApi.Ex;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : Controller
    {
        private readonly IUserCreateService _userCreateService;
        private readonly ICreateTokenService _createTokenService;

        public UserController(IUserCreateService userCreateService, ICreateTokenService createTokenService)
        {
            _userCreateService = userCreateService;
            _createTokenService = createTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserCreateRequest request)
        {
            try
            {
                var user = await _userCreateService.CreateUser(request.EmailAddress, request.Password);
                var webToken = _createTokenService.CreateToken(user);

                return Created(string.Empty, webToken);
            }
            catch (DuplicateUserException)
            {
                return Conflict("Email address is already in use");
            }
        }
    }
}