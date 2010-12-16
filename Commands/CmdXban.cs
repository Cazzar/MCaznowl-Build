using System;

namespace MCForge
{
   public class CmdXban : Command
   {
      
      public override string name { get { return "xban"; } }

      public override string shortcut { get { return ""; } }

      public override string type { get { return "other"; } }
   
      public override bool museumUsable { get { return false; } }

      public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }

public CmdXban() { }      
public override void Use(Player p, string message)
               
      {

                      if (message == "") { Help(p); return; }

                      Player who = Player.Find(message.Split(' ')[0]);
                      if (who != null) {

                     Command.all.Find("ban").Use(p,message);
                     Command.all.Find("undo").Use(p, message+" all");
                     Command.all.Find("banip").Use(p, "@"+message);
                     Command.all.Find("kick").Use(p,message);
                     Command.all.Find("undo").Use(p,message+" all");

                                       }

                      else {

                           Command.all.Find("ban").Use(p,message);                           
                           Command.all.Find("banip").Use(p, "@"+message);
                           Command.all.Find("undo").Use(p, message+" all");
                           
                            }
                     
                     
                                         
                     
                         
      }

      
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/xban [name] - undo [name] all + banip + ban + kick ^^ ");
      }
   }
}