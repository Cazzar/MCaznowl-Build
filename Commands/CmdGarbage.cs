using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    class CmdGarbage : Command
    {
        public override string name { get { return "garbage"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
        public override string keywords { get { return "memory clean unused"; } }
        public CmdGarbage() { }

        public override void Use(Player p, string message)
        {
            Player.SendMessage(p, "Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Player.SendMessage(p, "Garbage collection completed!");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/garbage - Forces the .NET garbage collector to run, which releases unused memory. You shouldn't need to use this often.");
        }
    }
}
