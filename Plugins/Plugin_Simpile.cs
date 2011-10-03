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
        public static bool Load(string pluginname, bool startup)
        {
            String creator = "";
            object instance = Activator.CreateInstance(Assembly.LoadFrom(pluginname).GetTypes()[0]);
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
