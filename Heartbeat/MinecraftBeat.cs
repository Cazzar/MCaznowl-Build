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
using System.IO;

namespace MCForge
{
    class MinecraftBeat : Beat
    {
        public string URL { get { return "http://www.minecraft.net/heartbeat.jsp"; } }
        public string Parameters { get; set; }
        public bool Log { get { return false; } }

        public void Prepare()
        {
             Parameters += "&salt=" + Server.salt +
                 "&users=" + Player.number;
        }

        public void OnPump(string line)
        {
            // Only run the code below if we receive a response
            if (!String.IsNullOrEmpty(line.Trim()))
            {
                string newHash = line.Substring(line.LastIndexOf('=') + 1);

                // Run this code if we don't already have a hash or if the hash has changed
                if (String.IsNullOrEmpty(Server.Hash) || !newHash.Equals(Server.Hash))
                {
                    Server.Hash = newHash;
                    string serverURL = line;

                    //serverURL = "http://" + serverURL.Substring(serverURL.IndexOf('.') + 1);
                    Server.s.UpdateUrl(serverURL);
                    File.WriteAllText("text/externalurl.txt", serverURL);
                    Server.s.Log("URL found: " + serverURL);
                }
            }
        }
    }
}
