
using UserApi.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(UserApi.Startup))]
namespace UserApi
{
    public class Startup : FunctionsStartup
    {
        private readonly IConfiguration _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            /*builder.Services.AddDbContext<IUserDbContext, UserDbContext>(opts =>
            {
                opts.UseSqlServer(_configuration["ConnectionString"]);
            });*/
        }
    }
}