using MongoDB.Bson;
using UserApi.Data.Models;

namespace UserApi.Data.Entities
{
    public class UserDocument
    {
        public ObjectId Id { get; set; }

        public User User { get; set; }
    }
}