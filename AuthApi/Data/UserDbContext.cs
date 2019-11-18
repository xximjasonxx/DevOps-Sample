
using System;
using System.Threading.Tasks;
using AuthApi.Data.Entities;
using AuthApi.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            //this.Database.Migrate();
        }

        public DbSet<User> Users { get; set; }

        public async Task SeedTestUsers()
        {
            // remove all users
            Users.RemoveRange(Users);

            // insert our standard users
            await Users.AddAsync(new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "duplicateuser@test.com",
                Password = new Rfc2898DeriveBytesPasswordHasher().HashPassword("password")
            });

            await Users.AddAsync(new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "validuser@test.com",
                Password = new Rfc2898DeriveBytesPasswordHasher().HashPassword("password")
            });

            await SaveChangesAsync();
        }
    }
}