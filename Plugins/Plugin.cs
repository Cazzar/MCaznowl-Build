using System;
using System.Collections.Generic;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;

namespace MCForge
{
    public abstract class Plugin
    {
        public static List<Plugins> all = new List<Plugins>();
        public abstract void Load(bool startup);
        public abstract void Unload(bool shutdown);
        public abstract string name { get; }
        public abstract int build { get; }
        public abstract string welcome { get; }
        public abstract void Help(Player p);
        public static Plugins Find(string name)
        {
            List<Plugins> tempList = new List<Plugins>();
            tempList.AddRange(all);
            Plugins tempPlayer = null; bool returnNull = false;

            foreach (Plugins p in tempList)
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
            try
            {
                Assembly asm = Assembly.LoadFrom("plugins/" + pluginname + ".dll");
                Type type = asm.GetTypes()[0];
                object instance = Activator.CreateInstance(type);
                Plugins.all.Add((Plugins)instance);
                ((Plugins)instance).Load(startup);
                Server.s.Log("Plugin: " + ((Plugins)instance).name + " loaded...build: " + ((Plugins)instance).build);
                Server.s.Log(((Plugins)instance).welcome);
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
            }
        }
        public static void Unload(Plugins p, bool shutdown)
        {
            p.Unload(shutdown);
            all.Remove(p);
            Server.s.Log(p.name + " was unloaded...how ever you cant re-load it until you restart!");
        }
		public static void Unload()
		{
			foreach (Plugin p in all)
			{
				Unload(p, true);
			}
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

