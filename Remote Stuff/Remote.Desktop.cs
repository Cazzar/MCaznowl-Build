using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MCForge.Remote
{
    public partial class Remote
    {

        #region Vars
        private Server.LogHandler _logHandler;
        private Player.OnPlayerConnect _onPlayerConnect;
        private Player.OnPlayerDisconnect _onPlayerDisconnect;
        #endregion
        private void RegEvents()
        {
            _logHandler += LogChatDesktop;
            _onPlayerConnect += PlayerConnect;
            _onPlayerDisconnect += PlayerDisconnect;
            Server.s.OnLog += _logHandler;
            Player.PlayerConnect += _onPlayerConnect;
            Player.PlayerDisconnect += _onPlayerDisconnect;

#if Debug
            Server.s.Log("Registered Events");
#endif

        }

        private void UnregDesktopEvents()
        {
            Server.s.OnLog -= _logHandler;
            Player.PlayerConnect -= _onPlayerConnect;
            Player.PlayerDisconnect -= _onPlayerDisconnect;
#if Debug
            Server.s.Log("Unregistered Events");
#endif
        }
        private void GenerateKeyDesktop(string key)
        {
            var biscut = new byte[(key.Length) + 2];
            BitConverter.GetBytes((short)key.Length).CopyTo(biscut, 0);
            Encoding.UTF8.GetBytes(key).CopyTo(biscut, 2);
            SendData(0x2, biscut);
        }
        #region Delegate Methods

        void LogChatDesktop(string message)
        {
            var bytes = new byte[(message.Length) + 2];
            BitConverter.GetBytes((short)message.Length).CopyTo(bytes, 0);
            Encoding.UTF8.GetBytes(message).CopyTo(bytes, 2);
            SendData(0x12, bytes);
        }
        void PlayerDisconnect(Player p, string kickReason)
        {
            RemovePlayerDesktop(p);
        }
        void PlayerConnect(Player p)
        {
           AddPlayerDesktop(p);
        }


        #endregion

        private void AddPlayerDesktop(Player p)
        {
            var bytes = new byte[10 + (p.name.Length + p.title.Length + p.group.name.Length)];
            bytes[0] = 2;
            BitConverter.GetBytes((short)p.name.Length).CopyTo(bytes, 1);
            BitConverter.GetBytes((short)p.title.Length).CopyTo(bytes, 3);
            BitConverter.GetBytes((short)p.group.name.Length).CopyTo(bytes, 5);

            Encoding.UTF8.GetBytes(p.name).CopyTo(bytes, 7);
            Encoding.UTF8.GetBytes(p.title).CopyTo(bytes, 8 + p.name.Length);
            Encoding.UTF8.GetBytes(p.group.name).CopyTo(bytes, 9 + p.name.Length + p.title.Length);
            SendData(0x13, bytes);
        }
        private void RemovePlayerDesktop(Player p)
        {
            var bytes = new byte[p.name.Length + 3];
            bytes[0] = 1;
            BitConverter.GetBytes((short)p.name.Length).CopyTo(bytes, 1);
            Encoding.UTF8.GetBytes(p.name).CopyTo(bytes, 3);
            SendData(0x13, bytes);
        }









        #region HANDLERS
        private void HandleDesktopLogin(byte[] message)
        {
            short length = BitConverter.ToInt16(message, 0);
            string msg = Encoding.UTF8.GetString(message, 2, length);
            //msg = this.DecryptDesktop(msg, "FORGEREMOTETIVITY");
            byte[] bs = new byte[1];
            //Server.s.Log(msg);
            if (msg.StartsWith(Protocal.ToString())) //TODO: make a better checker
                msg = msg.Replace(string.Format("{0}: ", Protocal), string.Empty);
            else
            {
                bs[0] = 3;
                SendData(0x40, bs);
                Server.s.Log("[Remote] A remote tried to connect with a different version.");
                return;
            }
            if (RemoteServer.tries >= 3)
            {
                bs[0] = 4;
                SendData(0x40, bs);
                Server.s.Log("[Remote] A remote tried to connect with exceeding incorrect credentials");
                return;
            }
            if (RemoteServer.tries == 6)
            {
                bs[0] = 5;
                SendData(0x40, bs);
                Server.s.Log("[Remote] Remote was locked from the console, type \"/remote tryreset\" to reset the try count");
                return;
            }

            if (HandleLogin(msg))
            {

                bs[0] = 1;
                if (OnRemoteLogin != null) OnRemoteLogin(this);
                SendData(0x40, bs);
                //_genDH(_gend);
                Server.s.Log("[Remote] Remote Verified, passing controls to it!");
                LoggedIn = true;
                if (Remotes != null) Remotes.Add(this);
                RegEvents();
                return;
            }
            bs[0] = 2;
            SendData(0x40, bs);
            Server.s.Log("[Remote] A Remote with incorrect information attempted to join.");
            RemoteServer.tries++;
            return;
        }
        private void HandleDesktopChat(byte[] message)
        {
            var length = BitConverter.ToInt16(message, 0);
            var m = Encoding.UTF8.GetString(message, 2, length);
            if (m.Any(ch => ch < 32 || ch >= 127))
            {
                Kick();
                return;
            }
            RemoteChat(m);

        }
        private void HandleDesktopRequest(byte[] message)
        {
            var requestId = message[0];
            switch (requestId)
            {
                //case 1: SendPlayersDesktop(); break;
                //case 2: SendMapsDesktop(); break;
                //case 3:SendProperties(); break;
                case 4:SendServerInfo();break;
                default: return;

            }
        }

        private void SendServerInfo()
        {
            var formatted = string.Format("{0}%{1}", Server.name, Server.motd);
            var bytes = new byte[formatted.Length + 2];
            BitConverter.GetBytes((short)formatted.Length).CopyTo(bytes, 0);
            Encoding.UTF8.GetBytes(formatted).CopyTo(bytes, 2);
            SendData(0x15, bytes);
        }

        #endregion


    }
}
