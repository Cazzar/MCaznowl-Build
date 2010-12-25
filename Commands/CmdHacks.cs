using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class CmdHacks : Command
    {
        public override string name { get { return "hacks"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdHacks() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            p.Kick("You have been banned from All MCForge servers and reported to Notch!");

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/hacks - HACK THE PLANET");
        }
    }

}
