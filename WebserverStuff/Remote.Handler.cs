using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Remote
{
   public partial class Remote
    {
       public bool HandleLogin(string message)
       {
           string[] splitted = message.Split(':');

           if (splitted[0] == "head" && splitted[1] == "lols")
           { username = splitted[0]; password = splitted[1];  return true; }
           else

           return false;
       }
       public void RemoteChat(string m)
       {
           if (m[0] == '/') //in future maybe use config defined character
           {
               m = m.Remove(0,1);
               string[] args = m.Split(' ');
               string cmd = args[0];
               Command command = Command.all.Find(cmd);

               if (command == null)
               {
                   Server.s.Log("Unrecognized command: " + cmd);
                   return;
               }

                   StringBuilder lol = new StringBuilder();
                   for(int i = 1; i <= args.Length - 1; i++)
                   {
                       lol.Append(args[i]);
                   }
                  
                   command.Use(null, lol.ToString());
                   return;

           }
           //else if (m[0] == '@')
           //{
           //    if (m[1] != ' ')
           //    {
           //        HandleCommand("msg", m.Substring(1));  //fix later
           //    }
           //    else if (m[1] == ' ')
           //    {
           //        HandleCommand("msg", m.Substring(2));
           //    }
           //}
           Server.s.Log("[Remote]: " + m);
           Player.GlobalMessage(c.navy + "[Remote]: " + c.white + m);
           return ;
       }

       /*public void RemoteCommand(string cmd, params string[] args)
       {
           Command command = Command.all.Find(cmd);
           if (command == null)
           {
               Server.s.Log("Unrecognized command: " + cmd);
               return;
           }

           if (command.ConsoleUseable)
           {
               command.Use(remotePlayer, args);
               return;
           }
           else
           {
               Server.ServerLogger.Log(LogLevel.Info, cmd + " command not useable in the console.");
               return;
           }*/
       }
    }
