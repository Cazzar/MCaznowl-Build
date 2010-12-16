using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    static class Extensions
    {
        public static string Truncate(this string source, int maxLength)
        {
            if (source.Length > maxLength)
            {
                source = source.Substring(0, maxLength);
            }
            return source;
        }
    }
}
