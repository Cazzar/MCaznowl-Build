using System;
using System.IO;

using MCForge;
namespace MCForge.Commands
{
    /// <summary>
    /// This is the command /infected
    /// use /help infected in-game for more info
    /// </summary>
    public class CmdInfected : Command
    {
        public override string name { get { return "infected"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdInfected() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } else { who = Player.Find(message); }
            if (ZombieGame.infectd.Count == 0)
            {
                Player.SendMessage(p, "No one is infected");
            }
            else
            {
                Player.SendMessage(p, "Players who are " + c.red + "infected " + Server.DefaultColor + "are:");
                string playerstring = "";
                ZombieGame.infectd.ForEach(delegate(Player player)
                {
                    playerstring = playerstring + c.red + player.name + Server.DefaultColor + ", ";
                });
                Player.SendMessage(p, playerstring);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/infected - shows who is infected");
        }
    }
}