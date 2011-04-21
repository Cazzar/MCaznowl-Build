/*
	Copyright 2011 MCForge
		
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.osedu.org/licenses/ECL-2.0
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
    public class CmdTnt : Command
    {
        public override string name { get { return "tnt"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdTnt() { }

        public override void Use(Player p, string message)
        {
            if (message.Split(' ').Length > 1) { Help(p); return; }

            if (p.BlockAction == 13 || p.BlockAction == 14)
            {
                p.BlockAction = 0; Player.SendMessage(p, "TNT mode is now &cOFF" + Server.DefaultColor + ".");
            }
            else if (message.ToLower() == "small" || message == "")
            {
                p.BlockAction = 13; Player.SendMessage(p, "TNT mode is now &aON" + Server.DefaultColor + ".");
            }
            else if (message.ToLower() == "big")
            {
                if (p.group.Permission > LevelPermission.AdvBuilder)
                {
                    p.BlockAction = 14; Player.SendMessage(p, "TNT (Big) mode is now &aON" + Server.DefaultColor + ".");
                }
                else
                {
                    Player.SendMessage(p, "This mode is reserved for OPs");
                }
            }
            else if (message.ToLower() == "nuke")
            {
                if (p.group.Permission > LevelPermission.AdvBuilder)
                {
                    p.BlockAction = 15; Player.SendMessage(p, "TNT (Nuke) mode is now &aON" + Server.DefaultColor + ".");
                }
                else
                {
                    Player.SendMessage(p, "This mode is reserved for OPs");
                }
            }
            else
            {
                Help(p);
            }

            p.painting = false;
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tnt [small/big/nuke] - Creates exploding TNT (with Physics 3).");
            Player.SendMessage(p, "Big and Nuke TNT is reserved for OP+.");
        }
    }
}