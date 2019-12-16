using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using UserApi.Data;

namespace UserApi.Functions
{
    public class UserCreatedFunction
    {
        private readonly IUserDbContext _userDbContext;

        public UserCreatedFunction()
        {
            //_userDbContext = userDbContext;
        }
        
        [FunctionName("UserCreatedFunction")]
        public static void Run([EventGridTrigger]JObject eventObject, ILogger logger)
        {
            logger.LogInformation("UserCreated Event Received by UserApi");
            logger.LogInformation(eventObject.ToString());
        }
    }
}