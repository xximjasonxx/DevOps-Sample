using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace UserApi.Framework.Binding
{
    public class UserTokenValueProvider : IValueProvider
    {
        private readonly DefaultHttpRequest _request;
        private readonly string _issuerToken;

        public UserTokenValueProvider(object headers, string issuerToken)
        {
            //_request = request;
            _issuerToken = issuerToken;
        }

        public Type Type => typeof(UserTokenResult);

        public Task<object> GetValueAsync()
        {
            return Task.FromResult((object)new UserTokenResult());
        }

        public string ToInvokeString()
        {
            return _request.ToString();
        }
    }
}