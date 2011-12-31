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

namespace MCForge {
    public class CmdUBan : Command {

        public override string name { get { return "uban"; } }

        public override string shortcut { get { return ""; } }

        public override string type { get { return "mod"; } }

        public override bool museumUsable { get { return false; } }

        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public CmdUBan() { }
        public override void Use(Player p, string message) {

            if (message == "") { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);
            string msg = message.Split(' ')[0];
            if (Server.devs.Contains(message.ToLower())) {
                Player.SendMessage(p, "You can't ban a MCForge Developer!");
                if (p != null) {
                    Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " attempted to ban a MCForge Developer!");
                    return;
                } else {
                    Player.GlobalMessage(Server.DefaultColor + "The Console attempted to ban a MCForge Developer!");
                }
                return;
            }
            if (who != null) {
                Command.all.Find("ban").Use(p, msg);
                Command.all.Find("kick").Use(p, message);
                Command.all.Find("xundo").Use(p, msg);

            } else {
                Command.all.Find("ban").Use(p, msg);
                Command.all.Find("xundo").Use(p, msg);

            }
        }

        public override void Help(Player p) {
            Player.SendMessage(p, "/uban [name] [message]- Bans, undoes, and kicks [name] with [message], if specified.");
        }
    }
}