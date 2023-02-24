using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Pdf.CommonUtility.AuthUtility
{
    public class HashingUtility
    {
        public static void GetNewPasswordHash(string password, out string passwordHash)
        {
            passwordHash = BC.EnhancedHashPassword(password);
        }

        public static bool VerifyPassword(string password, string hassedPassword)
        {
            return BC.EnhancedVerify(password, hassedPassword);
        }

        public static string GenerateRendomNumber()
        {
            Random rnd = new();
            int value = rnd.Next(100000, 999999);
            return value.ToString();
        }
    }
}
