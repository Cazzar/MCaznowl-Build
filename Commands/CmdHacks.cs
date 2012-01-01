﻿/*
	Copyright 2010 MCLawl Team - Written by Valek (Modified for use with MCForge)
 
   
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
using System.Linq;
using System.Text;

namespace MCForge
{
    public class CmdHacks : Command
    {
        public override string name { get { return "hacks"; } }
        public override string shortcut { get { return "hax"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdHacks() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            p.Kick("Your IP has been backtraced + reported to FBI Cyber Crimes Unit.");

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/hacks - GIVES YOU CONTROL OF THE SERVER!");
        }
    }

}
