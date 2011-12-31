using System;
using System.IO;

namespace MCForge
{
    public class CmdVoteResults : Command
    {
        public override string name { get { return "voteresults"; } }
        public override string shortcut { get { return "vr"; } }
        public override string type { get { return ""; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdVoteResults() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if ((message != null && String.IsNullOrEmpty(message))) { who = p; message = p.name; } else { who = Player.Find(message); }
            if (Server.YesVotes >= 1 || Server.NoVotes >= 1)
            {
                p.SendMessage(c.green + "Y: " + Server.YesVotes + c.red + " N: " + Server.NoVotes);
                return;
            }
            else
            {
                p.SendMessage("There hasn't been a vote yet!");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/voteresults - see the results of the last vote!");
        }
    }
}