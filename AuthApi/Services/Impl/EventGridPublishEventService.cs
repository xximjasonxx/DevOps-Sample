using System;
using System.Threading.Tasks;
using Common.EventModels;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;

namespace AuthApi.Services.Impl
{
    public class EventGridPublishEventService : IPublishEventService
    {
        private readonly IConfiguration _configuration;

        public EventGridPublishEventService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishUserCreateEventAsync(UserCreatedEvent userCreatedEvent)
        {
            var topicCredentials = new TopicCredentials(_configuration["EventTopicAccessKey"]);
            var topicUri = new Uri(_configuration["EventTopicEndpoint"]);
            var client = new EventGridClient(topicCredentials);
            var eventModel = new EventGridEvent
            {
                Id = Guid.NewGuid().ToString(),
                EventType = nameof(UserCreatedEvent),
                Data = userCreatedEvent,
                EventTime = DateTime.UtcNow,
                Subject = "UserCreated",
                DataVersion = "2.0"
            };

            await client.PublishEventsAsync(topicUri.Host, new[] { eventModel });
        }
    }
}