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
namespace MCForge
{
    using System;
    public class CmdUnflood : Command
    {
        public override string name { get { return "unflood"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Help(Player p) { Player.SendMessage(p, "/unflood [liquid] - Unfloods the map you are on of [liquid]"); }
        public override void Use(Player p, string message)
        {
            Level level = Level.Find(p.level.name);
            bool instant = level.Instant;
            if (message == "all")
            {
                Command.all.Find("physics").Use(p, "0");
                if (instant == false)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("replaceall").Use(p, "lavafall air");
                Command.all.Find("replaceall").Use(p, "waterfall air");
                Command.all.Find("replaceall").Use(p, "lava_fast air");
                Command.all.Find("replaceall").Use(p, "active_lava air");
                Command.all.Find("replaceall").Use(p, "active_water air");
                Command.all.Find("replaceall").Use(p, "active_hot_lava air");
                Command.all.Find("replaceall").Use(p, "magma air");
                Command.all.Find("reveal").Use(p, "all");
                if (instant == true)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("physics").Use(p, "1");
                Player.GlobalMessage("Unflooded!");
            }
            else
            {
                Command.all.Find("physics").Use(p, "0");
                if (instant == false)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("replaceall").Use(p, message + " air");
                Command.all.Find("reveal").Use(p, "all");
                if (instant == true)
                {
                    Command.all.Find("map").Use(p, "instant");
                }
                Command.all.Find("physics").Use(p, "1");
                Player.GlobalMessage("Unflooded!");
            }
        }
    }
}