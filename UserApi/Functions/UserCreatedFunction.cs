
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace UserApi.Functions
{
    public static class UserCreatedFunction
    {
        [FunctionName("UserCreatedFunction")]
        public static IActionResult Run([EventGridTrigger]JObject eventObject, ILogger logger)
        {
            return new OkObjectResult("Returned");
        }
    }
}