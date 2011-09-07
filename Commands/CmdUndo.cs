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
using System.Collections.Generic;
using System.IO;

namespace MCForge {
    public class CmdUndo : Command {
        public override string name { get { return "undo"; } }
        public override string shortcut { get { return "u"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdUndo() { }

        public override void Use(Player p, string message) {
            byte b; long seconds = -1; Player who = null; Player.UndoPos Pos; int CurrentPos = 0;
            if (p != null)
                p.RedoBuffer.Clear();

            if (message == "") {
                if (p == null) {
                    Player.SendMessage(null, "Console doesn't have an undo buffer.");
                    return;
                } else {
                    message = p.name + " 30";
                }
            }

            try {
                if (message.Split(' ').Length == 2) {
                    who = Player.Find(message.Split(' ')[0]);
                    seconds = getAllowed(p, message.Split(' ')[1].ToLower()); // If seconds are invalid, it displays a message and returns -1

                } else {
                    who = (p == null) ? null : Player.Find(p.name);
                    seconds = getAllowed(p, message);
                }
            } catch {
                Player.SendMessage(p, "Invalid seconds.  Using 30 seconds."); //only run if seconds is an invalid number
                seconds = 30;
            }

            //if (message.Split(' ').Length == 1) if (char.IsDigit(message, 0)) { message = p.name + " " + message; } else { message = message + " 30"; }

            //try { seconds = Convert.ToInt16(message.Split(' ')[1]); } catch { seconds = 2; }
            if (seconds < 0) return; //User already given message

            //At this point, we know the number is valid, and allowed for the particxular person's group.
            if (who != null) {
                if (p != null) {
                    if (who.group.Permission > p.group.Permission && who != p) { Player.SendMessage(p, "Cannot undo a user of higher or equal rank"); return; }
                    if (who != p && p.group.Permission < LevelPermission.Operator) { Player.SendMessage(p, "Only an OP+ may undo other people's actions"); return; }
                }

                for (CurrentPos = who.UndoBuffer.Count - 1; CurrentPos >= 0; --CurrentPos) {
                    try {
                        Pos = who.UndoBuffer[CurrentPos];
                        Level foundLevel = Level.FindExact(Pos.mapName);
                        b = foundLevel.GetTile(Pos.x, Pos.y, Pos.z);
                        if (Pos.timePlaced.AddSeconds(seconds) >= DateTime.Now) {
                            if (b == Pos.newtype || Block.Convert(b) == Block.water || Block.Convert(b) == Block.lava) {
                                foundLevel.Blockchange(Pos.x, Pos.y, Pos.z, Pos.type, true);

                                Pos.newtype = Pos.type; Pos.type = b;
                                if (p != null) p.RedoBuffer.Add(Pos);
                                who.UndoBuffer.RemoveAt(CurrentPos);
                            }
                        } else {
                            break;
                        }
                    } catch { }
                }

                if (p == who) {
                    Player.SendMessage(p, "Undid your actions for the past &b" + seconds + Server.DefaultColor + " seconds.");
                } else {
                    Player.GlobalChat(p, who.color + who.name + Server.DefaultColor + "'s actions for the past &b" + seconds + " seconds were undone.", false);
                    // Also notify console
                    Player.SendMessage(null, who.color + who.name + Server.DefaultColor + "'s actions for the past &b" + seconds + " seconds were undone.");
                }
                return;
            } else if (message.Split(' ')[0].ToLower() == "physics") {
                if (p.group.Permission < LevelPermission.AdvBuilder) { Player.SendMessage(p, "Reserved for Adv+"); return; }

                Command.all.Find("physics").Use(p, "0");
                Level.UndoPos uP;
                ushort x, y, z;

                if (p.level.UndoBuffer.Count != Server.physUndo) {
                    for (CurrentPos = p.level.currentUndo; CurrentPos >= 0; CurrentPos--) {
                        try {
                            uP = p.level.UndoBuffer[CurrentPos];
                            b = p.level.GetTile(uP.location);
                            if (uP.timePerformed.AddSeconds(seconds) >= DateTime.Now) {
                                if (b == uP.newType || Block.Convert(b) == Block.water || Block.Convert(b) == Block.lava) {
                                    p.level.IntToPos(uP.location, out x, out y, out z);
                                    p.level.Blockchange(p, x, y, z, uP.oldType, true);
                                }
                            } else {
                                break;
                            }
                        } catch { }
                    }
                } else {
                    for (CurrentPos = p.level.currentUndo; CurrentPos != p.level.currentUndo + 1; CurrentPos--) {
                        try {
                            if (CurrentPos < 0) CurrentPos = p.level.UndoBuffer.Count - 1;
                            uP = p.level.UndoBuffer[CurrentPos];
                            b = p.level.GetTile(uP.location);
                            if (uP.timePerformed.AddSeconds(seconds) >= DateTime.Now) {
                                if (b == uP.newType || Block.Convert(b) == Block.water || Block.Convert(b) == Block.lava) {
                                    p.level.IntToPos(uP.location, out x, out y, out z);
                                    p.level.Blockchange(p, x, y, z, uP.oldType, true);
                                }
                            } else {
                                break;
                            }
                        } catch { }
                    }
                }

                Player.GlobalMessage("Physics were undone &b" + seconds + Server.DefaultColor + " seconds");
                // Also notify console
                Player.SendMessage(null, "Physics were undone &b" + seconds + Server.DefaultColor + " seconds");
            } else { // Here, who == null, meaning the user specified is offline
                if (p != null) {
                    if (p.group.Permission < LevelPermission.Operator) { Player.SendMessage(p, "Reserved for OP+"); return; }
                }

                bool FoundUser = false;

                try {
                    DirectoryInfo di;
                    string[] fileContent;

                    if (p != null)
                        p.RedoBuffer.Clear();

                    if (Directory.Exists("extra/undo/" + message.Split(' ')[0])) {
                        di = new DirectoryInfo("extra/undo/" + message.Split(' ')[0]);

                        for (int i = di.GetFiles("*.undo").Length - 1; i >= 0; i--) {
                            fileContent = File.ReadAllText("extra/undo/" + message.Split(' ')[0] + "/" + i + ".undo").Split(' ');
                            if (!undoBlah(fileContent, seconds, p)) break;
                        }
                        FoundUser = true;
                    }

                    if (Directory.Exists("extra/undoPrevious/" + message.Split(' ')[0])) {
                        di = new DirectoryInfo("extra/undoPrevious/" + message.Split(' ')[0]);

                        for (int i = di.GetFiles("*.undo").Length - 1; i >= 0; i--) {
                            fileContent = File.ReadAllText("extra/undoPrevious/" + message.Split(' ')[0] + "/" + i + ".undo").Split(' ');
                            if (!undoBlah(fileContent, seconds, p)) break;
                        }
                        FoundUser = true;
                    }

                    if (FoundUser) {
                        Player.GlobalChat(p, Server.FindColor(message.Split(' ')[0]) + message.Split(' ')[0] + Server.DefaultColor + "'s actions for the past &b" + seconds + Server.DefaultColor + " seconds were undone.", false);
                        // Also notify console
                        Player.SendMessage(null, Server.FindColor(message.Split(' ')[0]) + message.Split(' ')[0] + Server.DefaultColor + "'s actions for the past &b" + seconds + Server.DefaultColor + " seconds were undone.");
                    } else Player.SendMessage(p, "Could not find player specified.");
                } catch (Exception e) {
                    Server.ErrorLog(e);
                }
            }
        }

        private long getAllowed(Player p, string param) {
            //We get the custom permission from the properties file.
            
            /* if (p.group.Permission < LevelPermission.Builder && seconds > 120) {
             *     Player.SendMessage(p, "Guests may only undo 2 minutes.");
             *     return;
             * } else if (p.group.Permission < LevelPermission.AdvBuilder && seconds > 300) {
             *     Player.SendMessage(p, "Builders may only undo 300 seconds.");
             *     return;
             * } else if (p.group.Permission < LevelPermission.Operator && seconds > 1200) {
             *     Player.SendMessage(p, "AdvBuilders may only undo 600 seconds.");
             *     return;
             * } else if (p.group.Permission == LevelPermission.Operator && seconds > 5400) {
             *     Player.SendMessage(p, "Operators may only undo 5400 seconds.");
             * }
             * return;
             */
            long secs;
            if (param == "all")
                secs = 500000;
            else
                secs = long.Parse(param); //caught by try/catch in outer method

            if (secs == 0) secs = 5400;

            if (p.group.maxUndo != 0 && secs > p.group.maxUndo) {
                Player.SendMessage(p, p.group.name + "s may only undo " + p.group.maxUndo + " seconds.");
                return -1;
            }
            return secs;
        }

        public bool undoBlah(string[] fileContent, Int64 seconds, Player p) {

            //fileContents += uP.map.name + " " + uP.x + " " + uP.y + " " + uP.z + " ";
            //fileContents += uP.timePlaced + " " + uP.type + " " + uP.newtype + " ";

            //Maps = 0, 7, 14, 21, 28, 35...
            //X = 1, 8, 15...
            //newtype = 6, 13, 20, 27...

            Player.UndoPos Pos;

            for (int i = fileContent.Length / 7; i >= 0; i--) {
                try {
                    if (Convert.ToDateTime(fileContent[(i * 7) + 4].Replace('&', ' ')).AddSeconds(seconds) >= DateTime.Now) {
                        Level foundLevel = Level.FindExact(fileContent[i * 7]);
                        if (foundLevel != null) {
                            Pos.mapName = foundLevel.name;
                            Pos.x = Convert.ToUInt16(fileContent[(i * 7) + 1]);
                            Pos.y = Convert.ToUInt16(fileContent[(i * 7) + 2]);
                            Pos.z = Convert.ToUInt16(fileContent[(i * 7) + 3]);

                            Pos.type = foundLevel.GetTile(Pos.x, Pos.y, Pos.z);

                            if (Pos.type == Convert.ToByte(fileContent[(i * 7) + 6]) || Block.Convert(Pos.type) == Block.water || Block.Convert(Pos.type) == Block.lava || Pos.type == Block.grass) {
                                Pos.newtype = Convert.ToByte(fileContent[(i * 7) + 5]);
                                Pos.timePlaced = DateTime.Now;

                                foundLevel.Blockchange(Pos.x, Pos.y, Pos.z, Pos.newtype, true);
                                if (p != null) p.RedoBuffer.Add(Pos);
                            }
                        }
                    } else return false;
                } catch { }
            }

            return true;
        }

        public override void Help(Player p) {
            Player.SendMessage(p, "/undo [player] [seconds] - Undoes the blockchanges made by [player] in the previous [seconds].");
            string rank = undoAllRank;
            if (rank != null)
                Player.SendMessage(p, "/undo [player] all - &cWill undo 138 hours 53 minutes and 20 seconds for [player] <" + rank + "+>");
            rank = undo30Rank;
            if (rank != null)
                Player.SendMessage(p, "/undo [player] 0 - &cWill undo 30 minutes <" + rank + "+>");
            Player.SendMessage(p, "/undo physics [seconds] - Undoes the physics for the current map");
        }

        public string undoAllRank {
            get {
                try {
                    return getLowestAbove(500000).name;
                } catch { // ensures that errors don't happen if the group is null.
                    return null;
                }
            }
        }

        public string undo30Rank {
            get {
                try {
                    return getLowestAbove(1800).name;
                } catch { // ensures that errors don't happen if the group is null.
                    return null;
                }
            }
        }

        private Group getLowestAbove(long amt)
        {
            Group lowest = null;
            foreach (Group g in Group.GroupList) {
                if (g.maxUndo == 0 || g.maxUndo >= amt) {
                    if (lowest == null || lowest.Permission > g.Permission) {
                        lowest = g;
                    }
                }
            }
            return lowest;
        }
    }
}