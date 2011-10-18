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

        public override void Use(Player p, string message)
        {
            if (String.IsNullOrEmpty(message)) { Help(p); return; }
            if (!Server.UseGlobalChat) { Player.SendMessage(p, "Global Chat is disabled."); return; }
            if (p != null && p.muted) { Player.SendMessage(p, "You are muted."); return; }
            if (p != null && p.muteGlobal) { Player.SendMessage(p, "You cannot use Global Chat while you have it muted."); return; }
                
            Server.GlobalChat.Say((p != null ? p.name + ": " : "Console: ") + message);
            Player.GlobalMessage(Server.GlobalChatColor + "<[Global] " + (p != null ? p.name + ": " : "Console: ") + "&f" + (Server.profanityFilter ? ProfanityFilter.Parse(message) : message), true);
            try { Gui.Window.thisWindow.LogGlobalChat("< " + (p != null ? p.name + ": " : "Console: ") + message); }
            catch { Server.s.Log("<[Global] " + (p != null ? p.name + ": " : "Console: ") + message); }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/global [message] - Send a message to Global Chat.");
        }
    }
}
