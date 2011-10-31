using System;
using System.IO;

namespace MCForge
{
    public class CmdDisInfect : Command
    {
        public override string name { get { return "disinfect"; } }
        public override string shortcut { get { return "di"; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdDisInfect() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (!who.infected || !Server.zombie.GameInProgess())
            {
                p.SendMessage("Cannot disinfect player");
            }
            else
            {
                if (!who.referee)
                {
                    int leftMinute = (int)((Server.zombie.amountOfMilliseconds / (1000 * 60)) % 60);
                    if (leftMinute > 1)
                    {
                        Server.zombie.DisinfectPlayer(who);
                        Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " just got Disnfected!");
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