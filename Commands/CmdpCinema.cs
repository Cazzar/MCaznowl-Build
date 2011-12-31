/*
	Copyright 2011 MCForge
	
	Written by Frederik Gelder
		
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
using System.Collections.Generic;
using System.Globalization;

namespace MCForge
{
    public class CmdpCinema : Command
    {
        CmdpCinema2[] cmdPC = new CmdpCinema2[100]; //reserving space for 100 movies.
        bool[] used = new bool[100];

        public override string name { get { return "pcinema"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override void Use(Player p, string message)
        {
            String[] tempmsg = message.Split(' ');
            String send = "";
            int movnum = 0;
            if (tempmsg.Length < 2 || tempmsg.Length > 3)
            {
                Help(p);
                return;
            }

            if (tempmsg.Length == 2)
            {
                if (tempmsg[0].ToLower(CultureInfo.CurrentCulture) == "abort")
                {
                    try
                    {
                        cmdPC[int.Parse(tempmsg[1], CultureInfo.CurrentCulture)].abort();

                        used[int.Parse(tempmsg[1], CultureInfo.CurrentCulture)] = false;
                    }
                    catch
                    {
                        Help(p);
                        return;
                    }
                    Player.SendMessage(p, "movie " + tempmsg[1] + " was aborted.");
                    return;
                }
                else if (tempmsg[0].ToLower(CultureInfo.CurrentCulture) == "delete")
                {
                    if (System.IO.File.Exists("extra/cin/" + tempmsg[1] + ".cin"))
                    {
                        System.IO.File.Delete("extra/cin/" + tempmsg[1] + ".cin");
                    }
                    return;
                }
                //no frametime. use default 1000. but that does pcinema2 for us
                send = tempmsg[1];
            }
            else if (tempmsg.Length == 3)
            {
                //frametime given
                send = tempmsg[1] + " " + tempmsg[2];
            }

            try
            {
                movnum = int.Parse(tempmsg[0], CultureInfo.CurrentCulture);
            }
            catch
            {
                Help(p);
                return;
            }

            if (used[movnum])
            {
                Player.SendMessage(p, "Movie is already used. stop it by using /pcinema abort [movienumber]");
                return;
            }
            else
            {
                //cmdPC[movnum] = new CmdpCinema2();
                try
                {
                    cmdPC[movnum].Use(p, send);//better not use a new instance. it worked but they were not stopable.
                }
                catch
                {
                    cmdPC[movnum] = new CmdpCinema2();
                    cmdPC[movnum].Use(p, send);
                }
                used[movnum] = true;
            }

        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/pCinema [movienumber] [filename] <frametime> - movienumber can be 0-99. filename explains itself. frametime is the time in ms each rame is displayed.no values under 200 accepted.else set to 200. You can delete unwanted movies with /pcinema delete <filename>");
        }
    }


}