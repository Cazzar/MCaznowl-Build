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
using System.Collections.Generic;


namespace MCForge
{
    public class CmdDrop : Command
    {
        public override string name { get { return "drop"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public CmdDrop() { }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            if (p.hasflag != null)
            {
                p.level.ctfgame.DropFlag(p, p.hasflag);
                return;
            }
            else
            {
                Player.SendMessage(p, "You are not carrying a flag.");
            }

        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/drop - Drop the flag if you are carrying it.");
        }
    }
}
