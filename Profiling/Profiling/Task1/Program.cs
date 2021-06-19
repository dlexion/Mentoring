using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = GeneratePasswordHashUsingSalt("Password123", Encoding.UTF8.GetBytes("someString1263127123761281826"));

            var sw = new Stopwatch();
            sw.Start();
            var result2 = GeneratePasswordHashUsingSalt("Password123", Encoding.UTF8.GetBytes("someString1263127123761281826"));
            sw.Stop();
            Console.WriteLine($"Elapsed milliseconds: {sw.ElapsedMilliseconds} | elapsed ticks: {sw.ElapsedTicks}");
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 10000; 
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate); 
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16 * sizeof(byte));
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20 * sizeof(byte));

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
}
