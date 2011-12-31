/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
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
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace MCForge
{
    public class Group
    {
        public delegate void RankSet(Player p, Group newrank);
        public static event RankSet OnPlayerRankSet;
        public delegate void GroupSave();
        public static event GroupSave OnGroupSave;
        public delegate void GroupLoad();
        public static event GroupLoad OnGroupLoad;
        public static bool cancelrank/* = false*/;
        //Move along...nothing to see here...
        public static void because(Player p, Group newrank) { if (OnPlayerRankSet != null) OnPlayerRankSet(p, newrank); }
        private string _name; 

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        private string _trueName; 

        public string trueName
        {
            get
            {
                return _trueName;
            }
            set
            {
                _trueName = value;
            }
        }
        private string _color; 

        public string color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }
        private LevelPermission _permission; 

        public LevelPermission Permission
        {
            get
            {
                return _permission;
            }
            set
            {
                _permission = value;
            }
        }
        private int _maxBlocks; 

        public int maxBlocks
        {
            get
            {
                return _maxBlocks;
            }
            set
            {
                _maxBlocks = value;
            }
        }
        private long _maxUndo; 

        public long maxUndo
        {
            get
            {
                return _maxUndo;
            }
            set
            {
                _maxUndo = value;
            }
        }
        private CommandList _commands; 

        public CommandList commands
        {
            get
            {
                return _commands;
            }
            set
            {
                _commands = value;
            }
        }
        private string _fileName; 

        public string fileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
        private PlayerList _playerList; 

        public PlayerList playerList
        {
            get
            {
                return _playerList;
            }
            set
            {
                _playerList = value;
            }
        }

        public Group()
        {
            Permission = LevelPermission.Null;
        }

        public Group(LevelPermission Perm, int maxB, long maxUn, string fullName, char newColor, string file)
        {
            Permission = Perm;
            maxBlocks = maxB;
            maxUndo = maxUn;
            trueName = fullName;
            name = trueName.ToLower(CultureInfo.CurrentCulture);
            color = "&" + newColor;
            fileName = file;
            if (name != "nobody")
                playerList = PlayerList.Load(fileName, this);
            else
                playerList = new PlayerList();
        }

        public void fillCommands()
        {
            CommandList _commands = new CommandList();
            GrpCommands.AddCommands(out _commands, Permission);
            commands = _commands;
        }

        public bool CanExecute(Command cmd) { return commands.Contains(cmd); }

        public static List<Group> GroupList = new List<Group>();
        public static Group standard;
        public static void InitAll()
        {
            GroupList = new List<Group>();

            if (File.Exists("properties/ranks.properties"))
            {
                string[] lines = File.ReadAllLines("properties/ranks.properties");

                Group thisGroup = new Group();
                int gots = 0, version = 1;
                if (lines.Length > 0 && lines[0] == "#Version 2") version = 2;

                foreach (string s in lines)
                {
                    try
                    {
                        if (!(s != null && String.IsNullOrEmpty(s)) && s[0] != '#')
                        {
                            if (s.Split('=').Length == 2)
                            {
                                string property = s.Split('=')[0].Trim();
                                string value = s.Split('=')[1].Trim();

                                if ((thisGroup.name != null && String.IsNullOrEmpty(thisGroup.name)) && property.ToLower(CultureInfo.CurrentCulture) != "rankname")
                                {
                                    Server.s.Log("Hitting an error at " + s + " of ranks.properties");
                                }
                                else
                                {
                                    switch (property.ToLower(CultureInfo.CurrentCulture))
                                    {
                                        case "rankname":
                                            gots = 0;
                                            thisGroup = new Group();

                                            if (value.ToLower(CultureInfo.CurrentCulture) == "developer" || value.ToLower(CultureInfo.CurrentCulture) == "dev" || value.ToLower(CultureInfo.CurrentCulture) == "developers" || value.ToLower(CultureInfo.CurrentCulture) == "devs")
                                                Server.s.Log("You are not a developer. Stop pretending you are.");
                                            if (value.ToLower(CultureInfo.CurrentCulture) == "adv" || value.ToLower(CultureInfo.CurrentCulture) == "op" || value.ToLower(CultureInfo.CurrentCulture) == "super" || value.ToLower(CultureInfo.CurrentCulture) == "nobody" || value.ToLower(CultureInfo.CurrentCulture) == "noone")
                                                Server.s.Log("Cannot have a rank named \"" + value.ToLower(CultureInfo.CurrentCulture) + "\", this rank is hard-coded.");
                                            else if (GroupList.Find(grp => grp.name == value.ToLower(CultureInfo.CurrentCulture)) == null)
                                                thisGroup.trueName = value;
                                            else
                                                Server.s.Log("Cannot add the rank " + value + " twice");
                                            break;
                                        case "permission":
                                            int foundPermission;

                                            try
                                            {
                                                foundPermission = int.Parse(value, CultureInfo.CurrentCulture);
                                            }
                                            catch { Server.s.Log("Invalid permission on " + s); break; }

                                            if (thisGroup.Permission != LevelPermission.Null)
                                            {
                                                Server.s.Log("Setting permission again on " + s);
                                                gots--;
                                            }

                                            bool allowed = true;
                                            if (GroupList.Find(grp => grp.Permission == (LevelPermission)foundPermission) != null)
                                                allowed = false;

                                            if (foundPermission > 119 || foundPermission < -50)
                                            {
                                                Server.s.Log("Permission must be between -50 and 119 for ranks");
                                                break;
                                            }

                                            if (allowed)
                                            {
                                                gots++;
                                                thisGroup.Permission = (LevelPermission)foundPermission;
                                            }
                                            else
                                            {
                                                Server.s.Log("Cannot have 2 ranks set at permission level " + value);
                                            }
                                            break;
                                        case "limit":
                                            int foundLimit;

                                            try
                                            {
                                                foundLimit = int.Parse(value, CultureInfo.CurrentCulture);
                                            }
                                            catch { Server.s.Log("Invalid limit on " + s); break; }

                                            gots++;
                                            thisGroup.maxBlocks = foundLimit;
                                            break;
                                        case "maxundo":
                                            int foundMax;

                                            try
                                            {
                                                foundMax = int.Parse(value, CultureInfo.CurrentCulture);
                                            }
                                            catch { Server.s.Log("Invalid maximum on " + s); break; }

                                            gots++;
                                            thisGroup.maxUndo = foundMax;
                                            break;
                                        case "color":
                                            char foundChar;

                                            try
                                            {
                                                foundChar = char.Parse(value);
                                            }
                                            catch { Server.s.Log("Incorrect color on " + s); break; }

                                            if ((foundChar >= '0' && foundChar <= '9') || (foundChar >= 'a' && foundChar <= 'f'))
                                            {
                                                gots++;
                                                thisGroup.color = foundChar.ToString(CultureInfo.CurrentCulture);
                                            }
                                            else
                                            {
                                                Server.s.Log("Invalid color code at " + s);
                                            }
                                            break;
                                        case "filename":
                                            if (value.Contains("\\") || value.Contains("/"))
                                            {
                                                Server.s.Log("Invalid filename on " + s);
                                                break;
                                            }

                                            gots++;
                                            thisGroup.fileName = value;
                                            break;
                                    }

                                    if ((gots >= 4 && version < 2) || gots >= 5)
                                    {
                                        if (version < 2)
                                        {
                                            if ((int)thisGroup.Permission >= 100)
                                                thisGroup.maxUndo = int.MaxValue;
                                            else if ((int)thisGroup.Permission >= 80)
                                                thisGroup.maxUndo = 5400;
                                        }

                                        GroupList.Add(new Group(thisGroup.Permission, thisGroup.maxBlocks, thisGroup.maxUndo, thisGroup.trueName, thisGroup.color[0], thisGroup.fileName));
                                    }
                                }
                            }
                            else
                            {
                                Server.s.Log("In ranks.properties, the line " + s + " is wrongly formatted");
                            }
                        }
                    }
                    catch (Exception e) { Server.s.Log("Encountered an error at line \"" + s + "\" in ranks.properties"); Server.ErrorLog(e); }
                }
            }

            if (GroupList.Find(grp => grp.Permission == LevelPermission.Banned) == null) GroupList.Add(new Group(LevelPermission.Banned, 1, 1, "Banned", '8', "banned.txt"));
            if (GroupList.Find(grp => grp.Permission == LevelPermission.Guest) == null) GroupList.Add(new Group(LevelPermission.Guest, 1, 120, "Guest", '7', "guest.txt"));
            if (GroupList.Find(grp => grp.Permission == LevelPermission.Builder) == null) GroupList.Add(new Group(LevelPermission.Builder, 400, 300, "Builder", '2', "builders.txt"));
            if (GroupList.Find(grp => grp.Permission == LevelPermission.AdvBuilder) == null) GroupList.Add(new Group(LevelPermission.AdvBuilder, 1200, 900, "AdvBuilder", '3', "advbuilders.txt"));
            if (GroupList.Find(grp => grp.Permission == LevelPermission.Operator) == null) GroupList.Add(new Group(LevelPermission.Operator, 2500, 5400, "Operator", 'c', "operators.txt"));
            if (GroupList.Find(grp => grp.Permission == LevelPermission.Admin) == null) GroupList.Add(new Group(LevelPermission.Admin, 65536, int.MaxValue, "SuperOP", 'e', "uberOps.txt"));
            GroupList.Add(new Group(LevelPermission.Nobody, 65536, -1, "Nobody", '0', "nobody.txt"));

            bool swap = true; Group storedGroup;
            while (swap)
            {
                swap = false;
                for (int i = 0; i < GroupList.Count - 1; i++)
                    if (GroupList[i].Permission > GroupList[i + 1].Permission)
                    {
                        swap = true;
                        storedGroup = GroupList[i];
                        GroupList[i] = GroupList[i + 1];
                        GroupList[i + 1] = storedGroup;
                    }
            }

            if (Group.Find(Server.defaultRank) != null) standard = Group.Find(Server.defaultRank);
            else standard = Group.findPerm(LevelPermission.Guest);

            foreach (Player pl in Player.players)
            {
                pl.group = GroupList.Find(g => g.name == pl.group.name);
            }
            if (OnGroupLoad != null)
                OnGroupLoad();
            saveGroups(GroupList);
        }
        public static void saveGroups(List<Group> givenList)
        {
            File.Create("properties/ranks.properties").Dispose();
            using (StreamWriter SW = File.CreateText("properties/ranks.properties"))
            {
                SW.WriteLine("#Version 2");
                SW.WriteLine("#RankName = string");
                SW.WriteLine("#     The name of the rank, use capitalization.");
                SW.WriteLine("#");
                SW.WriteLine("#Permission = num");
                SW.WriteLine("#     The \"permission\" of the rank. It's a number.");
                SW.WriteLine("#		There are pre-defined permissions already set. (for the old ranks)");
                SW.WriteLine("#		Banned = -20, Guest = 0, Builder = 30, AdvBuilder = 50, Operator = 80");
                SW.WriteLine("#		SuperOP = 100, Nobody = 120");
                SW.WriteLine("#		Must be greater than -50 and less than 120");
                SW.WriteLine("#		The higher the number, the more commands do (such as undo allowing more seconds)");
                SW.WriteLine("#Limit = num");
                SW.WriteLine("#     The command limit for the rank (can be changed in-game with /limit)");
                SW.WriteLine("#		Must be greater than 0 and less than 10000000");
                SW.WriteLine("#MaxUndo = num");
                SW.WriteLine("#     The undo limit for the rank, only applies when undoing others.");
                SW.WriteLine("#		Must be greater than 0 and less than " + int.MaxValue);
                SW.WriteLine("#Color = char");
                SW.WriteLine("#     A single letter or number denoting the color of the rank");
                SW.WriteLine("#	    Possibilities:");
                SW.WriteLine("#		    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, a, b, c, d, e, f");
                SW.WriteLine("#FileName = string.txt");
                SW.WriteLine("#     The file which players of this rank will be stored in");
                SW.WriteLine("#		It doesn't need to be a .txt file, but you may as well");
                SW.WriteLine("#		Generally a good idea to just use the same file name as the rank name");
                SW.WriteLine();
                SW.WriteLine();

                foreach (Group grp in givenList)
                {
                    if (grp.name != "nobody")
                    {
                        SW.WriteLine("RankName = " + grp.trueName);
                        SW.WriteLine("Permission = " + (int)grp.Permission);
                        SW.WriteLine("Limit = " + grp.maxBlocks);
                        SW.WriteLine("MaxUndo = " + grp.maxUndo);
                        SW.WriteLine("Color = " + grp.color[1]);
                        SW.WriteLine("FileName = " + grp.fileName);
                        SW.WriteLine();
                    }
                }
            }
            if (OnGroupSave != null)
                OnGroupSave();
        }

        public static bool Exists(string name)
        {
            name = name.ToLower(CultureInfo.CurrentCulture);
            foreach (Group gr in GroupList)
            {
                if (gr.name == name) { return true; }
            } return false;
        }
        public static Group Find(string name)
        {
            name = name.ToLower(CultureInfo.CurrentCulture);

            if (name == "adv") name = "advbuilder";
            if (name == "op") name = "operator";
            if (name == "super" || (name == "admin" && !Group.Exists("admin"))) name = "superop";
            if (name == "noone") name = "nobody";

            foreach (Group gr in GroupList)
            {
                if (gr.name == name.ToLower(CultureInfo.CurrentCulture)) { return gr; }
            } return null;
        }
        public static Group findPerm(LevelPermission Perm)
        {
            foreach (Group grp in GroupList)
            {
                if (grp.Permission == Perm) return grp;
            }
            return null;
        }
        public static Group findPermInt(int Perm)
        {
            foreach (Group grp in GroupList)
            {
                if ((int)grp.Permission == Perm) return grp;
            }
            return null;
        }

        public static string findPlayer(string playerName)
        {
            foreach (Group grp in Group.GroupList)
            {
                if (grp.playerList.Contains(playerName)) return grp.name;
            }
            return Group.standard.name;
        }
        public static Group findPlayerGroup(string playerName)
        {
            foreach (Group grp in Group.GroupList)
            {
                if (grp.playerList.Contains(playerName)) return grp;
            }
            return Group.standard;
        }

        public static string concatList(bool includeColor = true, bool skipExtra = false, bool permissions = false)
        {
            string returnString = "";
            foreach (Group grp in Group.GroupList)
            {
                if (!skipExtra || (grp.Permission > LevelPermission.Guest && grp.Permission < LevelPermission.Nobody))
                    if (includeColor)
                    {
                        returnString += ", " + grp.color + grp.name + Server.DefaultColor;
                    }
                    else if (permissions)
                    {
                        returnString += ", " + ((int)grp.Permission).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                        returnString += ", " + grp.name;
            }

            if (includeColor) returnString = returnString.Remove(returnString.Length - 2);

            return returnString.Remove(0, 2);
        }
    }


}