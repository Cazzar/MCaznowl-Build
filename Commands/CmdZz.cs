using System;

namespace MCForge
{
	public class CmdZz : Command
	{
                public override string name { get { return "zz"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
		public override void Use(Player p, string message)
		{
			Command.all.Find("static").Use(p, message);
                        Command.all.Find("cuboid").Use(p, message);
                        Player.SendMessage(p, p.color + p.name + Server.DefaultColor + " to stop this, use /abort");
		}
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/zz - Like cuboid but in static mode automatically.");
		}
	}
}