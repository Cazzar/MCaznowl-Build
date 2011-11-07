using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Remote
{
    public partial class Remote
    {
        private void HandleDesktopLogin(byte[] message)
        {
            short length = BitConverter.ToInt16(message, 0);
            string msg = Encoding.UTF8.GetString(message, 2, length);
            Server.s.Log("THE STUFF SAID: " + msg);
            //msg = this.Decrypt(msg, "FORGEREMOTETIVITY");
            byte[] bs = new byte[1];
            //Server.s.Log(msg);
            if (msg.StartsWith(protocal.ToString()))  //TODO: make a better checker
            {
                msg = msg.Replace(protocal.ToString() + ": ", "");
            }
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
                remotes.Add(this);

                Server.s.OnLog += new Server.LogHandler(LogChatDesktop);
                /* Server.s.OnAdmin += new Server.LogHandler(s_OnAdmin);
                 Server.s.OnOp += new Server.LogHandler(s_OnOp);
                 Server.s.OnSettingsUpdate += new Server.VoidHandler(s_OnSettingsUpdate);
                 Player.PlayerConnect += new Player.OnPlayerConnect(Player_PlayerConnect);
                 Player.PlayerDisconnect += new Player.OnPlayerDisconnect(Player_PlayerDisconnect);
                 Level.LevelLoad += new Level.OnLevelLoad(Level_LevelLoad);
                 Level.LevelUnload += new Level.OnLevelUnload(Remote_LevelUnload);
                 Group.OnGroupLoad += new Group.GroupLoad(GroupChanged);
                 Group.OnGroupSave += new Group.GroupSave(GroupChanged);*/
                return;
            }
            else
            {
                bs[0] = 2;
                SendData(0x40, bs);
                Server.s.Log("[Remote] A Remote with incorrect information attempted to join.");
                // LogPacket(7, message);
                RemoteServer.tries++;
                return;
            }

        }
        private void GenerateKeyDesktop(string key)
        {
            byte[] biscut = new byte[(key.Length * 2) + 2];
            BitConverter.GetBytes((short)key.Length).CopyTo(biscut, 0);
            Encoding.UTF8.GetBytes(key).CopyTo(biscut, 2);
            SendData(0x02, biscut);
        }
        void LogChatDesktop(string message)
        {

        }
    }
}
