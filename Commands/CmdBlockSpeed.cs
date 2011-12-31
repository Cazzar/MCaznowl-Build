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
using System.Globalization;

namespace MCForge
{
    public class CmdBlockSpeed : Command
    {
        public override string name { get { return "blockspeed"; } }
        public override string shortcut { get { return "bs"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdBlockSpeed() { }

        public override void Use(Player p, string text)
        {
            if ((text != null && String.IsNullOrEmpty(text)))
            {
                SendEstimation(p);
                return;
            }
            if (text == "clear")
            {
                Server.levels.ForEach((l) => { l.blockqueue.Clear(); });
                return;
            }
            if (text.StartsWith("bs", StringComparison.CurrentCulture))
            {
                try { Block1.blockupdates = int.Parse(text.Split(' ')[1], CultureInfo.CurrentCulture); }
                catch { Player.SendMessage(p, "Invalid number specified."); return; }
                Player.SendMessage(p, String.Format(CultureInfo.CurrentCulture, "Blocks per interval is now {0}.", Block1.blockupdates));
                return;
            }
            if (text.StartsWith("ts", StringComparison.CurrentCulture))
            {
                try { Block1.time = int.Parse(text.Split(' ')[1], CultureInfo.CurrentCulture); }
                catch { Player.SendMessage(p, "Invalid number specified."); return; }
                Player.SendMessage(p, String.Format(CultureInfo.CurrentCulture, "Block interval is now {0}.", Block1.time));
                return;
            }
            if (text.StartsWith("buf", StringComparison.CurrentCulture))
            {
                if (p.level.bufferblocks)
                {
                    p.level.bufferblocks = false;
                    Player.SendMessage(p, String.Format(CultureInfo.CurrentCulture, "Block buffering on {0} disabled.", p.level.name));
                }
                else
                {
                    p.level.bufferblocks = true;
                    Player.SendMessage(p, String.Format(CultureInfo.CurrentCulture, "Block buffering on {0} enabled.", p.level.name));
                }
                return;
            }
            if (text.StartsWith("net", StringComparison.CurrentCulture))
            {
                switch (int.Parse(text.Split(' ')[1], CultureInfo.CurrentCulture))
                {
                    case 2:
                        Block1.blockupdates = 25;
                        Block1.time = 100;
                        break;
                    case 4:
                        Block1.blockupdates = 50;
                        Block1.time = 100;
                        break;
                    case 8:
                        Block1.blockupdates = 100;
                        Block1.time = 100;
                        break;
                    case 12:
                        Block1.blockupdates = 200;
                        Block1.time = 100;
                        break;
                    case 16:
                        Block1.blockupdates = 200;
                        Block1.time = 100;
                        break;
                    case 161:
                        Block1.blockupdates = 100;
                        Block1.time = 50;
                        break;
                    case 20:
                        Block1.blockupdates = 125;
                        Block1.time = 50;
                        break;
                    case 24:
                        Block1.blockupdates = 150;
                        Block1.time = 50;
                        break;
                    default:
                        Block1.blockupdates = 200;
                        Block1.time = 100;
                        break;
                }
                SendEstimation(p);
                return;
            }
        }
        private static void SendEstimation(Player p)
        {
            Player.SendMessage(p, String.Format(CultureInfo.CurrentCulture, "{0} blocks every {1} milliseconds = {2} blocks per second.", Block1.blockupdates, Block1.time, Block1.blockupdates * (1000 / Block1.time)));
            Player.SendMessage(p, String.Format(CultureInfo.CurrentCulture, "Using ~{0}KB/s times {1} player(s) = ~{2}KB/s", (Block1.blockupdates * (1000 / Block1.time) * 8) / 1000, Player.players.Count, Player.players.Count * ((Block1.blockupdates * (1000 / Block1.time) * 8) / 1000)));
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/bs [option] [option value] - Options for block speeds.");
            Player.SendMessage(p, "Options are: bs (blocks per interval), ts (interval in milliseconds), buf (toggles buffering), clear, net.");
            Player.SendMessage(p, "/bs net [2,4,8,12,16,20,24] - Presets, divide by 8 and times by 1000 to get blocks per second.");
        }
    }
}