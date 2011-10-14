﻿/*
    Copyright 2011 MCForge/ForgeCraft team
	
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MCForge;
using System.IO;

namespace MCForge.Remote
{
    public partial class Remote
    {
        public static Remote This;
        public string ip;



        //public static Remote remote;
        byte[] buffer = new byte[0];
        byte[] tempbuffer = new byte[0xFF];

        bool disconnected = false;
        public bool LoggedIn { get; protected set; }

        public Socket socket;
        public static List<Remote> remotes = new List<Remote>();
        public string version = "2.3";

        public Remote()
        {
            Remote.This = this;
        }
        public void Start()
        {

            if (RemoteServer.enableRemote)
            {
                try
                {
                    RemoteProperties.Load();
                    ip = socket.RemoteEndPoint.ToString().Split(':')[0];
                    Player.GlobalMessage(c.navy + "A Remote has connected to the server");
                    Server.s.Log("[Remote] connected to the server.");

                    Server.s.OnLog += new Server.LogHandler(s_OnLog);
                    Server.s.OnAdmin += new Server.LogHandler(s_OnAdmin);
                    Server.s.OnOp += new Server.LogHandler(s_OnOp);
                    Server.GlobalChat.OnNewGlobalMessage +=new GlobalChatBot.LogHandler(GlobalChat_OnNewGlobalMessage);
                    Server.s.OnSettingsUpdate += new Server.VoidHandler(s_OnSettingsUpdate);
                    Player.PlayerConnect += new Player.OnPlayerConnect(Player_PlayerConnect);
                    Player.PlayerDisconnect += new Player.OnPlayerDisconnect(Player_PlayerDisconnect);
                    Level.LevelLoad += new Level.OnLevelLoad(Level_LevelLoad);
                    Level.LevelUnload += new Level.OnLevelUnload(Remote_LevelUnload);
                    Group.OnGroupLoad += new Group.GroupLoad(GroupChanged);
                    Group.OnGroupSave += new Group.GroupSave(GroupChanged);



                    socket.BeginReceive(tempbuffer, 0, tempbuffer.Length, SocketFlags.None, new AsyncCallback(Receive), this);
                }
                catch (Exception e)
                {
                    Server.s.Log(e.Message);
                    Server.s.Log(e.StackTrace);
                }
            }
        }

        static void Receive(IAsyncResult result)
        {
            Remote p = (Remote)result.AsyncState;
            if (p.disconnected || p.socket == null)
                return;
            try
            {
                int length = p.socket.EndReceive(result);
                if (length == 0) { p.Disconnect(); return; }

                byte[] b = new byte[p.buffer.Length + length];
                Buffer.BlockCopy(p.buffer, 0, b, 0, p.buffer.Length);
                Buffer.BlockCopy(p.tempbuffer, 0, b, p.buffer.Length, length);

                p.buffer = p.HandleMessage(b);
                p.socket.BeginReceive(p.tempbuffer, 0, p.tempbuffer.Length, SocketFlags.None,
                                      new AsyncCallback(Receive), p);
            }
            catch (SocketException)
            {
                p.Disconnect();
            }

            catch (Exception e)
            {
                p.Disconnect();
                Server.s.Log(e.Message);
                Server.s.Log(e.StackTrace);
            }
        }

        #region HandleMessages
        byte[] HandleMessage(byte[] buffer)
        {
            try
            {
                int length = 0; byte msg = buffer[0];
                switch (msg)
                {
                    case 11: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
                    case 12: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
                    case 13: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
                    case 14: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
                    case 25: length = 1; break;
                    default:
                        Server.s.Log("unhandled message id " + msg);
                        Kick();
                        return new byte[0];
                }
                if (buffer.Length > length)
                {
                    byte[] message = new byte[length];
                    Buffer.BlockCopy(buffer, 1, message, 0, length);

                    byte[] tempbuffer = new byte[buffer.Length - length - 1];
                    Buffer.BlockCopy(buffer, length + 1, tempbuffer, 0, buffer.Length - length - 1);

                    buffer = tempbuffer;

                    switch (msg)
                    {
                        case 11: HandleMobileLogin(message); break;   //Login 
                        case 12: HandleMobileChat(message); break;
                        case 13: HandleMobileRequest(message); break;
                        case 14: HandleMobileSettingsChange(message); break;
                        case 25: HandleMobileDC(); break;

                    }
                    if (buffer.Length > 0)
                        buffer = HandleMessage(buffer);
                    else
                        return new byte[0];
                }
            }
            catch (Exception e)
            {
                Server.s.Log(e.Message);
                Server.s.Log(e.StackTrace);
            }
            return buffer;
        }

        private void HandleMobileSettingsChange(byte[] message)
        {

            const string KEY_SERVER_NAME = "servername:= ";
            const string KEY_SERVER_MOTD = "servermotd:= ";
            const string KEY_SERVER_PORT = "serverport:= ";
            const string KEY_SERVER_IS_PUBLIC = "serverpublic:= ";
            const string KEY_MAIN_NAME = "servermapname:= ";
            const string KEY_ADMINS_JOIN = "serveradminjoin:= ";

            const string KEY_IRC_USE = "ircuse:= ";
            const string KEY_IRC_SERVER = "ircserver:= ";
            const string KEY_IRC_CHANNEL = "ircchannel:= ";
            const string KEY_IRC_OPCHANNEL = "ircopchannel:= ";
            const string KEY_IRC_NICK = "ircnick:= ";
            const string KEY_IRC_COLOR = "irccolor:= ";
            const string KEY_IRC_IDENT = "ircident:= ";
            const string KEY_IRC_PASS = "ircpass:= ";
            const string KEY_IRC_PORT = "ircport:= ";

            const string KEY_MISC_PHYSICSRESTART = "miscphysicssp:= ";
            const string KEY_MISC_RPLIMIT = "miscrplimit:= ";
            const string KEY_MISC_NORMRPLIMIT = "miscnormalrplimit:= ";
            const string KEY_MISC_GLOBALCHAT = "miscglobalchat:= ";
            const string KEY_MISC_GLOBALCOLOR = "miscglobalcolor:= ";
            const string KEY_MISC_GLOBALNAME = "miscglobalnick:= ";
            const string KEY_MISC_DOLLAR = "miscdollar:= ";
            const string KEY_MISC_SUPEROPRANK = "miscsuperop:= ";
            const string KEY_MISC_PARSEEMOTE = "miscparseemote:= ";

            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string mass = Encoding.UTF8.GetString(message, 2, length);
            Server.s.Log(mass);
            try
            {
                if (mass.StartsWith(KEY_SERVER_NAME))
                {
                    mass = mass.Replace(KEY_SERVER_NAME, "");
                    Server.name = mass;
                    Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_ADMINS_JOIN))
                {
                    mass = mass.Replace(KEY_ADMINS_JOIN, "");
                    Server.adminsjoinsilent = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_SERVER_MOTD))
                {
                    mass = mass.Replace(KEY_SERVER_MOTD, "");
                    Server.motd = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_SERVER_PORT))
                {
                    mass = mass.Replace(KEY_SERVER_PORT, "");
                    Server.port = int.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_SERVER_IS_PUBLIC))
                {
                    mass = mass.Replace(KEY_SERVER_IS_PUBLIC, "");
                    Server.pub = Boolean.Parse(mass);
                    Properties.Save("properties/server.properties");

                    return;
                }
                if (mass.StartsWith(KEY_MAIN_NAME))
                {
                    mass = mass.Replace(KEY_MAIN_NAME, "");
                    Server.level = mass; Properties.Save("properties/server.properties");
                    return;
                }
                //---------------------------------IRC--------------------------------//
                if (mass.StartsWith(KEY_IRC_USE))
                {
                    mass = mass.Replace(KEY_IRC_USE, "");
                    Server.irc = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_SERVER))
                {
                    mass = mass.Replace(KEY_IRC_SERVER, "");
                    Server.ircServer = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_CHANNEL))
                {
                    mass = mass.Replace(KEY_IRC_CHANNEL, "");
                    Server.ircChannel = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_OPCHANNEL))
                {
                    mass = mass.Replace(KEY_IRC_OPCHANNEL, "");
                    Server.ircOpChannel = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_NICK))
                {
                    mass = mass.Replace(KEY_IRC_NICK, "");
                    Server.ircNick = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_PORT))
                {
                    mass = mass.Replace(KEY_IRC_PORT, "");
                    Server.ircPort = int.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_PASS))
                {
                    mass = mass.Replace(KEY_IRC_PASS, "");
                    Server.ircPassword = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_COLOR))
                {
                    mass = mass.Replace(KEY_IRC_COLOR, "");
                    Server.IRCColour = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_IDENT))
                {
                    mass = mass.Replace(KEY_IRC_IDENT, "");
                    Server.ircIdentify = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }

                //------------------MISC-----------------------------------------------//
                if (mass.StartsWith(KEY_MISC_PHYSICSRESTART))
                {
                    mass = mass.Replace(KEY_MISC_PHYSICSRESTART, "");
                    Server.physicsRestart = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_RPLIMIT))
                {
                    mass = mass.Replace(KEY_MISC_RPLIMIT, "");
                    Server.rpLimit = int.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_NORMRPLIMIT))
                {
                    mass = mass.Replace(KEY_MISC_NORMRPLIMIT, "");
                    Server.rpNormLimit = int.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_GLOBALCHAT))
                {
                    mass = mass.Replace(KEY_MISC_GLOBALCHAT, "");
                    Server.UseGlobalChat = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_GLOBALCOLOR))
                {
                    mass = mass.Replace(KEY_MISC_GLOBALCOLOR, "");
                    Server.GlobalChatColor = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_GLOBALNAME))
                {
                    mass = mass.Replace(KEY_MISC_GLOBALNAME, "");
                    Server.GlobalChatNick = mass; Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_DOLLAR))
                {
                    mass = mass.Replace(KEY_MISC_DOLLAR, "");
                    Server.dollardollardollar = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_SUPEROPRANK))
                {
                    mass = mass.Replace(KEY_MISC_SUPEROPRANK, "");
                    Server.rankSuper = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_PARSEEMOTE))
                {
                    mass = mass.Replace(KEY_MISC_PARSEEMOTE, "");
                    Server.parseSmiley = Boolean.Parse(mass); Properties.Save("properties/server.properties");
                    return;
                }
            }
            catch (FormatException)
            {
                Server.s.Log("Remote sent invalid setting");
            }

        }
        private void HandleMobileRequest(byte[] message)
        {
            //throw new NotImplementedException();
        }
        private void HandleMobileChat(byte[] message)
        {
            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string m = Encoding.UTF8.GetString(message, 2, length);
            foreach (char ch in m)
            {
                if (ch < 32 || ch >= 127)
                {
                    Kick();
                    return;
                }
            }
            RemoteChat(m);

        }
        private void HandleMobileDC()
        {
            Disconnect();
        }
        private void HandleMobileLogin(byte[] message)
        {

            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string msg = Encoding.UTF8.GetString(message, 2, length);
            //Server.s.Log(msg);
            if (msg.StartsWith(version))  //TODO: make a better checker
            {
                msg = msg.Replace(version + ": ", "");

            }
            else
            {


                SendData("VERSION");
                Server.s.Log("[Remote] A remote tried to connect with a different version.");
            }


            if (HandleLogin(msg))
            {


                SendData("ACCEPTED");
                Server.s.Log("[Remote] Remote Verified, passing controls to it!");
                startUp();
                LoggedIn = true;
                remotes.Add(this);
            }
            else
            {

                SendData("FAIL");
                Server.s.Log("[Remote] A Remote with incorrect information attempted to join.");
            }
        }
        #endregion

        public void Kick()
        {
            if (disconnected) return;
            disconnected = true;
            if (LoggedIn)
            {
                Player.GlobalMessage("%5[Remote] %fhas been kicked from the server!");
                LoggedIn = false;
            }

            try
            {
                SendData(0x03);
                Server.s.Log("[Remote] has been kicked from the server!");
            }
            catch { }
            this.Dispose();
        }
        public void Disconnect()
        {
            if (disconnected) return;
            disconnected = true;
            if (LoggedIn) Player.GlobalMessage("%5[Remote] %fhas disconnected."); Server.s.Log("[Remote] has disconnected");
            LoggedIn = false;
            this.Dispose();
        }
        public void Dispose()
        {
            if (socket != null && socket.Connected)
            {
                try { socket.Close(); }
                catch { }
                socket = null;
            }
            remotes.Remove(this);
        }

        #region Packet senders/loggers
        public void SendData(int id) { SendData(id, new byte[0]); }
        public void SendData(int id, string p)
        {
            byte[] bytes = new byte[(p.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)p.Length).CopyTo(bytes, 0);
            Encoding.BigEndianUnicode.GetBytes(p).CopyTo(bytes, 2);
            SendData(id, bytes);
            System.Threading.Thread.Sleep(100);
        }
        public void SendData(string p)
        {
            byte[] bytes = new byte[(p.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)p.Length).CopyTo(bytes, 0);
            Encoding.BigEndianUnicode.GetBytes(p).CopyTo(bytes, 2);
            // Server.s.Log(p);
            SendData(bytes);
        }
        public void SendData(int id, byte[] send)
        {
            if (socket == null || !socket.Connected) return;
            byte[] buffer = new byte[send.Length + 1];
            buffer[0] = (byte)id;
            for (int i = 0; i < send.Length; i++)
            {
                buffer[i + 1] = send[i];
            }
            try
            {

                socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, delegate(IAsyncResult result) { }, null);
                buffer = null;
            }
            catch (SocketException e)
            {
                buffer = null;
                Disconnect();
            }
        }
        public void SendData(byte[] send)
        {
            if (socket == null || !socket.Connected) return;

            try
            {
                socket.Send(send);
                //Server.s.Log("SENT MESSAGES");
                send = null;
            }
            catch (SocketException)
            {
                send = null;
                Disconnect();
            }

        }
        void LogPacket(byte id, byte[] packet)
        {
            string s = "";

            if (packet.Length >= 1)
            {
                foreach (byte b in packet)
                {
                    s += b + ", ";
                }
                Server.s.Log("Packet " + id + " { " + s + "}");
            }
            else
            {
                Server.s.Log("Packet " + id + " had no DATA!");
            }
        }
        #endregion

        internal void startUp()
        {
            //sendGroups();
            sendPlayers();
            sendMaps();
            sendSettings();
        }
        internal void sendPlayers()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Player p in Player.players)
            {
                //builder.Append(p.name).Append(",").Append(p.level.name).Append(",").Append(p.group.name);  later
                addPlayer(p);
                System.Threading.Thread.Sleep(100);
            }
        }
        internal void sendSettings()
        {
            const string RECIEVED_SERVER_NAME = "SRVR_NAME: ";
            const string RECIEVED_SERVER_MOTD = "SRVR_MOTD: ";
            const string RECIEVED_SERVER_PORT = "SRVR_PORT: ";
            const string RECIEVED_SERVER_IS_PUBLIC = "SRVR_PUBLIC: ";
            const string RECIEVED_MAIN_NAME = "SRVR_MAIN_NAME: ";
            const string RECIEVED_ADMINS_JOIN = "SRVR_ADMINS_JOIN: ";

            const string RECIEVED_IRC_USE = "IRC_USE: ";
            const string RECIEVED_IRC_SERVER = "IRC_SERVER: ";
            const string RECIEVED_IRC_CHANNEL = "IRC_CHANNEL: ";
            const string RECIEVED_IRC_OPCHANNEL = "IRC_OPCHANNEL: ";
            const string RECIEVED_IRC_NICK = "IRC_NICK: ";
            const string RECIEVED_IRC_COLOR = "IRC_COLOR: ";
            const string RECIEVED_IRC_IDENT = "IRC_IDENT: ";
            const string RECIEVED_IRC_PASS = "IRC_PASS: ";
            const string RECIEVED_IRC_PORT = "IRC_PORT: ";

            const string RECIEVED_MISC_PHYSICSRESTART = "MISC_PHYSICSRESTART: ";
            const string RECIEVED_MISC_RPLIMIT = "MISC_RPLIMIT: ";
            const string RECIEVED_MISC_NORMRPLIMIT = "MISC_NORMRPLIMIT: ";
            const string RECIEVED_MISC_GLOBALCHAT = "MISC_GLOBALCHAT: ";
            const string RECIEVED_MISC_GLOBALCOLOR = "MISC_GLOBALCOLOR: ";
            const string RECIEVED_MISC_GLOBALNAME = "MISC_GLOBALNAME: ";
            const string RECIEVED_MISC_DOLLAR = "MISC_DOLLAR: ";
            const string RECIEVED_MISC_SUPEROPRANK = "MISC_SUPEROPRANK: ";
            const string RECIEVED_MISC_PARSEEMOTE = "MISC_PARSEEMOTE: ";

            SendData(0x08, RECIEVED_ADMINS_JOIN + Server.adminsjoinsilent.ToString().ToLower());
            SendData(0x08, RECIEVED_MAIN_NAME + Server.mainLevel.name.ToString());
            SendData(0x08, RECIEVED_SERVER_IS_PUBLIC + Server.pub.ToString().ToLower());
            SendData(0x08, RECIEVED_SERVER_PORT + Server.port.ToString().ToLower());
            SendData(0x08, RECIEVED_SERVER_MOTD + Server.motd.ToString());
            SendData(0x08, RECIEVED_SERVER_NAME + Server.name.ToString());

            SendData(0x08, RECIEVED_IRC_USE + Server.irc.ToString().ToLower());
            SendData(0x08, RECIEVED_IRC_SERVER + Server.ircServer);
            SendData(0x08, RECIEVED_IRC_CHANNEL + Server.ircChannel);
            SendData(0x08, RECIEVED_IRC_OPCHANNEL + Server.ircOpChannel);
            SendData(0x08, RECIEVED_IRC_NICK + Server.ircNick);
            SendData(0x08, RECIEVED_IRC_COLOR + Server.IRCColour);
            SendData(0x08, RECIEVED_IRC_IDENT + Server.ircIdentify.ToString().ToLower());
            SendData(0x08, RECIEVED_IRC_PASS + Server.ircPassword);
            SendData(0x08, RECIEVED_IRC_PORT + Server.ircPort);

            SendData(0x08, RECIEVED_MISC_PHYSICSRESTART + Server.physicsRestart.ToString().ToLower());
            SendData(0x08, RECIEVED_MISC_RPLIMIT + Server.rpLimit.ToString());
            SendData(0x08, RECIEVED_MISC_NORMRPLIMIT + Server.rpNormLimit.ToString());
            SendData(0x08, RECIEVED_MISC_GLOBALCHAT + Server.UseGlobalChat.ToString().ToLower());
            SendData(0x08, RECIEVED_MISC_GLOBALCOLOR + Server.GlobalChatColor);
            SendData(0x08, RECIEVED_MISC_GLOBALNAME + Server.GlobalChatNick);
            SendData(0x08, RECIEVED_MISC_DOLLAR + Server.dollardollardollar.ToString().ToLower());
            SendData(0x08, RECIEVED_MISC_SUPEROPRANK + Server.rankSuper.ToString().ToLower());
            SendData(0x08, RECIEVED_MISC_PARSEEMOTE + Server.parseSmiley.ToString().ToLower());
            SendData(0x08, "Done_!*");

            return;
        }
        internal void addPlayer(Player p)
        {
            if (p.title == null || p.title == "" || p.title == String.Empty)
            {
                SendData(0x04, new StringBuilder("ADD:").Append("Default").Append(",").Append(p.name)
                    .Append(",").Append(p.group.name).Append(",").Append(p.color).ToString());
                return;
            }
            else
            {
                SendData(0x04, new StringBuilder("ADD:").Append(p.title).Append(",").Append(p.name)
                    .Append(",").Append(p.group.name).Append(",").Append(p.color).ToString());
            }
        }
        internal void removePlayer(Player p)
        {
            SendData(0x04, "DELETE:" + p.name);
        }
        List<string> levels = new List<string>(Server.levels.Count);
        internal void sendMaps()
        {


            levels.Clear();
            try
            {
                DirectoryInfo di = new DirectoryInfo("levels/");
                FileInfo[] fi = di.GetFiles("*.lvl");
                foreach (Level l in Server.levels) { levels.Add(l.name.ToLower()); }

                foreach (FileInfo file in fi)
                {
                    if (!levels.Contains(file.Name.Replace(".lvl", "").ToLower()))
                    {
                        SendData(0x06, "UN_" + file.Name.Replace(".lvl", ""));
                    }
                }

                Server.levels.ForEach(delegate(Level l) { SendData(0x06, "LO_" + l.name); });

            }

            catch (Exception e) { Server.ErrorLog(e); }

        }
        internal void sendGroups()
        {
            foreach (Group g in Group.GroupList)
            {
                //SendData(0x07, g.name + "," +g.color + "," g.
            }
        }
        void s_OnOp(string message)
        {
            Player p = null;
            int id = message.IndexOf('>');
            if (id > 0)
            {
                string getname = null;
                if (id % 2 == 0)
                {
                    getname = message.Substring(12, (id / 2) - 1);
                }
                else if (id % 2 == 1)
                {
                    getname = message.Substring(12, (id / 2) + 1);
                }
                p = Player.Find(getname);
            }

            if (p == null)
            {
                string messaged = new StringBuilder().Append("Console").Append("ĥ").Append(message).ToString();
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)2;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message).ToString();
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)2;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
         
        }
        void s_OnAdmin(string message)
        {
            Player p = null;
            int id = message.IndexOf('>');
            if (id > 0)
            {
                string getname = null;
                if (id % 2 == 0)
                {
                    getname = message.Substring(12, (id / 2) - 1);
                }
                else if (id % 2 == 1)
                {
                    getname = message.Substring(12, (id / 2) + 1);
                }
                p = Player.Find(getname);
            }

            if (p == null)
            {
                string messaged = new StringBuilder().Append("Console").Append("ĥ").Append(message).ToString();
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)4;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message).ToString();
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)4;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
           
        }
        void GlobalChat_OnNewGlobalMessage(string message)
        {
            if (Server.UseGlobalChat)
            {
                message = message.Remove(0, 8);
                string messaged = new StringBuilder().Append("Global").Append("ĥ").Append(message).ToString();
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)3;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
        }
        void s_OnLog(string message)
        {

            Player p = null;
            int id = message.IndexOf('>');
            if (id > 0)
            {
                string getname = null;
                if (id % 2 == 0)
                {
                    getname = message.Substring(12, (id / 2) - 1);
                }
                else if (id % 2 == 1)
                {
                    getname = message.Substring(12, (id / 2) + 1);
                }
                p = Player.Find(getname);
            }

            if (p == null)
            {
                string messaged = new StringBuilder().Append("Console").Append("ĥ").Append(message).ToString();
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)1;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message).ToString();
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)1;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }

        }
        void Player_PlayerConnect(Player p)
        {
            addPlayer(p);
        }
        void Player_PlayerDisconnect(Player p, string message)
        {
            removePlayer(p);
        }
        void Level_LevelLoad(string l)
        {
            //Server.s.Log("WAS " + l + " LOADED?");
            SendData(0x06, "LO_" + l);
        }
        void Remote_LevelUnload(Level l)
        {
            SendData(0x06, "UN_" + l.name);
            //Server.s.Log("WAS " + l.name + " LOADED?");
        }
        void s_OnSettingsUpdate()
        {
            sendSettings();
        }
        void GroupChanged()
        {
            sendGroups();
        }

    }
}
