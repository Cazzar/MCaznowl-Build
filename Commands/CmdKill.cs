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
    public class CmdKill : Command
    {
        public override string name { get { return "kill"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdKill() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            Player who; string killMsg; int killMethod = 0;
            if (message.IndexOf(' ') == -1)
            {
                who = Player.Find(message);
                killMsg = " was killed by " + p.color + p.name;
            }
            else
            {
                who = Player.Find(message.Split(' ')[0]);
                message = message.Substring(message.IndexOf(' ') + 1);

                if (message.IndexOf(' ') == -1)
                {
                    if (message.ToLower() == "explode")
                    {
                        killMsg = " was exploded by " + p.color + p.name;
                        killMethod = 1;
                    }
                    else
                    {
                        killMsg = " " + message;
                    }
                }
                else
                {
                    if (message.Split(' ')[0].ToLower() == "explode")
                    {
                        killMethod = 1;
                        message = message.Substring(message.IndexOf(' ') + 1);
                    }

                    killMsg = " " + message;
                }
            }

            if (who == null)
            {
                p.HandleDeath(Block.rock, " killed itself in its confusion");
                Player.SendMessage(p, "Could not find player");
                return;
            }

            if (who.group.Permission > p.group.Permission)
            {
                p.HandleDeath(Block.rock, " was killed by " + who.color + who.name);
                Player.SendMessage(p, "Cannot kill someone of higher rank");
                return;
            }

            if (killMethod == 1)
                who.HandleDeath(Block.rock, killMsg, true);
            else
                who.HandleDeath(Block.rock, killMsg);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/kill <name> [explode] <message>");
            Player.SendMessage(p, "Kills <name> with <message>. Causes explosion if [explode] is written");
        }
    }
}