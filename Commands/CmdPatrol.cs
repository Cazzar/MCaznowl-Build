/*
        
        Written by Jack1312
        
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
using System.Collections.Generic;

using MCForge;
namespace MCForge.Commands
{
    class CmdPatrol : Command
    {
        public override string name { get { return "patrol"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdPatrol() { }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/patrol - Teleports you to a random " + Group.findPermInt(CommandOtherPerms.GetPerm(this)).name + " or lower");
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                Help(p);
                return;
            }
            if (p == null)
            {
                Player.SendMessage(p, "Are you stupid? =S You can't use this in the console!");
                return;
            }
            List<string> getpatrol = new List<string>();
            foreach (Player pl in Player.players)
            {
                if ((int)pl.group.Permission <= CommandOtherPerms.GetPerm(this))
                {
                    getpatrol.Add(pl.name);
                }
            }
            if (getpatrol.Count <= 0)
            {
                Player.SendMessage(p, "There must be at least one guest online to use this command!");
                return;
            }
            Random random = new Random();
            int index = random.Next(getpatrol.Count);
            string value = getpatrol[index];
            Player who = Player.Find(value);
            Command.all.Find("tp").Use(p, who.name);
            Player.SendMessage(p, "You are now visiting " + who.color + who.name + "!");
        }
    }
}