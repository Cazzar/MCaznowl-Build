using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace MCForge.Remote
{
   public partial class Remote
    {
        private void HandleMobileChangeGroup(byte[] message)
        {
            short length = util.BigEndianBitConverter.Big.ToInt16(message, 0);
            byte id = message[2];
            string messages = Encoding.UTF8.GetString(message, 3, length);
            messages = this.DecryptMobile(messages, _keyMobile);
            switch (id)
            {
                case 1:
                    string newGroupname = messages.Split('*')[1];
                    string oldName = messages.Split('*')[0];
                    Group g = Group.Find(oldName);
                    if (g != null)
                    {
                        g.name = newGroupname;
                        g.fileName = newGroupname + ".txt";  //Dont ask
                        g.trueName = newGroupname;
                        Group.saveGroups(Group.GroupList);
                    }
                    break;
                case 2:
                    string color = messages.Split('*')[1];
                    string name = messages.Split('*')[0];
                    Group ge = Group.Find(name);
                    if (ge != null)
                    {
                        ge.color = color;
                        Group.saveGroups(Group.GroupList);
                    }
                    break;
                case 3:
                    int limit = int.Parse(messages.Split('*')[1]);
                    string namee = messages.Split('*')[0];
                    Group gew = Group.Find(namee);
                    if (gew != null)
                    {
                        gew.maxBlocks = limit;
                        Group.saveGroups(Group.GroupList);
                    }
                    break;
                case 4:
                    int perms = int.Parse(messages.Split('*')[1]);
                    string name2 = messages.Split('*')[0];
                    Group gew2 = Group.Find(name2);
                    if (gew2 != null)
                    {
                        gew2.Permission = (LevelPermission)perms;
                        Group.saveGroups(Group.GroupList);
                    }
                    break;
                case 5:
                    string bondPAUSEJamesBond = messages.Split('*')[0];
                    Group grewp = Group.Find(bondPAUSEJamesBond);
                    if (grewp != null) Group.GroupList.Remove(grewp); Group.saveGroups(Group.GroupList);
                    break;
                case 6:
                    string[] stringers = messages.Split('*');
                    if (stringers.Length == 4)
                    {
                        Group newGroup = new Group(((LevelPermission)int.Parse(stringers[3])), int.Parse(stringers[2]), 0,
                            stringers[0], stringers[1][1], String.Empty, "NEWRANK" + int.Parse(stringers[3]).ToString() + ".txt");

                        Group.GroupList.Add(newGroup);
                        Group.saveGroups(Group.GroupList);
                    }
                    break;
                default: return;
            }
            Group.InitAll();

        }
        private void HandleMobileCommand(byte[] message)
        {
            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string m = Encoding.UTF8.GetString(message, 2, length);
            m = DecryptMobile(m, _keyMobile);
            if (m.Any(ch => ch < 32 || ch >= 127))
            {
                Kick();
                return;
            }
            RemoteChat(m);
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
            mass = DecryptMobile(mass, _keyMobile);
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
                    StartUpMobile();
                    break;
                case 2:
                    SendPlayersMobile();
                    break;
                case 3:
                    SendSettingsMobile();
                    break;
                case 4:
                    SendMapsMobile();
                    break;
                case 5:
                    SendGroupsMobile();
                    break;
                default: return;
            }
        }
        private void HandleMobileChat(byte[] message)
        {
            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string m = Encoding.UTF8.GetString(message, 2, length);
            m = DecryptMobile(m, _keyMobile);
            if (m.Any(ch => ch < 32 || ch >= 127))
            {
                Kick();
                return;
            }
            RemoteChat(m);

        }
        private void HandleMobileLogin(byte[] message)
        {

            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string msg = Encoding.UTF8.GetString(message, 2, length);
            msg = DecryptMobile(msg, "FORGEREMOTETIVITY");
            byte[] bs = new byte[1];
            //Server.s.Log(msg);
            if (msg.StartsWith(Protocal.ToString())) //TODO: make a better checker
                msg = msg.Replace(string.Format("{0}: ", Protocal), "");
            else
            {
                bs[0] = 0x3;
                SendData(0xb, bs);
                Server.s.Log("[Remote] A remote tried to connect with a different version.");
            }
            if (RemoteServer.tries >= 0x3)
            {
                bs[0] = 0x4;
                SendData(0xb, bs);
                Server.s.Log("[Remote] A remote tried to connect with exceeding incorrect credentials");
            }
            if (RemoteServer.tries == 0x6)
            {
                bs[0] = 0x5;
                SendData(0xb, bs);
                Server.s.Log("[Remote] Remote was locked from the console, type \"/remote tryreset\" to reset the try count");
            }

            if (HandleLogin(msg))
            {

                bs[0] = 1;
                if (OnRemoteLogin != null) OnRemoteLogin(this);
                SendData(11, bs);
                GenerateKeyMobile(_keyMobile);
                Server.s.Log("[Remote] Remote Verified, passing controls to it!");
                LoggedIn = true;
                Remotes.Add(this);
                regMobileEvents();
                


            }
            else
            {
                bs[0] = 0x2;
                SendData(11, bs);
                Server.s.Log("[Remote] A Remote with incorrect information attempted to join.");
                RemoteServer.tries++;

            }
        }

        private void regMobileEvents()
        {
            Server.s.OnLog += LogServerMobile;
            Server.s.OnAdmin += LogAdminMobile;
            Server.s.OnOp += LogOpMobile;
            Server.s.OnSettingsUpdate += SettingsUpdateMobile;

            //Player.PlayerChat +=new Player.OnPlayerChat(Player_PlayerChat);

            Player.PlayerConnect += PlayerConnectMobile;
            Player.PlayerDisconnect += PlayerDisconnectMobile;
            Level.LevelLoad += LevelLoadMobile;
            Level.LevelUnload += LevelUnloadMobile;
            Group.OnGroupLoad += GroupChangedMobile;
            Group.OnGroupSave += GroupChangedMobile;

            //Create var for them instead
        }
        public void unregMobileEvents()
        {
            //TODO: unregister events
        }
        public void SendStringDataMobile(int id, string p)
        {
            p = EncryptMobile(p, _keyMobile);
            byte[] bytes = new byte[(p.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)p.Length).CopyTo(bytes, 0);
            Encoding.BigEndianUnicode.GetBytes(p).CopyTo(bytes, 2);
            SendData(id, bytes);
            System.Threading.Thread.Sleep(100);
        }
        public void SendStringDataMobile(string p)
        {
            byte[] bytes = new byte[(p.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)p.Length).CopyTo(bytes, 0);
            Encoding.BigEndianUnicode.GetBytes(p).CopyTo(bytes, 2);
            SendData(bytes);
        }
        internal void StartUpMobile()
        {
            System.Object lockThis = new System.Object();
            lock (lockThis)
            {
                SendStepsMobile(0);
                SendMapsMobile();
                SendStepsMobile(1);
                SendSettingsMobile();
                SendStepsMobile(2);
                SendGroupsMobile();
                SendStepsMobile(3);
                SendPlayersMobile();
                SendStepsMobile(4);  //cancels it here
            }

        }
        private void GenerateKeyMobile(string key)
        {
            byte[] biscut = new byte[(key.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)key.Length).CopyTo(biscut, 0);
            Encoding.BigEndianUnicode.GetBytes(key).CopyTo(biscut, 2);
            SendData(0x02, biscut);
        }
        private void SendStepsMobile(short b)
        {
            byte[] sdd = new byte[2];
            util.BigEndianBitConverter.Big.GetBytes(b).CopyTo(sdd, 0);
            SendData(0x09, sdd);
        }
        internal void SendPlayersMobile()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Player p in Player.players)
            {
                AddPlayerMobile(p);
                System.Threading.Thread.Sleep(100);
            }
        }
        internal void SendSettingsMobile()
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


            SendStringDataMobile(0x08, RECIEVED_ADMINS_JOIN + Server.adminsjoinsilent.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_MAIN_NAME + Server.mainLevel.name.ToString());
            SendStringDataMobile(0x08, RECIEVED_SERVER_IS_PUBLIC + Server.pub.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_SERVER_PORT + Server.port.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_SERVER_MOTD + Server.motd.ToString());
            SendStringDataMobile(0x08, RECIEVED_SERVER_NAME + Server.name.ToString());

            SendStringDataMobile(0x08, RECIEVED_IRC_USE + Server.irc.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_IRC_SERVER + Server.ircServer);
            SendStringDataMobile(0x08, RECIEVED_IRC_CHANNEL + Server.ircChannel);
            SendStringDataMobile(0x08, RECIEVED_IRC_OPCHANNEL + Server.ircOpChannel);
            SendStringDataMobile(0x08, RECIEVED_IRC_NICK + Server.ircNick);
            SendStringDataMobile(0x08, RECIEVED_IRC_COLOR + Server.IRCColour);
            SendStringDataMobile(0x08, RECIEVED_IRC_IDENT + Server.ircIdentify.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_IRC_PASS + Server.ircPassword);
            SendStringDataMobile(0x08, RECIEVED_IRC_PORT + Server.ircPort);

            SendStringDataMobile(0x08, RECIEVED_MISC_PHYSICSRESTART + Server.physicsRestart.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_MISC_RPLIMIT + Server.rpLimit.ToString());
            SendStringDataMobile(0x08, RECIEVED_MISC_NORMRPLIMIT + Server.rpNormLimit.ToString());
            SendStringDataMobile(0x08, RECIEVED_MISC_GLOBALCHAT + Server.UseGlobalChat.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_MISC_GLOBALCOLOR + Server.GlobalChatColor);
            SendStringDataMobile(0x08, RECIEVED_MISC_GLOBALNAME + Server.GlobalChatNick);
            SendStringDataMobile(0x08, RECIEVED_MISC_DOLLAR + Server.dollardollardollar.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_MISC_SUPEROPRANK + Server.rankSuper.ToString().ToLower());
            SendStringDataMobile(0x08, RECIEVED_MISC_PARSEEMOTE + Server.parseSmiley.ToString().ToLower());
            SendStringDataMobile(0x08, "Done_!*");

            return;
        }
        internal void AddPlayerMobile(Player p)
        {
            SendStringDataMobile(0x04, new StringBuilder("ADD:").Append(p.title).Append(",").Append(p.name)
                .Append(",").Append(p.group.name).Append(",").Append(p.color).ToString());

        }
        internal void RemovePlayerMobile(Player p)
        {
            SendStringDataMobile(0x04, "DELETE:" + p.name);
        }
        List<string> levels = new List<string>(Server.levels.Count);
        internal void SendMapsMobile()
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
                        SendStringDataMobile(0x06, "UN_" + file.Name.Replace(".lvl", ""));
                    }
                }

                Server.levels.ForEach(delegate(Level l) { SendStringDataMobile(0x06, "LO_" + l.name); });

            }

            catch (Exception e) { Server.ErrorLog(e); }

        }
        internal void SendGroupsMobile()
        {
            SendStringDataMobile(0x07, "*");
            foreach (Group g in Group.GroupList)
            {
                if (g.Permission == LevelPermission.Nobody || g.name == "nobody" || g.trueName == "nobody") break;
                else
                    SendStringDataMobile(0x07, g.color + "," + g.name + "," + g.maxBlocks + "," + ((int)g.Permission).ToString());
            }
            SendStringDataMobile(0x07, "~" + (Group.GroupList.Count - 1).ToString());

        }
        void LogOpMobile(string message)
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
                messaged = EncryptMobile(messaged, _keyMobile);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)2;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                if (OnRemoteOpLog != null) OnRemoteOpLog(this, p, messaged);
                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message.Replace(p.name + ":", "")).ToString();
                messaged = EncryptMobile(messaged, _keyMobile);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)2;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                if (OnRemoteOpLog != null) OnRemoteOpLog(this, p, messaged);
                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }


        }
        


        void LogAdminMobile(string message)
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
                messaged = EncryptMobile(messaged, _keyMobile);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)4;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                if (OnRemoteAdminLog != null) OnRemoteAdminLog(this, p, messaged);
                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message.Replace(p.name + ":", "")).ToString();
                messaged = EncryptMobile(messaged, _keyMobile);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)4;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                if (OnRemoteAdminLog != null) OnRemoteAdminLog(this, p, messaged);
                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }

        }
        void LogServerMobile(string message)
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
                messaged = EncryptMobile(messaged, _keyMobile);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)1;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                if (OnRemoteLog != null) OnRemoteLog(this, p, messaged);
                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }
            else
            {
                string messaged = new StringBuilder().Append(p.name).Append("ĥ").Append(message).ToString();
                messaged = EncryptMobile(messaged, _keyMobile);
                byte[] buffed = new byte[(messaged.Length * 2) + 3];
                util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                buffed[0] = (byte)1;
                Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                if (OnRemoteLog != null) OnRemoteLog(this, p, messaged);
                SendData(0x05, buffed);
                System.Threading.Thread.Sleep(100);
            }


        }
        void PlayerConnectMobile(Player p)
        {
            AddPlayerMobile(p);
        }
        void PlayerDisconnectMobile(Player p, string message)
        {
            RemovePlayerMobile(p);
        }
        void LevelLoadMobile(string l)
        {
            //Server.s.Log("WAS " + l + " LOADED?");
            SendStringDataMobile(0x06, "LO_" + l);
        }
        void LevelUnloadMobile(Level l)
        {
            SendStringDataMobile(0x06, "UN_" + l.name);
            //Server.s.Log("WAS " + l.name + " LOADED?");
        }
        void SettingsUpdateMobile()
        {
            SendSettingsMobile();
        }
        void GroupChangedMobile()
        {
            SendGroupsMobile();
        }


        string DecryptMobile(string textToDecrypt, string key)
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

        string EncryptMobile(string textToEncrypt, string key)
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
