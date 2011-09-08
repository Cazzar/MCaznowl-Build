/*
	Copyright 2011 MCForge
    Created by Techjar (Jordan S.)
		
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
using System.Collections.Generic;

namespace MCForge
{
    class CmdLavaSurvival : Command
    {
        public override string name { get { return "lavasurvival"; } }
        public override string shortcut { get { return "ls"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public CmdLavaSurvival() { }

        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }
            if (String.IsNullOrEmpty(message)) { Help(p); return; }
            string[] s = message.Split(' ');
            if (p.group.Permission >= Server.lava.setupRank)
            {
                if (s[0] == "start")
                {
                    switch (Server.lava.Start(s.Length > 1 ? s[1] : ""))
                    {
                        case 0:
                            Player.GlobalMessage("Lava Survival has started! Join the fun with /ls go");
                            return;
                        case 1:
                            Player.SendMessage(p, "There is already an active Lava Survival game.");
                            return;
                        case 2:
                            Player.SendMessage(p, "You must at least 3 configured maps to play Lava Survival.");
                            return;
                        case 3:
                            Player.SendMessage(p, "The specified map doesn't exist.");
                            return;
                        default:
                            Player.SendMessage(p, "An unknown error occurred.");
                            return;
                    }
                }
                if (s[0] == "stop")
                {
                    switch (Server.lava.Stop())
                    {
                        case 0:
                            Player.GlobalMessage("Lava Survival has ended! We hope you had fun!");
                            return;
                        case 1:
                            Player.SendMessage(p, "There isn't an active Lava Survival game.");
                            return;
                        default:
                            Player.SendMessage(p, "An unknown error occurred.");
                            return;
                    }
                }
                if (s[0] == "setup")
                {
                    if (s.Length < 2) { SetupHelp(p); return; }
                    if (Server.lava.active) { Player.SendMessage(p, "You cannot configure Lava Survival while a game is active."); return; }
                    if (s[1] == "map")
                    {
                        if (s.Length < 3) { SetupHelp(p, "map"); return; }
                        Level foundLevel = Level.Find(s[2]);
                        if (foundLevel == null)
                        {
                            Player.SendMessage(p, "The level must be loaded to add/remove it.");
                            return;
                        }
                        else
                        {
                            if (Server.lava.HasMap(foundLevel.name))
                            {
                                Server.lava.RemoveMap(foundLevel.name);
                                foundLevel.motd = "ignore";
                                foundLevel.overload = 1500;
                                foundLevel.unload = true;
                                foundLevel.loadOnGoto = true;
                                Level.SaveSettings(foundLevel);
                                Player.SendMessage(p, "Map \"" + foundLevel.name + "\" has been removed.");
                                return;
                            }
                            else
                            {
                                Server.lava.AddMap(foundLevel.name);
                                foundLevel.motd = "Lava Survival: " + foundLevel.name.Capitalize();
                                foundLevel.overload = 1000000;
                                foundLevel.unload = false;
                                foundLevel.loadOnGoto = false;
                                Level.SaveSettings(foundLevel);
                                Player.SendMessage(p, "Map \"" + foundLevel.name + "\" has been added.");
                                return;
                            }
                        }
                    }
                    if (s[1] == "block")
                    {
                        if (s.Length < 2) { SetupHelp(p, "block"); return; }
                        if (!Server.lava.HasMap(p.level.name)) { Player.SendMessage(p, "Add the map before configuring it."); return; }
                        if (s.Length < 3)
                        {
                            LavaSurvival.MapSettings settings = Server.lava.LoadMapSettings(p.level.name);
                            Player.SendMessage(p, "Flood position: &b" + settings.blockFlood.x + ", " + settings.blockFlood.y + ", " + settings.blockFlood.z);
                            Player.SendMessage(p, "Layer position: &b" + settings.blockLayer.x + ", " + settings.blockLayer.y + ", " + settings.blockLayer.z);
                            return;
                        }

                        if (s[2] == "flood")
                        {
                            Player.SendMessage(p, "Place or destroy the block you want to be the total flood block spawn point.");
                            CatchPos cpos; cpos.mode = 0;
                            p.blockchangeObject = cpos;
                            p.ClearBlockchange();
                            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
                            return;
                        }
                        if (s[2] == "layer")
                        {
                            Player.SendMessage(p, "Place or destroy the block you want to be the layer flood base spawn point.");
                            CatchPos cpos; cpos.mode = 1;
                            p.blockchangeObject = cpos;
                            p.ClearBlockchange();
                            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
                            return;
                        }

                        SetupHelp(p, "block");
                        return;
                    }
                    if (s[1] == "settings")
                    {
                        if (s.Length < 4) { SetupHelp(p, "settings"); return; }
                        if (!Server.lava.HasMap(p.level.name)) { Player.SendMessage(p, "Add the map before configuring it."); return; }
                        
                        LavaSurvival.MapSettings settings = Server.lava.LoadMapSettings(p.level.name);
                        switch (s[2])
                        {
                            case "fast":
                                settings.fast = (byte)NumberClamp(decimal.Parse(s[3]), 0, 100);
                                Player.SendMessage(p, "Fast lava chance: &b" + settings.fast + "%");
                                break;
                            case "killer":
                                settings.killer = (byte)NumberClamp(decimal.Parse(s[3]), 0, 100);
                                Player.SendMessage(p, "Killer lava/water chance: &b" + settings.killer + "%");
                                break;
                            case "destroy":
                                settings.destroy = (byte)NumberClamp(decimal.Parse(s[3]), 0, 100);
                                Player.SendMessage(p, "Destroy blocks chance: &b" + settings.destroy + "%");
                                break;
                            case "water":
                                settings.water = (byte)NumberClamp(decimal.Parse(s[3]), 0, 100);
                                Player.SendMessage(p, "Water flood chance: &b" + settings.water + "%");
                                break;
                            case "layer":
                                settings.layer = (byte)NumberClamp(decimal.Parse(s[3]), 0, 100);
                                Player.SendMessage(p, "Layer flood chance: &b" + settings.layer + "%");
                                break;
                            case "layerheight":
                                settings.layerHeight = int.Parse(s[3]);
                                Player.SendMessage(p, "Layer height: &b" + settings.layerHeight + " blocks");
                                break;
                            case "layercount":
                                settings.layerCount = int.Parse(s[3]);
                                Player.SendMessage(p, "Layer count: &b" + settings.layerCount);
                                break;
                            case "layertime":
                                settings.layerInterval = double.Parse(s[3]);
                                Player.SendMessage(p, "Layer time: &b" + settings.layerInterval + " minutes");
                                break;
                            case "roundtime":
                                settings.roundTime = double.Parse(s[3]);
                                Player.SendMessage(p, "Round time: &b" + settings.roundTime + " minutes");
                                break;
                            case "floodtime":
                                settings.floodTime = double.Parse(s[3]);
                                Player.SendMessage(p, "Flood time: &b" + settings.floodTime + " minutes");
                                break;
                            default:
                                SetupHelp(p, "settings");
                                return;
                        }
                        Server.lava.SaveMapSettings(settings);
                    }
                }
            }
            if (s[0] == "go")
            {
                if (!Server.lava.active) { Player.SendMessage(p, "There is no Lava Survival game right now."); return; }
                Command.all.Find("goto").Use(p, Server.lava.map.name);
                return;
            }
            if (s[0] == "info")
            {
                if (!Server.lava.active) { Player.SendMessage(p, "There is no Lava Survival game right now."); return; }
                if (!Server.lava.roundActive) { Player.SendMessage(p, "The round of Lava Survival hasn't started yet."); return; }
                Server.lava.AnnounceRoundInfo(p);
                Server.lava.AnnounceTimeLeft(!Server.lava.flooded, true, p);
                return;
            }

            Help(p);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/lavasurvival <params> - Main command for Lava Survival.");
            Player.SendMessage(p, "The following params are available:");
            Player.SendMessage(p, "go - Join the fun!");
            Player.SendMessage(p, "info - View the current round info and time.");
            if (p == null || p.group.Permission >= Server.lava.setupRank)
            {
                Player.SendMessage(p, "start [map] - Start the Lava Survival game, optionally on the specified map.");
                Player.SendMessage(p, "stop - Stop the current Lava Survival game.");
                Player.SendMessage(p, "setup - Setup lava survival, use it for more info.");
            }
        }

        public void SetupHelp(Player p, string mode = "")
        {
            Player.SendMessage(p, "My ass!");
        }

        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            p.SendBlockchange(x, y, z, p.level.GetTile(x, y, z));
            CatchPos cpos = (CatchPos)p.blockchangeObject;

            LavaSurvival.MapSettings settings = Server.lava.LoadMapSettings(p.level.name);
            if (cpos.mode == 0) settings.blockFlood = new LavaSurvival.Pos(x, y, z);
            if (cpos.mode == 1) settings.blockLayer = new LavaSurvival.Pos(x, y, z);
            Server.lava.SaveMapSettings(settings);

            Player.SendMessage(p, "Position set! &b(" + x + ", " + y + ", " + z + ")");
        }


        private decimal NumberClamp(decimal value, decimal low, decimal high)
        {
            return Math.Max(Math.Min(value, high), low);
        }

        struct CatchPos { public byte mode; }
    }
}
