/*
 * Created by SharpDevelop.
 * User: 501st_commander
 * Date: 1/9/2011
 * Time: 11:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;


namespace MCForge
{
    /// <summary>
    /// Description of CmdHackRank.
    /// </summary>
    public class CmdHackRank : Command
    {
        public override string name { get { return "hackrank"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        private string m_old_color;

        public CmdHackRank() { }

        public override void Use(Player p, string message)
        {
            //set the old color
            m_old_color = p.color;

            //oops, forgot rank!
            if (message == "") { Help(p); return; }

            //get the rank name and color
            Group newRank = Group.Find(message.Split(' ')[0]);

            //set the new color
            p.color = newRank.color;

            //sent the trick text
            Player.GlobalMessage(p.color + p.name + Server.DefaultColor + "'s rank was set to " + newRank.color + newRank.name);
            Player.GlobalChat(null, "&6Congratulations!", false);
            p.SendMessage("You are now ranked " + newRank.color + newRank.name + Server.DefaultColor + ", type /help for your new set of commands.");

            //make the timer for the kick
            System.Timers.Timer messageTimer = new System.Timers.Timer(6000);

            //start the timer
            messageTimer.Start();

            //delegate the timer
            messageTimer.Elapsed += delegate
            {
                //kick him!
                p.Kick("You have been kicked for hacking the rank " + newRank.color + newRank.name);
                p.color = m_old_color;
                messageTimer.Stop();
            };

        }


        public override void Help(Player p)
        {
            p.SendMessage("/hackrank [rank] - Hacks a rank");
            p.SendMessage("Usable Ranks:");
            p.SendMessage(Group.concatList(true, true, false));
        }
    }
}