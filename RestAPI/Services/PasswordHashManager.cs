using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace RestAPI.Services
{
    public class PasswordHashManager
    {
        public string GetHashString(string login, string password)
        {
            string hash = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password,
                    Encoding.UTF8.GetBytes(login),
                    KeyDerivationPrf.HMACSHA512, 
                    10000, 
                    256 / 8
                ));

            return hash;
        }

        public bool CompareHash(string hash, string passwordHash)
        {
            return hash.SequenceEqual(passwordHash);
        }
    }
}
