/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
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
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Globalization;

namespace MCForge
{
    public abstract partial class Plugin
    {
        public static List<Plugin> all = new List<Plugin>();
        public static List<Plugin_Simple> all_simple = new List<Plugin_Simple>();
        public abstract void Load(bool startup);
        public abstract void Unload(bool shutdown);
        public abstract string name { get; }
        public abstract string website { get; }
        public abstract string MCForge_Version { get; } // Oldest version of MCForge the plugin is compatible with.
        public abstract int build { get; }
        public abstract string welcome { get; }
	    public abstract string creator { get; }
	    public abstract bool LoadAtStartup { get; }
        public abstract void Help(Player p);
        /// <summary>
        /// Look to see if a plugin is loaded
        /// </summary>
        /// <param name="name">The name of the plugin</param>
        /// <returns>Returns the plugin (returns null if non is found)</returns>
        public static Plugin Find(string name)
        {
            List<Plugin> tempList = new List<Plugin>();
            tempList.AddRange(all);
            Plugin tempPlayer = null; bool returnNull = false;

            foreach (Plugin p in tempList)
            {
                if (p.name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture)) return p;
                if (p.name.ToLower(CultureInfo.CurrentCulture).IndexOf(name.ToLower(CultureInfo.CurrentCulture), StringComparison.CurrentCulture) != -1)
                {
                    if (tempPlayer == null) tempPlayer = p;
                    else returnNull = true;

                }
            }

            if (returnNull == true) return null;
            if (tempPlayer != null) return tempPlayer;
            return null;
        }
        /// <summary>
        /// Load a plugin
        /// </summary>
        /// <param name="pluginname">The file path of the dll file</param>
        /// <param name="startup">Is this startup?</param>
        public static void Load(string pluginname, bool startup)
        {
	    String creator = "";
            try
            {
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
                        if (t.BaseType == typeof(Plugin))
                        {
                            instance = Activator.CreateInstance(t);
                            break;
                        }
                    }
                }
                catch { }
                if (instance == null)
                {
                    Server.s.Log("The plugin " + pluginname + " couldnt be loaded!");
                    return;
                }
                String plugin_version = ((Plugin)instance).MCForge_Version;
                if (!String.IsNullOrEmpty(plugin_version) && new Version(plugin_version) > Server.Version)
                {
                    Server.s.Log("This plugin (" + ((Plugin)instance).name + ") isnt compatible with this version of MCForge!");
                    Thread.Sleep(1000);
                    if (Server.unsafe_plugin)
                    {
                        Server.s.Log("Will attempt to load!");
                        goto here;
                    }
                    else
                        return;
                }
                here:
                Plugin.all.Add((Plugin)instance);
		        creator = ((Plugin)instance).creator;
                if (((Plugin)instance).LoadAtStartup)
                {
                    ((Plugin)instance).Load(startup);
                    Server.s.Log("Plugin: " + ((Plugin)instance).name + " loaded...build: " + ((Plugin)instance).build);
                }
                else
                    Server.s.Log("Plugin: " + ((Plugin)instance).name + " was not loaded, you can load it with /pload");
                Server.s.Log(((Plugin)instance).welcome);
            }
            catch (FileNotFoundException)
            {
                Plugin_Simple.Load(pluginname, startup);
            }
            catch (BadImageFormatException)
            {
                Plugin_Simple.Load(pluginname, startup);
            }
            catch (PathTooLongException)
            {
            }
            catch (FileLoadException)
            {
                Plugin_Simple.Load(pluginname, startup);
            }
            catch (Exception e)
            {
                try { Server.s.Log("Attempting a simple plugin!"); if (Plugin_Simple.Load(pluginname, startup)) return; }
                catch { }
                Server.ErrorLog(e);
				Server.s.Log("The plugin " + pluginname + " failed to load!");
				if (!(creator != null && String.IsNullOrEmpty(creator)))
					Server.s.Log("You can go bug " + creator + " about it");
				Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// Unload a plugin
        /// </summary>
        /// <param name="p">The plugin to unload</param>
        /// <param name="shutdown">Is this shutdown?</param>
        public static void Unload(Plugin p, bool shutdown)
        {
            try { p.Unload(shutdown);
            all.Remove(p);

            Server.s.Log(p.name + " was unloaded.");
            }
            catch { Server.s.Log("An Error occurred while unloading a plugin"); }
        }
        /// <summary>
        /// Unload all plugins
        /// </summary>
        public static void Unload()
        {
            all.ForEach(delegate(Plugin p)
            {
                Unload(p, true);
            });
        }
        /// <summary>
        /// Load all plugins
        /// </summary>
        public static void Load()
        {
            if (Directory.Exists("plugins"))
            {
                foreach (string file in Directory.GetFiles("plugins", "*.dll"))
                {
                    Load(file, true);
                }
            }
            else
                Directory.CreateDirectory("plugins");
            /*
             ===Load Internal Plugins===
             */
            Plugin.all_simple.Add(new MCForge.CTF.Setup());
        }
    }
}

