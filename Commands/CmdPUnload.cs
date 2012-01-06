using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MCForge;
namespace MCForge.Commands
{
    /// <summary>
    /// This is the command /punload
    /// use /help punload in-game for more info
    /// </summary>
    public class CmdPUnload : Command
    {
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool museumUsable { get { return true; } }
        public override string name { get { return "punload"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override void Use(Player p, string message)
        {
            if (Plugin.Find(message) != null)
                Plugin.Unload(Plugin.Find(message), false);
            else
                Player.SendMessage(p, "That plugin is not loaded!");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/punload <Plugin name> - Unload a plugin that is loaded");
        }
        public CmdPUnload() { }
    }
}
