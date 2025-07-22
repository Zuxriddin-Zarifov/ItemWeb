using System.Security.Cryptography;
using System.Text;

namespace Anjir.Core
{
    public static class HeshService
    {
        public static string HeshSha256(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // har baytni 2 xonali hex formatda yozadi
                }
                return sb.ToString();
            }
        }
    }
}
