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
using System.Linq;
using System.Text;

namespace MCForge
{
    public class CmdReview : Command
    {
        public override string name { get { return "review"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdReview() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            bool isopson = false;
            try
            {
                foreach (Player pl in Player.players)
                {
                    if (pl.group.Permission >= Server.opchatperm)
                    {
                        if (pl.hidden == false)
                        {
                            isopson = true;
                        }
                    }
                }
            }
            catch/* (Exception e)*/
            {
                isopson = true;
            }
            if (isopson == true)
            {
                Player.SendMessage(p, "You requested that operators see your building. They should be coming soon.");
                Player.GlobalMessageOps(p.color + p.name + "-" + Server.DefaultColor + " requests a review of their building");
            }
            else
            {
                Player.SendMessage(p, "There are no operators on to review your build. Please wait for one to come on or come back later.");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/review - Requests that ops come and look at your building. Don't abuse this.");
        }
    }

}
