using System;
using System.IO;

namespace MCForge
{
    public class CmdInfect : Command
    {
        public override string name { get { return "infect"; } }
        public override string shortcut { get { return "i"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdInfect() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (CmdZombieGame.infect.Contains(who))
            {
                p.SendMessage("Player cannot be infected!");
            }
            else
            {
                if (!who.referee)
                {
                    if (Server.infection)
                    {
                        CmdZombieGame.infect.Add(who);
                        CmdZombieGame.players.Remove(who);
                        who.color = c.red;
                        Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " just got Infected!");
                        Player.GlobalDie(who, false);
                        Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
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