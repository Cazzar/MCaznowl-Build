using System;
using System.IO;

namespace MCForge
{
    class WOMBeat : Beat
    {
        public string URL { get { return "https://direct.worldofminecraft.com/hb.php"; } }
        public string Parameters { get; set; }
        public bool Log { get { return false; } }
        //https://direct.worldofminecraft.com/server.php?ip=24.34.160.117&port=25566&salt=SERVER_SALT_HERE&alt=GamezGalaxy+Lavasurvival&desc=One+of+a+kind+Lavasurvival%21+www.gamezgalaxy.com&flags=%5BLavaSurvival%5D
        public void SetSettings(string IP, string Port, string SALT, string Name, string Disc, string flags)
        {
            string url = "https://direct.worldofminecraft.com/server.php?ip=" + IP + "&port=" + Port + "&salt=" + Server.salt + "&alt=" + Name.Replace(' ', '+') + "&desc=" + Disc.Replace(' ', '+') + "&flags=%5B" + flags + "%5D";
            //TODO use this url some how..
        }
        public void Prepare()
        {
             Parameters += "&salt=" + Server.salt +
                 "&users=" + Player.number;
        }

        public void OnPump(string line)
        {
            // Only run the code below if we receive a response
            /*if (!String.IsNullOrEmpty(line.Trim()))
            {
                string newHash = line.Substring(line.LastIndexOf('=') + 1);

                // Run this code if we don't already have a hash or if the hash has changed
                if (String.IsNullOrEmpty(Server.Hash) || !newHash.Equals(Server.Hash))
                {
                    Server.Hash = newHash;
                    string serverURL = line;

                    //serverURL = "http://" + serverURL.Substring(serverURL.IndexOf('.') + 1);
                    //Server.s.UpdateUrl(serverURL);
                    //File.WriteAllText("text/externalurl.txt", serverURL);
                    //Server.s.Log("URL found: " + serverURL);
                }
            }*/
        }
    }
}
