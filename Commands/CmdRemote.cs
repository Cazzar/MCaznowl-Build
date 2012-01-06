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
using MCForge.Remote;

using MCForge;
namespace MCForge.Commands
{
    class CmdRemote : Command
    {
        public override string name { get { return "remote"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "Mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdRemote() { }

        public override void Use(Player p, string message)
        {
            string[] splitted = message.Split(' ');
            if (splitted[0] == "resettry")
            {
                Remote.RemoteServer.tries = 0;

                if (p != null) { Player.SendMessage(p, "Remote's tries were reset"); }
                else { Server.s.Log("Remote's tries were reset"); }
                return;
            }
            Help(p);
        }
        public override void Help(Player p)
        {
            if (p != null)
            {
                Player.SendMessage(p, "/remote resettry - re-enables connection from remote");
                return;
            }
            Server.s.Log("/remote resettry - re-enables connection from remote \n");
        }
    }
}