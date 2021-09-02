using System;
using System.Security.Cryptography;

namespace EShop.Infrastructure.Security
{
    public class Encrypter : IEncrypter
    {
        private readonly string Salt = "VE8ZuB+RGTJZ/ep.PGu?Zd-ulIfi1eU_BTkdP.7#tN5e?8Ffik";
        public string GetHash(string value, string salt)
        {
            var derivedBytes = new Rfc2898DeriveBytes(value, GetBytes(salt), 1000);
            return Convert.ToBase64String(derivedBytes.GetBytes(50)); 
        }

        public string GetSalt()
        {
            return Salt;
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new Byte[value.Length];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
