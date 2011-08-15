using System;
using System.IO;

namespace MCForge
{
    public class pload : Command
    {
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool museumUsable { get { return true; } }
        public override string name { get { return "pload"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "Moderate"; } }
        public override void Use(Player p, string message)
        {
            if (File.Exists("plugins/" + message + ".dll"))
                Plugin.Load(message, false);
            else
                Player.SendMessage(p, "Plugin not found!");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/pload <filename> - Load a plugin in your plugins folder!");
        }
        public pload() { }
    }
}
