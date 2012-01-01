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
using System.IO;
namespace MCForge
{
    public class CmdNews : Command
    {
        public override string name { get { return "news"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public static string keywords { get { return "info latest"; } }
        public override void Use(Player p, string message)
        {
            string newsFile = "text/news.txt";
            if (!File.Exists(newsFile) || (File.Exists(newsFile) && File.ReadAllLines(newsFile).Length == -1))
            {
				using (StreamWriter SW = new StreamWriter(newsFile))
				{
					SW.WriteLine("News have not been created. Put News in '" + newsFile + "'.");
				}
                return;
            }
            string[] strArray = File.ReadAllLines(newsFile);
            if (message == "") { for (int j = 0; j < strArray.Length; j++) { Player.SendMessage(p, strArray[j]); } }
            else
            {
                string[] split = message.Split(' ');
                if (split[0] == "all") { if ((int)p.group.Permission < CommandOtherPerms.GetPerm(this)) { Player.SendMessage(p, "You must be at least " + Group.findPermInt(CommandOtherPerms.GetPerm(this)).name + " to send this to all players."); return; } for (int k = 0; k < strArray.Length; k++) { Player.GlobalMessage(strArray[k]); } return; }
                else
                {
                    Player player = Player.Find(split[0]);
                    if (player == null) { Player.SendMessage(p, "Could not find player \"" + split[0] + "\"!"); return; }
                    for (int l = 0; l < strArray.Length; l++) { Player.SendMessage(player, strArray[l]); }
                    Player.SendMessage(p, "The News were successfully sent to " + player.name + ".");
                    return;
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/news - Shows server news.");
            Player.SendMessage(p, "/news <player> - Sends the News to <player>.");
            Player.SendMessage(p, "/news all - Sends the News to everyone.");
        }
    }
}