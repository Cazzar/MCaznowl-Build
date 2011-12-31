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
using System.Text;
using System.Linq;
using System.Globalization;

namespace MCForge
{
    public class LavaSurvival : IDisposable
    {
        // Private variables
        private string propsPath = "properties/lavasurvival/";
        private List<string> maps, voted;
        private Dictionary<string, int> votes, deaths;
        private Random rand = new Random();
        private Timer announceTimer, voteTimer, transferTimer;
        private DateTime startTime;

        // Public variables
        private bool _sendingPlayers; 

        public bool sendingPlayers
        {
            get
            {
                return _sendingPlayers;
            }
            set
            {
                _sendingPlayers = value;
            }
        }
        private bool _voteActive; 

        public bool voteActive
        {
            get
            {
                return _voteActive;
            }
            set
            {
                _voteActive = value;
            }
        }
        private bool _flooded; 

        public bool flooded
        {
            get
            {
                return _flooded;
            }
            set
            {
                _flooded = value;
            }
        }
        private bool _roundActive; 

        public bool roundActive
        {
            get
            {
                return _roundActive;
            }
            set
            {
                _roundActive = value;
            }
        }
        private bool _active; 

        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }
        private Level _map; 

        public Level map
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
            }
        }
        private MapSettings _mapSettings; 

        public MapSettings mapSettings
        {
            get
            {
                return _mapSettings;
            }
            set
            {
                _mapSettings = value;
            }
        }
        private MapData _mapData; 

        public MapData mapData
        {
            get
            {
                return _mapData;
            }
            set
            {
                _mapData = value;
            }
        }

        // Settings
        private bool _sendAfkMain; 

        public bool sendAfkMain
        {
            get
            {
                return _sendAfkMain;
            }
            set
            {
                _sendAfkMain = value;
            }
        }
        private bool _startOnStartup; 

        public bool startOnStartup
        {
            get
            {
                return _startOnStartup;
            }
            set
            {
                _startOnStartup = value;
            }
        }
        private byte _voteCount; 

        public byte voteCount
        {
            get
            {
                return _voteCount;
            }
            set
            {
                _voteCount = value;
            }
        }
        private int _lifeNum; 

        public int lifeNum
        {
            get
            {
                return _lifeNum;
            }
            set
            {
                _lifeNum = value;
            }
        }
        private double _voteTime; 

        public double voteTime
        {
            get
            {
                return _voteTime;
            }
            set
            {
                _voteTime = value;
            }
        }
        private LevelPermission _controlRank; 

        public LevelPermission controlRank
        {
            get
            {
                return _controlRank;
            }
            set
            {
                _controlRank = value;
            }
        }
        private LevelPermission _setupRank; 

        public LevelPermission setupRank
        {
            get
            {
                return _setupRank;
            }
            set
            {
                _setupRank = value;
            }
        }

        // Plugin event delegates
        public delegate void GameStartHandler(Level map);
        public delegate void GameStopHandler();
        public delegate void MapChangeHandler(Level oldmap, Level newmap); // Keep in mind oldmap will be unloaded after this event finishes.
        public delegate void LavaFloodHandler(ushort x, ushort y, ushort z); // Only called on normal flood, not layer flood.
        public delegate void LayerFloodHandler(ushort x, ushort y, ushort z);
        public delegate void RoundStartHandler(Level map);
        public delegate void RoundEndHandler();
        public delegate void VoteStartHandler(string[] options);
        public delegate void VoteEndHandler(string winner);
        public delegate void PlayerDeathHandler(Player p); // Only called when the plaer is out of the round, not when they lose a life.

        // Plugin events
        public event GameStartHandler OnGameStart;
        public event GameStopHandler OnGameStop;
        public event MapChangeHandler OnMapChange;
        public event LavaFloodHandler OnLavaFlood;
        public event LayerFloodHandler OnLayerFlood;
        public event RoundStartHandler OnRoundStart;
        public event RoundEndHandler OnRoundEnd;
        public event VoteStartHandler OnVoteStart;
        public event VoteEndHandler OnVoteEnd;
        public event PlayerDeathHandler OnPlayerDeath;

        // Constructors
        public LavaSurvival()
        {
            maps = new List<string>();
            voted = new List<string>();
            votes = new Dictionary<string, int>();
            deaths = new Dictionary<string, int>();
            announceTimer = new Timer(60000);
            announceTimer.AutoReset = true;
            announceTimer.Elapsed += delegate
            {
                if (!flooded) AnnounceTimeLeft(true, false);
            };

            /*startOnStartup = false*/;
            sendAfkMain = true;
            voteCount = 2;
            voteTime = 2;
            lifeNum = 3;
            setupRank = LevelPermission.Admin;
            controlRank = LevelPermission.Operator;
            LoadSettings();
        }

        // Private methods
