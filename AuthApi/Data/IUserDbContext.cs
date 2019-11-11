using AuthApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public interface IUserDbContext
    {
         DbSet<User> Users { get; }
    }
}