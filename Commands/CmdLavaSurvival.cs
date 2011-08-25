using System;

namespace MCForge
{
    class CmdLavaSurvival : Command
    {
        public override string name { get { return "lavasurvival"; } }
        public override string shortcut { get { return "ls"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdLavaSurvival() { }

        public override void Use(Player p, string message)
        {
            
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/lavasurvival - Various commands to setup Lava Survival.");
        }
    }
}
