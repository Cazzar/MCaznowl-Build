using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MCForge
{
    public class CmdDurl : Command
    {
        public override string name { get { return "durl"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override string keywords { get { return "url wom"; } }
        public CmdDurl() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            Player.SendMessage(p, "mc://127.0.0.1:25565/" + message + "/" + CalculateMD5Hash(message + Server.salt));
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/durl [name] - creates a durl for [name]");
        }
    }
}