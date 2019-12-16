using UserApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace UserApi.Data
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}