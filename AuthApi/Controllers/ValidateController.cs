using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/v1/validate")]
    public class ValidateController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> ValidateToken()
        {
            return Ok();
        }
    }
}