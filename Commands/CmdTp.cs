/*
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

namespace MCForge
{
    public class CmdTp : Command
    {
        public override string name { get { return "tp"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdTp() { }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                Command.all.Find("spawn");
                return;
            }
            Player who = Player.Find(message);
            if (who == null || (who.hidden && p.group.Permission < LevelPermission.Admin)) { Player.SendMessage(p, "There is no player \"" + message + "\"!"); return; }
            if (p.level != who.level)
            {
                if(who.level.name.Contains("cMuseum"))
                {
                    Player.SendMessage(p, "Player \"" + message + "\" is in a museum!");
                    return;
                }
                else
                {
                    Command.all.Find("goto").Use(p, who.level.name);
                }
            }
            if (p.level == who.level)
            {
                if (who.Loading)
                {
                    Player.SendMessage(p, "Waiting for " + who.color + who.name + Server.DefaultColor + " to spawn...");
                    while (who.Loading) { }
                }
                while (p.Loading) { }  //Wait for player to spawn in new map
                unchecked { p.SendPos((byte)-1, who.pos[0], who.pos[1], who.pos[2], who.rot[0], 0); }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tp <player> - Teleports yourself to a player.");
            Player.SendMessage(p, "If <player> is blank, /spawn is used.");
        }
    }
}