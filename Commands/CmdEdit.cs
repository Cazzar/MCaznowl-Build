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
using System.IO;

namespace MCForge
{
    public class CmdEdit : Command
    {
        public override string name { get { return "edit"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdEdit() { }

        public override void Use(Player p, string message)
        {
            string type = message.Split(' ')[0];
            string text = message.Substring(message.IndexOf(' ') + 1).Replace("\\n", Environment.NewLine);
            
            if (type.ToLower() == "rules")
            {
                TextWriter tw = new StreamWriter(File.Create("text\\rules.txt"));
                tw.Write(text);
                tw.Flush();
                tw.Close();
                tw.Dispose();
                File.WriteAllText("text\\rules.txt", text);
            }
            if (type.ToLower() == "news")
            {
                TextWriter tw = new StreamWriter(File.Create("text\\news.txt"));
                tw.Write(text);
                tw.Flush();
                tw.Close();
                tw.Dispose();
                File.WriteAllText("text\\news.txt", text);
            }
            if (type.ToLower() == "messages")
            {
                TextWriter tw = new StreamWriter(File.Create("text\\messages.txt"));
                tw.Write(text);
                tw.Flush();
                tw.Close();
                tw.Dispose();
                File.WriteAllText("text\\messages.txt", text);
            }
            else Help(p);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/Edit <news/rules/messages> <message> - Edits news, messages or rules");
            Player.SendMessage(p, "Use \"\\n\" for new lines, use \">\" or \"<\" to extend message.");
        }
    }
}