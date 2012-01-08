/*
	Copyright 2010 MCForge Team - Written by cazzar
 
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

using MCForge;
namespace MCForge.Commands
{
    public class CmdSubmit : Command
    {
        public override string name { get { return "like"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdSubmit() { }
        public override void Use(Player p, string message)
        {
            
            if (p == null)
            {
                Player.SendMessage(p, "You have to be ingame to do that!");
                return;
            }

            if (message == "") { Help(p); return; }

            string mapnumber = null;
            if (message.Split(' ').Length == 2)
                mapnumber = message.Split(' ')[1];
            message = message.Split(' ')[0];

            string path = "levels/" + p.name;
            if (!string.IsNullOrEmpty(mapnumber))
                path += "." + mapnumber;
            path += ".lvl";
            string dest = "levels/submit/" + message + "." + p.name + ".lvl";

            if (System.IO.File.Exists(path))
            {
                if (!System.IO.Directory.Exists("levels/submit"))
                    System.IO.Directory.CreateDirectory("levels/submit"); //if not then make it
                System.IO.File.Copy(path, dest);
                Player.SendMessage(p, "Level submitted as " + message);
            }
            else
            {
                Player.SendMessage(p, "Level not found!");
            }
            
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/submit [mapname] <mapnumber> - submits your map as [mapname]");
        }
    }
}