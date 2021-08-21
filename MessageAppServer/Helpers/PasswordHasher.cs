using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MessageAppServer.Helpers
{
    public class PasswordHasher
    {
        private static readonly int StandardHashLength = 32;

        public static string HashPassword(string password, string salt = null)
        {
            // generate salt if one is not supplied
            salt = salt is null ? GenerateSalt(StandardHashLength) : salt;

            // append the salt to the end of the password
            string passwordSalt = $"{password}{salt}";
            byte[] passwordSaltBytes = Encoding.UTF8.GetBytes(passwordSalt);
            string hashedPassword;

            // using the SHA256 class compute the hash using the salt
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(passwordSaltBytes);
                hashedPassword = Convert.ToBase64String(hashValue);
            }
            return hashedPassword;
        }

        public static string GenerateSalt(int byteLength)
        {
            // random number generate class
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[byteLength];

            // generate the random number and return as string
            random.GetNonZeroBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static bool CheckPassword(string password, string salt, string hashedPassword)
        {
            // append the salt to the end of the password
            string hashedPasswordCheck = HashPassword(password, salt);
            return hashedPassword.Equals(hashedPasswordCheck);
        }
    }
}
