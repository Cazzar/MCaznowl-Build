/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
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
using System.Collections.Generic;
using System.Text;

namespace MCForge
{
    class CmdViewNext : Command
    {
        public override string name { get { return "viewnext"; } }
        public override string shortcut { get { return "vn"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdViewNext() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Player.SendMessage(p, "You can't execute this command as Console!");
                return;
            }
            string[] user = Server.viewlist.ToArray();
            Player who = Player.Find(user[0]);
            if (who == null)
            {
                Player.SendMessage(p, "Player " + user[0] + " doesn't exist or is offline. " + user[0] + " has been removed from the viewlist");
                Server.viewlist.Remove(user[0]);
                return;
            }
            if (who == p)
            {
                Player.SendMessage(p, "You can't teleport to yourself! You have been removed from the viewlist.");
                Server.viewlist.Remove(user[0]);
                return;
            }
            Server.viewlist.Remove(user[0]);
            unchecked { p.SendPos((byte)-1, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1]); }
            Player.SendMessage(p, "You have been teleported to " + user[0]);
            Player.SendMessage(who, "Your request has been answered by " + p.name + ".");
            int toallplayerscount = 0;
            foreach (string toallplayers in Server.viewlist)
            {
                Player who2 = Player.Find(toallplayers);
                Player.SendMessage(who2, "The viewlist has been rotated. you now have " + toallplayerscount.ToString() + " players waiting in front of you");
                toallplayerscount++;
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/viewnext - teleports you to the next player on the viewlist and rotates the list");
        }
    }
}
