namespace MCForge
{
    using System;
    public class CmdUnflood : Command
    {
        public override string name { get { return "unflood"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Help(Player p) { Player.SendMessage(p, "/unflood [liquid] - Unfloods the map you are on of [liquid]"); }
        public override void Use(Player p, string message)
        {
            Level level = Level.Find(p.level.name);
            bool instant = level.Instant;
            if (message == "all")
            {
                Command.all.Find("physics").Use(p, "0");
                if (instant == false)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("replaceall").Use(p, "lavafall air");
                Command.all.Find("replaceall").Use(p, "waterfall air");
                Command.all.Find("replaceall").Use(p, "lava_fast air");
                Command.all.Find("replaceall").Use(p, "active_lava air");
                Command.all.Find("replaceall").Use(p, "active_water air");
                Command.all.Find("replaceall").Use(p, "active_hot_lava air");
                Command.all.Find("replaceall").Use(p, "magma air");
                Command.all.Find("reveal").Use(p, "all");
                if (instant == true)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("physics").Use(p, "1");
                Player.GlobalMessage("Unflooded!");
            }
            else
            {
                Command.all.Find("physics").Use(p, "0");
                if (instant == false)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("replaceall").Use(p, message + " air");
                Command.all.Find("reveal").Use(p, "all");
                if (instant == true)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("physics").Use(p, "1");
                Player.GlobalMessage("Unflooded!");
            }
        }
    }
}