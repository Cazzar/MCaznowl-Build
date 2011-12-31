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

namespace MCForge
{
    public class CmdWrite : Command
    {
        public override string name { get { return "write"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdWrite() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            CatchPos cpos;

            cpos.givenMessage = message.ToUpper();
            cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
            Player.SendMessage(p, "Place two blocks to determine direction.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/write [message] - Writes [message] in blocks");
        }
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos bp = (CatchPos)p.blockchangeObject;
            bp.x = x; bp.y = y; bp.z = z; p.blockchangeObject = bp;
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }
        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            type = p.bindings[type];

            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);

            CatchPos cpos = (CatchPos)p.blockchangeObject;

            ushort cur;

            if (x == cpos.x && z == cpos.z) { Player.SendMessage(p, "No direction was selected"); return; }

            if (Math.Abs(cpos.x - x) > Math.Abs(cpos.z - z))
            {
                cur = cpos.x;
                if (x > cpos.x)
                {
                    foreach (char c in cpos.givenMessage)
                    {
                        cur = FindReference.writeLetter(p, c, cur, cpos.y, cpos.z, type, 0);
                    }
                }
                else
                {
                    foreach (char c in cpos.givenMessage)
                    {
                        cur = FindReference.writeLetter(p, c, cur, cpos.y, cpos.z, type, 1);
                    }
                }
            }
            else
            {
                cur = cpos.z;
                if (z > cpos.z)
                {
                    foreach (char c in cpos.givenMessage)
                    {
                        cur = FindReference.writeLetter(p, c, cpos.x, cpos.y, cur, type, 2);
                    }
                }
                else
                {
                    foreach (char c in cpos.givenMessage)
                    {
                        cur = FindReference.writeLetter(p, c, cpos.x, cpos.y, cur, type, 3);
                    }
                }
            }

            if (p.staticCommands) p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }

        struct CatchPos
        {
            public ushort x, y, z; public string givenMessage;
        }

    }
}
