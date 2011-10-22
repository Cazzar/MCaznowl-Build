/*
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
using System.Security.Cryptography;

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
        private string key;
        public Remote()
        {
            Remote.This = this;
            key = generateRandChars();
        }

        private string generateRandChars()
        {
            Random r = new Random();
            byte[] rs = new byte[31]; //{1,2,3,4,5,6,7,8,9,0, (byte)'a',(byte)'b',(byte)'c',(byte)'d',(byte)'e'};
            r.NextBytes(rs);
            return Encoding.UTF8.GetString(rs);

        }
        public void Start()
        {

            if (RemoteServer.enableRemote)
            {
                if (RemoteServer.tries < 7)
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
                        Server.ErrorLog(e);
                    }
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
                Server.ErrorLog(e);
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
                    case 13: length = 1; break;
                    case 14: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
                    case 15: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
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
                        case 15: HandleMobileCommand(message); break;
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
                Server.ErrorLog(e);
            }
            return buffer;
        }

        private void HandleMobileCommand(byte[] message)
        {
            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string mass = Encoding.UTF8.GetString(message, 2, length);
            mass = Decrypt(mass, key);
            string[] splitted = mass.Split('_');
            string ident = splitted[0];
            mass = mass.Replace(ident, "");
            mass = mass.Remove(0, 1);

            foreach (char c in mass)
            {
                if (c == '_')
                {
                    mass = mass.Replace(c, ' ');
                }

            }
            Command.all.Find(ident).Use(null, mass);
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
            mass = Decrypt(mass, key);
            try
            {
                if (mass.StartsWith(KEY_SERVER_NAME))
                {
                    mass = mass.Replace(KEY_SERVER_NAME, "");
                    Server.name = mass;
                    SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_ADMINS_JOIN))
                {
                    mass = mass.Replace(KEY_ADMINS_JOIN, "");
                    Server.adminsjoinsilent = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_SERVER_MOTD))
                {
                    mass = mass.Replace(KEY_SERVER_MOTD, "");
                    Server.motd = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_SERVER_PORT))
                {
                    mass = mass.Replace(KEY_SERVER_PORT, "");
                    Server.port = int.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_SERVER_IS_PUBLIC))
                {
                    mass = mass.Replace(KEY_SERVER_IS_PUBLIC, "");
                    Server.pub = Boolean.Parse(mass);
                    SrvProperties.Save("properties/server.properties");

                    return;
                }
                if (mass.StartsWith(KEY_MAIN_NAME))
                {
                    mass = mass.Replace(KEY_MAIN_NAME, "");
                    if (Player.ValidName(mass)) Server.level = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                //---------------------------------IRC--------------------------------//
                if (mass.StartsWith(KEY_IRC_USE))
                {
                    mass = mass.Replace(KEY_IRC_USE, "");
                    Server.irc = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_SERVER))
                {
                    mass = mass.Replace(KEY_IRC_SERVER, "");
                    Server.ircServer = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_CHANNEL))
                {
                    mass = mass.Replace(KEY_IRC_CHANNEL, "");
                    Server.ircChannel = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_OPCHANNEL))
                {
                    mass = mass.Replace(KEY_IRC_OPCHANNEL, "");
                    Server.ircOpChannel = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_NICK))
                {
                    mass = mass.Replace(KEY_IRC_NICK, "");
                    Server.ircNick = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_PORT))
                {
                    mass = mass.Replace(KEY_IRC_PORT, "");
                    Server.ircPort = int.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_PASS))
                {
                    mass = mass.Replace(KEY_IRC_PASS, "");
                    Server.ircPassword = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_COLOR))
                {
                    mass = mass.Replace(KEY_IRC_COLOR, "");
                    Server.IRCColour = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_IRC_IDENT))
                {
                    mass = mass.Replace(KEY_IRC_IDENT, "");
                    Server.ircIdentify = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }

                //------------------MISC-----------------------------------------------//
                if (mass.StartsWith(KEY_MISC_PHYSICSRESTART))
                {
                    mass = mass.Replace(KEY_MISC_PHYSICSRESTART, "");
                    Server.physicsRestart = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_RPLIMIT))
                {
                    mass = mass.Replace(KEY_MISC_RPLIMIT, "");
                    Server.rpLimit = int.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_NORMRPLIMIT))
                {
                    mass = mass.Replace(KEY_MISC_NORMRPLIMIT, "");
                    Server.rpNormLimit = int.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_GLOBALCHAT))
                {
                    mass = mass.Replace(KEY_MISC_GLOBALCHAT, "");
                    Server.UseGlobalChat = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_GLOBALCOLOR))
                {
                    mass = mass.Replace(KEY_MISC_GLOBALCOLOR, "");
                    Server.GlobalChatColor = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_GLOBALNAME))
                {
                    mass = mass.Replace(KEY_MISC_GLOBALNAME, "");
                    Server.GlobalChatNick = mass; SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_DOLLAR))
                {
                    mass = mass.Replace(KEY_MISC_DOLLAR, "");
                    Server.dollardollardollar = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_SUPEROPRANK))
                {
                    mass = mass.Replace(KEY_MISC_SUPEROPRANK, "");
                    Server.rankSuper = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
                    return;
                }
                if (mass.StartsWith(KEY_MISC_PARSEEMOTE))
                {
                    mass = mass.Replace(KEY_MISC_PARSEEMOTE, "");
                    Server.parseSmiley = Boolean.Parse(mass); SrvProperties.Save("properties/server.properties");
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
            //All = 1
            //Players = 2
            //Settings = 3
            //Maps = 4
            //Groups = 5
            switch (message[0])
            {
                case 1:
                    startUp();
                    break;
                case 2:
                    sendPlayers();
                    break;
                case 3:
                    sendSettings();
                    break;
                case 4:
                    sendMaps();
                    break;
                case 5:
                    sendGroups();
                    break;
                default: return;
            }
        }


        private void HandleMobileChat(byte[] message)
        {
            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string m = Encoding.UTF8.GetString(message, 2, length);
            m = this.Decrypt(m, key);
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
            msg = this.Decrypt(msg, "FORGEREMOTETIVITY");
            byte[] bs = new byte[1];
            //Server.s.Log(msg);
            if (msg.StartsWith(version))  //TODO: make a better checker
            {
                msg = msg.Replace(version + ": ", "");
            }
            else
            {

                bs[0] = 3;
                SendData(11, bs);
                Server.s.Log("[Remote] A remote tried to connect with a different version.");
            }
            if (RemoteServer.tries >= 3)
            {
                bs[0] = 4;
                SendData(11, bs);
                Server.s.Log("[Remote] A remote tried to connect with exceeding incorrect credentials");
            }
            if (RemoteServer.tries == 6)
            {
                bs[0] = 5;
                SendData(11, bs);
                Server.s.Log("[Remote] Remote was locked from the console, type \"/remote tryreset\" to reset the try count");
            }

            if (HandleLogin(msg))
            {

                bs[0] = 1;
                SendData(11, bs);
                sendHash(key);
                Server.s.Log("[Remote] Remote Verified, passing controls to it!");
                startUp(); // -- dont need because phone will request stuff when ready
                LoggedIn = true;
                remotes.Add(this);
            }
            else
            {
                bs[0] = 2;
                SendData(11, bs);
                Server.s.Log("[Remote] A Remote with incorrect information attempted to join.");
                RemoteServer.tries++;
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
            p = Encrypt(p, key);
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
            catch (SocketException)
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

                socket.BeginSend(send, 0, send.Length, SocketFlags.None, delegate(IAsyncResult result) { }, null);
                buffer = null;
            }
            catch (SocketException)
            {
                buffer = null;
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
            System.Object lockThis = new System.Object();
            lock (lockThis)
            {
                sendSteps(0);
                sendMaps();
                sendSteps(1);
                sendSettings();
                sendSteps(2);
                sendGroups();
                sendSteps(3);
                sendPlayers();
                sendSteps(4);  //cancels it here
            }

        }

        private void sendHash(string key)
        {
            byte[] biscut = new byte[(key.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)key.Length).CopyTo(biscut, 0);
            Encoding.BigEndianUnicode.GetBytes(key).CopyTo(biscut, 2);
            SendData(0x02, biscut);
        }

        private void sendSteps(short b)
        {
            byte[] sdd = new byte[2];
            util.BigEndianBitConverter.Big.GetBytes(b).CopyTo(sdd, 0);
            SendData(0x09, sdd);
        }
        internal void sendPlayers()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Player p in Player.players)
            {
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
            SendData(0x04, new StringBuilder("ADD:").Append(p.title).Append(",").Append(p.name)
                .Append(",").Append(p.group.name).Append(",").Append(p.color).ToString());

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
                SendData(0x07, g.color + "," + g.name);
            }
        }
        void s_OnOp(string message)
        {
            Player p = null;

            message = message.Remove(0, 11);
            message = message.Replace("(OPs): ", "");
            string[] secondSplit = message.Split(':');
            string getname = message.Substring(0, secondSplit[0].Length);
            p = Player.Find(getname);
            if (p == null)
            {
                string messaged = new StringBuilder().Append("Console").Append("ĥ").Append(message).ToString();
                messaged = Encrypt(messaged, key);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)2;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message.Replace(p.name + ":", "")).ToString();
                messaged = Encrypt(messaged, key);
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

            message = message.Remove(0, 11);
            message = message.Replace("(Admins): ", "");

            string[] secondSplit = message.Split(':');
            string getname = message.Substring(0, secondSplit[0].Length);
            p = Player.Find(getname);
            if (p == null)
            {
                string messaged = new StringBuilder().Append("Console").Append("ĥ").Append(message).ToString();
                messaged = Encrypt(messaged, key);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)4;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message.Replace(p.name + ":", "")).ToString();
                messaged = Encrypt(messaged, key);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)4;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);

                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }

        }
        void s_OnLog(string message)
        {

            Player p = null;


            //(xx:yy:zz){2}headdetect{1}:<nsdfs>
            if (message.IndexOf(">") > -1)
            {
                message = message.Remove(0, 11);
                string[] secondSplit = message.Split('>');
                string getname = message.Substring(1, secondSplit[0].Length - 1);
                p = Player.Find(getname);
            }
            if (p == null)
            {
                string messaged = new StringBuilder().Append("Console").Append("ĥ").Append(message).ToString();
                messaged = Encrypt(messaged, key);
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
                messaged = Encrypt(messaged, key);
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


        string Decrypt(string textToDecrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

        string Encrypt(string textToEncrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }


    }
}
