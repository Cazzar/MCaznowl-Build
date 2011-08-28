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
using System.Linq;
using System.Text;

namespace MCForge.Plugins
{
	public partial class Plugin
	{

        /// <summary>
        /// Check to see if an event is stopped
        /// </summary>
        /// <param name="e">The event to check</param>
        /// <param name="p">The Player that event is related to (null if not dealing with player event)</param>
        /// <returns>This returns true or false, true means its stopped, false means its not</returns>
        public static bool IsEventCancled(Events e, Player p)
        {
            switch (e)
            {
                case Events.BlockChange:
                    return p.cancelBlock;
                case Events.PlayerChat:
                    return p.cancelchat;
                case Events.PlayerCommand:
                    return p.cancelcommand;
                case Events.PlayerMove:
                    return p.cancelmove;
                case Events.LevelLoad:
                    return Level.cancelload;
                case Events.LevelSave:
                    return Level.cancelsave;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Cancel a server event
        /// </summary>
        /// <param name="e">The event that you want to cancel</param>
        /// <param name="p">The Player that event is related to (null if not dealing with player event)</param>
        public static void CancelEvent(Events e, Player p) {
            //TODO
            //Add some more events to be canceled
            switch (e)
            {
                case Events.BlockChange:
                    p.cancelBlock = true;
                    break;
                case Events.PlayerChat:
                    p.cancelchat = true;
                    break;
                case Events.PlayerCommand:
                    p.cancelcommand = true;
                    break;
                case Events.PlayerMove:
                    p.cancelmove = true;
                    break;
                case Events.LevelLoad:
                    Level.cancelsave = true;
                    break;
                case Events.LevelSave:
                    Level.cancelload = true;
                    break;
            }
        }
	}
}
