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
            // Only run the code below if we receive a response and don't already know the server's URL hash
            if (!String.IsNullOrEmpty(line) && String.IsNullOrEmpty(Server.Hash))
            {
                Server.Hash = line.Substring(line.LastIndexOf('=') + 1);
                string serverURL = line;

                //serverURL = "http://" + serverURL.Substring(serverURL.IndexOf('.') + 1);
                Server.s.UpdateUrl(serverURL);
                File.WriteAllText("text/externalurl.txt", serverURL);
                Server.s.Log("URL found: " + serverURL);
            }
        }
    }
}
