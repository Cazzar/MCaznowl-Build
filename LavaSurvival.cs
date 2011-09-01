/*
	Copyright 2011 MCForge
		
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
using System.Collections.Generic;
using System.Timers;

namespace MCForge
{
    class LavaSurvival
    {
        // Private variables
        private List<string> maps;

        // Public variables
        public bool active = false;
        public Level currentMap;
        public MapSettings mapSettings;

        // Constructors
        public LavaSurvival()
        {
            maps = new List<string>();
        }

        // Private methods


        // Public methods
        public bool Start()
        {
            return false;
        }
        public bool Stop()
        {
            return false;
        }

        public void LoadSettings()
        {

        }
        public void SaveSettings()
        {

        }

        public MapSettings LoadMapSettings(string name)
        {
            MapSettings settings = new MapSettings(name);
            if (!Directory.Exists("properties/lava")) Directory.CreateDirectory("properties/lava");
            if (!File.Exists("properties/lava/" + name + ".properties"))
            {
                SaveMapSettings(settings);
                return settings;
            }

            foreach (string line in File.ReadAllLines("properties/lava/" + name + ".properties"))
            {
                try
                {
                    if (line[0] != '#')
                    {
                        string value = line.Substring(line.IndexOf(" = ") + 3);

                        switch (line.Substring(0, line.IndexOf(" = ")).ToLower())
                        {
                            // THIS IS PLACEHOLDER CODE!!!!!
                            case "property":
                                break;
                        }
                    }
                }
                catch (Exception e) { Server.ErrorLog(e); }
            }
            return settings;
        }
        public void SaveMapSettings(MapSettings settings)
        {
            if (!Directory.Exists("properties/lava")) Directory.CreateDirectory("properties/lava");
            using (StreamWriter SW = File.CreateText("levels/level properties/" + level.name + ".properties"))
            {
                SW.WriteLine("#Lava Survival properties for " + settings.name);
                SW.WriteLine("Property = " /* INSERT VARIABLE HERE */);
            }
        }

        public void AddMap(string name)
        {
            if(!maps.Contains(name)) maps.Add(name);
        }
        public void RemoveMap(string name)
        {
            if (maps.Contains(name)) maps.Remove(name);
        }
        public bool HasMap(string name)
        {
            return maps.Contains(name);
        }

        // Internal classes
        public class MapSettings
        {
            public string name;
            public List<Pos> blocks;

            public MapSettings(string name)
            {
                this.name = name;
                blocks = new List<Pos>();
            }
        }

        public struct Pos { public ushort x, y, z; }
    }
}
