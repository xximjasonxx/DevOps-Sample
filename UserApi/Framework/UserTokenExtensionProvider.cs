using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Extensions;
using UserApi.Framework.Binding;

namespace UserApi.Framework
{
    public class UserTokenExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterDependencies();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            context.AddBindingRule<UserTokenAttribute>().Bind(new UserTokenBindingProvider(serviceProvider));
        }
    }
}