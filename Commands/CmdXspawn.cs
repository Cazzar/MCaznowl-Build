// xspawn wrote by jordanneil23.
using System;

namespace MCForge
{
	public class CmdXspawn : Command
	{
                public override string name { get { return "xspawn"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            Player player = Player.Find(message.Split(' ')[0]);
            if (player == null)
            {
                Player.SendMessage(p, "Error: " + player.color + player.name + Server.DefaultColor + " was not found");
                return;
            }
            if (player == p)
            {
                Player.SendMessage(p, "Error: Seriously? Just use /spawn!");
                return;
            }
            Command.all.Find("spawn").Use(player, "");
            Player.SendMessage(p, "Succesfully spawned " + player.color + player.name + Server.DefaultColor + ".");
            Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " was respawned by " + p.color + p.name + Server.DefaultColor + ".");
        }
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/xspawn - Spawn another player.");
            Player.SendMessage(p, "WARNING: It says who used it!");
		}
	}
}