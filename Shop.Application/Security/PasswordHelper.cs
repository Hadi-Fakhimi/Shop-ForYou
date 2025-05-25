using System.Security.Cryptography;
using System.Text;

namespace Shop.Application.Security
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var sha256 = SHA256.Create();
            byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();

        }
    }
}
