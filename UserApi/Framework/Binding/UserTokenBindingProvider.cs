using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace UserApi.Framework.Binding
{
    public class UserTokenBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new UserTokenBinding();
            return Task.FromResult(binding);
        }
    }
}