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
using System.Data;
//using MySql.Data.MySqlClient;
//using MySql.Data.Types;

namespace MCForge
{
    public class CmdClearBlockChanges : Command
    {
        public override string name { get { return "clearblockchanges"; } }
        public override string shortcut { get { return "cbc"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdClearBlockChanges() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Player.SendMessage(p, "This command can only be used in-game");
                return;
            }
            Level l = Level.Find(message);
            if (l == null && message != "") { Player.SendMessage(p, "Could not find level."); return; }
            if (l == null) l = p.level;

            if (Server.useMySQL) MySQL.executeQuery("TRUNCATE TABLE `Block" + l.name + "`"); else SQLite.executeQuery("DELETE FROM `Block" + l.name + "`");
            Player.SendMessage(p, "Cleared &cALL" + Server.DefaultColor + " recorded block changes in: &d" + l.name);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/clearblockchanges <map> - Clears the block changes stored in /about for <map>.");
            Player.SendMessage(p, "&cUSE WITH CAUTION");
        }
    }
}