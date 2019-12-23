using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace UserApi.Framework.AccessToken
{
    public class AccessTokenValueProvider : IValueProvider
    {
        private readonly DefaultHttpRequest _request;
        private readonly string _issuerToken;

        private readonly object _value = new AccessTokenResult();

        public AccessTokenValueProvider(DefaultHttpRequest request, string issuerToken)
        {
            _request = request;
            _issuerToken = issuerToken;
        }

        public Type Type => typeof(AccessTokenResult);

        public Task<object> GetValueAsync()
        {
            return Task.FromResult(_value);
        }

        public string ToInvokeString()
        {
            return _request.ToString();
        }
    }
}