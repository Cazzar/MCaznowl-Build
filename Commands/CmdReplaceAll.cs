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
using System.Text;

namespace MCForge
{
    class CmdReplaceAll : Command
    {
        public override string name { get { return "replaceall"; } }
        public override string shortcut { get { return "ra"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdReplaceAll() { }
        public static byte wait;

        public override void Use(Player p, string message)
        {
            wait = 0;
            if (message.IndexOf(' ') == -1 || message.Split(' ').Length > 2) { Help(p); wait = 1; return; }

            byte b1, b2;

            b1 = Block.Byte(message.Split(' ')[0]);
            b2 = Block.Byte(message.Split(' ')[1]);

            if (b1 == Block.Zero || b2 == Block.Zero) { Player.SendMessage(p, "Could not find specified blocks."); wait = 1; return; }
            if (!Block.canPlace(p, b1) && !Block.BuildIn(b2)) { Player.SendMessage(p, "Cannot replace that."); wait = 1; return; }
            if (!Block.canPlace(p, b2)) { Player.SendMessage(p, "Cannot place that."); wait = 1; return; }
            ushort x, y, z; int currentBlock = 0;
            List<Pos> stored = new List<Pos>(); Pos pos;

            foreach (byte b in p.level.blocks)
            {
                if (b == b1)
                {
                    p.level.IntToPos(currentBlock, out x, out y, out z);
                    pos.x = x; pos.y = y; pos.z = z;
                    stored.Add(pos);
                }
                currentBlock++;
            }

            if (stored.Count > (p.group.maxBlocks * 2)) { Player.SendMessage(p, "Cannot replace more than " + (p.group.maxBlocks * 2) + " blocks."); wait = 1; return; }

            Player.SendMessage(p, stored.Count + " blocks out of " + currentBlock + " are " + Block.Name(b1));

            if (p.level.bufferblocks)
            {
                foreach (Pos Pos in stored)
                {
                    BlockQueue.Addblock(p, Pos.x, Pos.y, Pos.z, b2);
                }
            }
            else
            {
                foreach (Pos Pos in stored)
                {
                    p.level.Blockchange(p, Pos.x, Pos.y, Pos.z, b2);
                }
            }

            Player.SendMessage(p, "&4/replaceall finished!");
            wait = 2;
        }
        public struct Pos { public ushort x, y, z; }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/replaceall [block1] [block2] - Replaces all of [block1] with [block2] in a map");
        }
    }
}