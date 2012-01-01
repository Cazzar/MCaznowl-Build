﻿/*
   ______________________________________________________
Can be included and resold as part of a larger work (sublicensable)
Commercial use allowed under the following conditions :
Command May only be used by MCForge users.This only gives permission to MCForge and users, This may not be posted on any other site and or forums without the permission of the maker [xTYx728]
Can modify source-code but cannot distribute modifications (derivative works)
Support provided as follows :
For this command any, issues you may contact the maker for help.
Attribution to software creator must be made as specified:
Any edits MUST be reported back to the MCForge forums and credit MUST be given to the respected maker
Software trademarks are included in the license
Software patents are included in the license
Additional terms:
This command is not to be posted on ANY other site and (or) forums. Any edits credit MUST be given to the respected owner.
_______________________________________________________________________________________________________

for full licences please go to http://www.binpress.com/license/view/l/2043171cc2d4e3237680e59d87e4dae6
_______________________________________________________________________________________________________
 Idea by tommyz_. made by xTYx728
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace MCForge
{
    public class CmdKillPhysics : Command
    {
        public override string name { get { return "killphysics"; } }
        public override string shortcut { get { return "kp"; } }
        public override string type { get { return ""; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                foreach (Level l in Server.levels)
                {
                    if (l.physics > 0)
                    {
                        Command.all.Find("physics").Use(null, l.name + " 0");
                        Thread.Sleep(500);
                    }
                }
                Player.SendMessage(p, "All physics killed");
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/killphysics - kills all physics on every level.");

        }
    }
}
