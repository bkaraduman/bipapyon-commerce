using System.Text;

namespace BiPapyon.Common.Infrastructure
{
    public  class PasswordEncryptor
    {
        public static string Encrypt(string password)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(data);
        }
    }
}
