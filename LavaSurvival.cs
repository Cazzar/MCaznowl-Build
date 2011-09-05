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
        private string propsPath = "properties/lavasurvival/";
        private List<string> maps;

        // Public variables
        public bool active = false;
        public bool configMode = false;
        public Level map;
        public MapSettings mapSettings;

        // Settings


        // Constructors
        public LavaSurvival()
        {
            maps = new List<string>();
        }

        // Private methods
        private string ConcatStrings(List<string> list, string separator)
        {
            string str = "";
            try
            {
                foreach (string s in list)
                    str += separator + s;
                str = str.Remove(0, 1);
            }
            catch { }
            return str;
        }

        private decimal NumberClamp(decimal value, decimal low, decimal high)
        {
            return Math.Max(Math.Min(value, high), low);
        }

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
            if (!File.Exists("properties/lavasurvival.properties"))
            {
                SaveSettings();
                return;
            }

            foreach (string line in File.ReadAllLines("properties/lavasurvival.properties"))
            {
                try
                {
                    if (line[0] != '#')
                    {
                        string value = line.Substring(line.IndexOf(" = ") + 3);
                        switch (line.Substring(0, line.IndexOf(" = ")).ToLower())
                        {
                            case "maps":
                                foreach (string mapname in value.Split(','))
                                    if(maps.Contains(mapname)) maps.Add(mapname);
                                break;
                        }
                    }
                }
                catch (Exception e) { Server.ErrorLog(e); }
            }
        }
        public void SaveSettings()
        {
            File.Create("properties/lavasurvival.properties").Dispose();
            using (StreamWriter SW = File.CreateText("properties/lavasurvival.properties"))
            {
                SW.WriteLine("#Lava Survival main properties");
                SW.WriteLine("maps = " + ConcatStrings(maps, ","));
            }
        }

        public MapSettings LoadMapSettings(string name)
        {
            MapSettings settings = new MapSettings(name);
            if (!Directory.Exists(propsPath)) Directory.CreateDirectory(propsPath);
            if (!File.Exists(propsPath + name + ".properties"))
            {
                SaveMapSettings(settings);
                return settings;
            }

            foreach (string line in File.ReadAllLines(propsPath + name + ".properties"))
            {
                try
                {
                    if (line[0] != '#')
                    {
                        string value = line.Substring(line.IndexOf(" = ") + 3);
                        switch (line.Substring(0, line.IndexOf(" = ")).ToLower())
                        {
                            case "fast-chance":
                                settings.fast = (byte)NumberClamp(Convert.ToDecimal(value), 0, 100);
                                break;
                            case "killer-chance":
                                settings.killer = (byte)NumberClamp(Convert.ToDecimal(value), 0, 100);
                                break;
                            case "destroy-chance":
                                settings.destroy = (byte)NumberClamp(Convert.ToDecimal(value), 0, 100);
                                break;
                            case "water-chance":
                                settings.water = (byte)NumberClamp(Convert.ToDecimal(value), 0, 100);
                                break;
                            case "layer-chance":
                                settings.layer = (byte)NumberClamp(Convert.ToDecimal(value), 0, 100);
                                break;
                            case "layer-height":
                                settings.layerHeight = Convert.ToInt32(value);
                                break;
                            case "layer-count":
                                settings.layerCount = Convert.ToInt32(value);
                                break;
                            case "layer-interval":
                                settings.layerInterval = Convert.ToDouble(value);
                                break;
                            case "round-time":
                                settings.roundTime = Convert.ToDouble(value);
                                break;
                            case "flood-time":
                                settings.floodTime = Convert.ToDouble(value);
                                break;
                            case "block-flood":
                                settings.blockFlood = new Pos(Convert.ToUInt16(value.Split(',')[0]), Convert.ToUInt16(value.Split(',')[1]), Convert.ToUInt16(value.Split(',')[2]));
                                break;
                            case "block-layer":
                                settings.blockLayer = new Pos(Convert.ToUInt16(value.Split(',')[0]), Convert.ToUInt16(value.Split(',')[1]), Convert.ToUInt16(value.Split(',')[2]));
                                break;
                        }
                    }
                }
                catch { }
            }
            return settings;
        }
        public void SaveMapSettings(MapSettings settings)
        {
            if (!Directory.Exists(propsPath)) Directory.CreateDirectory(propsPath);

            File.Create(propsPath + settings.name + ".properties").Dispose();
            using (StreamWriter SW = File.CreateText(propsPath + settings.name + ".properties"))
            {
                SW.WriteLine("#Lava Survival properties for " + settings.name);
                SW.WriteLine("fast-chance = " + settings.fast);
                SW.WriteLine("killer-chance = " + settings.killer);
                SW.WriteLine("destroy-chance = " + settings.destroy);
                SW.WriteLine("water-chance = " + settings.water);
                SW.WriteLine("layer-chance = " + settings.layer);
                SW.WriteLine("layer-height = " + settings.layerHeight);
                SW.WriteLine("layer-count = " + settings.layerCount);
                SW.WriteLine("layer-interval = " + settings.layerInterval);
                SW.WriteLine("round-time = " + settings.roundTime);
                SW.WriteLine("flood-time = " + settings.floodTime);
                SW.WriteLine("block-flood = " + settings.blockFlood.x + "," + settings.blockFlood.y + "," + settings.blockFlood.z);
                SW.WriteLine("block-layer = " + settings.blockLayer.x + "," + settings.blockLayer.y + "," + settings.blockLayer.z);
            }
        }

        public void AddMap(string name)
        {
            if (!maps.Contains(name.ToLower()))
            {
                maps.Add(name.ToLower());
                SaveSettings();
            }
        }
        public void RemoveMap(string name)
        {
            if (maps.Contains(name.ToLower()))
            {
                maps.Remove(name.ToLower());
                SaveSettings();
            }
        }
        public bool HasMap(string name)
        {
            return maps.Contains(name.ToLower());
        }

        // Internal classes
        public class MapSettings
        {
            public string name;
            public byte fast;
            public byte killer;
            public byte destroy;
            public byte water;
            public byte layer;
            public int layerHeight;
            public int layerCount;
            public double layerInterval;
            public double roundTime;
            public double floodTime;
            public Pos blockFlood;
            public Pos blockLayer;

            public MapSettings(string name)
            {
                this.name = name;
                fast = 0;
                killer = 0;
                destroy = 0;
                water = 0;
                layer = 0;
                layerHeight = 3;
                layerCount = 10;
                layerInterval = 2;
                roundTime = 30;
                floodTime = 10;
                blockFlood = new Pos(0, 0, 0);
                blockLayer = new Pos(0, 0, 0);
            }
        }

        public class MapData
        {

        }

        public struct Pos
        {
            public ushort x, y, z;

            public Pos(ushort x, ushort y, ushort z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
    }
}
