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
using System.Linq;
using System.Text;
using System.IO;

namespace MCForge
{
    public static class Ban
    {
        public static void Banplayer(Player p, Player who, string reason, bool stealth, string oldrank)
        {
            // Getting date and time.
            string dayname = DateTime.Now.DayOfWeek.ToString();
            string daynumber = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            // Creating date + time string that looks nice to read:
            string datetime = dayname + " " + daynumber + " " + month + " " + year + ", at " + hour + ":" + minute;
            // checking if p = player or console
            string player;
            if (p == null) player = "Console";
            else player = p.name;
            // Checking stealth
            string stealthn;
            if (stealth) stealthn = "true";
            else stealthn = "false";
            Write(player, who, reason, stealthn, datetime, oldrank);
        }
        public static void Write(string pl, Player who, string reason, string stealthstr, string datetime, string oldrank)
        {
            string filepath = "text/bans/" + who.name + ".txt";
            if (File.Exists(filepath)) File.Delete(filepath);
            File.CreateText(filepath).Close();

            TextWriter tw = new StreamWriter(filepath);
            tw.WriteLine("banned-by=" + pl);
            tw.WriteLine("reason=" + reason);
            tw.WriteLine("timedate=" + datetime);
            tw.WriteLine("oldrank=" + oldrank);
            tw.WriteLine("stealth=" + stealthstr);
            tw.Close();
        }
    }
}
