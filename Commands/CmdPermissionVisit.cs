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
using System.IO;

namespace MCForge
{
    public class CmdPermissionVisit : Command
    {
        public override string name { get { return "pervisit"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdPermissionVisit() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 2 || number < 1) { Help(p); return; }
            if (number == 1)
            {
                LevelPermission Perm = Level.PermissionFromName(message);
                if (Perm == LevelPermission.Null) { Player.SendMessage(p, "Not a valid rank"); return; }
                if (p.level.permissionvisit > p.group.Permission) {
					Player.SendMessage(p, "You cannot change the pervisit of a level with a pervisit higher than your rank.");
					return;
				}
                File.Create("levels/level properties/" + p.level.name + ".properties").Dispose();
				using (StreamWriter SW = File.CreateText("levels/level properties/" + p.level.name + ".properties"))
				{
					SW.WriteLine("#Level properties for " + p.level.name);
					SW.WriteLine("Theme = " + p.level.theme);
					SW.WriteLine("Physics = " + p.level.physics.ToString());
					SW.WriteLine("Physics speed = " + p.level.speedPhysics.ToString());
					SW.WriteLine("Physics overload = " + p.level.overload.ToString());
					SW.WriteLine("Finite mode = " + p.level.finite.ToString());
					SW.WriteLine("Animal AI = " + p.level.ai.ToString());
					SW.WriteLine("Edge water = " + p.level.edgeWater.ToString());
					SW.WriteLine("Survival death = " + p.level.Death.ToString());
					SW.WriteLine("Fall = " + p.level.fall.ToString());
					SW.WriteLine("Drown = " + p.level.drown.ToString());
					SW.WriteLine("MOTD = " + p.level.motd);
					SW.WriteLine("JailX = " + p.level.jailx.ToString());
					SW.WriteLine("JailY = " + p.level.jaily.ToString());
					SW.WriteLine("JailZ = " + p.level.jailz.ToString());
					SW.WriteLine("Unload = " + p.level.unload);
					SW.WriteLine("PerBuild = " + Group.findPerm(p.level.permissionbuild).trueName.ToLower());
					SW.WriteLine("PerVisit = " + message.ToLower());
				}
                p.level.permissionvisit = Perm;
                Server.s.Log(p.level.name + " visit permission changed to " + message + ".");
                Player.GlobalMessageLevel(p.level, "visit permission changed to " + message + ".");
            }
            else
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                LevelPermission Perm = Level.PermissionFromName(s);
                if (Perm == LevelPermission.Null) { Player.SendMessage(p, "Not a valid rank"); return; }

                Level level = Level.Find(t);
                if (level.permissionvisit > p.group.Permission)
                {
                    Player.SendMessage(p, "You cannot change the pervisit of a level with a pervisit higher than your rank.");
                    return;
                }
                if (level != null)
                {
                    File.Create("levels/level properties/" + level.name + ".properties").Dispose();
					using (StreamWriter SW = File.CreateText("levels/level properties/" + level.name + ".properties"))
					{
						SW.WriteLine("#Level properties for " + level.name);
						SW.WriteLine("Theme = " + level.theme);
						SW.WriteLine("Physics = " + level.physics.ToString());
						SW.WriteLine("Physics speed = " + level.speedPhysics.ToString());
						SW.WriteLine("Physics overload = " + level.overload.ToString());
						SW.WriteLine("Finite mode = " + level.finite.ToString());
						SW.WriteLine("Animal AI = " + level.ai.ToString());
						SW.WriteLine("Edge water = " + level.edgeWater.ToString());
						SW.WriteLine("Survival death = " + level.Death.ToString());
						SW.WriteLine("Fall = " + level.fall.ToString());
						SW.WriteLine("Drown = " + level.drown.ToString());
						SW.WriteLine("MOTD = " + level.motd);
						SW.WriteLine("JailX = " + level.jailx.ToString());
						SW.WriteLine("JailY = " + level.jaily.ToString());
						SW.WriteLine("JailZ = " + level.jailz.ToString());
						SW.WriteLine("Unload = " + level.unload);
						SW.WriteLine("PerBuild = " + s.ToLower());
						SW.WriteLine("PerVisit = " + Group.findPerm(level.permissionvisit).trueName.ToLower());
					}
                    level.permissionvisit = Perm;
                    Server.s.Log(level.name + " visit permission changed to " + s + ".");
                    Player.GlobalMessageLevel(level, "visit permission changed to " + s + ".");
                    if (p != null)
                        if (p.level != level) { Player.SendMessage(p, "visit permission changed to " + s + " on " + level.name + "."); }
                    return;
                }
                else
                    Player.SendMessage(p, "There is no level \"" + s + "\" loaded.");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/PerVisit <map> <rank> - Sets visiting permission for a map.");
        }
    }
}