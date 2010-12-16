using System;

namespace MCForge
{
   public class CmdReload : Command
   {
      public override string name { get { return "reload"; } }
      public override string shortcut { get { return "re"; } }
      public override string type { get { return "other"; } }
      public override bool museumUsable { get { return false; } }
      public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
      public override void Use(Player p, string message)
      {
         Command.all.Find("unload").Use(p, message);
         Command.all.Find("load").Use(p, message);
      }

      // This one controls what happens when you use /help [commandname].
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/reload [map name] - Reloads the map.");
      }
   }
}