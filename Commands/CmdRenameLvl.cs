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
//using MySql.Data.MySqlClient;
//using MySql.Data.Types;

namespace MCForge
{
    public class CmdRenameLvl : Command
    {
        public override string name { get { return "renamelvl"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdRenameLvl() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') == -1) { Help(p); return; }
            Level foundLevel = Level.Find(message.Split(' ')[0]);
            if (foundLevel == null)
            {
                Player.SendMessage(p, "Level not found");
                return;
            }

            string newName = message.Split(' ')[1];

            if (File.Exists("levels/" + newName + ".lvl")) { Player.SendMessage(p, "Level already exists."); return; }
            if (foundLevel == Server.mainLevel) { Player.SendMessage(p, "Cannot rename the main level."); return; }

            foundLevel.Unload();

            try
            {
                File.Move("levels/" + foundLevel.name + ".lvl", "levels/" + newName + ".lvl");

                try
                {
                    File.Move("levels/level properties/" + foundLevel.name + ".properties", "levels/level properties/" + newName + ".properties");
                }
                catch { }
                try
                {
                    File.Move("levels/level properties/" + foundLevel.name, "levels/level properties/" + newName + ".properties");
                }
                catch { }

                //Move and rename backups
                try
                {
                    for (int i = 1; ; i++)
                    {
                        string foundLevelDir = @Server.backupLocation + "/" + foundLevel.name + "/" + i + "/";
                        string newNameDir = @Server.backupLocation + "/" + newName + "/" + i + "/";

                        if (File.Exists(foundLevelDir + foundLevel.name + ".lvl"))
                        {
                            Directory.CreateDirectory(newNameDir);
                            File.Move(foundLevelDir + foundLevel.name + ".lvl", newNameDir + newName + ".lvl");
                            if (DirectoryEmpty(foundLevelDir))
                                Directory.Delete(foundLevelDir);
                        }
                        else
                        {
                            File.Delete("levels/" + foundLevel.name + ".lvl.backup");
                            if (Directory.Exists(@Server.backupLocation + "/" + foundLevel.name + "/"))
                            {
                                if (DirectoryEmpty(@Server.backupLocation + "/" + foundLevel.name + "/"))
                                    Directory.Delete(@Server.backupLocation + "/" + foundLevel.name + "/");
                            }
                            break;
                        }
                    }
                }
                catch { }

                if (Server.useMySQL == true)
                {
                    MySQL.executeQuery("RENAME TABLE `Block" + foundLevel.name.ToLower() + "` TO `Block" + newName.ToLower() +
                        "`, `Portals" + foundLevel.name.ToLower() + "` TO `Portals" + newName.ToLower() +
                        "`, `Messages" + foundLevel.name.ToLower() + "` TO Messages" + newName.ToLower() +
                        ", `Zone" + foundLevel.name.ToLower() + "` TO `Zone" + newName.ToLower() + "`");
                }
                try { Command.all.Find("load").Use(p, newName); }
                catch { }
                Player.GlobalMessage("Renamed " + foundLevel.name + " to " + newName);
            }
            catch (Exception e) { Player.SendMessage(p, "Error when renaming."); Server.ErrorLog(e); }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/renamelvl <level> <new name> - Renames <level> to <new name>");
            Player.SendMessage(p, "Portals going to <level> will be lost");
        }

        public static bool DirectoryEmpty(string dir)
        {
            if (System.IO.Directory.GetDirectories(dir).Length > 0)
                return false;
            if (System.IO.Directory.GetFiles(dir).Length > 0)
                return false;

            return true;
        }
    }
}