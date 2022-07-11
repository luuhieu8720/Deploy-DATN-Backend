using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DATN_Back_end.Extensions
{
    public static class StringExtensions
    {
        private const string passwordKey = "wcAGm,~W(!^+8`DD&e]LPZ2S4+;bhN7DR?!T2Z{tt:_/,qmKu_,9UE~f}(}fpVAV";

        public static string Encrypt(this string source)
        {
            source += passwordKey;
            var byteSourceContext = Encoding.ASCII.GetBytes(source);
            var md5Hash = new MD5CryptoServiceProvider();
            var byteHash = md5Hash.ComputeHash(byteSourceContext);

            return string.Concat(byteHash.Select(x => x.ToString("x2")));
        }

        public static string FindRegex(this string source, string pattern)
        {
            var rx = new Regex(pattern);
            return rx.Match(source).Groups[1].Value;
        }
    }
}
