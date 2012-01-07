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
using System.Data;
using System.Collections.Generic;
using MCForge.SQL;
//using MySql.Data.MySqlClient;
//using MySql.Data.Types;

namespace MCForge
{
    public class CmdDeleteLvl : Command
    {
        public override string name { get { return "deletelvl"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdDeleteLvl() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            //check if the player owns the map (map name is their name or starts with their name)
            if (p.group.Permission < LevelPermission.Operator)
            {
                if (message.Split('.').Length >= 2)
                {
                    if (message.Split('.')[0].ToLower() != p.name.ToLower())
                    {
                        Player.SendMessage(p, "You cannot delete maps you do not own");
                        return;
                    }
                }
                else
                {
                    if (message.ToLower() != p.name.ToLower())
                    {
                        Player.SendMessage(p, "You cannot delete maps you do not own");
                        return;
                    }
                }
            }

            Level foundLevel = Level.Find(message);
            if (foundLevel != null) foundLevel.Unload();

            if (foundLevel == Server.mainLevel) { Player.SendMessage(p, "Cannot delete the main level."); return; }

            try
            {
                if (!Directory.Exists("levels/deleted")) Directory.CreateDirectory("levels/deleted");

                if (File.Exists("levels/" + message + ".lvl"))
                {
                    if (File.Exists("levels/deleted/" + message + ".lvl"))
                    {
                        int currentNum = 0;
                        while (File.Exists("levels/deleted/" + message + currentNum + ".lvl")) currentNum++;

                        File.Move("levels/" + message + ".lvl", "levels/deleted/" + message + currentNum + ".lvl");
                    }
                    else
                    {
                        File.Move("levels/" + message + ".lvl", "levels/deleted/" + message + ".lvl");
                    }
                    Player.SendMessage(p, "Created backup.");

                    try { File.Delete("levels/level properties/" + message + ".properties"); }
                    catch { }
                    try { File.Delete("levels/level properties/" + message); }
                    catch { }

                    if (Server.useMySQL) MySQL.executeQuery("DROP TABLE `Block" + message + "`, `Portals" + message + "`, `Messages" + message + "`, `Zone" + message + "`"); else SQLite.executeQuery("DROP TABLE Block" + message + ", Portals" + message + ", Messages" + message + " Zone" + message + "");

                    Player.GlobalMessage("Level " + message + " was deleted.");
                }
                else
                {
                    Player.SendMessage(p, "Could not find specified level.");
                }
            }
            catch (Exception e) { Player.SendMessage(p, "Error when deleting."); Server.ErrorLog(e); }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/deletelvl [map] - Completely deletes [map] (portals, MBs, everything");
            Player.SendMessage(p, "A backup of the map will be placed in the levels/deleted folder");
        }
    }
}