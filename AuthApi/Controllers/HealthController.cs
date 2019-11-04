
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/v1/ping")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}