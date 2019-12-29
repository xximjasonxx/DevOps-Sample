using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserApi.Extensions;
using UserApi.Services.Result;

namespace UserApi.Services.Impl
{
    public class JwtTokenReadTokenService : IReadTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenReadTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenReadResult ReadToken(string tokenString)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtIssuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtAudience"]
            };
            
            var identity = handler.ValidateToken(tokenString, validations, out var tokenSecure).Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception("boom - Identity is not valid");
            }

            return new TokenReadResult
            {
                Username = identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            };
        }
    }
}