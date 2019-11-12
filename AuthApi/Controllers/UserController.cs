
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

        public UserController(IUserCreateService userCreateService)
        {
            _userCreateService = userCreateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserCreateRequest request)
        {
            try
            {
                var user = await _userCreateService.CreateUser(request.EmailAddress, request.Password);
                return Created(string.Empty, user.Id);
            }
            catch (DuplicateUserException)
            {
                return Conflict("Email address is already in use");
            }
        }
    }
}