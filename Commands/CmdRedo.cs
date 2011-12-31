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
    public class CmdRedo : Command
    {
        public override string name { get { return "redo"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdRedo() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            byte b;

            p.RedoBuffer.ForEach(delegate(Player.UndoPos Pos)
            {
                Level foundLevel = Level.FindExact(Pos.mapName);
                if (foundLevel != null)
                {
                    b = foundLevel.GetTile(Pos.x, Pos.y, Pos.z);
                    foundLevel.Blockchange(Pos.x, Pos.y, Pos.z, Pos.type);
                    Pos.newtype = Pos.type;
                    Pos.type = b;
                    Pos.timePlaced = DateTime.Now;
                    p.UndoBuffer.Add(Pos);
                }
            });

            Player.SendMessage(p, "Redo performed.");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/redo - Redoes the Undo you just performed.");
        }
    }
}