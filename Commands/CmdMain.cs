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
    public class CmdMain : Command
    {
        public override string name { get { return "main"; } }
        public override string shortcut { get { return "h"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdMain() { }

        public override void Use(Player p, string message)
        {
            if (p.level.name == Server.mainLevel.name) { Player.SendMessage(p, "You are already on the servers main level!"); return; }
            Command.all.Find("goto").Use(p, Server.mainLevel.name);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/main - Sends you to the main level.");
        }
    }
}