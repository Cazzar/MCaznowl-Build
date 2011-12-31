﻿/*
	Copyright 2011 MCForge

	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;

namespace MCForge
{
   public class CmdFakeRank : Command
   {
      public override string name { get { return "fakerank"; } }
      public override string shortcut { get { return "frk"; } } 
      public override string type { get { return "other"; } }
      public override bool museumUsable { get { return true; } }
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/fakerank <name> <rank> - Sends a fake rank change message.");
      }
      public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
      public override void Use(Player p, string message)
      {
	  
         if (message == ""){Help(p); return;}
		 
         Player plr = Player.Find(message.Split (' ')[0]);
         Group grp = Group.Find(message.Split (' ')[1]);
		 
         if (plr == null)
         {
            Player.SendMessage(p, Server.DefaultColor + "Player not found!");
            return;
         }
         if (grp == null)
         {
             Player.SendMessage(p, Server.DefaultColor + "No rank entered.");
             return;
         }
         if (Group.GroupList.Contains(grp))
         {
             
             if (grp.name == "banned")
             {
                 Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + " is now &8banned" + Server.DefaultColor + "!");
             }
             else
             {
                 Player.GlobalMessage(plr.color + plr.name + Server.DefaultColor + "'s rank was set to " + grp.color + grp.name + Server.DefaultColor + ".");
                 Player.GlobalMessage("&6Congratulations!");
             }
         }
   
         else
         {
             Player.SendMessage(p, Server.DefaultColor + "Invalid Rank Entered!");
             return;
         }

      }
   }
}
