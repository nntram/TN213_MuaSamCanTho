using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web;

namespace TN213_MuaSamCanTho.Tools
{
    public static class Assistance
    {
        public static string MaHoaMatKhau(string input)
        {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(input);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
            {
                str += b.ToString("X2");
            }
            return str;
        }

        
    }

  
}