using System;
using System.Collections.Generic;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MCForge
{
    public abstract class Plugin
    {
        public static List<Plugin> all = new List<Plugin>();
        public abstract void Load(bool startup);
        public abstract void Unload(bool shutdown);
        public abstract string name { get; }
        public abstract string website { get; }
        public abstract string MCForge_Version { get; }
        public abstract int build { get; }
        public abstract string welcome { get; }
	public abstract string creator { get; }
	public abstract bool LoadAtStartup { get; }
        public abstract void Help(Player p);
        public static Plugin Find(string name)
        {
            List<Plugin> tempList = new List<Plugin>();
            tempList.AddRange(all);
            Plugin tempPlayer = null; bool returnNull = false;

            foreach (Plugin p in tempList)
            {
                if (p.name.ToLower() == name.ToLower()) return p;
                if (p.name.ToLower().IndexOf(name.ToLower()) != -1)
                {
                    if (tempPlayer == null) tempPlayer = p;
                    else returnNull = true;

                }
            }

            if (returnNull == true) return null;
            if (tempPlayer != null) return tempPlayer;
            return null;
        }
        public static void Load(string pluginname, bool startup)
        {
	    String creator = "";
            try
            {
                object instance = Activator.CreateInstance(Assembly.LoadFrom(pluginname).GetTypes()[0]);
                if (((Plugin)instance).MCForge_Version != Server.Version && ((Plugin)instance).MCForge_Version != "")
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
            catch (FileNotFoundException e)
            {
                Server.ErrorLog(e);
            }
            catch (BadImageFormatException e)
            {
                Server.ErrorLog(e);
            }
            catch (PathTooLongException)
            {
            }
            catch (FileLoadException e)
            {
                Server.ErrorLog(e);
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
				Server.s.Log("The plugin " + pluginname + " failed to load!");
				if (creator != "")
					Server.s.Log("You can go bug " + creator + " about it");
				Thread.Sleep(1000);
            }
        }
        public static void Unload(Plugin p, bool shutdown)
        {
            p.Unload(shutdown);
            all.Remove(p);
            Server.s.Log(p.name + " was unloaded...how ever you cant re-load it until you restart!");
        }
	public static void Unload()
	{
		all.ForEach(delegate(Plugin p)
		{
			Unload(p, true);
		});
	}
        public static void Load()
        {
            if (Directory.Exists("plugins"))
            {
                foreach (string file in Directory.GetFiles("plugins", "*.dll"))
                {
                    Load(file, true);
                }
            }
        }
    }
}

