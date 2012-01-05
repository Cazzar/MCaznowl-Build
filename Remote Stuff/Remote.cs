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
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using MCForge;

namespace MCForge.Remote
{
    public partial class Remote
    {
        //public static Remote This;
        public string ip;

        byte[] _bu = new byte[0];
        readonly byte[] _tbu = new byte[0xFF];

        private bool Upcast { get; set; }
        public bool LoggedIn { get; protected set; }

        public Socket Socket;
        public static List<Remote> Remotes = new List<Remote>();
        public int Protocal = 2;
        private readonly string _keyMobile;
        public byte RemoteType = 4;
        public Remote()
        {
           // Remote.This = this;
            _keyMobile = generateRandChars();
        }

        private string generateRandChars()
        {
            /* Random r = new Random();
            byte[] rs = new byte[31];
            r.NextBytes(rs);
            return Encoding.UTF8.GetString(rs);
            */
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);


        }
        public void Start()
        {
            if (!RemoteServer.enableRemote) return;
            try
            {
                RemoteProperties.Load();
                ip = Socket.RemoteEndPoint.ToString().Split(':')[0];
                Player.GlobalMessage(c.navy + "A Remote has connected to the server");
                Server.s.Log("[Remote] connected to the server.");


                Socket.BeginReceive(_tbu, 0, _tbu.Length, SocketFlags.None, Receive, this);
            }

            catch (Exception e)
            {
                Server.ErrorLog(e);
            }
        }

        static void Receive(IAsyncResult result)
        {
            Remote p = (Remote)result.AsyncState;
            if (p.Upcast || p.Socket == null)
                return;
            try
            {
                int length = p.Socket.EndReceive(result);
                if (length == 0) { p.Disconnect(); return; }

                byte[] b = new byte[p._bu.Length + length];
                Buffer.BlockCopy(p._bu, 0, b, 0, p._bu.Length);
                Buffer.BlockCopy(p._tbu, 0, b, p._bu.Length, length);

                p._bu = p.HandleMessage(b);
                p.Socket.BeginReceive(p._tbu, 0, p._tbu.Length, SocketFlags.None,
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

        byte[] HandleMessage(byte[] buffer)
        {

            if (OnRemoteRecieveData != null) OnRemoteRecieveData(this, (short)buffer.Length);
            
            try
            {
                int length;
                byte msg = buffer[0];
                switch (msg)
                {


                    case 7: length = (BitConverter.ToInt16(buffer, 1) + 2); break;
                    case 2: length = (BitConverter.ToInt16(buffer, 1) + 2); break;
                    case 3: length = 1; break;
                    case 4: length = (BitConverter.ToInt16(buffer, 1) + 2); break;
                    case 5: length = (BitConverter.ToInt16(buffer, 1) + 2); break;
                    case 6: length = (BitConverter.ToInt16(buffer, 1) + 3); break;
                    case 8: length = 1; break;



                    /*
                     * MOBILE
                     */
                    case 11: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); break;
                    case 12: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); if (!LoggedIn) goto default; break;
                    case 13: length = 1; if (!LoggedIn) goto default; break;
                    case 14: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); if (!LoggedIn) goto default; break;
                    case 15: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 2)); if (!LoggedIn) goto default; break;
                    case 16: length = ((util.EndianBitConverter.Big.ToInt16(buffer, 1) + 3)); if (!LoggedIn) goto default; break;
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
                        case 7: HandleDesktopLogin(message); RemoteType = 0; break;
                        case 2: HandleDesktopChat(message); break;
                        case 3: HandleDesktopRequest(message);break;
                        case 11: HandleMobileLogin(message); RemoteType = 1; break;   //Login 
                        case 12: HandleMobileChat(message); break;
                        case 13: HandleMobileRequest(message); break;
                        case 14: HandleMobileSettingsChange(message); break;
                        case 15: HandleMobileCommand(message); break;
                        case 16: HandleMobileChangeGroup(message); break;

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


        
        public List<string> GetUnloaded()
        {
            var tmpList = new List<string>(Server.levels.Count);
            var realList = new List<string>();
            try
            {
                DirectoryInfo di = new DirectoryInfo("levels/");
                FileInfo[] fi = di.GetFiles("*.lvl");
                tmpList.AddRange(Server.levels.Select(l => l.name));
                realList.AddRange(from l in fi where !tmpList.Contains(l.Name.Replace(".lvl", "")) select l.Name.Replace(".lvl", ""));
                return realList;
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
                return null;
            }
        }

        public void KickedOrDisconnect(bool kicked)
        {


            if (LoggedIn)
            {
                Player.GlobalMessage("%5[Remote] %f" + (kicked ? " has been kicked from server!" : "has disconnected."));
                Server.s.Log("[Remote]" + (kicked ? " has been kicked from server!" : "has disconnected."));
                if(kicked)SendData(0x03);
            }
            LoggedIn = false;


            if (!kicked) { if (OnRemoteDisconnect != null) OnRemoteDisconnect(this); }
            else { if (OnRemoteKick != null) OnRemoteKick(this); }

            this.Dispose();
            UnregEvents();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
        }

        private void UnregEvents()
        {
            switch (RemoteType)
            {
                case 1:
                    unregMobileEvents();
                    break;
                case 0:
                    UnregDesktopEvents();
                    break;
                case 2:
                    break;
            }
        }

        public void Disconnect()
        {
            KickedOrDisconnect(false);
        }
        public void Kick()
        {
            KickedOrDisconnect(true);
        }

        public void SendData(int id) { SendData(id, new byte[0]); }
        public void SendData(int id, byte[] send)
        {
            if (Socket == null || !Socket.Connected) return;
            var buffer = new byte[send.Length + 1];
            buffer[0] = (byte)id;
            for (var i = 0; i < send.Length; i++)
            {
                buffer[i + 1] = send[i];
            }
            try
            {

                Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, ar => { },  null);
                if (OnRemoteSendData != null) OnRemoteSendData(this, (short)(id + send.Length));
            }
            catch (SocketException)
            {
                Disconnect();
            }
        }
        public void SendData(byte[] send)
        {
            if (Socket == null || !Socket.Connected) return;
            try
            {

                Socket.BeginSend(send, 0, send.Length, SocketFlags.None, delegate{ }, null);
                if (OnRemoteSendData != null) OnRemoteSendData(this, (short)(send.Length));
                _bu = null;
            }
            catch (SocketException)
            {
                _bu = null;
                Disconnect();
            }

        }
        void LogPacket(byte id, byte[] packet)
        {
            string s = "";

            if (packet.Length >= 1)
            {
                s = packet.Aggregate(s, (current, b) => current + (b + ", "));
                Server.s.Log(string.Format("Packet {0} {{ {1}}}", id, s));
            }
            else
            {
                Server.s.Log(string.Format("Packet {0} had no DATA!", id));
            }
        }


        public void Dispose()
        {
            if (Socket != null && Socket.Connected)
            {
                try { Socket.Close(); }
                finally
                {
                    Socket = null;

                }
            }
            Remotes.Remove(this);
        }
    }
}