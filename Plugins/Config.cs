using System;
using System.IO;
using System.Collections.Generic;

namespace MCForge
{
    public class Config
    {
        private string filename = "";
        private List<string> config = new List<string>();
        private Plugin p;
        private ConfigType c;
        private Dictionary<string, string> saves = new Dictionary<string, string>();
        /// <summary>
        /// Create a new config object
        /// </summary>
        /// <param name="p">The plugin that is using the config object</param>
        /// <param name="c">The config type you want</param>
        /// <param name="filename">The file name (DONT INCLUDE EXTENTION)</param>
        public Config(Plugin p, ConfigType c, string filename)
        {
            this.filename = filename;
            if (!Directory.Exists("plugins/" + p.name)) Directory.CreateDirectory("plugins/" + p.name);
            if (!File.Exists("plugins/" + p.name + "/" + filename + ".config") && c == ConfigType.Setting) File.Create("plugins/" + p.name + "/" + filename + ".config");
            if (!File.Exists("plugins/" + p.name + "/" + filename + ".file") && c == ConfigType.SaveFile) File.Create("plugins/" + p.name + "/" + filename + ".file");
            this.p = p;
            this.c = c;
        }
        /// <summary>
        /// Load the config or save file
        /// </summary>
        /// <returns>If it returns true, then it loaded correctly, otherwise there was an error</returns>
        public bool LoadFile()
        {
            try
            {
                if (c == ConfigType.Setting)
                {
                    if (!File.Exists("plugins/" + p.name + "/" + filename + ".config"))
                        return false;
                    string[] lines = File.ReadAllLines("plugins/" + p.name + "/" + filename + ".config");
                    foreach (string l in lines)
                        config.Add(l);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
        /// <summary>
        /// Add item to settings file
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="defualtvalue">The defualt value</param>
        public void AddItem(string name, object defualtvalue)
        {
            if (c == ConfigType.Setting)
                config.Add(name + "=" + defualtvalue.ToString());
        }
        /// <summary>
        /// Save a item to your save file
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <param name="value">value</param>
        public void SaveItem(string name, string value)
        {
            if (c == ConfigType.SaveFile)
                saves.Add(name, value);
        }
        /// <summary>
        /// Load a item from your save file or config file
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <returns>The object (returns null if non is found)</returns>
        public object GetItem(string name)
        {
            if (c == ConfigType.Setting)
            {
                foreach (string l in config)
                {
                    if (!l.StartsWith("#"))
                    {
                        if (l == name)
                            return l.Split('=')[1];
                    }
                }
            }
            if (c == ConfigType.SaveFile)
                return saves[name];
            return null;
        }
        /// <summary>
        /// Change an item in your save file or config file
        /// </summary>
        /// <param name="name">The name of the item to change</param>
        /// <param name="value">The new value</param>
        public void ChangeItem(string name, object value)
        {
            if (c == ConfigType.Setting)
            {
                foreach (string l in config)
                {
                    if (!l.StartsWith("#"))
                    {
                        if (l == name)
                            config[config.IndexOf(l)] = name + "=" + value.ToString();
                    }
                }
            }
            if (c == ConfigType.SaveFile)
                saves[name] = value.ToString();
        }
        /// <summary>
        /// Write the config or save file to the disk
        /// </summary>
        public void SaveConfig()
        {
            if (c == ConfigType.Setting)
                    File.WriteAllLines("plugins/" + p.name + "/" + filename + ".config", config.ToArray());
            if (c == ConfigType.SaveFile)
            {
                foreach (object value in saves.Values)
                {
                    //TODO
                    //Save objects
                }
            }
        }
    }
}
