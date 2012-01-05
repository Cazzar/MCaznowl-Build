using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    class CmdGlobal : Command
    {
        public override string name { get { return "global"; } }
        public override string shortcut { get { return "gc"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdGlobal() { }
        //bla
        public override void Use(Player p, string message)
        {
            if (String.IsNullOrEmpty(message)) { Help(p); return; }
            if (!Server.UseGlobalChat) { Player.SendMessage(p, "Global Chat is disabled."); return; }
            if (p != null && p.muted) { Player.SendMessage(p, "You are muted."); return; }
            if (p != null && p.muteGlobal) { Player.SendMessage(p, "You cannot use Global Chat while you have it muted."); return; }
            if (p != null && !Server.gcaccepted.Contains(p.name.ToLower())) { RulesMethod(p); return; }    
            Server.GlobalChat.Say((p != null ? p.name + ": " : "Console: ") + message);
            Player.GlobalMessage(Server.GlobalChatColor + "<[Global] " + (p != null ? p.name + ": " : "Console: ") + "&f" + (Server.profanityFilter ? ProfanityFilter.Parse(message) : message), true);
            try { Gui.Window.thisWindow.LogGlobalChat("< " + (p != null ? p.name + ": " : "Console: ") + message); }
            catch { Server.s.Log("<[Global] " + (p != null ? p.name + ": " : "Console: ") + message); }
        }
        public void RulesMethod(Player p)
        {
            Player.SendMessage(p, "&cBy using the Global Chat you agree to the following rules:");
            Player.SendMessage(p, "1. No Spamming");
            Player.SendMessage(p, "2. No Advertising (Trying to get people to your server)");
            Player.SendMessage(p, "3. No links");
            Player.SendMessage(p, "4. No Excessive Cursing (You are allowed to curse, but not pointed at anybody)");
            Player.SendMessage(p, "5. No use of $ Variables.");
            Player.SendMessage(p, "6. English only. No exceptions.");
            Player.SendMessage(p, "7. Be respectful");
            Player.SendMessage(p, "8. Do not ask for ranks");
            Player.SendMessage(p, "9. Do not ask for a server name");
            Player.SendMessage(p, "10. Use common sense.");
            Player.SendMessage(p, "11. Don't say any server name");
            Player.SendMessage(p, "&1Type /gcaccept to accept these rules");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/global [message] - Send a message to Global Chat.");
        }
    }
}
