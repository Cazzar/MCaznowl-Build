using System;

namespace MCForge
{
    public class CmdWarn : Command
    {
        public override string name { get { return "warn"; } }

        string reason;

        public override string shortcut { get { return ""; } }

        public override string type { get { return "mod"; } }

        public override bool museumUsable { get { return true; } }

        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }

        public override void Use(Player p, string message)
        {
            string warnedby;

            if (message == "") { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);

            // Make sure we have a valid player
            if (who == null)
            {
                Player.SendMessage(p, "Player not found!");
                return;
            }

            // Don't warn a dev
            if (Server.devs.Contains(who.name))
            {
                Player.SendMessage(p, "why are you warning a dev??");
                return;
            }

            // Don't warn yourself... derp
            if (who == p)
            {
                Player.SendMessage(p, "you can't warn yourself");
                return;
            }

            // Check the caller's rank
            if (p != null && p.group.Permission <= who.group.Permission)
            {
                Player.SendMessage(p, "you can't warn a player equal or higher rank.");
                return;
            }
            
            // We need a reason
            if (message.Split(' ').Length == 1)
            {
                // No reason was given
                reason = "you know why.";
            }
            else
            {
                reason = message.Substring(message.IndexOf(' ') + 1).Trim();
            }

            warnedby = (p == null) ? "console" : p.color + p.name;
            Player.GlobalMessage(warnedby + " %ewarned " + who.color + who.name + " %ebecause:");
            Player.GlobalMessage("&c" + reason);

            //Player.SendMessage(who, "Do it again ");
            if (who.warn == 0)
            {
                Player.SendMessage(who, "Do it again twice and you will get kicked!");
                who.warn = 1;
                return;
            }
            if (who.warn == 1)
            {
                Player.SendMessage(who, "Do it one more time and you will get kicked!");
                who.warn = 2;
                return;
            }
            if (who.warn == 2)
            {
                Player.GlobalMessage(who.color + who.name + " " + Server.DefaultColor + "was warn-kicked by " + warnedby);
                who.warn = 0;
                who.Kick("KICKED BECAUSE " + reason + "");
                return;
            }

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/warn - Warns a player.");
        }
    }
}