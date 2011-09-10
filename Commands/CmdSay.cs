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
    class CmdSay : Command
    {
        public override string name { get { return "say"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdSay() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            message = message.Replace("%", "&"); // Alow colors in global messages
            Player.GlobalMessage(message);
            message = message.Replace("&", ""); // converts the MC color codes to IRC. Doesn't seem to work with multiple colors
            //IRCBot.Say(message);
            Server.IRC.Say(message);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/say - broadcasts a global message to everyone in the server.");
        }
    }
}
