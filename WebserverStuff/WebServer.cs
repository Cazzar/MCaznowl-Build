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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MCForge.Remote
{
   public class RemoteServer
    {
        public Socket listen;
        public static int port = 5050;
        public static string username = "head";
        public static string password = "lols";
        public static bool enableRemote = true;
        

        static bool shutdown = false;
       
        

        public void Start()
        {
            RemoteProperties.Load();
            if (enableRemote)
            {
                try
                {
                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
                    listen = new Socket(endpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    listen.Bind(endpoint);
                    listen.Listen((int)SocketOptionName.MaxConnections);

                    listen.BeginAccept(new AsyncCallback(Accept), null);

                   Server.s. Log("Creating listening socket on port " + port + " for remote console...");          
                }
                catch (SocketException e) { Server.s.Log(e.Message + e.StackTrace); }
                catch (Exception e) { Server.s.Log(e.Message + e.StackTrace); }
            }
        }

        void Accept(IAsyncResult result)
        {
            if (Server.shuttingDown == false)
            {
                Remote p = null;
                bool begin = false;
                try
                {
                    p = new Remote();

                    p.socket = listen.EndAccept(result);
                    new Thread(new ThreadStart(p.Start)).Start();

                    listen.BeginAccept(new AsyncCallback(Accept), null);
                    begin = true;

                   

                }
                catch (SocketException)
                {
                    if (p != null)
                        p.Disconnect();
                    if (!begin)
                        listen.BeginAccept(new AsyncCallback(Accept), null);
                }
                catch (Exception e)
                {
                    Server.s.Log(e.Message);
                    Server.s.Log(e.StackTrace);
                    if (p != null)
                        p.Disconnect();
                    if (!begin)
                        listen.BeginAccept(new AsyncCallback(Accept), null);
                }
            }
        }
        
        
        public static void Close()
        {
            shutdown = true;
        }
    }
}