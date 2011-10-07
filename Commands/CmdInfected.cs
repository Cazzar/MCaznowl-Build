using System;
using System.IO;

namespace MCForge
{
    public class CmdInfected : Command
    {
        public override string name { get { return "infected"; } }
        public override string shortcut { get { return "infected"; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdInfected() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (CmdZombieGame.infect.Count == 0)
            {
                Player.SendMessage(p, "No one is infected");
            }
            else
            {
                Player.SendMessage(p, "Players who are " + c.red + "infected " + c.yellow + "are:");
                CmdZombieGame.infect.ForEach(delegate(Player player)
                {
                    Player.SendMessage(p, player.name);
                });
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/infected - shows who is infected");
        }
    }
}