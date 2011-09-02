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
using System.IO;
using System.Collections.Generic;
using Sharkbite.Irc;
//using System.Threading;

namespace MCForge
{
    public class GlobalChatBot
    {
        private Connection connection;
        private string server, channel, nick;
        private bool reset = false;
        private byte retries = 0;
        public GlobalChatBot(string nick)
        {
            server = "irc.geekshed.net"; channel = "#MCForge"; this.nick = nick; 
            connection = new Connection(new ConnectionArgs(nick, server), false, false);
            if (Server.irc)
            {
                // Regster events for incoming
                connection.Listener.OnNick += new NickEventHandler(Listener_OnNick);
                //connection.Listener.OnNickError += new NickErrorEventHandler(Listener_OnNickError);
                connection.Listener.OnRegistered += new RegisteredEventHandler(Listener_OnRegistered);
                connection.Listener.OnPublic += new PublicMessageEventHandler(Listener_OnPublic);
                connection.Listener.OnError += new ErrorMessageEventHandler(Listener_OnError);
                connection.Listener.OnQuit += new QuitEventHandler(Listener_OnQuit);
                connection.Listener.OnJoin += new JoinEventHandler(Listener_OnJoin);
                connection.Listener.OnPart += new PartEventHandler(Listener_OnPart);
                //connection.Listener.OnKick += new KickEventHandler(Listener_OnKick);
                connection.Listener.OnDisconnected += new DisconnectedEventHandler(Listener_OnDisconnected);
            }
        }
        public void Say(string message, bool opchat = false)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PublicMessage(channel, message);
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
            Disconnect("Global Chat resetting...");
            Connect();
        }
        void Listener_OnJoin(UserInfo user, string channel)
        {
            if (user.Nick == nick)
                Server.s.Log("Joined Global Chat!");
        }
        void Listener_OnPart(UserInfo user, string channel, string reason)
        {
            if (user.Nick == nick) return;
            Server.s.Log(user.Nick + " has left channel " + channel);
            //Player.GlobalMessage(Server.IRCColour + "[IRC] " + user.Nick + " has left the" + (channel == opchannel ? " operator " : " ") + "channel");
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

        void Listener_OnPublic(UserInfo user, string channel, string message)
        {
            string allowedchars = "1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./!@#$%^*()_+QWERTYUIOPASDFGHJKL:\"ZXCVBNM<>? ";
            string msg = message;

            foreach (char ch in msg)
            {
                if (allowedchars.IndexOf(ch) == -1)
                    msg = msg.Replace(ch.ToString(), "*");
            }
            if (Player.MessageHasBadColorCodes(null, msg)) return;
            Server.s.Log("[Global] " + user.Nick + ": " + msg);
            //Player.GlobalMessage(Server.IRCColour + "[" + (channel == opchannel ? "(Op) " : "") + "IRC] " + user.Nick + ": &f" + msg.Trim());
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
            connection.Sender.Join(channel);
        }

        void Listener_OnDisconnected()
        {
            if (!reset && retries < 3) { retries++; Connect(); }
        }

        void Listener_OnNick(UserInfo user, string newNick)
        {
            Player.GlobalMessage("[Global] " + user.Nick + Server.DefaultColor + " is now known as " + newNick);
        }
        public void Connect()
        {
            if (!Server.irc) return;

            Server.s.Log("Connecting to Global Chat...");
            try { connection.Connect(); }
            catch (Exception e)
            {
                Server.s.Log("Failed to connect to Global Chat!");
                Server.ErrorLog(e);
            }
        }
        void Disconnect(string message = "Disconnecting")
        {
            if (Server.irc && IsConnected()) { connection.Disconnect(message); Server.s.Log("Disconnected from Global Chat!"); }
        }
        public bool IsConnected()
        {
            if (!Server.irc) return false;
            try { return connection.Connected; }
            catch { return false; }
        }
    }
}
