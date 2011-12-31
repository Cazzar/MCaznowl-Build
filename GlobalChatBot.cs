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
//using System.Timers;
using System.Text;
using Sharkbite.Irc;
using System.Globalization;
//using System.Threading;

namespace MCForge
{
    public class GlobalChatBot
    {
        public delegate void LogHandler(string nick, string message);
        public event LogHandler OnNewGlobalMessage;

        public delegate void KickHandler(string reason);
        public event KickHandler OnGlobalKicked;

        private Connection connection;
        private string server, channel, nick;
        private bool reset/* = false*/;
        private byte retries/* = 0*/;
        public GlobalChatBot(string nick)
        {
            server = "irc.geekshed.net"; channel = "#MCForge"; this.nick = nick.Replace(" ", "");
            connection = new Connection(new ConnectionArgs(nick, server), false, false);
            if (Server.UseGlobalChat)
            {
                // Regster events for incoming
                connection.Listener.OnNickError += new NickErrorEventHandler(Listener_OnNickError);
                connection.Listener.OnRegistered += new RegisteredEventHandler(Listener_OnRegistered);
                connection.Listener.OnPublic += new PublicMessageEventHandler(Listener_OnPublic);
                connection.Listener.OnJoin += new JoinEventHandler(Listener_OnJoin);
                connection.Listener.OnKick += new KickEventHandler(Listener_OnKick);
                connection.Listener.OnError += new ErrorMessageEventHandler(Listener_OnError);
                connection.Listener.OnDisconnected += new DisconnectedEventHandler(Listener_OnDisconnected);
            }
        }
        public void Say(string message)
        {
            if (Server.UseGlobalChat && IsConnected())
                connection.Sender.PublicMessage(channel, message);
        }
        public void Pm(string user, string message)
        {
            if (Server.UseGlobalChat && IsConnected())
                connection.Sender.PrivateMessage(user, message);
        }
        public void Reset()
        {
            if (!Server.UseGlobalChat) return;
            reset = true;
            retries = 0;
            Disconnect("Global Chat bot resetting...");
            Connect();
        }

        void Listener_OnJoin(UserInfo user, string channel)
        {
            if (user.Nick == nick)
                Server.s.Log("Joined the Global Chat!");
        }

        void Listener_OnError(ReplyCode code, string message)
        {
            //Server.s.Log("IRC Error: " + message);
        }

        void Listener_OnPublic(UserInfo user, string channel, string message)
        {
            //string allowedchars = "1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./!@#$%^*()_+QWERTYUIOPASDFGHJKL:\"ZXCVBNM<>? ";
            //string msg = message;

            message = message.MCCharFilter();
            if (Player.MessageHasBadColorCodes(null, message))
                return;
            if (OnNewGlobalMessage != null)
            {
                OnNewGlobalMessage(user.Nick, message);
            }
            if (Server.devs.Contains(message.Split(':')[0]) && message.StartsWith("[Dev]", StringComparison.CurrentCulture) == false && message.StartsWith("[Developer]", StringComparison.CurrentCulture) == false) { message = "[Dev]" + message; }
            try { Gui.Window.thisWindow.LogGlobalChat("> " + user.Nick + ": " + message); }
            catch { Server.s.Log(">[Global] " + user.Nick + ": " + message); }
            Player.GlobalMessage(String.Format(CultureInfo.CurrentCulture, "{0}>[Global] {1}: &f{2}", Server.GlobalChatColor, user.Nick, Server.profanityFilter ? ProfanityFilter.Parse(message) : message), true);
        }

        void Listener_OnRegistered()
        {
            reset = false;
            retries = 0;
            connection.Sender.Join(channel);
        }

        void Listener_OnDisconnected()
        {
            if (!reset && retries < 5) { retries++; Connect(); }
        }

        void Listener_OnNickError(string badNick, string reason)
        {
            Server.s.Log("Global Chat nick \"" + badNick + "\" is  taken, please choose a different nick.");
        }

        void Listener_OnKick(UserInfo user, string channel, string kickee, string reason)
        {
            if (kickee.Trim().ToLower(CultureInfo.CurrentCulture) == nick.ToLower(CultureInfo.CurrentCulture))
            {
                Server.s.Log("Kicked from Global Chat: " + reason);
                if (OnGlobalKicked != null) OnGlobalKicked(reason);
                Server.s.Log("Attempting to rejoin...");
                connection.Sender.Join(channel);
            }

        }

        public void Connect()
        {
            if (!Server.UseGlobalChat || Server.shuttingDown) return;
            try { connection.Connect(); }
            catch { }
        }

        public void Disconnect(string reason)
        {
            if (Server.UseGlobalChat && IsConnected()) { connection.Disconnect(reason); Server.s.Log("Disconnected from Global Chat!"); }
        }

        public bool IsConnected()
        {
            if (!Server.UseGlobalChat) return false;
            try { return connection.Connected; }
            catch { return false; }
        }
    }
}
