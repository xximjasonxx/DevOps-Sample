
using AuthApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<User> Users { get; set; 
        
        }
    }
}