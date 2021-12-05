using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TN213_MuaSamCanTho.Tools
{
    public static class Assistance
    {
        public static string MaHoaMatKhau(string input)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(input);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string spass = "";

            foreach (byte item in hasData)
            {
                spass += item;
            }

            return spass;
        }
    }

  
}