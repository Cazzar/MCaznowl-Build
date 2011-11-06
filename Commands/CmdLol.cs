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
    public class CmdLol : Command
    {
        public override string name { get { return "lol"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdLol() { }

        public override void Use(Player p, string message)
        {
            int direction = 0;
            if (p.rot[0] >= 46 && p.rot[0] < 135)
            {
                direction = 1;
            }
            if (p.rot[0] >= 136 && p.rot[0] < 225)
            {
                direction = 2;
            }
            if (p.rot[0] >= 226 && p.rot[0] < 315)
            {
                direction = 3;
            }
            if (p.rot[0] >= 316 || p.rot[0] < 45)
            {
                direction = 4;
            }
            ushort levelx = (ushort)(p.level.width * 32);
            ushort levelz = (ushort)(p.level.length * 32);
            ushort y = (ushort)(p.pos[1] - 20);
            ushort px = p.pos[0];
            ushort pz = p.pos[2];
            if (direction == 1)
            {
                while (px < levelx)
                {
                    px++;
                    unchecked { p.SendPos((byte)-1, px, y, p.pos[2], p.rot[0], p.rot[1]); }
                }
            }
            if (direction == 2)
            {
                while (pz < levelz)
                {
                    pz++;
                    unchecked { p.SendPos((byte)-1, p.pos[0], y, pz, p.rot[0], p.rot[1]); }
                }
            }
            if (direction == 3)
            {
                while (px > 32)
                {
                    px--;
                    unchecked { p.SendPos((byte)-1, px, y, p.pos[2], p.rot[0], p.rot[1]); }
                }
            }
            if (direction == 4)
            {
                while (pz > 32)
                {
                    pz--;
                    unchecked { p.SendPos((byte)-1, p.pos[0], y, pz, p.rot[0], p.rot[1]); }
                }
            }
            return;
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/lol - moves you strangely");
        }
    }
}