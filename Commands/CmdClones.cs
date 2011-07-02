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
using System.Collections.Generic;
using System.Text;
//using MySql.Data.MySqlClient;
//using MySql.Data.Types;
using System.Data;

namespace MCForge
{
    class CmdClones : Command
    {

        public override string name { get { return "clones"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdClones() { }

        public override void Use(Player p, string message)
        {
            if (message == "") message = p.name;

            string originalName = message.ToLower();

            Player who = Player.Find(message);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player. Searching Player DB.");

                DataTable FindIP = MySQL.fillData("SELECT IP FROM Players WHERE Name='" + message + "'");

                if (FindIP.Rows.Count == 0) { Player.SendMessage(p, "Could not find any player by the name entered."); FindIP.Dispose(); return; }

                message = FindIP.Rows[0]["IP"].ToString();
                FindIP.Dispose();
            }
            else
            {
                message = who.ip;
            }

            DataTable Clones = MySQL.fillData("SELECT Name FROM Players WHERE IP='" + message + "'");

            if (Clones.Rows.Count == 0) { Player.SendMessage(p, "Could not find any record of the player entered."); return; }

            List<string> foundPeople = new List<string>();
            for (int i = 0; i < Clones.Rows.Count; ++i)
            {
                if (!foundPeople.Contains(Clones.Rows[i]["Name"].ToString().ToLower()))
                    foundPeople.Add(Clones.Rows[i]["Name"].ToString().ToLower());
            }

            Clones.Dispose();
            if (foundPeople.Count <= 1) { Player.SendMessage(p, originalName + " has no clones."); return; }

            Player.SendMessage(p, "These people have the same IP address:");
            Player.SendMessage(p, string.Join(", ", foundPeople.ToArray()));
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/clones <name> - Finds everyone with the same IP has <name>");
        }
    }
}
