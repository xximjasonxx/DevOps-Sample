
using System.Threading.Tasks;
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
            var user = await _userCreateService.CreateUser(request.EmailAddress, request.Password);
            return Created(string.Empty, string.Empty);
        }
    }
}