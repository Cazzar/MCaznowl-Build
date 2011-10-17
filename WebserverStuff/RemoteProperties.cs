using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

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
                        if (line[0] != '#')
                        {
                            string value = line.Substring(line.IndexOf(" = ") + 3);
                            switch (line.Substring(0, line.IndexOf(" = ")))
                            {
                                case "RemoteEnable": RemoteServer.enableRemote = bool.Parse(value); break;
                                case "RemoteUsername": RemoteServer.username = value; break;
                                case "RemotePassword": RemoteServer.password = value; break;
                                case "RemotePort": RemoteServer.port = int.Parse(value); break;
                            }

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
                using (StreamWriter w = File.CreateText(fileName))
                {
                    w.WriteLine("RemoteEnable = " + RemoteServer.enableRemote.ToString().ToLower());
                    w.WriteLine("RemoteUsername = " + RemoteServer.username);
                    w.WriteLine("RemotePassword = " + RemoteServer.password);  //TODO: encrypt
                    w.WriteLine("RemotePort = " + RemoteServer.port.ToString());
                }
            }
            catch
            {
                Server.s.Log("remote properties save failed " + fileName);
            }
        }
    }
}