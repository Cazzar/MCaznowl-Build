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
using System.Collections.Generic;
using System.Data;
//using MySql.Data.MySqlClient;
//using MySql.Data.Types;

namespace MCForge
{
    public class CmdSend : Command
    {
        public override string name { get { return "send"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdSend() { }

        public override void Use(Player p, string message)
        {
            if (message == "" || message.IndexOf(' ') == -1) { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);

            string whoTo, fromname;
            if (who != null) whoTo = who.name;
            else whoTo = message.Split(' ')[0];
            if (p != null) fromname = p.name;
            else fromname = "Console";

            message = message.Substring(message.IndexOf(' ') + 1);

            //DB
            if (Server.useMySQL)
            {
                if (message.Length > 255) { Player.SendMessage(p, "Message was too long. The text below has been trimmed."); Player.SendMessage(p, message.Substring(256)); message = message.Remove(256); }
                MySQL.executeQuery("CREATE TABLE if not exists `Inbox" + whoTo + "` (PlayerFrom CHAR(20), TimeSent DATETIME, Contents VARCHAR(255));");
                MySQL.executeQuery("INSERT INTO `Inbox" + whoTo + "` (PlayerFrom, TimeSent, Contents) VALUES ('" + fromname + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + message.Replace("'", "\\'") + "')");
            }
            else
            {
                SQLite.executeQuery("CREATE TABLE if not exists `Inbox" + whoTo + "` (PlayerFrom TEXT, TimeSent DATETIME, Contents TEXT);");
                Server.s.Log(message.Replace("'", "\\'"));
                SQLite.executeQuery("INSERT INTO `Inbox" + whoTo + "` (PlayerFrom, TimeSent, Contents) VALUES ('" + fromname + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + message.Replace("'", "''") + "')");
            }
            //DB

            Player.SendMessage(p, "Message sent to &5" + whoTo + ".");
            if (who != null) who.SendMessage("Message recieved from &5" + fromname + Server.DefaultColor + ".");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/send [name] <message> - Sends <message> to [name].");
        }
    }
}