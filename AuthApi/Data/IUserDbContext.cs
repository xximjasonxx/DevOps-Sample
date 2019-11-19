using System.Threading;
using System.Threading.Tasks;
using AuthApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public interface IUserDbContext
    {
        Database Database { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken));
        Task SeedTestUsers();
    }
}