using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MCForge
{
    public class CmdSeen : Command
    {
        public override string name { get { return "seen"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdSeen() { }

        public override void Use(Player p, string message)
        {
            Player pl = Player.Find(message);
            if (pl != null && !pl.hidden)
            {
                Player.SendMessage(p, pl.color + pl.name + Server.DefaultColor + " is currently online.");
                return;
            }

            DataTable playerDb = MySQL.fillData("SELECT * FROM Players WHERE Name='" + message + "'");
            if (playerDb.Rows != null && playerDb.Rows.Count > 0)
            {
                Player.SendMessage(p, message + " was last seen: " + playerDb.Rows[0]["LastLogin"]);
            }
            else
            {
                Player.SendMessage(p, "Unable to find player");
            }

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/seen [player] - says when a player was last seen on the server");
        }
    }
}
