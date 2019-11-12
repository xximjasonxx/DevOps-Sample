namespace AuthApi.Services
{
    public interface IPasswordHasher
    {
         string HashPassword(string rawPassword);
         bool CheckPasswordHash(string hashedPassword, string rawPassword);
    }
}