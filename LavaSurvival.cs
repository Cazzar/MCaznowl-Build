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
        public Level map;
        public MapSettings mapSettings;

        // Constructors
        public LavaSurvival()
        {
            maps = new List<string>();
        }

        // Private methods
        private string ConcatBlocks(List<BlockPos> blocks)
        {
            string str = "";
            try
            {
                foreach (BlockPos bp in blocks)
                    str += bp.b.ToString() + "," + bp.x.ToString() + "," + bp.y.ToString() + "," + bp.z.ToString() + " ";
            }
            catch { }
            return str.Trim();
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

        }
        public void SaveSettings()
        {

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
                            case "blocks":
                                try
                                {
                                    string[] blocks = value.Split(' ');
                                    foreach (string pos in blocks)
                                    {
                                        try { settings.blocks.Add(new BlockPos(Convert.ToByte(pos.Split(',')[0]), Convert.ToUInt16(pos.Split(',')[1]), Convert.ToUInt16(pos.Split(',')[2]), Convert.ToUInt16(pos.Split(',')[3]))); }
                                        catch { }
                                    }
                                }
                                catch { }
                                break;
                            case "layerblocks":
                                try
                                {
                                    string[] blocks = value.Split(' ');
                                    foreach (string pos in blocks)
                                    {
                                        try { settings.layerBlocks.Add(new BlockPos(Convert.ToByte(pos.Split(',')[0]), Convert.ToUInt16(pos.Split(',')[1]), Convert.ToUInt16(pos.Split(',')[2]), Convert.ToUInt16(pos.Split(',')[3]))); }
                                        catch { }
                                    }
                                }
                                catch { }
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
            if (!Directory.Exists(propsPath)) Directory.CreateDirectory(propsPath);

            File.Create(propsPath + settings.name + ".properties").Dispose();
            using (StreamWriter SW = File.CreateText(propsPath + settings.name + ".properties"))
            {
                SW.WriteLine("#Lava Survival properties for " + settings.name);
                SW.WriteLine("Blocks = " + ConcatBlocks(settings.blocks));
                SW.WriteLine("LayerBlocks = " + ConcatBlocks(settings.layerBlocks));
            }
        }

        public void AddMap(string name)
        {
            if (!maps.Contains(name.ToLower())) maps.Add(name.ToLower());
        }
        public void RemoveMap(string name)
        {
            if (maps.Contains(name.ToLower())) maps.Remove(name.ToLower());
        }
        public bool HasMap(string name)
        {
            return maps.Contains(name.ToLower());
        }

        // Internal classes
        public class MapSettings
        {
            public string name;
            public List<BlockPos> blocks;
            public List<BlockPos> layerBlocks;

            public MapSettings(string name)
            {
                this.name = name;
                blocks = new List<BlockPos>();
                layerBlocks = new List<BlockPos>();
            }
        }

        public struct BlockPos
        {
            public byte b;
            public ushort x, y, z;

            public BlockPos(byte b, ushort x, ushort y, ushort z)
            {
                this.b = b;
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
    }
}
