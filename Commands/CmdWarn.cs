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
          string[] param = message.Split(' ');
          string reason;
          if (param[1] == "" || param[1] == " ")
          {
              reason = "you know why.";
          }
          else reason = message.Substring(message.IndexOf(" "));

          
          Player who = Player.Find(param[0]);


          if (p == null) //then it is a console
          {
              if (who == null)
              {
                  Player.SendMessage(null, "Player not found!");
				  return;
              }                           
              if (Server.devs.Contains(who.name))
              {
                  Player.SendMessage(null, "why are you warning a dev??");
                  return;
              }

              Player.GlobalMessage("console has %ewarned " + who.color + who.name + " %ebecause : &c");
              Player.GlobalMessage(reason);
              Player.SendMessage(who, "Do it again ");
              {
                  if (who.warn == 0)
                  {
                      Player.SendMessage(who, "twice and you get kicked!");
                      who.warn = who.warn + 1;
                      return;
                  }
                  if (who.warn == 1)
                  {
                      Player.SendMessage(who, "one more time and you get kicked!");
                      who.warn = who.warn + 1;
                      return;
                  }
                  if (who.warn == 2)
                  {
                      Player.GlobalMessage("console has warn kicked" + who.color + who.name + "%ebecause : &c");
                      Player.GlobalMessage(reason);
                      who.warn = 0;
                      who.Kick("BEACUASE " + reason + "");
                      return;
                  }
              }
          }
          if (who == null)
          {
              Player.SendMessage(p, "Player not found!");
          }
          if (who.ToString() == p.name)
          {
              Player.SendMessage(p, "you can't warn yourself");
              return;
          }
          if (p.group.Permission <= who.group.Permission)
          {
              Player.SendMessage(p, "you can't warn a player eqaul or higher rank.");
              return;
          }
          if (Server.devs.Contains(who.name))
          {
              Player.SendMessage(p, "why are you warning a dev??");
              return;
          }
          {
              Player.GlobalMessage(p.color + p.name + " %ewarned " + who.color + who.name + " %ebecause : &c");
              Player.GlobalMessage(reason);
              Player.SendMessage(who, "Do it again ");
              {
                  if (who.warn == 0)
                  {
                      Player.SendMessage(who, "twice and you get kicked!");
                      who.warn = who.warn + 1;
                      return;
                  }
                  if (who.warn == 1)
                  {
                      Player.SendMessage(who, "one more time and you get kicked!");
                      who.warn = who.warn + 1;
                      return;
                  }
                  if (who.warn == 2)
                  {
                      Player.GlobalMessage(p.color + p.name + "has warn kicked" + who.color + who.name + "%ebecause : &c");
                      Player.GlobalMessage(reason);
                      who.warn = 0;
                      who.Kick("BEACUASE " + reason + "");
                      return;
                  }
              }         
          }
      }
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/warn - Warns a player.");
         Player.SendMessage(p, "/warn <reason> - Warns a player and your reason is given.");
      }
   }
}