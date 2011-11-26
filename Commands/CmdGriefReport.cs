using System;
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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.IO;


namespace MCForge
{
    class CmdGriefReport : Command
    {
        public override string name { get { return "griefreport"; } }
        public override string shortcut { get { return "gr"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdGriefReport() { }

        public override void Use(Player p, string message)
        {
            string md5name = "";
            string name = "";
            if (message == "")
            {
                Help(p);
                Player.SendMessage(p, "&cYou need to enter a name!");
                return;
            }
            Player who = Player.Find(message);
            Group hey = Group.findPlayerGroup(message);
            if (who == null)
            {
                Player.SendMessage(p, "The player has not been found"); return;
            }
            else
            {
                name = who.name;
            }
            md5name = EncodePassword(name);
            string alltext = File.ReadAllText("text/grdata.txt");
            if (alltext.Contains(md5name))
            {
                Player.SendMessage(p, "&cThe player has already been reported!!!");
                return;
            }
            else
            {
                WebClient wc = new WebClient();
                try
                {
                    wc.DownloadString("http://mcforge.bemacizedgaming.com/griefregister/grdata.php?name=" + name.ToString());
                    Player.SendMessage(p, "The player has been reported succesfully!");
                }
                catch
                {
                    Server.s.Log("Grief data sending failed!");
                    Player.SendMessage(p, "&cSomething went wrong! The player has not been reported.");
                }
                finally
                {
                    if (wc != null)
                        wc.Dispose();
                }
                try
                {
                    StreamWriter sw;
                    sw = File.AppendText("text/grdata.txt");
                    sw.WriteLine(md5name);
                    sw.Close();
                }
                catch
                {

                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/griefreport <name> - Report a griefer to MCForge. DO NOT ABUSE!!!");
        }
        public string EncodePassword(string originalPassword)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }
    }
}


