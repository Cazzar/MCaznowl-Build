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

namespace MCForge
{
    public class CmdZTime : Command
    {
        public override string name { get { return "ztime"; } }
        public override string shortcut { get { return "zt"; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdZTime() { }

        public override void Use(Player p, string message)
        {
            if (Server.zombie.ZombieStatus() == 0) { Player.SendMessage(p, "There is no Zombie Survival game currently in progress."); return; }
            if (!Server.zombieRound) { p.SendMessage("The current zombie round hasn't started yet!"); return; }

            TimeSpan t = TimeSpan.FromMilliseconds(Server.zombie.amountOfMilliseconds);

            string time = string.Format("{0:D2}minutes, {1:D2}seconds",
                                    t.Minutes,
                                    t.Seconds);
            message = time + " remaining for the current round!";
            Player.SendMessage(p, message);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ztime - Shows the current zombie survival round time left.");
        }
    }
}