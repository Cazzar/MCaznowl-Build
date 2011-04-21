/*
	Copyright 2011 MCForge.
	
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    class TChaloBeat : Beat
    {
        public string URL { get { return "http://minecraft.tchalo.com/announce.php"; } }
        public string Parameters { get; set; }
        public bool Log { get { return false; } }

        public void Prepare()
        {
            if (Server.Hash == null)
                throw new Exception("Hash not set");



            // build list of current players in server
            if (Player.number > 0)
            {
                string players = "";
                int hidden = 0;
                foreach (Player p in Player.players)
                {
                    if (p.hidden)
                    {
                        hidden++;
                        continue;
                    }
                    players += p.name + ",";
                }
                if (Player.number - hidden > 0)
                    Parameters += "&players=" + players.Substring(0, players.Length - 1);
            }

            string worlds = "";
            foreach (Level l in Server.levels)
            {
                worlds += l.name + ",";
                Parameters += "&worlds=" + worlds.Substring(0, worlds.Length - 1);
            }

            Parameters += "&motd=" + Heart.UrlEncode(Server.motd) +
                    "&hash=" + Server.Hash +
                    "&data=" + Server.Version + "," + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                    "&server=MCForge" +
                    "&details=Running MCForge version " + Server.Version +
                    "&users=" + Player.number;
        }

        public void OnPump(string line)
        {
            return;
        }

    }
}
