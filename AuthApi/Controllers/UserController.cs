
using System.Threading.Tasks;
using AuthApi.Ex;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : Controller
    {
        private readonly IUserCreateService _userCreateService;
        private readonly ICreateTokenService _createTokenService;
        private readonly ITelemetryService _telemetryService;
        private readonly IPublishEventService _publishEventService;

        public UserController(IUserCreateService userCreateService, ICreateTokenService createTokenService,
            ITelemetryService telemetryService, IPublishEventService publishEventService)
        {
            _userCreateService = userCreateService;
            _createTokenService = createTokenService;
            _telemetryService = telemetryService;
            _publishEventService = publishEventService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserCreateRequest request)
        {
            try
            {
                var user = await _userCreateService.CreateUser(request.EmailAddress, request.Password, request.Username);
                var webToken = _createTokenService.CreateToken(user);

                _telemetryService.TrackEvent("User Created");
                await _publishEventService.PublishUserCreateEventAsync(new UserCreatedEvent
                {
                    UserId = user.Id,
                    Username = user.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                });

                return Created(string.Empty, webToken);
            }
            catch (DuplicateUserException dex)
            {
                return Conflict(dex.Message);
            }
        }
    }
}