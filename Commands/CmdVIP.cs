?using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class CmdVIP : Command
    {
        public override string name { get { return "vip"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdVIP() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            string[] split = message.Split(' ');
            if (split[0] == "add")
            {
                if (split.Length < 2) { Help(p); return; }
                Player pl = Player.Find(split[1]);
                if (pl != null) split[1] = pl.name;
                if (VIP.Find(split[1])) Player.SendMessage(p, (pl == null ? "" : pl.color) + split[1] + " is already a VIP!");
                else
                {
                    VIP.Add(split[1]);
                    Player.SendMessage(p, (pl == null ? "" : pl.color) + split[1] + " is now a VIP.");
                    if (pl != null) Player.SendMessage(pl, "You are now a VIP!");
                }
            }
            else if (split[0] == "remove")
            {
                if (split.Length < 2) { Help(p); return; }
                Player pl = Player.Find(split[1]);
                if (pl != null) split[1] = pl.name;
                if (!VIP.Find(split[1])) Player.SendMessage(p, (pl == null ? "" : pl.color) + split[1] + " is not a VIP!");
                else
                {
                    VIP.Remove(split[1]);
                    Player.SendMessage(p, (pl == null ? "" : pl.color) + split[1] + " is no longer a VIP.");
                    if (pl != null) Player.SendMessage(pl, "You are no longer a VIP!");
                }
            }
            else if (split[0] == "list")
            {
                List<string> list = VIP.GetAll();
                if (list.Count < 1) Player.SendMessage(p, "There are no VIPs.");
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string name in list)
                        sb.Append(name).Append(", ");
                    Player.SendMessage(p, "There are " + list.Count + " VIPs:");
                    Player.SendMessage(p, sb.Remove(sb.Length - 2, 2).ToString());
                }
            }
            else Help(p);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "VIPs are players who can join regardless of the player limit.");
            Player.SendMessage(p, "/vip add <name> - Add a VIP.");
            Player.SendMessage(p, "/vip remove <name> - Remove a VIP.");
            Player.SendMessage(p, "/vip list - List all VIPs.");
        }
    }
}