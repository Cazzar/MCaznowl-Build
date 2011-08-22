using System;
using System.IO;

namespace MCForge
{
    public class CmdDisInfect : Command
    {
        public override string name { get { return "disinfect"; } }
        public override string shortcut { get { return "di"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdDisInfect() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (CmdZombieGame.players.Contains(who))
            {
                p.SendMessage("Cannot disinfect player");
            }
            else
            {
                if (!who.referee)
                {
                    int leftMinute = CmdZombieGame.timeMinute - DateTime.Now.Minute + 7;
                    if (leftMinute != 0)
                    {
                        if (Server.infection)
                        {
                            CmdZombieGame.infect.Remove(who);
                            CmdZombieGame.players.Add(who);
                            who.color = who.group.color;
                            Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " just got disinfected!");
                            Player.GlobalDie(who, false);
                            Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                        }
                    }
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/disinfect [name] - disinfects [name]");
        }
    }
}