using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using UserApi.Data;
using UserApi.Framework.AccessToken;

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
            [AccessToken]AccessTokenResult principal)
        {
            return null;
        }
    }
}