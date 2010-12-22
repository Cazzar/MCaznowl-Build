namespace MCForge
{
    public class CmdOZone : Command
    {
        public override string name { get { return "ozone"; } }
        public override string shortcut { get { return "oz"; } }
        public override string type { get { return "mod"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool museumUsable { get { return false; } }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ozone <rank/player> - Zones the entire map to <rank/player>");
            Player.SendMessage(p, "To delete a zone, just use /zone del anywhere on the map");
        }
        public override void Use(Player p, string message)
        {
            if (message == "") { this.Help(p); }
            else
            {
                int x2 = p.level.width - 1;
                int y2 = p.level.depth - 1;
                int z2 = p.level.height - 1;
                Command zone = Command.all.Find("zone");
                Command click = Command.all.Find("click");
                zone.Use(p, "add " + message);
                click.Use(p, 0 + " " + 0 + " " + 0);
                click.Use(p, x2 + " " + y2 + " " + z2);
            }
        }
    }
}