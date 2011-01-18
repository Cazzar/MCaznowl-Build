using System;

namespace MCForge
{
   public class CmdKickAll : Command
   {
      public override string name { get { return "kickall"; } }
      public override string shortcut { get { return ""; } }
      public override string type { get { return "mod"; } }
      public override bool museumUsable { get { return true; } }
      public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
      public override void Help(Player p){Player.SendMessage(p, "/kickall - Kicks all players from the server.");}
      public override void Use(Player p, string message)
      {
         p.Kick("Kicked for trying to kick all players from the server!");
      }
   }
}