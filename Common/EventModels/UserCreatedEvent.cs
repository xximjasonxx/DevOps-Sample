
using System;

namespace Common.EventModels
{
    public class UserCreatedEvent
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}