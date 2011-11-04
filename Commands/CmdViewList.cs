/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
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
using System.Text;

namespace MCForge
{
    class CmdViewList : Command
    {
        public override string name { get { return "viewlist"; } }
        public override string shortcut { get { return "vl"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdViewList() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Player.SendMessage(p, "You can't execute this command as Console!");
                return;
            }
            if (message == "enter" || message == "view" || message == "leave")
            {
                if (message == "enter")
                {
                    foreach (string testwho in Server.viewlist)
                    {
                        if (testwho == p.name)
                        {
                            Player.SendMessage(p, "You already entered the viewlist!");
                            return;
                        }
                    }
                    Server.viewlist.Add(p.name);
                    int viewlistpos = Server.viewlist.IndexOf(p.name);
                    Player.SendMessage(p, "You entered the viewlist. You have " + viewlistpos.ToString() + " people in front of you in the row");
                    return;
                }
                if (message == "view")
                {
                    if (Server.viewlist.Count != 0)
                    {
                        Player.SendMessage(p, "&9Players in viewlist:");
                        int viewnumb = 1;
                        foreach (string golist in Server.viewlist)
                        {
                            Player.SendMessage(p, "&a" + viewnumb.ToString() + ". " + golist);
                            viewnumb++;
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "There are no players in the viewlist!");
                    }
                }
                if (message == "leave")
                {
                    bool leavetest = false;
                    foreach (string testwho2 in Server.viewlist)
                    {
                        if (testwho2 == p.name)
                        {
                            leavetest = true;
                        }
                    }
                    if (!leavetest)
                    {
                        Player.SendMessage(p, "You aren't in the viewlist so you can't leave it!");
                        return;
                    }
                    Server.viewlist.Remove(p.name);
                    int toallplayerscount = 1;
                    foreach (string toallplayers in Server.viewlist)
                    {
                        Player tosend = Player.Find(toallplayers);
                        Player.SendMessage(tosend, "The viewlist has been changed. Your now on spot " + toallplayerscount.ToString() + ".");
                        toallplayerscount++;
                    }
                    Player.SendMessage(p, "You have left the viewlist!");
                    return;
                }
            }
            else
            {
                Help(p);
                return;
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/viewlist <enter/view/leave> - Lets you enter, view, or leave the viewlist");
        }
    }
}
