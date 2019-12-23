using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            var userToken = userTokenGroups.ElementAt(1);
            var issuerToken = "movieappwmp";

            return Task.FromResult<IValueProvider>(new UserTokenValueProvider(userToken, issuerToken));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor();
        }
    }
}