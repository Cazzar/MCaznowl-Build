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

           if (splitted[0] == RemoteServer.username && splitted[1] == RemoteServer.password)
               return true;
           else

               return false;
       }
       public void RemoteChat(string m)
       {
           if (m[0] == '/')
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

           Server.s.Log("[Remote]: " + m);
           Player.GlobalMessage(c.navy + "[Remote]: " + c.white + m);
           return ;
       }

       }
    }
