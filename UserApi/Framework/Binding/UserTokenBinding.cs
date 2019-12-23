using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace UserApi.Framework.Binding
{
    public class UserTokenBinding : IBinding
    {
        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var headers = context.BindingData["Header"] as DefaultHttpRequest;

            var issuerToken = "movieappwmp";

            return Task.FromResult<IValueProvider>(new UserTokenValueProvider(headers, issuerToken));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor();
        }
    }
}