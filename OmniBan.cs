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
using System.Net;
using System.IO;
using System.Globalization;

namespace MCForge
{
    public class OmniBan
    {
        private List<string> _bans; 

        public List<string> bans
        {
            get
            {
                return _bans;
            }
            set
            {
                _bans = value;
            }
        }
        private string _kickMsg; 

        public string kickMsg
        {
            get
            {
                return _kickMsg;
            }
            set
            {
                _kickMsg = value;
            }
        }


        public OmniBan()
        {
            bans = new List<string>();
            kickMsg = "Omniban! Contact MCForge staff to appeal.";
        }

        public void Load(bool web)
        {
            if (web)
            {
                try
                {
                    string data = "";
                    try
                    {
                        using (WebClient WC = new WebClient())
                            data = WC.DownloadString("http://mcforge.net/omniban.php").ToLower(CultureInfo.CurrentCulture);
                    }
                    catch { Load(false); return; }

                    bans.Clear();
                    bans.AddRange(data.Split(';'));
                    Save();
                }
                catch (Exception e) { Server.ErrorLog(e); }
            }
            else
            {
                if (!File.Exists("text/omniban.txt")) return;

                try
                {
                    foreach (string line in File.ReadAllLines("text/omniban.txt"))
                        if (!String.IsNullOrEmpty(line)) bans.Add(line.ToLower(CultureInfo.CurrentCulture));
                }
                catch (Exception e) { Server.ErrorLog(e); }
            }
        }

        public void Save()
        {
            try
            {
                File.Create("text/omniban.txt").Dispose();
                using (StreamWriter SW = File.CreateText("text/omniban.txt"))
                    foreach (string ban in bans)
                        SW.WriteLine(ban);
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public void KickAll()
        {
            try
            {
                kickall:
                foreach (Player p in Player.players)
                    if (CheckPlayer(p))
                    {
                        p.Kick(kickMsg);
                        goto kickall;
                    }
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public bool CheckPlayer(Player p)
        {
            return p.name.ToLower(CultureInfo.CurrentCulture) != Server.server_owner.ToLower(CultureInfo.CurrentCulture) && !Player.IPInPrivateRange(p.ip) && (bans.Contains(p.name.ToLower(CultureInfo.CurrentCulture)) || bans.Contains(p.ip));
        }
    }
}
