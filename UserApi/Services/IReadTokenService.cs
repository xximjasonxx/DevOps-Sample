using UserApi.Services.Result;

namespace UserApi.Services
{
    public interface IReadTokenService
    {
        TokenReadResult ReadToken(string tokenString);
    }
}