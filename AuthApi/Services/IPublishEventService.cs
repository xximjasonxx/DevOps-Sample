
using System;
using System.Threading.Tasks;
using Common.EventModels;

namespace AuthApi.Services
{
    public interface IPublishEventService
    {
        Task PublishUserCreateEventAsync(UserCreatedEvent userCreatedEvent);
    }
}