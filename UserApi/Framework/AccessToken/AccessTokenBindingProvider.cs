using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace UserApi.Framework.AccessToken
{
    public class AccessTokenBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new AccessTokenBinding();
            return Task.FromResult(binding);
        }
    }
}