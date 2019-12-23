using System;
using Microsoft.Azure.WebJobs.Description;

namespace UserApi.Framework.Binding
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public sealed class UserTokenAttribute : Attribute
    {
        
    }
}