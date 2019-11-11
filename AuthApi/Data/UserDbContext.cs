
using AuthApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; 
        
        }
    }
}