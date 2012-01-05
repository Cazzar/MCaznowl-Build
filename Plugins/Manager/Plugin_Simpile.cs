using System;
using System.Collections.Generic;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MCForge
{
    public abstract class Plugin_Simple
    {
        public abstract void Load(bool startup);
        public abstract void Unload(bool shutdown);
        public abstract string name { get; }
        public abstract string creator { get; }
        public abstract string MCForge_Version { get; }
        /// <summary>
        /// Load a simple plugin
        /// </summary>
        /// <param name="pluginname">The filepath to load</param>
        /// <param name="startup">Weather the server is starting up or not</param>
        /// <returns>Weather the plugin loaded or not</returns>
        public static bool Load(string pluginname, bool startup)
        {
            String creator = "";
            object instance = null;
            Assembly lib = null;
            using (FileStream fs = File.Open(pluginname, FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] buffer = new byte[1024];
                    int read = 0;
                    while ((read = fs.Read(buffer, 0, 1024)) > 0)
                        ms.Write(buffer, 0, read);
                    lib = Assembly.Load(ms.ToArray());
                    ms.Close();
                    ms.Dispose();
                }
                fs.Close();
                fs.Dispose();
            }
            try
            {
                foreach (Type t in lib.GetTypes())
                {
                    if (t.BaseType == typeof(Plugin_Simple))
                    {
                        instance = Activator.CreateInstance(lib.GetTypes()[0]);
                        break;
                    }
                }
            }
            catch { }
            if (instance == null)
            {
                Server.s.Log("The plugin " + pluginname + " couldnt be loaded!");
                return false;
            }
            String plugin_version = ((Plugin_Simple)instance).MCForge_Version;
            if (plugin_version != "" && new Version(plugin_version) != Server.Version)
            {
                Server.s.Log("This plugin (" + ((Plugin_Simple)instance).name + ") isnt compatible with this version of MCForge!");
                Thread.Sleep(1000);
                if (Server.unsafe_plugin)
                {
                    Server.s.Log("Will attempt to load!");
                    goto here;
                }
                else
                    return false;
            }
        here:
            Plugin.all_simple.Add((Plugin_Simple)instance);
            creator = ((Plugin_Simple)instance).creator;
            ((Plugin_Simple)instance).Load(startup);
            Server.s.Log("Plugin: " + ((Plugin_Simple)instance).name + " loaded...");
            Thread.Sleep(1000);
            return true;
        }
    }
}
