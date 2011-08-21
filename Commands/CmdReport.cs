/*
 * Written By Jack1312

	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
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
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net;

namespace MCForge
{
    public class CmdReport : Command
    {
        public override string name { get { return "report"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdReport() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Player.SendMessage(p, "This command can not be used in console!");
                return;
            }
            int number = message.Split(' ').Length;
            if (message == "")
            {
                Help(p);
                return;
            }
            if (number == 1)
            {
                if (p.group.Permission < LevelPermission.Operator)
                {
                    Player.SendMessage(p, "You need to be an operator to do that!");
                    return;
                }
                if (message.ToLower() == "check")
                {

                    if (!Directory.Exists("extra/reported"))
                    {
                        Directory.CreateDirectory("extra/reported");
                    }
                    bool foundone = false;
                    DirectoryInfo di = new DirectoryInfo("extra/reported");
                    FileInfo[] fi = di.GetFiles("*.txt");
                    Player.SendMessage(p, "The following players have been reported:");
                    foreach (FileInfo file in fi)
                    {
                        foundone = true;
                        var parsed = file.Name.Replace(".txt", "");
                        Player.SendMessage(p, "- " + parsed);
                    }
                    if (foundone == true)
                    {
                        Player.SendMessage(p, "Use /report view [Player] to view the reason and the reporter.");
                        Player.SendMessage(p, "Use /report remove [Player] to remove the report on a player.");
                    }
                    else
                    {
                        Player.SendMessage(p, "No reports were found!");
                        return;
                    }
                    return;
                }
            }


            if (number >= 2)
            {
                int pos = message.IndexOf(' ');
                string msg1 = message.Substring(0, pos).ToLower();
                string msg2 = message.Substring(pos + 1).ToLower();
                if (msg1.ToLower() == "view")
                {
                    if (p.group.Permission < LevelPermission.Operator)
                    {
                        Player.SendMessage(p, "You need to be an operator to do that!");
                        return;
                    }
                    if (!File.Exists("extra/reported/" + msg2 + ".txt"))
                    {
                        Player.SendMessage(p, "The player you specified has not been reported!");
                        return;
                    }
                    var readtext = File.ReadAllText("extra/reported/" + msg2 + ".txt");
                    Player.SendMessage(p, readtext);
                    return;
                }
                if (msg1.ToLower() == "remove")
                {
                    if (p.group.Permission < LevelPermission.Operator)
                    {
                        Player.SendMessage(p, "You need to be an operator to do that!");
                        return;
                    }
                    if (!File.Exists("extra/reported/" + msg2 + ".txt"))
                    {
                        Player.SendMessage(p, "The player you specified has not been reported!");
                        return;
                    }
                    if (!Directory.Exists("extra/reportedbackups"))
                    {
                        Directory.CreateDirectory("extra/reportedbackups");
                    }
                    if (File.Exists("extra/reportedbackups/" + msg2 + ".txt"))
                    {
                        File.Delete("extra/reportedbackups/" + msg2 + ".txt");
                    }
                    File.Move("extra/reported/" + msg2 + ".txt", "extra/reportedbackups/" + msg2 + ".txt");
                    Player.SendMessage(p, msg2 + "'s report has been deleted.");
                    Server.s.Log(msg2 + "'s report has been deleted by " + p.name);
                    return;
                }
                if (File.Exists("extra/reported/" + msg1 + ".txt"))
                {
                    File.WriteAllText("extra/reported/" + msg1 + "(2).txt", msg2 + " - Reported by " + p.name + "." + " DateTime: " + DateTime.Now);
                    Player.SendMessage(p, "Your report has been sent, it should be viewed when an operator is next online!");
                    return;
                }
                if (File.Exists("extra/reported/" + msg1 + "(2).txt"))
                {
                    File.WriteAllText("extra/reported/" + msg1 + "(3).txt", msg2 + " - Reported by " + p.name + "." + " DateTime: " + DateTime.Now);
                    Player.SendMessage(p, "Your report has been sent, it should be viewed when an operator is next online!");
                    return;
                }
                if (File.Exists("extra/reported/" + msg1 + "(3).txt"))
                {
                    Player.SendMessage(p, "The player you have reported has already been reported 3 times! Please wait patiently or come back when an op is online!");
                    return;
                }
                File.WriteAllText("extra/reported/" + msg1 + ".txt", msg2 + " - Reported by " + p.name + "." + " DateTime: " + DateTime.Now);
                Player.SendMessage(p, "Your report has been sent, it should be viewed when an operator is next online!");

            }

        }



        public override void Help(Player p)
        {
            Player.SendMessage(p, "/report [Player] [Reason] - Reports the specified player for the reason/");
            if (p.group.Permission >= LevelPermission.Operator)
            {
                Player.SendMessage(p, "/report check - Checks the reported list!");
                Player.SendMessage(p, "/report view [Player] - View the report on the specified player");
                Player.SendMessage(p, "/report delete [Player] - Delete the report on the specified player");
            }
        }
    }
}
