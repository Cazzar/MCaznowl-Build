/*
	Copyright 2010 MCForge team
    Written by SebbiUltimate
*/
using System;
using System.Threading;

namespace MCForge
{
	public class CmdFetch : Command
	{
		public override string name { get { return "fetch"; } }
		public override string shortcut { get { return "fb"; } }
		public override string type { get { return "mod"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
		public override void Use(Player p, string message)
		{
            if (p == null)
            {
                Player.SendMessage(p, "Console cannot use this command. Try using /move instead.");
                return;
            }

            Player who = Player.Find(message);
            if (who == null || who.hidden)
            {
                Player.SendMessage(p, "Could not find player.");
                return;
            }

            if (p.level != who.level)
            {
                Player.SendMessage(p, who.name + " is in a different Level. Forcefetching has started!");
                Level where = p.level;
                Command.all.Find("goto").Use(who, where.name);
                Thread.Sleep(1000);
                // Sleep for a bit while they load
                while (who.Loading) { Thread.Sleep(250); }
            }

            unchecked
            {
                who.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0);
            }
		}
		public override void Help(Player p)
		{
			Player.SendMessage(p, "/fetch <player> - Fetches Player forced!");
            Player.SendMessage(p, "Moves Player to your Level first");
		}
	}
}