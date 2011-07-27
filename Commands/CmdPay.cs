/*
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

namespace MCForge
{
    public class CmdPay : Command
    {
        public override string name { get { return "pay"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdPay() { }

        public override void Use(Player p, string message)
        {
            if (message.IndexOf(' ') == -1) { Help(p); return; }
            if (message.Split(' ').Length != 2) { Help(p); return; }

            Player who = Player.Find(message.Split(' ')[0]);
            if (who == null) { Player.SendMessage(p, "Could not find player entered"); return; }
            if (who == p) { Player.SendMessage(p, "Sorry. Can't allow you to pay yourself"); return; }

            int amountPaid;
            try { amountPaid = int.Parse(message.Split(' ')[1]); }
            catch { Player.SendMessage(p, "Invalid amount"); return; }

            if (who.money + amountPaid > 16777215) { Player.SendMessage(p, "Players cannot have over 16777215 " + Server.moneys); return; }
            if (p.money - amountPaid < 0) { Player.SendMessage(p, "You don't have that much " + Server.moneys); return; }
            if (amountPaid < 0) { Player.SendMessage(p, "Cannot pay negative " + Server.moneys); return; }

            who.money += amountPaid;
            p.money -= amountPaid;
            Player.GlobalMessage(p.color + p.name + Server.DefaultColor + " paid " + who.color + who.name + Server.DefaultColor + " " + amountPaid + " " + Server.moneys);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/pay [player] <amount> - Pays <amount> of " + Server.moneys + " to [player]");
        }
    }
}