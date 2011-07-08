/*
	Copyright 2011 MCForge
	
	Written by fenderrock87
	
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

namespace MCForge
{
    class MCForgeBeat : Beat
    {
        public string URL { get { return ServerSettings.HeartbeatAnnounce; } }
        public string Parameters { get; set; }
        public bool Log { get { return false; } }

        public void Prepare()
        {
            if (String.IsNullOrEmpty(Server.Hash))
            {
                Server.Hash = "";
                // THIS IS TEMPORARILY COMMENTED OUT SO PEOPLE CAN STILL HEARTBEAT WITH US USING A SAVED HASH!!
                //throw new Exception("Hash not set");
            }

            int hidden = 0;
            if (Player.number > 0)
            {
                string players = "";
                
                foreach (Player p in Player.players)
                {
                    if (p.hidden)
                    {
                        hidden++;
                        continue;
                    }
                    players += p.name + " (" + p.group.name + ")" + ",";
                }
                if (Player.number - hidden > 0)
                    Parameters += "&players=" + players.Substring(0, players.Length - 1);
            }

            if (Server.levels != null && Server.levels.Count > 0)
            {
                IEnumerable<string> worlds = from l in Server.levels select l.name;
                Parameters += "&worlds=" + String.Join(", ", worlds.ToArray());
            }

            Parameters += "&motd=" + Heart.UrlEncode(Server.motd) +
                    "&lvlcount=" + (byte)Server.levels.Count +
                    "&serverversion=" + Server.Version/*.Replace(".0", "")*/ +
                    "&hash=" + Server.Hash +
                    "&users=" + (Player.number - hidden) +
                    "&permalinkhash=" + Permalink.UniqueHash;
        }

        public void OnPump(string line)
        {
            line = line.Trim();
            if (!String.IsNullOrEmpty(line))
            {
                try
                {
                    Uri oldURL = Permalink.URL;
                    Uri newUrl = new Uri(line);
                    if (oldURL == null && newUrl != null)
                    {
                        // We got the URL!
                        Permalink.URL = newUrl;
                        // TODO: Place this in the UI somewhere
                    }
                }
                catch { }
            }
            return;
        }

    }
}
