using System;
using System.Collections.Generic;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;

namespace Auto_Lava
{
    public delegate bool OnCommand(string cmd, string message, Player p);
    public abstract class Plugins
    {
        public static List<Plugins> all = new List<Plugins>();
        public abstract void Load(bool startup = false);
        public abstract void Unload(bool shutdown = false);
        public abstract string name { get; }
        public abstract int build { get; }
        public abstract string welcome { get; }
        public abstract void Help(Player p);
        public static event OnCommand CommandUsed = null;
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
        public static void Load(string pluginname)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom("plugins/" + pluginname + ".dll");
                Type type = asm.GetTypes()[0];
                object instance = Activator.CreateInstance(type);
                Plugins.all.Add((Plugins)instance);
                ((Plugins)instance).Load(true);
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
        public static void Unload(Plugins p)
        {
            p.Unload();
            all.Remove(p);
            Server.s.Log(p.name + " was unloaded...how ever you cant re-load it until you restart!");
        }
        public static bool CheckCommand(string cmd, string message, Player p)
        {
            if (CommandUsed != null)
                return CommandUsed(cmd, message, p);
            else
                return false;
        }
        public static void Load()
        {
            if (Directory.Exists("plugins"))
            {
                foreach (string file in Directory.GetFiles("plugins", "*.dll"))
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFrom(file);
                        Type type = asm.GetTypes()[0];
                        object instance = Activator.CreateInstance(type);
                        Plugins.all.Add((Plugins)instance);
                        ((Plugins)instance).Load(true);
                        Server.s.Log("Plugin: " + ((Plugins)instance).name + " loaded...build: " + ((Plugins)instance).build);
                        Server.s.Log(((Plugins)instance).welcome);
                    }
                    catch (FileNotFoundException e)
                    {
                        Server.ErrorLog(e);
                        continue;
                    }
                    catch (BadImageFormatException e)
                    {
                        Server.ErrorLog(e);
                        continue;
                    }
                    catch (PathTooLongException)
                    {
                        continue;
                    }
                    catch (FileLoadException e)
                    {
                        Server.ErrorLog(e);
                        continue;
                    }
                    catch (Exception e)
                    {
                        Server.ErrorLog(e);
                        continue;
                    }
                }
            }
        }
    }
}