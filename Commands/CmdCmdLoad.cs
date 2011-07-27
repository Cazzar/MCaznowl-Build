/*
	Copyright 2010 MCLawl Team - Written by Valek (Modified for use with MCForge)
 
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
using System.Reflection;

namespace MCForge
{
    class CmdCmdLoad : Command
    {
        public override string name { get { return "cmdload"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
        public CmdCmdLoad() { }

        public override void Use(Player p, string message)
        {
            if(message == "") { Help(p); return; }
            if (Command.all.Contains(message.Split(' ')[0]))
            {
                Player.SendMessage(p, "That command is already loaded!");
                return;
            }
            message = "Cmd" + message.Split(' ')[0]; ;
            string error = Scripting.Load(message);
            if (error != null)
            {
                Player.SendMessage(p, error);
                return;
            }
            GrpCommands.fillRanks();
            Player.SendMessage(p, "Command was successfully loaded.");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/cmdload <command name> - Loads a command into the server for use.");
        }
    }
}
