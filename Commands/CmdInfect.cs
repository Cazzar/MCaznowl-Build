using System;
using System.IO;

namespace MCForge
{
    public class CmdInfect : Command
    {
        public override string name { get { return "infect"; } }
        public override string shortcut { get { return "i"; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public static string keywords { get { return "virus inf"; } }
        public CmdInfect() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (who.infected)
            {
                p.SendMessage("Player cannot be infected!");
            }
            else
            {
                if (!who.referee)
                {
                    if (Server.zombie.GameInProgess())
                    {
                        Server.zombie.InfectPlayer(who);
                        Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " just got Infected!");
                    }
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/infect [name] - infects [name]");
        }
    }
}