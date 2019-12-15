using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserApi.Data;

namespace UserApi.Functions
{
    public class PingFunction
    {
        private readonly IUserDbContext _userDbContext;

        public PingFunction(IUserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [FunctionName("PingFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/ping")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new OkResult();
        }
    }
}
