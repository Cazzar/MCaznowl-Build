using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MCForge
{
        public class CmdGcaccept : Command
    {
        public override string name { get { return "gcaccept"; } }
        public override string shortcut { get { return "gca"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdGcaccept() { }

        public override void Use(Player p, string message)
        {
            if (Server.gcaccepted.Contains(p.name.ToLower())) { Player.SendMessage(p, "You already accepted the global chat rules!"); return; }
            Server.gcaccepted.Add(p.name.ToLower());
            File.WriteAllLines("text/gcaccepted.txt", Server.gcaccepted.ToArray());
            Player.SendMessage(p, "Congratulations! You can now use the Global Chat");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/gcaccept - Accept the global chat rules.");
        }
    }
}