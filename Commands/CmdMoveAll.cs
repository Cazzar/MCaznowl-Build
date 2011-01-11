using System;
namespace MCForge
{

    public class CmdMoveAll : Command
    {
        public override string name { get { return "moveall"; } }
        public override string shortcut { get { return "ma"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            Level level = Level.Find(message.Split(' ')[0]);
            if (level == null) { Player.SendMessage(p, "There is no level named '" + message.Split(' ')[0] + "'."); return; }
            foreach (Player pl in Player.players) { if (pl.group.Permission < p.group.Permission) Command.all.Find("move").Use(p, pl.name + " " + level.name); else Player.SendMessage(p, "You cannot move " + pl.color + pl.name + Server.DefaultColor + " because they are of equal or higher rank"); }
        }
        public override void Help(Player p) { Player.SendMessage(p, "/moveall <level> - Moves all players to the level specified."); }
    }
}