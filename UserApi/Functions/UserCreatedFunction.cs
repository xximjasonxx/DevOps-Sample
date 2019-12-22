using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using UserApi.Data;
using Microsoft.Azure.EventGrid.Models;
using UserApi.Data.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UserApi.Data.Models;
using System;

namespace UserApi.Functions
{
    public class UserCreatedFunction
    {
        private readonly IDataProvider _dataProvider;

        public UserCreatedFunction(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        
        [FunctionName("UserCreatedFunction")]
        public async Task Run([EventGridTrigger]EventGridEvent eventData, ILogger logger)
        {
            try
            {
                logger.LogInformation("UserCreated Event Received by UserApi");
                var user = JsonConvert.DeserializeObject<User>(eventData.Data.ToString());

                await _dataProvider.AddUserAsync(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception Encounted Creating User");
            }     
        }
    }
}