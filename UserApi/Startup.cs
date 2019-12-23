
using UserApi.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Data.Impl;
using UserApi.Framework.AccessToken;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs;

[assembly: FunctionsStartup(typeof(UserApi.Startup))]
namespace UserApi
{
    public class Startup : IWebJobsStartup
    {
        // public override void Configure(IFunctionsHostBuilder builder)
        // {
        //     builder.Services.AddTransient<IDataProvider, MongoDataProvider>();
        // }

        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddTransient<IDataProvider, MongoDataProvider>();

            builder.AddExtension<AccessTokenExtensionProvider>();
        }
    }
}