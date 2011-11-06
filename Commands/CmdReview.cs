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
    class CmdReview : Command
    {
        public override string name { get { return "review"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdReview() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
            {
                Player.SendMessage(p, "You can't execute this command as Console!");
                return;
            } 
            if (message == "enter" || message == "view" || message == "leave" || message == "clear" || message == "next")
            {
                if (message == "enter")
                {
                    Group gr = Group.Find(Server.reviewenter);
                    LevelPermission lp = gr.Permission;
                    if (p.group.Permission >= lp)
                    {
                    foreach (string testwho in Server.reviewlist)
                    {
                        if (testwho == p.name)
                        {
                            Player.SendMessage(p, "You already entered the review queue!");
                            return;
                        }
                    }
                    Server.reviewlist.Add(p.name);
                    int reviewlistpos = Server.reviewlist.IndexOf(p.name);
                    Player.SendMessage(p, "You entered the review queue. You have " + reviewlistpos.ToString() + " people in front of you in the queue");
                    return;
                    }
                }
                if (message == "view")
                {
                    Group gr = Group.Find(Server.reviewview);
                    LevelPermission lp = gr.Permission;
                    if (p.group.Permission >= lp)
                    {
                    if (Server.reviewlist.Count != 0)
                    {
                        Player.SendMessage(p, "&9Players in the review queue:");
                        int viewnumb = 1;
                        foreach (string golist in Server.reviewlist)
                        {
                            Player.SendMessage(p, "&a" + viewnumb.ToString() + ". " + golist);
                            viewnumb++;
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "There are no players in the review queue!");
                    }
                }
                }
                if (message == "leave")
                {
                    Group gr = Group.Find(Server.reviewleave);
                    LevelPermission lp = gr.Permission;
                    if (p.group.Permission >= lp)
                    {
                    bool leavetest = false;
                    foreach (string testwho2 in Server.reviewlist)
                    {
                        if (testwho2 == p.name)
                        {
                            leavetest = true;
                        }
                    }
                    if (!leavetest)
                    {
                        Player.SendMessage(p, "You aren't in the review queue so you can't leave it!");
                        return;
                    }
                    Server.reviewlist.Remove(p.name);
                    int toallplayerscount = 1;
                    foreach (string toallplayers in Server.reviewlist)
                    {
                        Player tosend = Player.Find(toallplayers);
                        Player.SendMessage(tosend, "The review queue has changed. Your now on spot " + toallplayerscount.ToString() + ".");
                        toallplayerscount++;
                    }
                    Player.SendMessage(p, "You have left the review queue!");
                    return;
                }
                }
                if (message == "next")
                {
                   Group gr = Group.Find(Server.reviewnext);
                    LevelPermission lp = gr.Permission;
                    if (p.group.Permission >= lp)
                    {
                        if (p == null)
                        {
                            Player.SendMessage(p, "You can't execute this command as Console!");
                            return;
                        }
                        string[] user = Server.reviewlist.ToArray();
                        Player who = Player.Find(user[0]);
                        if (who == null)
                        {
                            Player.SendMessage(p, "Player " + user[0] + " doesn't exist or is offline. " + user[0] + " has been removed from the review queue");
                            Server.reviewlist.Remove(user[0]);
                            return;
                        }
                        if (who == p)
                        {
                            Player.SendMessage(p, "You can't teleport to yourself! You have been removed from the review queue.");
                            Server.reviewlist.Remove(user[0]);
                            return;
                        }
                        Server.reviewlist.Remove(user[0]);
                        unchecked { p.SendPos((byte)-1, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1]); }
                        Player.SendMessage(p, "You have been teleported to " + user[0]);
                        Player.SendMessage(who, "Your request has been answered by " + p.name + ".");
                        int toallplayerscount = 0;
                        foreach (string toallplayers in Server.reviewlist)
                        {
                            Player who2 = Player.Find(toallplayers);
                            Player.SendMessage(who2, "The review queue has been rotated. you now have " + toallplayerscount.ToString() + " players waiting in front of you");
                            toallplayerscount++;
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "&cYou have no permission to use the review queue!");
                    }
                }
                if (message == "clear")
                {
                    Group gr = Group.Find(Server.reviewclear);
                    LevelPermission lp = gr.Permission;
                    if (p.group.Permission >= lp)
                    {
                        Server.reviewlist.Clear();
                        Player.SendMessage(p, "The review queue has been cleared");
                        return;
                    }
                    else
                    {
                        Player.SendMessage(p, "&cYou have no permission to clear the Review Queue!");
                    }
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
            Player.SendMessage(p, "/review <enter/view/leave/next/clear> - Lets you enter, view, leave, or clear the reviewlist or teleport you to the next player in the review queue.");
        }
    }
}
