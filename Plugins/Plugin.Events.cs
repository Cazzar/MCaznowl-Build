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
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
	public partial class Plugin
	{

        /// <summary>
        /// Check to see if a Player event is stopped
        /// </summary>
        /// <param name="e">The event to check</param>
        /// <param name="p">The Player that event is related to</param>
        /// <returns>This returns true or false, true means its stopped, false means its not</returns>
        public static bool IsPlayerEventCancled(PlayerEvents e, Player p)
        {
            switch (e)
            {
                case PlayerEvents.BlockChange:
                    return p.cancelBlock;
                case PlayerEvents.PlayerChat:
                    return p.cancelchat;
                case PlayerEvents.PlayerCommand:
                    return p.cancelcommand;
                case PlayerEvents.PlayerMove:
                    return p.cancelmove;
                case PlayerEvents.MYSQLSave:
                    return p.cancelmysql;
                case PlayerEvents.PlayerRankChange:
                    return Group.cancelrank;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Cancel Level event
        /// </summary>
        /// <param name="e">The event to cancel</param>
        /// <param name="l">The level to cancel the event on</param>
        public static void CancelLevelEvent(LevelEvents e, Level l)
        {
            switch (e)
            {
                case LevelEvents.LevelUnload:
                    break;
            }
        }
        /// <summary>
        /// Cancel Global Level Event
        /// </summary>
        /// <param name="e">The event you want to cancel</param>
        public static void CancelGlobalLevelEvent(GlobalLevelEvents e)
        {
            switch (e)
            {
                case GlobalLevelEvents.LevelLoad:
                    Level.cancelload = true;
                    break;
                case GlobalLevelEvents.LevelSave:
                    Level.cancelsave = true;
                    break;
            }
        }
        /// <summary>
        /// Cancel a player event
        /// </summary>
        /// <param name="e">The event that you want to cancel</param>
        /// <param name="p">The Player that event is related to</param>
        public static void CancelPlayerEvent(PlayerEvents e, Player p) {
            //TODO
            //Add some more events to be canceled
            switch (e)
            {
                case PlayerEvents.BlockChange:
                    p.cancelBlock = true;
                    break;
                case PlayerEvents.PlayerChat:
                    p.cancelchat = true;
                    break;
                case PlayerEvents.PlayerCommand:
                    p.cancelcommand = true;
                    break;
                case PlayerEvents.PlayerMove:
                    p.cancelmove = true;
                    break;
                case PlayerEvents.MYSQLSave:
                    p.cancelmysql = true;
                    break;
                case PlayerEvents.PlayerRankChange:
                    Group.cancelrank = true;
                    break;
            }
        }
	}
}
