using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AliHelper.Signature.Methods
{
    public class Encrypt
    {
        /// <summary>
        /// HMAC SHA1算法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] HMAC_SHA1(string value,string key)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] valueBytes = Encoding.UTF8.GetBytes(value);
                HMACSHA1 hmac = new HMACSHA1(keyBytes);
                return hmac.ComputeHash(valueBytes);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
