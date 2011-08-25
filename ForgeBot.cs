using System;
using Sharkbite.Irc;

namespace MCForge
{
    public class ForgeBot
    {
        private Connection connection;
        private string channel;
        private string nick;
        private string server;
        public ForgeBot(string channel, string nick, string server)
        {
            this.channel = channel; this.nick = nick; this.server = server;
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
        }

        void  Listener_OnJoin(UserInfo user, string channel)
        {
 	        Player.GlobalMessage(Server.IRCColour + "[IRC] " + user.Nick + " joined IRC");
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
            Player.GlobalMessage(Server.IRCColour + user.Nick + " left irc (" + reason + ")");
        }

        void Listener_OnError(ReplyCode code, string message)
        {
            //Error
        }

        void Listener_OnPrivate(UserInfo user, string message)
        {
            //Commands stuff
        }

        void Listener_OnPublic(UserInfo user, string channel, string message)
        {
            Player.GlobalMessage(Server.IRCColour + "[IRC] " + user.Nick + ": " + message);
        }

        void Listener_OnRegistered()
        {
            connection.Sender.Join(channel);
        }

        void Listener_OnNick(UserInfo user, string newNick)
        {
            Player.GlobalMessage(Server.IRCColour + "[IRC] " + user.Nick + " changed nick to " + newNick);
        }
        bool Player_PlayerChat(Player p, string message)
        {
            connection.Sender.PublicMessage(channel, p.name + ": " + message);
            return true;
        }
        void Connect()
        {
            connection = new Connection(new ConnectionArgs(Server.ircNick, Server.ircServer), false, false);
        }
    }
}
