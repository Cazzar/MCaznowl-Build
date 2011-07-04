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
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace MCForge
{
    public class CmdReload : Command
    {
        public override string name { get { return "reload"; } }
        public override string shortcut { get { return "rd"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdReload() { }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/reload <map> - Reloads the specified map. Uses the current map if no message is given.");
        }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            {
                if (!File.Exists("levels/" + message + ".lvl"))
                {
                    Player.SendMessage(p, "The specified level does not exist!");
                    return;
                }
                if (Server.mainLevel.name != message) ;
                {
                    Command.all.Find("unload").Use(p, message);
                    Command.all.Find("load").Use(p, message);
                    Command.all.Find("goto").Use(p, message);
                    foreach (Player pl in Player.players)
                    {
                        if (pl.level == p.level && pl != p)
                        {
                            unchecked { pl.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0); }
                            Command.all.Find("goto").Use(pl, message);
                        }
                    }
                    Player.SendMessage(p, "&cThe map, " + message + " has been reloaded!");
                    IRCBot.Say("The map, " + message + " has been reloaded.");
                    Server.s.Log("The map, " + message + " was reloaded by " + p.name);
                    return;
                }
                Player.SendMessage(p, "You cannot reload the main level!");

            }
        }
    }
}