using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using UserApi.Data;
using Microsoft.Azure.EventGrid.Models;
using UserApi.Data.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace UserApi.Functions
{
    public class UserCreatedFunction
    {
        public UserCreatedFunction()
        {
        }
        
        [FunctionName("UserCreatedFunction")]
        public async Task Run([EventGridTrigger]EventGridEvent eventData, ILogger logger)
        {
            logger.LogInformation("UserCreated Event Received by UserApi");
            var user = JsonConvert.DeserializeObject<User>(eventData.Data.ToString());

            //await _userDbContext.Users.AddAsync(user);
            //await _userDbContext.SaveChangesAsync();            
        }
    }
}