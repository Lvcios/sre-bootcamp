using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wizeline.Common
{
    public static class EncryptionFunctions
    {

        public static string GetSHA512(string salt, string password)
        {
            var crypt = new System.Security.Cryptography.SHA512Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes($"{password}{salt}"));
            foreach (byte tbyte in crypto)
            {
                hash.Append(tbyte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
