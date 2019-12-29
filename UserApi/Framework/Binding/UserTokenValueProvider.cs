using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<object> GetValueAsync()
        {
            try
            {
                var readResult = _readTokenService.ReadToken(_userToken);
                return new UserTokenResult { Username = readResult.Username, TokenState = TokenState.Valid };
            }
            catch (ArgumentNullException)
            {
                // token not provided
                return new UserTokenResult { TokenState = TokenState.Empty };
            }
            catch
            {
                // token was bad
                return new UserTokenResult { TokenState = TokenState.Invalid };
            }
        }

        public string ToInvokeString()
        {
            return nameof(UserTokenValueProvider);
        }
    }
}