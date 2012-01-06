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

namespace MCForge
{
    public class CmdOfflineAllow : Command
    {
        public override string name { get { return "offlineallow"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdOfflineAllow() { }
        public override void Use(Player p, string message)
        {
            Level lvl = null;

            if (message == "") { Help(p); return; }

            if (p.group.Permission > LevelPermission.Operator)
            {
                string[] parameters = message.Split(' ');
                if (parameters.Length == 2)
                {
                    message = parameters[1];
                    lvl = Level.Find(parameters[0]);
                    if (lvl == null)
                    {
                        Player.SendMessage(p, "Map not found");
                        return;
                    }
                }
                else lvl = Level.Find(p.name);
            }
            else
                lvl = Level.Find(p.name);

            allowPlayer(p, lvl, message);

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/offlineallow [player] - Adds offline player [player] on your maps build allow list");
            if (p.group.Permission > LevelPermission.Operator) Player.SendMessage(p, "/offlineallow [map] [player] - Adds offline player [player] on [maps] build allow list");
        }
        void allowPlayer(Player p, Level lvl, string message)
        {
            if (lvl == null)
            {
                Player.SendMessage(p, "You do not have a level, type /newlvl to make a new level");
                return;
            }

            lvl.AllowedPlayers.Add(message.ToLower());
            Player.SendMessage(p, "\"" + c.gold + message + Server.DefaultColor + "\" was added to the allow list.");
        }
    }
}
