﻿/*
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
 * 
 * TEMPRANKLIST SCRIPT BY BeMacized www.bemacized.com
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace MCForge
{
    class CmdTempRankList : Command
    {
        public override string name { get { return "tempranklist"; } }
        public override string shortcut { get { return "trl"; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdTempRankList() { }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                Help(p);
                Player.SendMessage(p, "&cThis command contains no arguments");
                goto end;
            }
            string alltext = File.ReadAllText("text/tempranks.txt");
            if (alltext == "")
            {
                Player.SendMessage(p, "&cThere are no players with a temporary rank assigned.");
                goto end;
            }
            Player.SendMessage(p, "&ePlayers with a temporary rank assigned:");
            foreach (string line in File.ReadAllLines("text/tempranks.txt"))
            {
                string printtoconsole = line.Split(' ')[0];
                Player.SendMessage(p, "&a" + printtoconsole);
            }
        end:
            Thread.Sleep(0);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/tempranklist - Lists every user with a temporary rank");
        }
    }
}
