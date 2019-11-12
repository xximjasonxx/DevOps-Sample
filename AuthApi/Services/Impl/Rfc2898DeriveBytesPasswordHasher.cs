using System;
using System.Linq;
using System.Security.Cryptography;

namespace AuthApi.Services.Impl
{
    public class Rfc2898DeriveBytesPasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 20000;

        public string HashPassword(string rawPassword)
        {
            using (var algorithm = new Rfc2898DeriveBytes(rawPassword, SaltSize, Iterations, HashAlgorithmName.SHA256))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{salt}.{key}";
            }
        }

        public bool CheckPasswordHash(string hashedPassword, string rawPassword)
        {
            var hashParts = hashedPassword.Split('.', 2);
            if (hashParts.Length != 2)
                throw new FormatException("Hashed Passsword was not in the excepted format");

            var salt = Convert.FromBase64String(hashParts[0]);
            var key = Convert.FromBase64String(hashParts[1]);

            using (var algorithm = new Rfc2898DeriveBytes(rawPassword, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var checkKey = algorithm.GetBytes(KeySize);
                return checkKey.SequenceEqual(key);
            }
        }
    }
}