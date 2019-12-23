using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace UserApi.Framework.Binding
{
    public class UserTokenBindingProvider : IBindingProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public UserTokenBindingProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new UserTokenBinding(_serviceProvider);
            return Task.FromResult(binding);
        }
    }
}