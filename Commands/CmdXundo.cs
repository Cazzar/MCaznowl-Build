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

namespace MCForge
{
    public class CmdXundo : Command
    {

        public override string name { get { return "xundo"; } }

        public override string shortcut { get { return ""; } }

        public override string type { get { return "other"; } }

        public override bool museumUsable { get { return false; } }

        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public CmdXundo() { }
        public override void Use(Player p, string message)
        {

            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number != 1) { Help(p); return; }
         
            Player who = Player.Find(message);


            string error1 = "You are not allowed to use this command";
            string error2 = "You are not allowed to undo this player";

           
            if (p.group.Permission == LevelPermission.Operator)
            {
                if (who != null)
                {
                    if (who.group.Permission != LevelPermission.Operator)
                    {
                        p.group.Permission = LevelPermission.Admin;
                        Command.all.Find("undo").Use(p, who.name + " all");
                        p.group.Permission = LevelPermission.Operator;
                        return;
                    }
                    Player.SendMessage(p, error2);
                }
                else
                {
                    p.group.Permission = LevelPermission.Admin;
                    Command.all.Find("undo").Use(p, message + " all");
                    p.group.Permission = LevelPermission.Operator;
                    return;
                }
            }


            if (p.group.Permission > LevelPermission.Operator)
            {
                Command.all.Find("undo").Use(p, message + " all");
                return;
            }
            Player.SendMessage(p, error1);

        }



        public override void Help(Player p)
        {
            Player.SendMessage(p, "/xundo [name]  -  works as 'undo [name] all' but now Ops can use it");
        }
    }
}