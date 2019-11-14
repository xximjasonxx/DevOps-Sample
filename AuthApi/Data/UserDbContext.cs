
using System;
using AuthApi.Data.Entities;
using AuthApi.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed a user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    EmailAddress = "duplicateuser@test.com",
                    Password = new Rfc2898DeriveBytesPasswordHasher().HashPassword("password")
                });
        }
    }
}