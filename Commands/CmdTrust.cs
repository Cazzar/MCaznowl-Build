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

namespace MCForge
{
    public class CmdTrust : Command
    {
        public override string name { get { return "trust"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "moderation"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdTrust() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') != -1) { Help(p); return; }

            Player who = Player.Find(message);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player specified");
                return;
            }
            else
            {
                who.ignoreGrief = !who.ignoreGrief;
                Player.SendMessage(p, who.color + who.name + Server.DefaultColor + "'s trust status: " + who.ignoreGrief);
                who.SendMessage("Your trust status was changed to: " + who.ignoreGrief);
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/trust <name> - Turns off the anti-grief for <name>");
        }
    }
}