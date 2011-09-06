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
        private List<string> maps, voted;
        private Dictionary<string, int> votes;
        private Timer voteTimer;

        // Public variables
        public bool active = false, voteActive = false;
        public Level map;
        public MapSettings mapSettings;
        public MapData mapData;

        // Settings
        public bool startOnStartup;
        public byte voteCount;
        public double voteTime;

        // Constructors
        public LavaSurvival()
        {
            maps = new List<string>();
            voted = new List<string>();
            votes = new Dictionary<string, int>();

            startOnStartup = false;
            voteCount = 2;
            voteTime = 2;
            LoadSettings();
        }

        // Private methods
        private string ConcatStrings(List<string> list, string separator)
        {
            string str = "";
            try
            {
                foreach (string s in list)
                    str += separator + s;
                str = str.Remove(0, separator.Length);
            }
            catch { }
            return str;
        }

        private decimal NumberClamp(decimal value, decimal low, decimal high)
        {
            return Math.Max(Math.Min(value, high), low);
        }

        // Public methods
        public byte Start()
        {
            if (active) return 1;
            if (maps.Count < 1) return 2;

            return 0;
        }
        public byte Stop()
        {
            if (!active) return 1;

            return 0;
        }

        public void LoadMap(string name)
        {
            if (!HasMap(name)) return;
            if (map != null) map.Unload();
        }

        public void StartVote()
        {
            if (maps.Count < 2) return;

            byte i = 0;
            string opt, str = "";
            Random rand = new Random();
            while (i < Math.Min(voteCount, maps.Count))
            {
                opt = maps[rand.Next(maps.Count)];
                if (!votes.ContainsKey(opt))
                {
                    votes.Add(opt, 0);
                    i++;
                }
            }

            foreach (KeyValuePair<string, int> kvp in votes)
                str += Server.DefaultColor + ", &5" + Extensions.Capitalize(kvp.Key);

            voteActive = true;
            map.ChatLevel("Vote for the next map! The vote ends in " + voteTime + " minutes.");
            map.ChatLevel("Choices: " + str.Remove(0, 4));
        }

        public void EndVote()
        {
            voteActive = false;
            KeyValuePair<string, int> most = new KeyValuePair<string, int>();
            foreach (KeyValuePair<string, int> kvp in votes)
            {
                if (kvp.Value > most.Value) most = kvp;
                map.ChatLevelOps("&5" + Extensions.Capitalize(kvp.Key) + "&f: &a" + kvp.Value);
            }

            map.ChatLevel("Vote ended! &5" + Extensions.Capitalize(most.Key) + Server.DefaultColor + " won with &a" + most.Value + Server.DefaultColor + " votes.");
            map.ChatLevel("You will be transferred in 5 seconds...");
            Timer timer = new Timer(5000);
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(delegate
            {
                Command.all.Find("load").Use(null, most.Key);
                Player.players.ForEach(delegate(Player pl)
                {
                    if (HasPlayer(pl))
                    {
                        if (Server.afkset.Contains(pl.name)) Command.all.Find("main").Use(pl, "");
                        else Command.all.Find("goto").Use(pl, most.Key);
                    }
                });
                Command.all.Find("unload").Use(null, map.name);
                map = Level.Find(most.Key);
                timer.Dispose();
            });
            timer.Start();
        }

        public bool AddVote(Player p, string vote)
        {
            if (!voteActive || voted.Contains(p.name) || !votes.ContainsKey(vote)) return false;
            int temp = votes[vote] + 1;
            votes.Remove(vote);
            votes.Add(vote, temp);
            voted.Add(p.name);
            return true;
        }

        public bool HasVote(string vote)
        {
            return voteActive && votes.ContainsKey(vote);
        }

        public bool HasPlayer(Player p)
        {
            return p.level == map;
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
                            case "start-on-startup":
                                startOnStartup = bool.Parse(value);
                                break;
                            case "vote-count":
                                voteCount = (byte)NumberClamp(decimal.Parse(value), 2, 10);
                                break;
                            case "vote-time":
                                voteTime = int.Parse(value);
                                break;
                            case "maps":
                                foreach (string mapname in value.Split(','))
                                    if(!maps.Contains(mapname)) maps.Add(mapname);
                                break;
                        }
                    }
                }
                catch (Exception e) { Server.ErrorLog(e); }
            }
            SaveSettings();
        }
        public void SaveSettings()
        {
            File.Create("properties/lavasurvival.properties").Dispose();
            using (StreamWriter SW = File.CreateText("properties/lavasurvival.properties"))
            {
                SW.WriteLine("#Lava Survival main properties");
                SW.WriteLine("start-on-startup = " + startOnStartup.ToString().ToLower());
                SW.WriteLine("vote-count = " + voteCount);
                SW.WriteLine("vote-time = " + voteTime);
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
                                settings.fast = (byte)NumberClamp(decimal.Parse(value), 0, 100);
                                break;
                            case "killer-chance":
                                settings.killer = (byte)NumberClamp(decimal.Parse(value), 0, 100);
                                break;
                            case "destroy-chance":
                                settings.destroy = (byte)NumberClamp(decimal.Parse(value), 0, 100);
                                break;
                            case "water-chance":
                                settings.water = (byte)NumberClamp(decimal.Parse(value), 0, 100);
                                break;
                            case "layer-chance":
                                settings.layer = (byte)NumberClamp(decimal.Parse(value), 0, 100);
                                break;
                            case "layer-height":
                                settings.layerHeight = int.Parse(value);
                                break;
                            case "layer-count":
                                settings.layerCount = int.Parse(value);
                                break;
                            case "layer-interval":
                                settings.layerInterval = double.Parse(value);
                                break;
                            case "round-time":
                                settings.roundTime = double.Parse(value);
                                break;
                            case "flood-time":
                                settings.floodTime = double.Parse(value);
                                break;
                            case "block-flood":
                                settings.blockFlood = new Pos(ushort.Parse(value.Split(',')[0]), ushort.Parse(value.Split(',')[1]), ushort.Parse(value.Split(',')[2]));
                                break;
                            case "block-layer":
                                settings.blockLayer = new Pos(ushort.Parse(value.Split(',')[0]), ushort.Parse(value.Split(',')[1]), ushort.Parse(value.Split(',')[2]));
                                break;
                        }
                    }
                }
                catch (Exception e) { Server.ErrorLog(e); }
            }
            SaveMapSettings(settings);
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
            public byte fast, killer, destroy, water, layer;
            public int layerHeight, layerCount;
            public double layerInterval, roundTime, floodTime;
            public Pos blockFlood, blockLayer;

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

        public class MapData : IDisposable
        {
            public bool fast, killer, destroy, water, layer;
            public int currentLayer;
            public Timer roundTimer, floodTimer, layerTimer;

            public MapData(MapSettings settings)
            {
                fast = false;
                killer = false;
                destroy = false;
                water = false;
                layer = false;
                currentLayer = 1;
                roundTimer = new Timer(TimeSpan.FromMinutes(settings.roundTime).TotalMilliseconds); roundTimer.AutoReset = false;
                floodTimer = new Timer(TimeSpan.FromMinutes(settings.floodTime).TotalMilliseconds); floodTimer.AutoReset = false;
                layerTimer = new Timer(TimeSpan.FromMinutes(settings.layerInterval).TotalMilliseconds); layerTimer.AutoReset = true;
            }

            public void Dispose()
            {
                roundTimer.Dispose();
                floodTimer.Dispose();
                layerTimer.Dispose();
            }
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
