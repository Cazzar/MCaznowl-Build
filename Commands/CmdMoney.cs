/*
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
	public class CmdMoney : Command
	{
		public override string name { get { return "money"; } }
		public override string shortcut { get { return ""; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return true; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public override void Use(Player p, string message)
		{
            if (message == "")
            {
                Player.SendMessage(p, "You currently have " + p.money + " " + Server.moneys + ".");
            }
            else
            {
                Player who = Player.Find(message);
                if (who == null)
                {
                    Player.SendMessage(p, "Error: Player is not online.");
                    return;
                }
                if (who.group.Permission >= p.group.Permission)
                {
                    Player.SendMessage(p, "Cannot see the money of someone of equal or greater rank.");
                    return;
                }

                Player.SendMessage(p, who.color + who.name + Server.DefaultColor + " currently has " + who.money + " " + Server.moneys + ".");
            }  
        }

		public override void Help(Player p)
		{
			Player.SendMessage(p, "/money <player> - Shows how much " + Server.moneys + " <player> has");
		}
	}
}