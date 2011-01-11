using System;

namespace MCForge
{
    public class CmdMain : Command
    {
        public override string name { get { return "main"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdMain() { }

        public override void Use(Player p, string message)
        {
            if (p.level.name == Server.mainLevel.name) { Player.SendMessage(p, "You are already on the servers main level!"); return; }
            Command.all.Find("goto").Use(p, Server.mainLevel.name);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/home - Sends you to the main level.");
        }
    }
}