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
    public class CmdAdminChat : Command
    {
        public override string name { get { return "adminchat"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdAdminChat() { }

        public override void Use(Player p, string message)
        {
            p.adminchat = !p.adminchat;
            if (p.adminchat) Player.SendMessage(p, "All messages will now be sent to Admins only");
            else Player.SendMessage(p, "Admin chat turned off");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/adminchat - Makes all messages sent go to Admins by default");
        }
    }
}


