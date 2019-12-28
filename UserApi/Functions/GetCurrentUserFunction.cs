using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserApi.Data;
using UserApi.Framework.Binding;

namespace UserApi.Functions
{
    public class GetCurrentUserFunction
    {
        private readonly IDataProvider _dataProvider;

        public GetCurrentUserFunction(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        [FunctionName("GetCurrentUserFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "current/user")]HttpRequestMessage eventData,
            ILogger logger,
            [UserToken]UserTokenResult userResult)
        {   
            if (userResult.TokenState == TokenState.Valid)
            {
                var user = await _dataProvider.GetUserByUsername(userResult.Username);
                if (user == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(user);
            }
            else if (userResult.TokenState == TokenState.Empty)
            {
                return new BadRequestResult();
            }
            else
            {
                return new UnauthorizedResult();
            }
        }
    }
}