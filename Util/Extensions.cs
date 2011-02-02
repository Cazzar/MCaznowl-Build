using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;

namespace MCForge
{
    public static class Extensions
    {
        public static string Truncate(this string source, int maxLength)
        {
            if (source.Length > maxLength)
            {
                source = source.Substring(0, maxLength);
            }
            return source;
        }
        public static byte[] GZip(this byte[] bytes)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            GZipStream gs = new GZipStream(ms, CompressionMode.Compress, true);
            gs.Write(bytes, 0, bytes.Length);
            gs.Close();
            gs.Dispose();
            ms.Position = 0;
            bytes = new byte[ms.Length];
            ms.Read(bytes, 0, (int)ms.Length);
            ms.Close();
            ms.Dispose();
            return bytes;
        }
        public static string[] Slice(this string[] str, int offset)
        {
            return str.Slice(offset, 0);
        }
        public static string[] Slice(this string[] str, int offset, int length)
        {
            IEnumerable<string> tmp = str.ToList();
            if (offset > 0)
            {
                tmp = str.Skip(offset);
            }
            else throw new NotImplementedException("This function only supports positive integers for offset");

            if(length > 0)
            {
                tmp = tmp.Take(length);
            }
            else if (length == 0)
            {
                // Do nothing
            }
            else throw new NotImplementedException("This function only supports non-negative integers for length");
            
            return tmp.ToArray();
        }
    }
}
