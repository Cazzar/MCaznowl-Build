﻿/*
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
    public class CmdAllowed : Command
    {
        public override string name { get { return "allowed"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "info"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdAllowed() { }
        public override void Use(Player p, string message)
        {
            Level lvl = null;

            if (p.group.Permission > LevelPermission.Operator)
            {
                if (!String.IsNullOrEmpty(message))
                {
                    lvl = Level.Find(message);
                    if (lvl == null)
                    {
                        Player.SendMessage(p, "Map not found");
                        return;
                    }
                }
                else
                {
                    lvl = Level.Find(p.name);
                    if (lvl == null)
                    {
                        Player.SendMessage(p, "You do not have a level, type /newlvl to make a new level");
                        return;
                    }
                }
            }
            else
            {
                lvl = Level.Find(p.name);
                if (lvl == null)
                {
                    Player.SendMessage(p, "You do not have a level, type /newlvl to make a new level");
                    return;
                }
            }
            string allowed = string.Join(", ", lvl.AllowedPlayers.ToArray());

            Player.SendMessage(p, string.IsNullOrEmpty(allowed) ? "Nobody is allowed to build" : "Allowed players: " + c.gold + allowed);

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/allowed <map> - Shows the players on your maps build allow list");
            Player.SendMessage(p, "Use /allowed <map> to check your extra maps or who can build on others maps");
        }
    }
}