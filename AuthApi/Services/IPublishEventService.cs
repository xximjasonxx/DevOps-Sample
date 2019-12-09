
using System;
using System.Threading.Tasks;
using AuthApi.Models;

namespace AuthApi.Services
{
    public interface IPublishEventService
    {
        Task PublishUserCreateEventAsync(UserCreatedEvent userCreatedEvent);
    }
}