/*
	Copyright 2010 MCForge Team - Written by cazzar
 
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

using MCForge;
namespace MCForge.Commands
{
    public class CmdLiked : Command
    {
        public override string name { get { return "liked"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "info"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdLiked() { }
        public override void Use(Player p, string message)
        {
            Level lvl = null;
            lvl = Level.Find(message);

            if (lvl == null)
            {
                Player.SendMessage(p, "Level not found");
                return;
            }

            Player.SendMessage(p, "Liked: " + c.gold + string.Join(", ", lvl.Liked.ToArray()));

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/liked [map] - Shows the players that liked/disliked [map]");
        }
    }
}