using System;
using Microsoft.Azure.WebJobs.Description;

namespace UserApi.Framework.AccessToken
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public sealed class AccessTokenAttribute : Attribute
    {
        
    }
}