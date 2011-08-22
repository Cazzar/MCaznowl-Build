using System;
using System.IO;

namespace MCForge
{
    public class CmdEndRound : Command
    {
        public override string name { get { return "endround"; } }
        public override string shortcut { get { return "er"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdEndRound() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (Server.infection)
            {
                CmdZombieGame.IEndRound();
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/endround - ends the round");
        }
    }
}