//  COMMENTED BY CODEIT.RIGHT
//        private void LevelCommand(string name, string msg = "")
//        {
//            Command cmd = Command.all.Find(name.Trim());
//            if (cmd != null && map != null)
//                try { cmd.Use(null, map.name + " " + msg.Trim()); }
//                catch (Exception e) { Server.ErrorLog(e); }
//        }

        // Public methods
        public byte Start(string mapName = "")
        {
            if (active) return 1; // Already started
            if (maps.Count < 3) return 2; // Not enough maps
            if (!String.IsNullOrEmpty(mapName) && !HasMap(mapName)) return 3; // Map doesn't exist

            deaths.Clear();
            active = true;
            Server.s.Log("[Lava Survival] Game started.");
            try { LoadMap(String.IsNullOrEmpty(mapName) ? maps[rand.Next(maps.Count)] : mapName); }
            catch (Exception e) { Server.ErrorLog(e); active = false; return 4; }
            if (OnGameStart != null)
                OnGameStart(map);
            return 0;
        }
        public byte Stop()
        {
            if (!active) return 1; // Not started

            if (OnGameStop != null)
                OnGameStop();
            active = false;
            roundActive = false;
            voteActive = false;
            flooded = false;
            deaths.Clear();
            if (announceTimer.Enabled) announceTimer.Stop();
            try { mapData.Dispose(); }
            catch { }
            try { voteTimer.Dispose(); }
            catch { }
            try { transferTimer.Dispose(); }
            catch { }
            map.Unload(true, false);
            map = null;
            Server.s.Log("[Lava Survival] Game stopped.");
            return 0;
        }

        public void StartRound()
        {
            if (roundActive) return;

            if (OnRoundStart != null)
                OnRoundStart(map);
            try
            {
                deaths.Clear();
                mapData.roundTimer.Elapsed += delegate { EndRound(); };
                mapData.floodTimer.Elapsed += delegate { DoFlood(); };
                mapData.roundTimer.Start();
                mapData.floodTimer.Start();
                announceTimer.Start();
                startTime = DateTime.Now;
                roundActive = true;
                Server.s.Log("[Lava Survival] Round started. Map: " + map.name);
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public void EndRound()
        {
            if (!roundActive) return;

            if (OnRoundEnd != null)
                OnRoundEnd();
            roundActive = false;
            flooded = false;
            try
            {
                try { mapData.Dispose(); }
                catch { }
                map.setPhysics(5);
                map.ChatLevel("The round has ended!");
                Server.s.Log("[Lava Survival] Round ended. Voting...");
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
                Server.s.Log("[Lava Survival] Map flooding.");
                if (mapData.layer)
                {
                    DoFloodLayer();
                    mapData.layerTimer.Elapsed += delegate
                    {
                        if (mapData.currentLayer <= mapSettings.layerCount)
                        {
                            DoFloodLayer();
                        }
                        else
                            mapData.layerTimer.Stop();
                    };
                    mapData.layerTimer.Start();
                }
                else
                {
                    map.Blockchange((ushort)mapSettings.blockFlood.x, (ushort)mapSettings.blockFlood.y, (ushort)mapSettings.blockFlood.z, mapData.block, true);
                    if (OnLavaFlood != null)
                        OnLavaFlood((ushort)mapSettings.blockFlood.x, (ushort)mapSettings.blockFlood.y, (ushort)mapSettings.blockFlood.z);
                }
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        public void DoFloodLayer()
        {
            map.ChatLevel("&4Layer " + mapData.currentLayer + " flooding...");
            Server.s.Log("[Lava Survival] Layer " + mapData.currentLayer + " flooding.");
            map.Blockchange((ushort)mapSettings.blockLayer.x, (ushort)(mapSettings.blockLayer.y + ((mapSettings.layerHeight * mapData.currentLayer) - 1)), (ushort)mapSettings.blockLayer.z, mapData.block, true);
            if (OnLayerFlood != null)
                OnLayerFlood((ushort)mapSettings.blockLayer.x, (ushort)(mapSettings.blockLayer.y + ((mapSettings.layerHeight * mapData.currentLayer) - 1)), (ushort)mapSettings.blockLayer.z);
            mapData.currentLayer++;
        }

        public void AnnounceTimeLeft(bool flood, bool round, Player p = null, bool console = false)
        {
            if (!active || !roundActive || startTime == null || map == null) return;

            if (flood)
            {
                double floodMinutes = Math.Ceiling((startTime.AddMinutes(mapSettings.floodTime) - DateTime.Now).TotalMinutes);
                if (p == null && !console) map.ChatLevel("&3" + floodMinutes + " minute" + (floodMinutes == 1 ? "" : "s") + Server.DefaultColor + " until the flood.");
                else Player.SendMessage(p, "&3" + floodMinutes + " minute" + (floodMinutes == 1 ? "" : "s") + Server.DefaultColor + " until the flood.");
            }
            if (round)
            {
                double roundMinutes = Math.Ceiling((startTime.AddMinutes(mapSettings.roundTime) - DateTime.Now).TotalMinutes);
                if (p == null && !console) map.ChatLevel("&3" + roundMinutes + " minute" + (roundMinutes == 1 ? "" : "s") + Server.DefaultColor + " until the round ends.");
                else Player.SendMessage(p, "&3" + roundMinutes + " minute" + (roundMinutes == 1 ? "" : "s") + Server.DefaultColor + " until the round ends.");
            }
        }

        public void AnnounceRoundInfo(Player p = null, bool console = false)
        {
            if (p == null && !console)
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
            else
            {
                if (mapData.water) Player.SendMessage(p, "The map will be flooded with &9water " + Server.DefaultColor + "this round!");
                if (mapData.layer) Player.SendMessage(p, "The " + (mapData.water ? "water" : "lava") + " will &aflood in layers " + Server.DefaultColor + "this round!");
                if (mapData.fast) Player.SendMessage(p, "The lava will be &cfast " + Server.DefaultColor + "this round!");
                if (mapData.killer) Player.SendMessage(p, "The " + (mapData.water ? "water" : "lava") + " will &ckill you " + Server.DefaultColor + "this round!");
                if (mapData.destroy) Player.SendMessage(p, "The " + (mapData.water ? "water" : "lava") + " will &cdestroy plants " + (mapData.water ? "" : "and flammable blocks ") + Server.DefaultColor + "this round!");
            }
        }

        public void LoadMap(string name)
        {
            if (String.IsNullOrEmpty(name) || !HasMap(name)) return;

            name = name.ToLower(CultureInfo.CurrentCulture);
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
                sendingPlayers = true;
                try
                {
                    Player.players.ForEach(delegate(Player pl)
                    {
                        if (pl.level == oldMap)
                        {
                            if (sendAfkMain && Server.afkset.Contains(pl.name)) Command.all.Find("main").Use(pl, "");
                            else Command.all.Find("goto").Use(pl, map.name);
                        }
                    });
                    if (OnMapChange != null)
                        OnMapChange(oldMap, map);
                    oldMap.Unload(true, false);
                }
                catch { }
                sendingPlayers = false;

                StartRound();
            }
        }

        public void StartVote()
        {
            if (maps.Count < 3) return;

            // Make sure these are cleared or bad stuff happens!
            votes.Clear();
            voted.Clear();

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

            if (OnVoteStart != null)
                OnVoteStart(votes.Keys.ToList().ToArray());
            map.ChatLevel("Vote for the next map! The vote ends in " + voteTime + " minute" + (voteTime == 1 ? "" : "s") +".");
            map.ChatLevel("Choices: " + str.Remove(0, 4));

            voteTimer = new Timer(TimeSpan.FromMinutes(voteTime).TotalMilliseconds);
            voteTimer.AutoReset = false;
            voteTimer.Elapsed += delegate
            {
                try
                {
                    EndVote();
                    voteTimer.Dispose();
                }
                catch (Exception e) { Server.ErrorLog(e); }
            };
            voteTimer.Start();
            voteActive = true;
        }

        public void EndVote()
        {
            if (!voteActive) return;

            voteActive = false;
            Server.s.Log("[Lava Survival] Vote ended.");
            KeyValuePair<string, int> most = new KeyValuePair<string, int>(String.Empty, -1);
            foreach (KeyValuePair<string, int> kvp in votes)
            {
                if (kvp.Value > most.Value) most = kvp;
                map.ChatLevelOps("&5" + kvp.Key.Capitalize() + "&f: &a" + kvp.Value);
            }
            votes.Clear();
            voted.Clear();

            if (OnVoteEnd != null)
                OnVoteEnd(most.Key);
            map.ChatLevel("The vote has ended! &5" + most.Key.Capitalize() + Server.DefaultColor + " won with &a" + most.Value + Server.DefaultColor + " vote" + (most.Value == 1 ? "" : "s") + ".");
            map.ChatLevel("You will be transferred in 5 seconds...");
            transferTimer = new Timer(5000);
            transferTimer.AutoReset = false;
            transferTimer.Elapsed += delegate
            {
                try
                {
                    LoadMap(most.Key);
                    transferTimer.Dispose();
                }
                catch (Exception e) { Server.ErrorLog(e); }
            };
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
        public void KillPlayer(Player p, bool silent = false)
        {
            if (lifeNum < 1) return;
            string name = p.name.ToLower(CultureInfo.CurrentCulture);
            if (!deaths.ContainsKey(name))
                deaths.Add(name, 0);
            deaths[name]++;
            if (!silent && IsPlayerDead(p))
            {
                if (OnPlayerDeath != null)
                    OnPlayerDeath(p);
                Player.players.ForEach(delegate(Player pl)
                {
                    if (pl != p && HasPlayer(pl))
                        Player.SendMessage(pl, p.color + p.name + " &4ran out of lives, and is out of the round!");
                });
                Player.SendMessage(p, "&4You ran out of lives, and are out of the round!");
                Player.SendMessage(p, "&4You can still watch, but you cannot build.");
            }
        }
        public bool IsPlayerDead(Player p)
        {
            string name = p.name.ToLower(CultureInfo.CurrentCulture);
            if (lifeNum < 1 || !deaths.ContainsKey(name))
                return false;
            return (deaths[name] >= lifeNum);
        }

        public MapData GenerateMapData(MapSettings settings)
        {
            MapData data = new MapData(settings);
            data.killer = rand.Next(1, 101) <= settings.killer;
            data.destroy = rand.Next(1, 101) <= settings.destroy;
            data.water = rand.Next(1, 101) <= settings.water;
            data.layer = rand.Next(1, 101) <= settings.layer;
            data.fast = rand.Next(1, 101) <= settings.fast && !data.water;
            data.block = data.water ? (data.killer ? Block.activedeathwater : Block.water) : (data.fast ? (data.killer ? Block.fastdeathlava : Block.lava_fast) : (data.killer ? Block.activedeathlava : Block.lava));
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
                        string value = line.Substring(line.IndexOf(" = ", StringComparison.CurrentCulture) + 3);
                        switch (line.Substring(0, line.IndexOf(" = ", StringComparison.CurrentCulture)).ToLower(CultureInfo.CurrentCulture))
                        {
                            case "start-on-startup":
                                startOnStartup = bool.Parse(value);
                                break;
                            case "send-afk-to-main":
                                sendAfkMain = bool.Parse(value);
                                break;
                            case "vote-count":
                                voteCount = (byte)MathHelper.Clamp(decimal.Parse(value, CultureInfo.CurrentCulture), 2, 10);
                                break;
                            case "vote-time":
                                voteTime = double.Parse(value, CultureInfo.CurrentCulture);
                                break;
                            case "lives":
                                lifeNum = int.Parse(value, CultureInfo.CurrentCulture);
                                break;
                            case "setup-rank":
                                if (Group.Find(value.ToLower(CultureInfo.CurrentCulture)) != null)
                                    setupRank = Group.Find(value.ToLower(CultureInfo.CurrentCulture)).Permission;
                                break;
                            case "control-rank":
                                if (Group.Find(value.ToLower(CultureInfo.CurrentCulture)) != null)
                                    controlRank = Group.Find(value.ToLower(CultureInfo.CurrentCulture)).Permission;
                                break;
                            case "maps":
                                foreach (string mapname in value.Split(','))
                                    if(!String.IsNullOrEmpty(mapname) && !maps.Contains(mapname)) maps.Add(mapname);
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
                SW.WriteLine("start-on-startup = " + startOnStartup.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
                SW.WriteLine("send-afk-to-main = " + sendAfkMain.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
                SW.WriteLine("vote-count = " + voteCount.ToString(CultureInfo.CurrentCulture));
                SW.WriteLine("vote-time = " + voteTime.ToString(CultureInfo.CurrentCulture));
                SW.WriteLine("lives = " + lifeNum.ToString(CultureInfo.CurrentCulture));
                SW.WriteLine("setup-rank = " + Level.PermissionToName(setupRank).ToLower(CultureInfo.CurrentCulture));
                SW.WriteLine("control-rank = " + Level.PermissionToName(controlRank).ToLower(CultureInfo.CurrentCulture));
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
                        string[] sp;
                        string value = line.Substring(line.IndexOf(" = ", StringComparison.CurrentCulture) + 3);
                        switch (line.Substring(0, line.IndexOf(" = ", StringComparison.CurrentCulture)).ToLower(CultureInfo.CurrentCulture))
                        {
                            case "fast-chance":
                                settings.fast = (byte)MathHelper.Clamp(decimal.Parse(value, CultureInfo.CurrentCulture), 0, 100);
                                break;
                            case "killer-chance":
                                settings.killer = (byte)MathHelper.Clamp(decimal.Parse(value, CultureInfo.CurrentCulture), 0, 100);
                                break;
                            case "destroy-chance":
                                settings.destroy = (byte)MathHelper.Clamp(decimal.Parse(value, CultureInfo.CurrentCulture), 0, 100);
                                break;
                            case "water-chance":
                                settings.water = (byte)MathHelper.Clamp(decimal.Parse(value, CultureInfo.CurrentCulture), 0, 100);
                                break;
                            case "layer-chance":
                                settings.layer = (byte)MathHelper.Clamp(decimal.Parse(value, CultureInfo.CurrentCulture), 0, 100);
                                break;
                            case "layer-height":
                                settings.layerHeight = int.Parse(value, CultureInfo.CurrentCulture);
                                break;
                            case "layer-count":
                                settings.layerCount = int.Parse(value, CultureInfo.CurrentCulture);
                                break;
                            case "layer-interval":
                                settings.layerInterval = double.Parse(value, CultureInfo.CurrentCulture);
                                break;
                            case "round-time":
                                settings.roundTime = double.Parse(value, CultureInfo.CurrentCulture);
                                break;
                            case "flood-time":
                                settings.floodTime = double.Parse(value, CultureInfo.CurrentCulture);
                                break;
                            case "block-flood":
                                sp = value.Split(',');
                                settings.blockFlood = new Pos(ushort.Parse(sp[0], CultureInfo.CurrentCulture), ushort.Parse(sp[1], CultureInfo.CurrentCulture), ushort.Parse(sp[2], CultureInfo.CurrentCulture));
                                break;
                            case "block-layer":
                                sp = value.Split(',');
                                settings.blockLayer = new Pos(ushort.Parse(sp[0], CultureInfo.CurrentCulture), ushort.Parse(sp[1], CultureInfo.CurrentCulture), ushort.Parse(sp[2], CultureInfo.CurrentCulture));
                                break;
                            case "safe-zone":
                                sp = value.Split('-');
                                string[] p1 = sp[0].Split(','), p2 = sp[1].Split(',');
                                settings.safeZone = new Pos[] { new Pos(ushort.Parse(p1[0], CultureInfo.CurrentCulture), ushort.Parse(p1[1], CultureInfo.CurrentCulture), ushort.Parse(p1[2], CultureInfo.CurrentCulture)), new Pos(ushort.Parse(p2[0], CultureInfo.CurrentCulture), ushort.Parse(p2[1], CultureInfo.CurrentCulture), ushort.Parse(p2[2], CultureInfo.CurrentCulture)) };
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
                SW.WriteLine("block-flood = " + settings.blockFlood.ToString());
                SW.WriteLine("block-layer = " + settings.blockLayer.ToString());
                SW.WriteLine(String.Format(CultureInfo.CurrentCulture, "safe-zone = {0}-{1}", settings.safeZone[0].ToString(), settings.safeZone[1].ToString()));
            }
        }

        public void AddMap(string name)
        {
            if (!String.IsNullOrEmpty(name) && !maps.Contains(name.ToLower(CultureInfo.CurrentCulture)))
            {
                maps.Add(name.ToLower(CultureInfo.CurrentCulture));
                SaveSettings();
            }
        }
        public void RemoveMap(string name)
        {
            if (maps.Contains(name.ToLower(CultureInfo.CurrentCulture)))
            {
                maps.Remove(name.ToLower(CultureInfo.CurrentCulture));
                SaveSettings();
            }
        }
        public bool HasMap(string name)
        {
            return maps.Contains(name.ToLower(CultureInfo.CurrentCulture));
        }

        public bool InSafeZone(Pos pos)
        {
            return InSafeZone(pos.x, pos.y, pos.z);
        }

        public bool InSafeZone(ushort x, ushort y, ushort z)
        {
            if (mapSettings == null) return false;
            if (x >= mapSettings.safeZone[0].x && x <= mapSettings.safeZone[1].x && y >= mapSettings.safeZone[0].y && y <= mapSettings.safeZone[1].y && z >= mapSettings.safeZone[0].z && z <= mapSettings.safeZone[1].z)
                return true;
            return false;
        }

        // Accessors
        public string VoteString
        {
            get
            {
                if (votes.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (KeyValuePair<string, int> kvp in votes)
                        sb.AppendFormat("{0}, &5{1}", Server.DefaultColor, kvp.Key.Capitalize());
                    sb.Remove(0, 4);
                    return sb.ToString();
                }
                return String.Empty;
            }
        }

        public List<string> Maps
        {
            get
            {
                return new List<string>(maps);
            }
        }

        // Internal classes
        public class MapSettings
        {
            private string _name; 

            public string name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
            private byte _layer; 

            public byte layer
            {
                get
                {
                    return _layer;
                }
                set
                {
                    _layer = value;
                }
            }
            private byte _water; 

            public byte water
            {
                get
                {
                    return _water;
                }
                set
                {
                    _water = value;
                }
            }
            private byte _destroy; 

            public byte destroy
            {
                get
                {
                    return _destroy;
                }
                set
                {
                    _destroy = value;
                }
            }
            private byte _killer; 

            public byte killer
            {
                get
                {
                    return _killer;
                }
                set
                {
                    _killer = value;
                }
            }
            private byte _fast; 

            public byte fast
            {
                get
                {
                    return _fast;
                }
                set
                {
                    _fast = value;
                }
            }
            private int _layerCount; 

            public int layerCount
            {
                get
                {
                    return _layerCount;
                }
                set
                {
                    _layerCount = value;
                }
            }
            private int _layerHeight; 

            public int layerHeight
            {
                get
                {
                    return _layerHeight;
                }
                set
                {
                    _layerHeight = value;
                }
            }
            private double _floodTime; 

            public double floodTime
            {
                get
                {
                    return _floodTime;
                }
                set
                {
                    _floodTime = value;
                }
            }
            private double _roundTime; 

            public double roundTime
            {
                get
                {
                    return _roundTime;
                }
                set
                {
                    _roundTime = value;
                }
            }
            private double _layerInterval; 

            public double layerInterval
            {
                get
                {
                    return _layerInterval;
                }
                set
                {
                    _layerInterval = value;
                }
            }
            private Pos _blockLayer; 

            public Pos blockLayer
            {
                get
                {
                    return _blockLayer;
                }
                set
                {
                    _blockLayer = value;
                }
            }
            private Pos _blockFlood; 

            public Pos blockFlood
            {
                get
                {
                    return _blockFlood;
                }
                set
                {
                    _blockFlood = value;
                }
            }
            private Pos[] _safeZone; 

            public Pos[] safeZone
            {
                get
                {
                    return _safeZone;
                }
                set
                {
                    _safeZone = value;
                }
            }

            public MapSettings(string name)
            {
                this.name = name;
                /*fast = 0*/;
                killer = 100;
                /*destroy = 0*/;
                /*water = 0*/;
                /*layer = 0*/;
                layerHeight = 3;
                layerCount = 10;
                layerInterval = 2;
                roundTime = 15;
                floodTime = 5;
                blockFlood = new Pos();
                blockLayer = new Pos();
                safeZone = new Pos[] { new Pos(), new Pos() };
            }
        }

        public class MapData : IDisposable
        {
            private bool _layer; 

            public bool layer
            {
                get
                {
                    return _layer;
                }
                set
                {
                    _layer = value;
                }
            }
            private bool _water; 

            public bool water
            {
                get
                {
                    return _water;
                }
                set
                {
                    _water = value;
                }
            }
            private bool _destroy; 

            public bool destroy
            {
                get
                {
                    return _destroy;
                }
                set
                {
                    _destroy = value;
                }
            }
            private bool _killer; 

            public bool killer
            {
                get
                {
                    return _killer;
                }
                set
                {
                    _killer = value;
                }
            }
            private bool _fast; 

            public bool fast
            {
                get
                {
                    return _fast;
                }
                set
                {
                    _fast = value;
                }
            }
            private byte _block; 

            public byte block
            {
                get
                {
                    return _block;
                }
                set
                {
                    _block = value;
                }
            }
            private int _currentLayer; 

            public int currentLayer
            {
                get
                {
                    return _currentLayer;
                }
                set
                {
                    _currentLayer = value;
                }
            }
            private Timer _layerTimer; 

            public Timer layerTimer
            {
                get
                {
                    return _layerTimer;
                }
                set
                {
                    _layerTimer = value;
                }
            }
            private Timer _floodTimer; 

            public Timer floodTimer
            {
                get
                {
                    return _floodTimer;
                }
                set
                {
                    _floodTimer = value;
                }
            }
            private Timer _roundTimer; 

            public Timer roundTimer
            {
                get
                {
                    return _roundTimer;
                }
                set
                {
                    _roundTimer = value;
                }
            }

            public MapData(MapSettings settings)
            {
                /*fast = false*/;
                /*killer = false*/;
                /*destroy = false*/;
                /*water = false*/;
                /*layer = false*/;
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

            public override string ToString()
            {
                return this.ToString(",");
            }

            public string ToString(string separator)
            {
                return String.Format(CultureInfo.CurrentCulture, "{1}{0}{2}{0}{3}", separator, this.x, this.y, this.z);
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Object obj)
            {
                throw new NotImplementedException();
            }

            public static bool operator ==(Pos x, Pos y)
            {
                throw new NotImplementedException();
            }

            public static bool operator !=(Pos x, Pos y)
            {
                throw new NotImplementedException();
            }
        }

        #region IDisposable Implementation

        protected bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release diposable objects used by this instance here.

                    if (announceTimer != null)
                        announceTimer.Dispose();
                    if (voteTimer != null)
                        voteTimer.Dispose();
                    if (transferTimer != null)
                        transferTimer.Dispose();
                    if (map != null)
                        map.Dispose();
                    if (mapData != null)
                        mapData.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
