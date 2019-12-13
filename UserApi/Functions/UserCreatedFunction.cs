using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace UserApi.Functions
{
    public static class UserCreatedFunction
    {
        [FunctionName("UserCreatedFunction")]
        public static void Run([EventGridTrigger]JObject eventObject, ILogger logger)
        {
            log.LogInformation("UserCreated Event Received by UserApi");
        }
    }
}