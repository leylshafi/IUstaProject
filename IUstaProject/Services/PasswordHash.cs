using System.Security.Cryptography;
using System.Text;

namespace IUstaProject.Services
{
    public static class PasswordHash
    {
        public static void Create(string password, out byte[] PassHash, out byte[] PassSalt)
        {
            using var hmac = new HMACSHA512();
            PassSalt = hmac.Key;
            PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool ConfirmPasswordHash(string password, byte[] PassHash, byte[] PassSalt)
        {
            using var hmac = new HMACSHA512(PassSalt);
            var compHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return compHash.SequenceEqual(PassHash);
        }

    }
}
