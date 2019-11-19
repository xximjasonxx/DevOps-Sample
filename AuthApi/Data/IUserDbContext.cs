using System.Threading;
using System.Threading.Tasks;
using AuthApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AuthApi.Data
{
    public interface IUserDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken));
        Task SeedTestUsers();
    }
}