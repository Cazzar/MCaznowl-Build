using System;

namespace MCForge
{
   public class CmdWarn : Command
   {
      public override string name { get { return "warn"; } }

      public override string shortcut { get { return ""; } }

      public override string type { get { return "mod"; } }

      public override bool museumUsable { get { return true; } }

      public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            Player who = Player.Find(message.Split(' ')[0]);
            if (who == null)
            {
                Player.SendMessage(p, "Player not found!");
            }
            {
                Player.GlobalMessage(p.color + p.name + "%ewarned" + who.color + who.name + "%e!");
                Player.SendMessage(p, "%eDo it again you could be banned!");
            }
        }

      public override void Help(Player p)
      {
         Player.SendMessage(p, "/warn - Warns a player.");
      }
   }
}