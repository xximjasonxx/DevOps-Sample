
using System;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Data;
using UserApi.Data.Impl;
using UserApi.Services;
using UserApi.Services.Impl;

namespace UserApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDataProvider, MongoDataProvider>();
            serviceCollection.AddTransient<IReadTokenService, JwtTokenReadTokenService>();
        }

        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }
    }
}