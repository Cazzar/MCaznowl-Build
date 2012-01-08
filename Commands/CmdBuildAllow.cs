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
    public class CmdBuildAllow : Command
    {
        public override string name { get { return "buildallow"; } }
        public override string shortcut { get { return "ballow"; } }
        public override string type { get { return "info"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdBuildAllow() { }
        public override void Use(Player p, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Help(p);
            }
            var remove = false;
            if (message.Split(' ').Length == 2)
            {
                if (message.Split(' ')[1].ToLower() == "remove")
                {
                    remove = true;
                    message = message.Split(' ')[0];
                }
                else
                    remove = false;
            }
            if (p == null) { Player.SendMessage(p, "You have to use this ingame!"); return; }
            Level lvl = null;
            Player who = null;
            who = Player.Find(message);
            lvl = p.level;

            if (lvl == null)
            {
                Player.SendMessage(p, "Level not found");
                return;
            }
            if (who == null)
            {
                Player.SendMessage(p, "Player not found, make sure thry are online!");
                return;
            }
            if (remove)
            {
                Player.SendMessage(p, "Removed " + who.color + who.name + Server.DefaultColor + "from the build bypass list");
                lvl.PerbuildEvade.Remove(who);
                return;
            }
            lvl.PerbuildEvade.Add(who);
            Player.SendMessage(p, "Added " + who.color + who.name + Server.DefaultColor + "to the build bypass list");

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/buildallow [name] <remove> - Allows [name] to bypass perbuild on the map you are currently on");
            Player.SendMessage(p, "Removes player from perbuild evasion if remove is specified");
        }
    }
}