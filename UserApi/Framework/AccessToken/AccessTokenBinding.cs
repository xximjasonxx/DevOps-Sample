using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace UserApi.Framework.AccessToken
{
    public class AccessTokenBinding : IBinding
    {
        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var request = context.BindingData["$request"] as DefaultHttpRequest;

            var issuerToken = "movieappwmp";

            return Task.FromResult<IValueProvider>(new AccessTokenValueProvider(request, issuerToken));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor();
        }
    }
}