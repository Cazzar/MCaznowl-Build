/*
Copyright 2011 MCForge
Licensed under the
Educational Community License, Version 2.0 (the "License"); you may
not use this file except in compliance with the License. You may
obtain a copy of the License at
http://www.opensource.org/licenses/ecl2.php
Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the License for the specific language governing
permissions and limitations under the License.
*/	

using System;

namespace MCForge
{
    public class CmdAdminChat : Command
    {
        public override string name { get { return "adminchat"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdAdminChat() { }

        public override void Use(Player p, string message)
        {
            p.adminchat = !p.adminchat;
            if (p.adminchat) Player.SendMessage(p, "All messages will now be sent to Admins only");
            else Player.SendMessage(p, "Admin chat turned off");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/adminchat - Makes all messages sent go to Admins by default");
        }
    }
}


