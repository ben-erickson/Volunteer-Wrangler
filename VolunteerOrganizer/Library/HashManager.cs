using System.Security.Cryptography;
using System.Text;

namespace VolunteerOrganizer.Library
{
    public static class HashManager
    {
        public static string HashString(string source)
        {
            string hash = "";

            using (SHA256 sha256hash = SHA256.Create())
            {
                hash = GetHash(sha256hash, source);
            }

            return hash;
        }

        public static string GetHash(HashAlgorithm hashAlgorithm, string hashSource)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(hashSource));

            StringBuilder hashBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                hashBuilder.Append(data[i].ToString("x2"));
            }

            return hashBuilder.ToString();
        }
    }
}
