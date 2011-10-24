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
 * 
 * DELTEMPRANK SCRIPT BY BeMacized www.bemacized.com
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace MCForge
{
    class CmdDelTempRank : Command
    {
        public override string name { get { return "deltemprank"; } }
        public override string shortcut { get { return "dtr"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdDelTempRank() { }
        public override void Use(Player p, string message)
        {
            string alltext = File.ReadAllText("text/tempranks.txt");
            if (alltext.Contains(message) == false)
            {
                Player.SendMessage(p, "&cPlayer &a" + message + "&c Has not been assigned a temporary rank. Cannot unnasign.");
                goto end;
            }
            string alltempranks = "";
           Player who = Player.Find(message);
           foreach (string line in File.ReadAllLines("text/tempranks.txt"))
           {
               if (line.Contains(message))
               {
                   string group = line.Split(' ')[2];
                   Group oldgroup = Group.findPlayerGroup(who.name);
                   Group newgroup = Group.Find(group);
                   Command.all.Find("setrank").Use(null, who.name + " " + newgroup.name);
                   Player.SendMessage(p, "&eTemporary rank of &a" + message + "&e has been unassigned");
                   Player.SendMessage(who, "&eYour temporary rank has been unassigned");
               }
               if (!line.Contains(message))
               {
                   alltempranks = alltempranks + line + "\r\n";
               }
           }
           File.WriteAllText("text/tempranks.txt", alltempranks);
           

           
        end:
            System.Threading.Thread.Sleep(0);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/deltemprank - Deletes someones temporary rank");
        }
        
        
    }
}
