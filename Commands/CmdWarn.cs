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
            if (message.Split(' ')[1] == "")
                reason = message.Substring(message.IndexOf(' ', message.IndexOf(' ') + 1));            		
            else reason = "you know why.";

            if (message == "") { Help(p); return; }
            Player who = Player.Find(message.Split(' ')[0]);


            if (p == null)
            {
                if (who == null)
                {
                    Player.SendMessage(p, "Player not found!");
                    return;
                }

                if (Server.devs.Contains(who.name))
                {
                    Player.SendMessage(p, "why are you warning a dev??");
                    return;
                }
                Player.GlobalMessage("console %ewarned " + who.color + who.name + " %ebecause : &c");
                Player.GlobalMessage(reason);
                Player.SendMessage(who, "Do it again ");
                {
                    if (who.warn == 0)
                    {
                        Player.SendMessage(who, "twice and you get kicked!");
                        who.warn = 1;
                        return;
                    }
                    if (who.warn == 1)
                    {
                        Player.SendMessage(who, "one more time and you get kicked!");
                        who.warn = 2;
                        return;
                    }
                    if (who.warn == 2)
                    {
                        Player.GlobalMessage("console have warn kicked" + who.color + who.name + "%ebecause : &c");
                        Player.GlobalMessage(reason);
                        who.warn = 0;
                        who.Kick("BECAUSE " + reason + "");
                        return;
                    }
                }
            }


            if (who == null)
            {
                Player.SendMessage(p, "Player not found!");
                return;
            }
            if (who == p)
            {
                Player.SendMessage(p, "you can't warn yourself");
                return;
            }
            if (p.group.Permission <= who.group.Permission)
            {
                Player.SendMessage(p, "you can't warn a player equal or higher rank.");
                return;
            }
            if (Server.devs.Contains(who.name))
            {
                Player.SendMessage(p, "why are you warning a dev??");
                return;
            }
            {
                Player.GlobalMessage(p.color + p.name + " %ewarned " + who.color + who.name + " %ebecause : &c");
                Player.GlobalMessage(reason);
                Player.SendMessage(who, "Do it again ");
                {
                    if (who.warn == 0)
                    {
                        Player.SendMessage(who, "twice and you get kicked!");
                        who.warn = 1;
                        return;
                    }
                    if (who.warn == 1)
                    {
                        Player.SendMessage(who, "one more time and you get kicked!");
                        who.warn = 2;
                        return;
                    }
                    if (who.warn == 2)
                    {
                        Player.GlobalMessage(p.color + p.name + "have warn kicked" + who.color + who.name + "");                       
                        who.warn = 0;
                        who.Kick("BECAUSE " + reason + "");
                        return;
                    }
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/warn - Warns a player.");
        }
    }
}