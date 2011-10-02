/*
    Copyright 2011 ForgeCraft team
	
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

namespace MCForge.Remote
{
    public partial class Remote
    {
        public string ip;
        public string username;
        public string password;
     
        public static Remote remote = new Remote();
        byte[] buffer = new byte[0];
        byte[] tempbuffer = new byte[0xFF];

        bool disconnected = false;
        public bool LoggedIn { get; protected set; }

        public Socket socket;
        public static List<Remote> remotes = new List<Remote>();
        public string version = "1.1";

        public Remote()
        {

        }
        public void Start()
        {
            try
            {

                ip = socket.RemoteEndPoint.ToString().Split(':')[0];
                Player.GlobalMessage(c.navy + "A Remote has connected to the server");
                Server.s.Log("[Remote] " + ip + " connected to the server.");


                socket.BeginReceive(tempbuffer, 0, tempbuffer.Length, SocketFlags.None, new AsyncCallback(Receive), this);
            }
            catch (Exception e)
            {
                Server.s.Log(e.Message);
                Server.s.Log(e.StackTrace);
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
        byte[] HandleMessage(byte[] buffer)
        {
            try
            {
                int length = 0; byte msg = buffer[0];
                // Get the length of the message by checking the first byte
                switch (msg)
                {
                    case 0: length = 0; LoggedIn = true; break;  //Remote Connection
                    case 1: length = 4; break;   //version and type
                    case 2: length = ((BitConverter.ToInt16(buffer, 1) * 2) + 2); break; //Login Info Exchange
                    case 3: length = ((BitConverter.ToInt16(buffer, 1) * 2) + 2); break;  //Handshake
                    case 4: length = ((BitConverter.ToInt16(buffer, 1) * 2) + 2); break;  //Chat
                    //case 0x04: length = 1; break; //Entity Use
                    //case 0x05: length = 1; break; //respawn

                    //case 0x06: length = 1; break; //OnGround incoming
                    //case 0x07: length = 33; break; //Pos incoming
                    //case 0x08: length = 9; break; //???
                    case 10: length = ((BitConverter.ToInt16(buffer, 1) * 2) + 2); break; //DC
                        
                    case 11: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;   
                    case 12: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
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
                        case 1: HandleInfo(message); break;    //2 - 5
                        case 0x02: HandleRemoteLogin(message); break;
                        case 0x03: HandleRemoteHandshake(message); break;
                        case 0x04: HandleRemoteChatMessagePacket(message); break;

                        
                        case 11: HandleMobileLogin(message); break;   //Login 
                        case 12: HandleMobileChat(message); break;
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

        private void HandleMobileChat(byte[] message)
        {
            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string m = Encoding.UTF8.GetString(message, 2, length);

            if (m.Length > 119)
            {
                
                Kick();
                return;
            }
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
            
                Server.s.Log("[Remote] Disconnected.");
                Player.GlobalMessage("[Remote] Disconnected.");
                Disconnect();
                
            
        }

       

        private void HandleMobileLogin(byte[] message)
        {

            short length = util.EndianBitConverter.Big.ToInt16(message, 0);
            string msg = Encoding.UTF8.GetString(message, 2, length);
            //Server.Log(msg);
            if (msg.StartsWith("2.3: "))  //TODO: make a better checker
            {
                msg = msg.Replace("2.3: ", "");

            }
            else
            {
                
                Server.s.Log("[Remote] A remote tried to connect with a different version.");
                SendData("VERSION");
            }

            
            if (HandleLogin(msg))
            {
                 
                 Server.s.Log("[Remote] Remote Verified, passing controls to it!");
                 SendData("ACCEPTED");
                 LoggedIn = true;
                 remotes.Add(this);
            }
            else
            {
                
                SendData("FAIL");
                Server.s.Log("[Remote] A Remote with incorrect information attempted to join.");
            }
        }

        private void HandleInfo(byte[] message)
        {
            short type = BitConverter.ToInt16(message, 0);
            short version = BitConverter.ToInt16(message, 2);

            switch (type)
            {
                case 1: Server.s.Log("Desktop remote has joined"); break;
                case 2: Server.s.Log("Mobile Remote has joined"); break;
                default: Server.s.Log("Unknown type of remote has attempted to join"); Kick(); return;
            }
            //if (version != this.version)
                //Kick("You have a different version");
        }
        private void HandleRemoteDCPacket(byte[] message)
        {
            Server.s.Log("DC");
        }

        private void HandleRemoteChatMessagePacket(byte[] message)
        {
            Server.s.Log("REMOTE TRY TO TALK");
        }

        private void HandleRemoteHandshake(byte[] message)
        {
            Server.s.Log("REMOTE REQUEST DATA");
        }

        private void HandleRemoteLogin(byte[] message)
        {

            short length = BitConverter.ToInt16(message, 0);
            if (length > 32) { Kick(); return; }
            string Info = Encoding.BigEndianUnicode.GetString(message, 2, (length * 2));

            string[] seperate = Info.Split(':');  //need a better way of sending and getting passwords
            string password = seperate[1];
            username = seperate[0];








        }

        private void HandleRemoteJoin(byte[] message)
        {
            Server.s.Log("Remote Has Joined");
        }

        public void Kick()
        {
            if (disconnected) return;

            
            disconnected = true;
            if (LoggedIn){
                Player.GlobalMessage("%5[Remote] %fhas been kicked from the server!");
            LoggedIn = false;
            }

            try
            {

                //string accepted = message;
                //byte[] bytes = new byte[(accepted.Length * 2) + 2];
                //util.EndianBitConverter.Big.GetBytes((short)accepted.Length).CopyTo(bytes, 0);
                //Encoding.BigEndianUnicode.GetBytes(accepted).CopyTo(bytes, 2);
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

            
            if (LoggedIn)
                Player.GlobalMessage("%5[Remote] %fhas disconnected. (REMOTE)");
            LoggedIn = false;

 

            this.Dispose();
        }
        public void Dispose()
        {
            //SaveAttributes();

            if (socket != null && socket.Connected)
            {
                try { socket.Close(); }
                catch { }
                socket = null;
            }
        }

        public void SendData(int id) { SendData(id, new byte[0]); }
        public void SendData(int id, string p)
        {
            byte[] bytes = new byte[(p.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)p.Length).CopyTo(bytes, 0);
            Encoding.BigEndianUnicode.GetBytes(p).CopyTo(bytes, 2);
            SendData(id, bytes);
        }
        public void SendData(string p)
        {
            byte[] bytes = new byte[(p.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)p.Length).CopyTo(bytes, 0);
            Encoding.BigEndianUnicode.GetBytes(p).CopyTo(bytes, 2);
            SendData(bytes);
        }
        public void SendData(int id, byte[] send)
        {
            if (socket == null || !socket.Connected) return;

            byte[] buffer = new byte[send.Length + 1];
            buffer[0] = (byte)id;
            send.CopyTo(buffer, 1);

            try
            {
                socket.Send(buffer);
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
                socket.Send(send);
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
        public static Remote getRemote()
        {

            foreach (Remote tmp in Remote.remotes)
            {
                if (remotes.Count == 1)
                {
                    return tmp;
                }
            }
            return null;
        }

        internal void sendLog(string p)
        {
            byte[] bytes = new byte[(p.Length * 2) + 2];
            util.EndianBitConverter.Big.GetBytes((short)p.Length).CopyTo(bytes, 0);
            Encoding.BigEndianUnicode.GetBytes(p).CopyTo(bytes, 2);
            SendData(0x05, bytes);
            System.Threading.Thread.Sleep(100);
           
        }
        internal void sendPlayers()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Player p in Player.players) 
            {
                //builder.Append(i).Append(",").Append(p.name).Append(",").Append(p.level.name).Append(",").Append(p.group.name);  later
                System.Threading.Thread.Sleep(100);
                SendData(0x04, p.name);
            }

        }
    }
}
