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
    public class CmdSummon : Command
    {
        public override string name { get { return "summon"; } }
        public override string shortcut { get { return "s"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdSummon() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (message.ToLower() == "all")
            {
                foreach (Player pl in Player.players)
                {
                    if (pl.level == p.level && pl != p)
                    {
                        unchecked { pl.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0); }
                        pl.SendMessage("You were summoned by " + p.color + p.name + Server.DefaultColor + ".");
                    }
                }
                return;
            }

            Player who = Player.Find(message);
            if (who == null || who.hidden) { Player.SendMessage(p, "There is no player \"" + message + "\"!"); return; }
            if (p.level != who.level) { Player.SendMessage(p, who.name + " is in a different level."); return; }
            unchecked { who.SendPos((byte)-1, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0); }
            who.SendMessage("You were summoned by " + p.color + p.name + Server.DefaultColor + ".");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/summon <player> - Summons a player to your position.");
            Player.SendMessage(p, "/summon all - Summons all players in the map");
        }
    }
}