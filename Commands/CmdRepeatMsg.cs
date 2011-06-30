/* 
	Copyright 2011 MCDerp Team Based in Canada
	MCDerp has been Licensed with the below license:
	
	http://www.binpress.com/license/view/l/62e7c4034ccb45cd39d8dcbe9ed87bd8
	Or, you can read the below summary;

	Can be used on 1 site, unlimited servers
	Personal use only (cannot be resold or distributed)
	Non-commercial use only
	Cannot modify source-code for any purpose (cannot create derivative works)
	Software trademarks are included in the license
	Software patents are included in the license
*/
		

using System;

namespace MCForge
{
    public class CmdRepeatMsg : Command
    {
        public override string name { get { return "RepeatMessage"; } }
        public override string shortcut { get { return "rm"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdRepeatMsg() { }

        public override void Use(Player p, string message)
        {
            bool repeating = false;
            int time = 0;
            if (message == ("cancel"))
            {
                repeating = false;
            }
            else
            {
                time = int.Parse(message.Split(' ')[0]);
                string sendmessage = message.Split(' ')[1];
                if (time > 30)
                {
                    Player.SendMessage(p, "Can't have an amount of more than 30 minutes");
                }
                if (time < 1)
                {
                    Player.SendMessage(p, "You must have at least 1 minute");
                }
                else
                {
                    start:
                    Player.GlobalMessage(Server.DefaultColor + sendmessage);
                    System.Threading.Thread.Sleep(60000 * time);
                    if (repeating == true)
                    {
                        goto start;
                    }
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/RepeatMsg <time> <message> - Repeats a message every <time> minutes");
            Player.SendMessage(p, "/RepeatMsg <cancel> - Stop Repeat message");
        }
    }
}