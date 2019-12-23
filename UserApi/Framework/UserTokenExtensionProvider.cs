using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Extensions;
using UserApi.Framework.Binding;

namespace UserApi.Framework
{
    public class UserTokenExtensionProvider : IExtensionConfigProvider
    {
        private readonly IConfiguration _configuration;

        public UserTokenExtensionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterDependencies();
            serviceCollection.AddSingleton<IConfiguration>(_configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            context.AddBindingRule<UserTokenAttribute>().Bind(new UserTokenBindingProvider(serviceProvider));
        }
    }
}