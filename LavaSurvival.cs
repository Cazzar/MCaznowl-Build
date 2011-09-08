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
    public class LavaSurvival
    {
        // Private variables
        private string propsPath = "properties/lavasurvival/";
        private List<string> maps, voted;
        private Dictionary<string, int> votes;
        private Random rand = new Random();
        private Timer announceTimer, voteTimer, transferTimer;
        private DateTime startTime;

        // Public variables
        public bool active = false, roundActive = false, flooded = false, voteActive = false;
        public Level map;
        public MapSettings mapSettings;
        public MapData mapData;

        // Settings
        public bool startOnStartup, sendAfkMain;
        public byte voteCount;
        public double voteTime;
        public LevelPermission setupRank;

        // Constructors
        public LavaSurvival()
        {
            maps = new List<string>();
            voted = new List<string>();
            votes = new Dictionary<string, int>();
            announceTimer = new Timer(60000);
            announceTimer.AutoReset = true;
            announceTimer.Elapsed += new ElapsedEventHandler(delegate
            {
                AnnounceTimeLeft(true, false);
            });

            startOnStartup = false;
            sendAfkMain = true;
            voteCount = 2;
            voteTime = 2;
            setupRank = LevelPermission.Operator;
            LoadSettings();
        }

        // Private methods
        private decimal NumberClamp(decimal value, decimal low, decimal high)
        {
            return Math.Max(Math.Min(value, high), low);
        }

        private void LevelCommand(string name, string msg = "")
        {
            Command cmd = Command.all.Find(name.Trim());
            if (cmd != null && map != null)
                try { cmd.Use(null, map.name + " " + msg.Trim()); }
                catch (Exception e) { Server.ErrorLog(e); }
        }

        // Public methods
        public byte Start(string mapName = "")
        {
            if (active) return 1; // Already started
            if (maps.Count < 3) return 2; // Not enough maps
            if (!String.IsNullOrEmpty(mapName) && !HasMap(mapName)) return 3; // Map doesn't exist

            active = true;
            try { LoadMap(String.IsNullOrEmpty(mapName) ? maps[rand.Next(maps.Count)] : mapName); }
            catch (Exception e) { Server.ErrorLog(e); }
            return 0;
        }
        public byte Stop()
        {
            if (!active) return 1; // Not started

            active = false;
            roundActive = false;
            voteActive = false;
            if (announceTimer.Enabled) announceTimer.Stop();
            try { mapData.Dispose(); }
            catch (Exception e) { Server.ErrorLog(e); }
            try { voteTimer.Dispose(); }
            catch (Exception e) { Server.ErrorLog(e); }
            try { transferTimer.Dispose(); }
            catch (Exception e) { Server.ErrorLog(e); }
            LevelCommand("unload");
            return 0;
        }

        public void StartRound()
        {
            if (roundActive) return;

            try
            {
                mapData.roundTimer.Elapsed += new ElapsedEventHandler(delegate { EndRound(); });
                mapData.floodTimer.Elapsed += new ElapsedEventHandler(delegate { DoFlood(); });
                mapData.roundTimer.Start();
                mapData.floodTimer.Start();
                announceTimer.Start();
                startTime = DateTime.Now;
                roundActive = true;
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public void EndRound()
        {
            if (!roundActive) return;

            roundActive = false;
            flooded = false;
            try
            {
                mapData.Dispose();
                map.ChatLevel("The round has ended!");
                StartVote();
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public void DoFlood()
        {
            if (!active || !roundActive || flooded || map == null) return;
            flooded = true;

            try
            {
                announceTimer.Stop();
                map.ChatLevel("&4Look out, here comes the flood!");
                if (mapData.layer)
                {
                    DoFloodLayer();
                    mapData.layerTimer.Elapsed += new ElapsedEventHandler(delegate
                    {
                        if (mapData.currentLayer <= mapSettings.layerCount)
                        {
                            DoFloodLayer();
                        }
                        else
                            mapData.layerTimer.Stop();
                    });
                    mapData.layerTimer.Start();
                }
                else
                    map.Blockchange((ushort)mapSettings.blockLayer.x, (ushort)mapSettings.blockLayer.y, (ushort)mapSettings.blockLayer.z, mapData.block, true);
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public void DoFloodLayer()
        {
            map.ChatLevel("&4Layer " + mapData.currentLayer + " flooding...");
            map.Blockchange((ushort)mapSettings.blockLayer.x, (ushort)(mapSettings.blockLayer.y + ((mapSettings.layerHeight * mapData.currentLayer) - 1)), (ushort)mapSettings.blockLayer.z, mapData.block, true);
            mapData.currentLayer++;
        }

        public void AnnounceTimeLeft(bool flood, bool round)
        {
            if (!active || !roundActive || startTime == null || map == null) return;

            if (flood)
            {
                double floodMinutes = Math.Round((startTime.AddMinutes(mapSettings.floodTime) - DateTime.Now).TotalMinutes, 0, MidpointRounding.AwayFromZero);
                map.ChatLevel("&3" + floodMinutes + " minute" + (floodMinutes == 1 ? "" : "s") + Server.DefaultColor + " until the flood.");
            }
            if (round)
            {
                double roundMinutes = Math.Round((startTime.AddMinutes(mapSettings.roundTime) - DateTime.Now).TotalMinutes, 0, MidpointRounding.AwayFromZero);
                map.ChatLevel("&3" + roundMinutes + " minute" + (roundMinutes == 1 ? "" : "s") + Server.DefaultColor + " until the round ends.");
            }
        }

        public void AnnounceRoundInfo()
        {
            if (mapData.water) map.ChatLevel("The map will be flooded with &9water " + Server.DefaultColor + "this round!");
            if (mapData.layer)
            {
                map.ChatLevel("The " + (mapData.water ? "water" : "lava") + " will &aflood in layers " + Server.DefaultColor + "this round!");
                map.ChatLevelOps("There will be " + mapSettings.layerCount + " layers, each " + mapSettings.layerHeight + " blocks high.");
                map.ChatLevelOps("There will be another layer every " + mapSettings.layerInterval + " minutes.");
            }
            if (mapData.fast) map.ChatLevel("The lava will be &cfast " + Server.DefaultColor + "this round!");
            if (mapData.killer) map.ChatLevel("The " + (mapData.water ? "water" : "lava") + " will &ckill you " + Server.DefaultColor + "this round!");
            if (mapData.destroy) map.ChatLevel("The " + (mapData.water ? "water" : "lava") + " will &cdestroy plants " + (mapData.water ? "" : "and flammable blocks ") + Server.DefaultColor + "this round!");
        }

        public void LoadMap(string name)
        {
            if (String.IsNullOrEmpty(name) || !HasMap(name)) return;

            name = name.ToLower();
            Level oldMap = null;
            if (active && map != null) oldMap = map;
            Command.all.Find("load").Use(null, name);
            map = Level.Find(name);

            if (map != null)
            {
                mapSettings = LoadMapSettings(name);
                mapData = GenerateMapData(mapSettings);

                map.setPhysics(mapData.destroy ? 2 : 1);
                map.motd = "Lava Survival: " + map.name.Capitalize();
                map.overload = 1000000;
                map.unload = false;
                map.loadOnGoto = false;
                Level.SaveSettings(map);
            }
            
            if (active && map != null)
            {
                try
                {
                    Player.players.ForEach(delegate(Player pl)
                    {
                        if (pl.level == oldMap)
                        {
                            if (sendAfkMain && Server.afkset.Contains(pl.name)) Command.all.Find("main").Use(pl, "");
                            else Command.all.Find("goto").Use(pl, map.name);
                            while (pl.Loading) System.Threading.Thread.Sleep(250); // Sleep for a bit while they load...
                        }
                    });
                    Command.all.Find("unload").Use(null, oldMap.name);
                }
                catch { }

                StartRound();
            }
        }

        public void StartVote()
        {
            if (maps.Count < 3) return;

            byte i = 0;
            string opt, str = "";
            while (i < Math.Min(voteCount, maps.Count - 1))
            {
                opt = maps[rand.Next(maps.Count)];
                if (!votes.ContainsKey(opt) && opt != map.name)
                {
                    votes.Add(opt, 0);
                    str += Server.DefaultColor + ", &5" + opt.Capitalize();
                    i++;
                }
            }

            map.ChatLevel("Vote for the next map! The vote ends in " + voteTime + " minutes.");
            map.ChatLevel("Choices: " + str.Remove(0, 4));

            voteTimer = new Timer(TimeSpan.FromMinutes(voteTime).TotalMilliseconds);
            voteTimer.AutoReset = false;
            voteTimer.Elapsed += new ElapsedEventHandler(delegate
            {
                try
                {
                    EndVote();
                    voteTimer.Dispose();
                }
                catch (Exception e) { Server.ErrorLog(e); }
            });
            voteTimer.Start();
            voteActive = true;
        }

        public void EndVote()
        {
            voteActive = false;
            KeyValuePair<string, int> most = new KeyValuePair<string, int>("", -1);
            foreach (KeyValuePair<string, int> kvp in votes)
            {
                if (kvp.Value > most.Value) most = kvp;
                map.ChatLevelOps("&5" + kvp.Key.Capitalize() + "&f: &a" + kvp.Value);
            }
            votes.Clear();
            voted.Clear();

            map.ChatLevel("The vote has ended! &5" + most.Key.Capitalize() + Server.DefaultColor + " won with &a" + most.Value + Server.DefaultColor + " vote" + (most.Value == 1 ? "" : "s") + ".");
            map.ChatLevel("You will be transferred in 5 seconds...");
            transferTimer = new Timer(5000);
            transferTimer.AutoReset = false;
            transferTimer.Elapsed += new ElapsedEventHandler(delegate
            {
                try
                {
                    LoadMap(most.Key);
                    transferTimer.Dispose();
                }
                catch (Exception e) { Server.ErrorLog(e); }
            });
            transferTimer.Start();
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

        public MapData GenerateMapData(MapSettings settings)
        {
            MapData data = new MapData(settings);
            data.killer = rand.Next(1, 101) <= settings.killer;
            data.destroy = rand.Next(1, 101) <= settings.destroy;
            data.water = rand.Next(1, 101) <= settings.water;
            data.layer = rand.Next(1, 101) <= settings.layer;
            data.fast = rand.Next(1, 101) <= settings.fast && !data.killer && !data.water;
            data.block = data.water ? (data.killer ? Block.activedeathwater : Block.water) : (data.fast ? Block.lava_fast : (data.killer ? Block.activedeathlava : Block.lava));
            return data;
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
                            case "send-afk-to-main":
                                sendAfkMain = bool.Parse(value);
                                break;
                            case "vote-count":
                                voteCount = (byte)NumberClamp(decimal.Parse(value), 2, 10);
                                break;
                            case "vote-time":
                                voteTime = double.Parse(value);
                                break;
                            case "setup-rank":
                                setupRank = Level.PermissionFromName(value.ToLower());
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
        }
        public void SaveSettings()
        {
            File.Create("properties/lavasurvival.properties").Dispose();
            using (StreamWriter SW = File.CreateText("properties/lavasurvival.properties"))
            {
                SW.WriteLine("#Lava Survival main properties");
                SW.WriteLine("start-on-startup = " + startOnStartup.ToString().ToLower());
                SW.WriteLine("send-afk-to-main = " + sendAfkMain.ToString().ToLower());
                SW.WriteLine("vote-count = " + voteCount.ToString());
                SW.WriteLine("vote-time = " + voteTime.ToString());
                SW.WriteLine("setup-rank = " + Level.PermissionToName(setupRank).ToLower());
                SW.WriteLine("maps = " + maps.Concatenate(","));
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
            public byte block;
            public int currentLayer;
            public Timer roundTimer, floodTimer, layerTimer;

            public MapData(MapSettings settings)
            {
                fast = false;
                killer = false;
                destroy = false;
                water = false;
                layer = false;
                block = Block.lava;
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
