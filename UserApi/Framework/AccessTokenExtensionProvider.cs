using Microsoft.Azure.WebJobs.Host.Config;

namespace UserApi.Framework.AccessToken
{
    public class AccessTokenExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var provider = new AccessTokenBindingProvider();
            var rule = context.AddBindingRule<AccessTokenAttribute>().Bind(provider);
        }
    }
}