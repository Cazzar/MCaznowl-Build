/*
	Copyright 2011 MCForge
		
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.osedu.org/licenses/ECL-2.0
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;

namespace MCForge
{
   public class CmdReload : Command
   {
      public override string name { get { return "reload"; } }
      public override string shortcut { get { return "re"; } }
      public override string type { get { return "map"; } }
      public override bool museumUsable { get { return false; } }
      public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
      public override void Use(Player p, string message)
      {
         Command.all.Find("unload").Use(p, message);
         Command.all.Find("load").Use(p, message);
      }

      // This one controls what happens when you use /help [commandname].
      public override void Help(Player p)
      {
         Player.SendMessage(p, "/reload [map name] - Reloads the map.");
      }
   }
}