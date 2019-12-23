using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using UserApi.Data.Entities;
using UserApi.Data.Models;

namespace UserApi.Data.Impl
{
    public class MongoDataProvider : IDataProvider
    {
        private const string UserDataCollectionName = "users";

        private readonly IConfiguration _configuration;

        public MongoDataProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddUserAsync(User user)
        {
            var client = new MongoClient(_configuration["ConnectionString"]);
            var database = client.GetDatabase(_configuration["DatabaseName"]);
            var collection = database.GetCollection<UserDocument>(UserDataCollectionName);

            await collection.InsertOneAsync(new UserDocument
            {
                User = user
            });
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var client = new MongoClient(_configuration["ConnectionString"]);
            var database = client.GetDatabase(_configuration["DatabaseName"]);
            var collection = database.GetCollection<UserDocument>(UserDataCollectionName);

            var filter = Builders<UserDocument>.Filter.Eq(uf => uf.User.Username, username);
            var document = (await collection.FindAsync(filter)).FirstOrDefault();

            return document?.User;
        }
    }
}