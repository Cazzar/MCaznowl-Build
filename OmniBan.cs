using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MCForge
{
    public class OmniBan
    {
        public List<string> bans;
        public string kickMsg;


        public OmniBan()
        {
            bans = new List<string>();
            kickMsg = "You are banned from all MCForge servers!";
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
                            data = WC.DownloadString("http://mcforge.net/omniban.php").ToLower();
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
                        if (!String.IsNullOrEmpty(line)) bans.Add(line.ToLower());
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
            return p.name.ToLower() != Server.server_owner.ToLower() && !Player.IPInPrivateRange(p.ip) && bans.Contains(p.name.ToLower());
        }
    }
}
