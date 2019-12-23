using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using UserApi.Services;

namespace UserApi.Framework.Binding
{
    public class UserTokenValueProvider : IValueProvider
    {
        private readonly IReadTokenService _readTokenService;
        private readonly string _userToken;
        private readonly string _issuerToken;

        public UserTokenValueProvider(string userToken, string issuerToken, IReadTokenService readTokenService)
        {
            _userToken = userToken;
            _issuerToken = issuerToken;
            _readTokenService = readTokenService;
        }

        public Type Type => typeof(UserTokenResult);

        public Task<object> GetValueAsync()
        {
            var readResult = _readTokenService.ReadToken(_userToken);
            return Task.FromResult((object)new UserTokenResult
            {
                Username = readResult.Username
            });
        }

        public string ToInvokeString()
        {
            return nameof(UserTokenValueProvider);
        }
    }
}