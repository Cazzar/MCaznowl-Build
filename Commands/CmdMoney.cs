
using System;

namespace MCForge
{
	public class CmdMoney : Command
	{
		public override string name { get { return "money"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return true; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public override void Use(Player p, string message)
		{
            if (message == "")
            {
                Player.SendMessage(p, "You currently have " + p.money + " " + Server.moneys + ".");
            }
            else
            {
                Player who = Player.Find(message);
                if (who == null)
                {
                    Player.SendMessage(p, "Error: Player is not online.");
                    return;
                }
                if (who.group.Permission >= p.group.Permission)
                {
                    Player.SendMessage(p, "Cannot see the momey of someone of equal or greater rank.");
                    return;
                }

                Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " has got currently " + who.money + " " + Server.moneys + ".");
            }  
        }

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/money <player> - Shows how many " + Server.moneys + " <player> has");
		}
	}
}