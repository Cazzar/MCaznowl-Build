﻿/*
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
using System.IO;
using System.Collections.Generic;
using Sharkbite.Irc;
using System.Text;
//using System.Threading;

namespace MCForge
{
    public class ForgeBot
    {
        private Connection connection;
        private List<string> banCmd;
        private string channel, opchannel;
        private string nick;
        private string server;
        private bool reset = false;
        private byte retries = 0;
        public string usedCmd = "";
        public ForgeBot(string channel, string opchannel, string nick, string server)
        {
            this.channel = channel.Trim(); this.opchannel = opchannel.Trim(); this.nick = nick.Replace(" ", ""); this.server = server;
            connection = new Connection(new ConnectionArgs(nick, server), false, false);
            banCmd = new List<string>();
            if (Server.irc)
            {
                // Regster events for outgoing
                Player.PlayerChat += new Player.OnPlayerChat(Player_PlayerChat);
                Player.PlayerConnect += new Player.OnPlayerConnect(Player_PlayerConnect);
                Player.PlayerDisconnect += new Player.OnPlayerDisconnect(Player_PlayerDisconnect);

                // Regster events for incoming
                connection.Listener.OnNick += new NickEventHandler(Listener_OnNick);
                connection.Listener.OnRegistered += new RegisteredEventHandler(Listener_OnRegistered);
                connection.Listener.OnPublic += new PublicMessageEventHandler(Listener_OnPublic);
                connection.Listener.OnPrivate += new PrivateMessageEventHandler(Listener_OnPrivate);
                connection.Listener.OnError += new ErrorMessageEventHandler(Listener_OnError);
                connection.Listener.OnQuit += new QuitEventHandler(Listener_OnQuit);
                connection.Listener.OnJoin += new JoinEventHandler(Listener_OnJoin);
                connection.Listener.OnPart += new PartEventHandler(Listener_OnPart);
                connection.Listener.OnDisconnected += new DisconnectedEventHandler(Listener_OnDisconnected);

                // Load banned commands list
                if (!File.Exists("text/ircbancmd.txt")) File.Create("text/ircbancmd.txt").Dispose();
                foreach (string line in File.ReadAllLines("text/ircbancmd.txt"))
                    banCmd.Add(line);
            }
        }
        public void Say(string message, bool opchat = false)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PublicMessage(opchat ? opchannel : channel, message);
        }
        public void Pm(string user, string message)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PrivateMessage(user, message);
        }
        public void Reset()
        {
            if (!Server.irc) return;
            reset = true;
            retries = 0;
            Disconnect("Bot resetting...");
            Connect();
        }
        void Listener_OnJoin(UserInfo user, string channel)
        {
            doJoinLeaveMessage(user.Nick, "joined", channel);
        }
        void Listener_OnPart(UserInfo user, string channel, string reason)
        {
            if (user.Nick == nick) return;
            doJoinLeaveMessage(user.Nick, "left", channel);
        }

        private void doJoinLeaveMessage(string who, string verb, string channel)
        {
            Server.s.Log(String.Format("{0} has {1} channel {2}", who, verb, channel));
            Player.GlobalMessage(String.Format("{0}[IRC] {1} has {2} the{3} channel", Server.IRCColour, who, verb, (channel == opchannel ? " operator" : "")));
        }
        void Player_PlayerDisconnect(Player p, string reason)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PublicMessage(channel, p.name + " left the game (" + reason + ")");
        }

        void Player_PlayerConnect(Player p)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PublicMessage(channel, p.name + " joined the game");
        }

        void Listener_OnQuit(UserInfo user, string reason)
        {
            if (user.Nick == nick) return;
            Server.s.Log(user.Nick + " has left IRC");
            Player.GlobalMessage(Server.IRCColour + user.Nick + Server.DefaultColor + " has left IRC");
        }

        void Listener_OnError(ReplyCode code, string message)
        {
            Server.s.Log("IRC Error: " + message);
        }

        void Listener_OnPrivate(UserInfo user, string message)
        {
            if (!Server.ircControllers.Contains(user.Nick)) { Pm(user.Nick, "You are not an IRC controller!"); return; }
            if (message.Split(' ')[0] == "resetbot" || banCmd.Contains(message.Split(' ')[0])) { Pm(user.Nick, "You cannot use this command from IRC!"); return; }
            if (Player.CommandHasBadColourCodes(null, message)) { Pm(user.Nick, "Your command had invalid color codes!"); return; }

            Command cmd = Command.all.Find(message.Split(' ')[0]);
            if (cmd != null)
            {
                Server.s.Log("IRC Command: /" + message);
                usedCmd = user.Nick;
                try { cmd.Use(null, message.Split(' ').Length > 1 ? message.Substring(message.IndexOf(' ')).Trim() : ""); }
                catch { Pm(user.Nick, "Failed command!"); }
                usedCmd = "";
            }
            else
                Pm(user.Nick, "Unknown command!");
        }

        void Listener_OnPublic(UserInfo user, string channel, string message)
        {
            //string allowedchars = "1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./!@#$%^*()_+QWERTYUIOPASDFGHJKL:\"ZXCVBNM<>? ";
            // Allowed chars are any ASCII char between 20h/32 and 7Ah/122 inclusive, except for 26h/38 (&) and 60h/96 (`)

            StringBuilder sb = new StringBuilder();

            foreach (char b in Encoding.ASCII.GetBytes(message))
            {
                if (b != 38 && b != 96 && b >= 32 && b <= 122)
                    sb.Append(b);
                else
                    sb.Append("*");
            }

            String msg = sb.ToString().Trim();

            if (Player.MessageHasBadColorCodes(null, msg))
                return;

            Server.s.Log(String.Format("[{0}IRC] {1}: {2}", (channel == opchannel ? "(Op) " : ""), user.Nick, msg));
            Player.GlobalMessage(String.Format("{0}[{1}IRC] {2}: &f{3}", (channel == opchannel ? "(Op) " : ""), Server.IRCColour, user.Nick, Server.profanityFilter ? ProfanityFilter.Parse(msg) : msg));
        }

        void Listener_OnRegistered()
        {
            Server.s.Log("Connected to IRC!");
            reset = false;
            retries = 0;
            if (Server.ircIdentify && Server.ircPassword != "")
            {
                Server.s.Log("Identifying with NickServ");
                connection.Sender.PrivateMessage("nickserv", "IDENTIFY " + Server.ircPassword);
            }

            Server.s.Log("Joining channels...");

            if (!String.IsNullOrEmpty(channel))
                connection.Sender.Join(channel);
            if (!String.IsNullOrEmpty(opchannel))
                connection.Sender.Join(opchannel);
        }

        void Listener_OnDisconnected()
        {
            if (!reset && retries < 3) { retries++; Connect(); }
        }

        void Listener_OnNick(UserInfo user, string newNick)
        {
            //Player.GlobalMessage(Server.IRCColour + "[IRC] " + user.Nick + " changed nick to " + newNick);

            string key;
            if (newNick.Split('|').Length == 2)
            {
                key = newNick.Split('|')[1];
                if (key != null && key != "")
                {
                    switch (key)
                    {
                        case "AFK":
                            Player.GlobalMessage("[IRC] " + Server.IRCColour + user.Nick + Server.DefaultColor + " is AFK"); Server.ircafkset.Add(user.Nick); break;
                        case "Away":
                            Player.GlobalMessage("[IRC] " + Server.IRCColour + user.Nick + Server.DefaultColor + " is Away"); Server.ircafkset.Add(user.Nick); break;
                    }
                }
            }
            else if (Server.ircafkset.Contains(newNick))
            {
                Player.GlobalMessage("[IRC] " + Server.IRCColour + newNick + Server.DefaultColor + " is back");
                Server.ircafkset.Remove(newNick);
            }
            else
                Player.GlobalMessage("[IRC] " + Server.IRCColour + user.Nick + Server.DefaultColor + " is now known as " + newNick);
        }
        void Player_PlayerChat(Player p, string message)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PublicMessage(p.opchat ? opchannel : channel, p.name + ": " + message);
        }
        public void Connect()
        {
            if (!Server.irc) return;

            /*new Thread(new ThreadStart(delegate
            {
                try { connection.Connect(); }
                catch (Exception e)
                {
                    Server.s.Log("Failed to connect to IRC");
                    Server.ErrorLog(e);
                }
            })).Start();*/

            Server.s.Log("Connecting to IRC...");
            try { connection.Connect(); }
            catch (Exception e)
            {
                Server.s.Log("Failed to connect to IRC!");
                Server.ErrorLog(e);
            }
        }
        void Disconnect(string message = "Disconnecting")
        {
            if (Server.irc && IsConnected()) { connection.Disconnect(message); Server.s.Log("Disconnected from IRC!"); }
        }
        public bool IsConnected()
        {
            if (!Server.irc) return false;
            try { return connection.Connected; }
            catch { return false; }
        }
    }
}
