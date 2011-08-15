/*
	Copyright 2011 MCForge
	
	Author: fenderrock87
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
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
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				GZipStream gs = new GZipStream(ms, CompressionMode.Compress, true);
				gs.Write(bytes, 0, bytes.Length);
				gs.Close();
				ms.Position = 0;
				bytes = new byte[ms.Length];
				ms.Read(bytes, 0, (int)ms.Length);
				ms.Close();
				ms.Dispose();
			}
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
