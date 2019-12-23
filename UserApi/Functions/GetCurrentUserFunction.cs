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
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "current/user")]HttpRequestMessage eventData,
            ILogger logger,
            [UserToken]UserTokenResult userResult)
        {
            var user = await _dataProvider.GetUserByUsername(userResult.Username);
            if (user == null)
            {
                throw new Exception("boom - No Current User");
            }

            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            responseMessage.Content = new StringContent(JsonConvert.SerializeObject(user));
            return responseMessage;
        }
    }
}