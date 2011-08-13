/*
 
        Written by Jack1312
  
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
using System.IO;

namespace MCForge
{
    public class CmdIgnore : Command
    {
        public override string name { get { return "ignore"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdIgnore() { }
        public static byte lastused = 0;
        public static string ignore = "";
        public static string ignore2 = "";

        public override void Use(Player p, string message)
        {
            if (message.Split(' ')[0] == "global")
            {
                if (p.ignoreglobal == false)
                {
                    Player.globalignores.Add(p.name);
                    p.ignoreglobal = true;
                    Player.SendMessage(p, "&cGlobal Chat is now being ignored!");
                    return;
                }
                p.ignoreglobal = false;
                Player.globalignores.Remove(p.name);
                Player.SendMessage(p, "&cGlobal Chat is no longer being ignored!");
                return;
            }
            if (message.Split(' ')[0] == "list")
            {
                Player.SendMessage(p, "&cCurrently ignoring the following players:");
                foreach (string ignoring in p.listignored)
                {
                    Player.SendMessage(p, "- " + ignoring);
                }
                return;
            }
            Player who = Player.Find(message.Split(' ')[0]);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player specified!");
                return;
            }

            if (who.name == p.name)
            {
                Player.SendMessage(p, "You cannot ignore yourself!!");
                return;
            }

            if (who.group.Permission >= Server.opchatperm)
            {
                if (p.group.Permission <= who.group.Permission)
                {
                    Player.SendMessage(p, "You cannot ignore an operator of higher rank!");
                    return;
                }
            }
            
     
            if (!Directory.Exists("ranks/ignore"))
            {
                Directory.CreateDirectory("ranks/ignore");
            }
            if (!File.Exists("ranks/ignore/" + p.name + ".txt"))
            {
                File.Create("ranks/ignore/" + p.name + ".txt").Dispose();
            }
            string chosenpath = "ranks/ignore/" + p.name + ".txt";
            if (!File.Exists(chosenpath))
            {
                p.listignored.Add(who.name);
                Player.SendMessage(p, "Player now ignored: &c" + who.name + "!");
                return;
            }
            
            if (!p.listignored.Contains(who.name))
            {
                p.listignored.Add(who.name);
                Player.SendMessage(p, "Player now ignored: &c" + who.name + "!");
                return;
            }
            if (p.listignored.Contains(who.name))
            {
                p.listignored.Remove(who.name);
                Player.SendMessage(p, "Player is no longer ignored: &c" + who.name + "!");
                return;
            }
            Player.SendMessage(p, "Something is stuffed.... Tell a MCForge Developer!");
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/ignore [Player] - Ignores the specified player.");
            Player.SendMessage(p, "/ignore global - Ignores the global chat.");
            Player.SendMessage(p, "Note: You cannot ignore operators of higher rank!");
        }
    }
}