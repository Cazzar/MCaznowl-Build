using System;
using Sharkbite.Irc;
//using System.Threading;

namespace MCForge
{
    public class ForgeBot
    {
        private Connection connection;
        private string channel, opchannel;
        private string nick;
        private string server;
        private bool reset = false;
        public string usedCmd = "";
        public ForgeBot(string channel, string opchannel, string nick, string server)
        {
            this.channel = channel; this.opchannel = opchannel; this.nick = nick; this.server = server;
            connection = new Connection(new ConnectionArgs(nick, server), false, false);
            //Regstering events for outgoing
            Player.PlayerChat += new Player.OnPlayerChat(Player_PlayerChat);
            Player.PlayerConnect += new Player.OnPlayerConnect(Player_PlayerConnect);
            Player.PlayerDisconnect += new Player.OnPlayerDisconnect(Player_PlayerDisconnect);
            //Regstering events for incoming
            connection.Listener.OnNick += new NickEventHandler(Listener_OnNick);
            connection.Listener.OnRegistered += new RegisteredEventHandler(Listener_OnRegistered);
            connection.Listener.OnPublic += new PublicMessageEventHandler(Listener_OnPublic);
            connection.Listener.OnPrivate += new PrivateMessageEventHandler(Listener_OnPrivate);
            connection.Listener.OnError += new ErrorMessageEventHandler(Listener_OnError);
            connection.Listener.OnQuit += new QuitEventHandler(Listener_OnQuit);
            connection.Listener.OnJoin += new JoinEventHandler(Listener_OnJoin);
            connection.Listener.OnPart += new PartEventHandler(Listener_OnPart);
            connection.Listener.OnDisconnected += new DisconnectedEventHandler(Listener_OnDisconnected);
        }
        public void Say(string message, bool opchat = false)
        {
            if(Server.irc && IsConnected())
                connection.Sender.PublicMessage(opchat ? opchannel : channel, message);
        }
        public void Pm(string user, string message)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PrivateMessage(user, message);
        }
        public void Reset()
        {
            reset = true;
            Disconnect("Bot resetting...");
            Connect();
        }
        void  Listener_OnJoin(UserInfo user, string channel)
        {
            Server.s.Log(user.Nick + " has joined channel " + channel);
 	        Player.GlobalMessage(Server.IRCColour + "[IRC] " + user.Nick + " has left the" + (channel == opchannel ? " operator " : " ") + "channel");
        }
        void Listener_OnPart(UserInfo user, string channel, string reason)
        {
            Server.s.Log(user.Nick + " has left channel " + channel);
            Player.GlobalMessage(Server.IRCColour + "[IRC] " + user.Nick + " has left the" + (channel == opchannel ? " operator " : " ") + "channel");
        }

        void Player_PlayerDisconnect(Player p, string reason)
        {
            connection.Sender.PublicMessage(channel, p.name + " left the game (" + reason + ")");
        }

        void Player_PlayerConnect(Player p)
        {
            connection.Sender.PublicMessage(channel, p.name + " joined the game");
        }

        void Listener_OnQuit(UserInfo user, string reason)
        {
            Server.s.Log(user.Nick + " has left IRC");
            Player.GlobalMessage(Server.IRCColour + user.Nick + Server.DefaultColor + " has left IRC");
        }

        void Listener_OnError(ReplyCode code, string message)
        {
            Server.s.Log("IRC Error: " + message);
        }

        void Listener_OnPrivate(UserInfo user, string message)
        {
            Command cmd = Command.all.Find(message.Split(' ')[0]);
            if (cmd != null)
            {
                usedCmd = user.Nick;
                cmd.Use(null, message.Substring(message.IndexOf(' ')).Trim());
                usedCmd = "";
            }
            else
                Pm(user.Nick, "Unknown command!");
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
            Server.s.Log("[" + (channel == opchannel ? "(Op) " : "") + "IRC] " + user.Nick + ": " + msg);
            Player.GlobalMessage(Server.IRCColour + "[" + (channel == opchannel ? "(Op) " : "") + "IRC] " + user.Nick + ": &f" + msg);
        }

        void Listener_OnRegistered()
        {
            reset = false;
            if (Server.ircIdentify && Server.ircPassword != "")
            {
                Server.s.Log("Identifying with NickServ");
                connection.Sender.PrivateMessage("nickserv", "IDENTIFY " + Server.ircPassword);
            }
            connection.Sender.Join(channel);
            connection.Sender.Join(opchannel);
        }

        void Listener_OnDisconnected()
        {
            if(!reset) Connect();
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
                            Player.GlobalMessage("[IRC] " + Server.IRCColour + user.Nick + Server.DefaultColor + " is AFK"); Server.afkset.Add(user.Nick); break;
                        case "Away":
                            Player.GlobalMessage("[IRC] " + Server.IRCColour + user.Nick + Server.DefaultColor + " is Away"); Server.afkset.Add(user.Nick); break;
                    }
                }
            }
            else if (Server.afkset.Contains(newNick))
            {
                Player.GlobalMessage("[IRC] " + Server.IRCColour + newNick + Server.DefaultColor + " is back");
                Server.afkset.Remove(newNick);
            }
            else
                Player.GlobalMessage("[IRC] " + Server.IRCColour + user.Nick + Server.DefaultColor + " is now known as " + newNick);
        }
        bool Player_PlayerChat(Player p, string message)
        {
            if (Server.irc && IsConnected())
                connection.Sender.PublicMessage(p.opchat ? opchannel : channel, p.name + ": " + message);
            return true;
        }
        public void Connect()
        {
            /*new Thread(new ThreadStart(delegate
            {
                try { connection.Connect(); }
                catch (Exception e)
                {
                    Server.s.Log("Failed to connect to IRC");
                    Server.ErrorLog(e);
                }
            })).Start();*/

            try { connection.Connect(); }
            catch (Exception e)
            {
                Server.s.Log("Failed to connect to IRC");
                Server.ErrorLog(e);
            }
        }
        void Disconnect(string message = "Disconnecting")
        {
            if(IsConnected()) connection.Disconnect(message);
        }
        public bool IsConnected()
        {
            try { return connection.Connected; }
            catch { return false; }
        }
    }
}
