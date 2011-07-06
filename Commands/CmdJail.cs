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

namespace MCForge
{
    public class CmdJail : Command
    {
        public override string name { get { return "jail"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdJail() { }

        public override void Use(Player p, string message)
        {
            if ((message.ToLower() == "set") && p != null)
            {
                p.level.jailx = p.pos[0]; p.level.jaily = p.pos[1]; p.level.jailz = p.pos[2];
                p.level.jailrotx = p.rot[0]; p.level.jailroty = p.rot[1];
                Player.SendMessage(p, "Set Jail point.");
            }
            else
            {
                Player who = Player.Find(message);
                if (who != null)
                {
                    if (!who.jailed)
                    {
                        if (p != null) if (who.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot jail someone of equal or greater rank."); return; }
                        Player.GlobalDie(who, false);
                        Player.GlobalSpawn(who, p.level.jailx, p.level.jaily, p.level.jailz, p.level.jailrotx, p.level.jailroty, true);
                        who.jailed = true;
                        Player.GlobalChat(null, who.color + who.name + Server.DefaultColor + " was &8jailed", false);
                    }
                    else
                    {
                        who.jailed = false;
                        Player.GlobalChat(null, who.color + who.name + Server.DefaultColor + " was &afreed" + Server.DefaultColor + " from jail", false);
                    }
                }
                else
                {
                    Player.SendMessage(p, "Could not find specified player.");
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/jail [user] - Places [user] in jail unable to use commands.");
            Player.SendMessage(p, "/jail [set] - Creates the jail point for the map.");
            Player.SendMessage(p, "This command has been deprecated in favor of /xjail.");
        }
    }
}