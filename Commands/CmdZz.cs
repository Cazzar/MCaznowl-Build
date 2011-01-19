using System;

namespace MCForge
{
	public class CmdZz : Command
	{
        public override string name { get { return "zz"; } }
		public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
		public override void Use(Player p, string message)
		{
            if ((p.group.CanExecute(Command.all.Find("cuboid"))) && (p.group.CanExecute(Command.all.Find("static"))))
            {
                if ((!p.staticCommands == true) && (!p.megaBoid == true))
                {
                    Command.all.Find("static").Use(p, message);
                    Command.all.Find("cuboid").Use(p, message);
                    Player.SendMessage(p, p.color + p.name + Server.DefaultColor + " to stop this, use /zz again");
                }
                else 
                {
                    p.ClearBlockchange();
                    p.staticCommands = false;
                    Player.SendMessage(p, "/zz has ended.");
                }
            }
            else { Player.SendMessage(p, "Sorry, your rank cannot use one of the commands this uses!"); }
               
		}
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/zz - Like cuboid but in static mode automatically.");
		}
	}
}