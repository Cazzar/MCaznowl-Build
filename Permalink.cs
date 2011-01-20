using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Security.Cryptography;

namespace MCForge
{
    public static class Permalink
    {
        public static Uri URL;
        public static string UniqueHash
        {
            get
            {
                return GenerateUniqueHash();
            }
        }

        private static string GenerateUniqueHash()
        {
            string macs = "";

            // get network interfaces' physical addresses
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                PhysicalAddress pa = ni.GetPhysicalAddress();
                macs += pa.ToString();
            }

            // also add the server's current port, so that one machine may run multiple servers
            macs += Server.port.ToString();

            // generate hash
            var md5 = new MD5CryptoServiceProvider();
            byte[] originalBytes = Encoding.ASCII.GetBytes(macs);
            byte[] hash = md5.ComputeHash(originalBytes);

            // convert hash to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            // the the final hash as a string
            return sb.ToString();
        }
    }
}
