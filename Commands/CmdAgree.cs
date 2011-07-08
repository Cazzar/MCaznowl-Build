/*
 * Written By Jack1312

	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.osedu.org/licenses/ECL-2.0
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;

namespace MCForge
{
    public class CmdAgree : Command
    {
        public override string name { get { return "agree"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdAgree() { }

        public override void Use(Player p, string message)
        {
            if (Server.agreetorulesonentry == false)
            {
                Player.SendMessage(p, "This command can only be used if agree-to-rules-on-entry is enabled in the console!");
                return;
            }
            if (p == null)
            {
                Player.SendMessage(p, "This command can only be used in-game");
                return;
            }
            if (p.group.Permission > LevelPermission.Guest)
            {
                Player.SendMessage(p, "Your rank is higher than guest and you have already agreed to the rules!");
                return;
            }
            var agreed = File.ReadAllText("ranks/agreed.txt");
            var checklogs = File.ReadAllText("logs/" + DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-" + DateTime.Now.ToString("dd") + ".txt");
            if (!checklogs.Contains(p.name.ToLower() + " used /rules"))
            {
                Player.SendMessage(p, "&9You must read /rules before agreeing!");
                return;
            }
            if (agreed.Contains(p.name.ToLower()))
            {
                Player.SendMessage(p, "You have already agreed to the rules!");
                return;
            }
            p.jailed = false;
            Player.SendMessage(p, "Thankyou, for agreeing to follow the rules. You may now build and use commands!");
            string playerspath = "ranks/agreed.txt";
            if (File.Exists(playerspath))
            {
                File.AppendAllText(playerspath, p.name.ToLower());


            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/agree - Agree to the rules when entering the server");
        }
    }
}