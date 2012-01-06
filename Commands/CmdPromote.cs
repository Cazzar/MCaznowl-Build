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

using MCForge;
namespace MCForge.Commands
{
    public class CmdPromote : Command
    {
        public override string name { get { return "promote"; } }
        public override string shortcut { get { return "pr"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdPromote() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') != -1) { Help(p); return; }
            Player who = Player.Find(message);
            string foundName;
            Group foundGroup;
            if (who == null)
            {
                foundName = message;
                foundGroup = Group.findPlayerGroup(message);
            }
            else
            {
                foundName = who.name;
                foundGroup = who.group;
            }

            Group nextGroup = null; bool nextOne = false;
            for (int i = 0; i < Group.GroupList.Count; i++)
            {
                Group grp = Group.GroupList[i];
                if (nextOne)
                {
                    if (grp.Permission >= LevelPermission.Nobody) break;
                    nextGroup = grp;
                    break;
                }
                if (grp == foundGroup)
                    nextOne = true;
            }

            if (nextGroup != null)
                Command.all.Find("setrank").Use(p, foundName + " " + nextGroup.name + " " + Server.customPromoteMessage);
            else
                Player.SendMessage(p, "No higher ranks exist");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/promote <name> - Promotes <name> up a rank");
        }
    }
}