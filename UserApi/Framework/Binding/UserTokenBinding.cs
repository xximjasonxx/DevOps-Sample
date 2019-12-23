using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserApi.Extensions;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using UserApi.Services;

namespace UserApi.Framework.Binding
{
    public class UserTokenBinding : IBinding
    {
        private readonly IReadTokenService _readTokenService;

        public UserTokenBinding(IServiceProvider serviceProvider)
        {
            _readTokenService = serviceProvider.GetService<IReadTokenService>();
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var headers = context.BindingData["Headers"] as IDictionary<string, string>;
            if (!headers.ContainsKey("Authorization"))
            {
                throw new Exception("boom - no auth header");
            }

            var userTokenGroups = Regex.Match(headers["Authorization"], @"^Bearer (\S+)$").Groups;
            if (userTokenGroups.Count < 2)
            {
                throw new Exception("boom - bad format");
            }

            var userToken = userTokenGroups.ElementAt(1).Value;
            var issuerToken = "movieappwmp";

            return Task.FromResult<IValueProvider>(new UserTokenValueProvider(userToken, issuerToken, _readTokenService));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor();
        }
    }
}