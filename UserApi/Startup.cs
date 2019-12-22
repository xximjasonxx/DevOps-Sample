
using UserApi.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using UserApi.Data.Impl;

[assembly: FunctionsStartup(typeof(UserApi.Startup))]
namespace UserApi
{
    public class Startup : FunctionsStartup
    {
        private readonly IConfiguration _configuration;

        public Startup()
        {
            /*_configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();*/
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IDataProvider, MongoDataProvider>();
        }
    }
}