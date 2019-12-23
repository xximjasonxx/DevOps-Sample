using Microsoft.Azure.WebJobs.Host.Config;
using UserApi.Framework.Binding;

namespace UserApi.Framework
{
    public class UserTokenExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var provider = new UserTokenBindingProvider();
            var rule = context.AddBindingRule<UserTokenAttribute>().Bind(provider);
        }
    }
}