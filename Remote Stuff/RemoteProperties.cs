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

using System.IO;

namespace MCForge.Remote
{
    public class RemoteProperties
    {
        public static void Load()
        {
            if (File.Exists("properties/remote.properties"))
            {
                foreach (string line in File.ReadAllLines("properties/remote.properties"))
                {
                    try
                    {
                        if (line[0] == '#') continue;
                        string value = line.Substring(line.IndexOf(" = ") + 3);
                        switch (line.Substring(0, line.IndexOf(" = ")))
                        {
                            case "RemoteEnable": RemoteServer.enableRemote = bool.Parse(value); break;
                            case "RemoteUsername": RemoteServer.username = value; break;
                            case "RemotePassword": RemoteServer.password = value; break;
                            case "RemotePort": RemoteServer.port = int.Parse(value); break;
                        }
                    }
                    catch { Server.s.Log("Failed to load remote properties!"); }
                }
            }

            else { Save("properties/remote.properties"); Server.s.Log("Created properties file for remote, in Properties Folder"); }
        }

        public static void Save(string fileName)
        {
            try
            {
                File.Create(fileName).Dispose();
                using (var w = File.CreateText(fileName))
                {
                    w.WriteLine("RemoteEnable = {0}", RemoteServer.enableRemote.ToString().ToLower());
                    w.WriteLine("RemoteUsername = {0}", RemoteServer.username);
                    w.WriteLine("RemotePassword = {0}", RemoteServer.password);
                    w.WriteLine("RemotePort = {0}", RemoteServer.port);
                }
            }
            catch
            {
                Server.s.Log(string.Format("remote properties save failed {0}", fileName));
            }
        }
    }
}