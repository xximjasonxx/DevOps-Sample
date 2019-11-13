using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApi.Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Services.Impl
{
    public class JwtCreateTokenService : ICreateTokenService
    {
        private const string JwtKey = "e54186e5-0f36-4faa-bad6-82a00c044ee4";
        private const string JwtIssuer = "MovieApp";
        private const string JwtAudience = "MovieApp";

        public string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(JwtIssuer,
                JwtAudience,
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}