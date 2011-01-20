/*
 * Copyright MCForge 2011
 * Written by SebbiUltimate
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace MCForge
{
    public class CmdSendCmd : Command
    {
        public override string name { get { return "sendcmd"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
        public override void Use(Player p, string message)
        {
            Player player = Player.Find(message.Split(' ')[0]);
            if (player == null)
            {
                Player.SendMessage(p, "Error: Player is not online.");
            }
            else
            {
                if (p == null) { }
                else { if (player.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot use this on someone of equal or greater rank."); return; } }
                string command;
                string command2;
                try
                {
                    command = message.Split(' ')[1];
                    command2 = message.Split(' ')[2];
                    Command.all.Find(command).Use(player, command2);
                }
                catch
                {
                    Player.SendMessage(p, "No parameter found");
                    command = message.Split(' ')[1];
                    Command.all.Find(command).Use(player, "");
                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/sendcmd - Make another user use a command, (/sendcmd player command parameter)");
            Player.SendMessage(p, "ex: /sendcmd bob tp bob2");
        }
    }
}



