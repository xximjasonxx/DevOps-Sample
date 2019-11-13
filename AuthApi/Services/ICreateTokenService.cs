using AuthApi.Data.Entities;

namespace AuthApi.Services
{
    public interface ICreateTokenService
    {
         string CreateToken(User user);
    }
}