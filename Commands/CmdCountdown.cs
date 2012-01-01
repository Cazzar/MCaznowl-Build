﻿//--------------------------------------------------\\
//the whole of the game 'countdown' was made by edh649\\
//======================================================\\
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;

namespace MCForge
{
    public class CmdCountdown : Command
    {
        public override string name { get { return "countdown"; } }
        public override string shortcut { get { return "cd"; } }
        public override string type { get { return "game"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public static string keywords { get { return "count down game"; } }
        public CmdCountdown() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (p == null)
            {
                Server.s.Log("'null' or console tried to use /countdown. This command is limited to ingame, sorry!!");
                return;
            }

            string[] command = message.ToLower().Split(' ');
            string par0 = String.Empty;
            string par1 = String.Empty;
            string par2 = String.Empty;
            string par3 = String.Empty;
            try
            {
                par0 = command[0];
                par1 = command[1];
                par2 = command[2];
                par3 = command[3];
            }
            catch { }

            if (par0 == "help")
            {
                Command.all.Find("help").Use(p, "countdown");
                return;
            }

            if (par0 == "goto")
            {
                try
                {
                    Command.all.Find("goto").Use(p, "countdown");
                }
                catch
                {
                    Player.SendMessage(p, "Countdown level not loaded");
                    return;
                }
            }

            else if (par0 == "join")
            {
                switch (CountdownGame.gamestatus)
                {
                    case CountdownGameStatus.Disabled:
                        Player.SendMessage(p, "Sorry - Countdown isn't enabled yet");
                        return;
                    case CountdownGameStatus.Enabled:
                        if (!CountdownGame.players.Contains(p))
                        {
                            CountdownGame.players.Add(p);
                            Player.SendMessage(p, "You've joined the Countdown game!!");
                            Player.GlobalMessage(p.name + " has joined Countdown!!");
                            if (p.level != CountdownGame.mapon)
                            {
                                Player.SendMessage(p, "You can type '/countdown goto' to goto the countdown map!!");
                            }
                            p.playerofcountdown = true;
                        }
                        else
                        {
                            Player.SendMessage(p, "Sorry, you have already joined!!, to leave please type /countdown leave");
                            return;
                        }
                        break;
                    case CountdownGameStatus.AboutToStart:
                        Player.SendMessage(p, "Sorry - The game is about to start");
                        return; ;
                    case CountdownGameStatus.InProgress:
                        Player.SendMessage(p, "Sorry - The game is already in progress.");
                        return;
                    case CountdownGameStatus.Finished:
                        Player.SendMessage(p, "Sorry - The game has finished. Get an op to reset it.");
                        return;
                }
            }

            else if (par0 == "leave")
            {
                if (CountdownGame.players.Contains(p))
                {
                    switch (CountdownGame.gamestatus)
                    {
                        case CountdownGameStatus.Disabled:
                            Player.SendMessage(p, "Sorry - Countdown isn't enabled yet");
                            return;
                        case CountdownGameStatus.Enabled:
                            CountdownGame.players.Remove(p);
                            CountdownGame.playersleftlist.Remove(p);
                            Player.SendMessage(p, "You've left the game.");
                            p.playerofcountdown = false;
                            break;
                        case CountdownGameStatus.AboutToStart:
                            Player.SendMessage(p, "Sorry - The game is about to start");
                            return; ;
                        case CountdownGameStatus.InProgress:
                            Player.SendMessage(p, "Sorry - you are in a game that is in progress, please wait till its finished or till you've died.");
                            return;
                        case CountdownGameStatus.Finished:
                            CountdownGame.players.Remove(p);
                            if (CountdownGame.playersleftlist.Contains(p))
                            {
                                CountdownGame.playersleftlist.Remove(p);
                            }
                            p.playerofcountdown = false;
                            Player.SendMessage(p, "You've left the game.");
                            break;
                    }
                }
                else if (!(CountdownGame.playersleftlist.Contains(p)) && CountdownGame.players.Contains(p))
                {
                    CountdownGame.players.Remove(p);
                    Player.SendMessage(p, "You've left the game.");
                }
                else
                {
                    Player.SendMessage(p, "You haven't joined the game yet!!");
                    return;
                }
            }

            else if (par0 == "players")
            {
                switch (CountdownGame.gamestatus)
                {
                    case CountdownGameStatus.Disabled:
                        Player.SendMessage(p, "The game has not been enabled yet.");
                        return;

                    case CountdownGameStatus.Enabled:
                        Player.SendMessage(p, "Players who have joined:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            Player.SendMessage(p, plya.color + plya.name);
                        }
                        break;

                    case CountdownGameStatus.AboutToStart:
                        Player.SendMessage(p, "Players who are about to play:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            {
                                Player.SendMessage(p, plya.color + plya.name);
                            }
                        }
                        break;

                    case CountdownGameStatus.InProgress:
                        Player.SendMessage(p, "Players left playing:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            {
                                if (CountdownGame.playersleftlist.Contains(plya))
                                {
                                    Player.SendMessage(p, plya.color + plya.name + Server.DefaultColor + " who is &aIN");
                                }
                                else
                                {
                                    Player.SendMessage(p, plya.color + plya.name + Server.DefaultColor + " who is &cOUT");
                                }
                            }
                        }
                        break;

                    case CountdownGameStatus.Finished:
                        Player.SendMessage(p, "Players who were playing:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            Player.SendMessage(p, plya.color + plya.name);
                        }
                        break;
                }
            }

            else if (par0 == "rules")
            {
                if (String.IsNullOrEmpty(par1))
                {
                    Player.SendMessage(p, "The aim of the game is to stay alive the longest.");
                    Player.SendMessage(p, "Don't fall in the lava!!");
                    Player.SendMessage(p, "Blocks on the ground will disapear randomly, first going yellow, then orange, then red and finally disappering.");
                    Player.SendMessage(p, "The last person alive will win!!");
                }

                else if (par1 == "send")
                {
                    if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 1))
                    {
                        if (par2 == "all")
                        {
                            Player.GlobalMessage("Countdown Rules being sent to everyone by " + p.color + p.name + ":");
                            Player.GlobalMessage("The aim of the game is to stay alive the longest.");
                            Player.GlobalMessage("Don't fall in the lava!!");
                            Player.GlobalMessage("Blocks on the ground will disapear randomly, first going yellow, then orange, then red and finally disappering.");
                            Player.GlobalMessage("The last person alive will win!!");
                            Player.SendMessage(p, "Countdown rules sent to everyone");
                        }
                        else if (par2 == "map")
                        {
                            Player.GlobalMessageLevel(p.level, "Countdown Rules being sent to " + p.level.name + " by " + p.color + p.name + ":");
                            Player.GlobalMessageLevel(p.level, "The aim of the game is to stay alive the longest.");
                            Player.GlobalMessageLevel(p.level, "Don't fall in the lava!!");
                            Player.GlobalMessageLevel(p.level, "Blocks on the ground will disapear randomly, first going yellow, then orange, then red and finally disappering.");
                            Player.GlobalMessageLevel(p.level, "The last person alive will win!!");
                            Player.SendMessage(p, "Countdown rules sent to: " + p.level.name);
                        }
                    }
                    else if (!String.IsNullOrEmpty(par2))
                    {
                        Player who = Player.Find(par2);
                        if (who == null)
                        {
                            Player.SendMessage(p, "That wasn't an online player.");
                            return;
                        }
                        else if (who == p)
                        {
                            Player.SendMessage(p, "You can't send rules to yourself, use '/countdown rules' to send it to your self!!");
                            return;
                        }
                        else if (p.group.Permission < who.group.Permission)
                        {
                            Player.SendMessage(p, "You can't send rules to someone of a higher rank than yourself!!");
                            return;
                        }
                        else
                        {
                            Player.SendMessage(who, "Countdown rules sent to you by " + p.color + p.name);
                            Player.SendMessage(who, "The aim of the game is to stay alive the longest.");
                            Player.SendMessage(who, "Don't fall in the lava!!");
                            Player.SendMessage(who, "Blocks on the ground will disapear randomly, first going yellow, then orange, then red and finally disawhowhoering.");
                            Player.SendMessage(who, "The last person alive will win!!");
                            Player.SendMessage(p, "Countdown rules sent to: " + who.color + who.name);
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, par1 + " wasn't a correct parameter.");
                        return;
                    }
                }
            }

            else if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 2))
            {
                if (par0 == "download")
                {
                    try
                    {
                        using (WebClient WEB = new WebClient())
                        {
                            WEB.DownloadFile("http://db.tt/R0x1MFS", "levels/countdown.lvl");
                            Player.SendMessage(p, "Downloaded map, now loading map and sending you to it.");
                        }
                    }
                    catch
                    {
                        Player.SendMessage(p, "Sorry, Downloading Failed. PLease try again later");
                        return;
                    }
                    Command.all.Find("load").Use(p, "countdown");
                    Command.all.Find("goto").Use(p, "countdown");
                    Thread.Sleep(1000);
                    // Sleep for a bit while they load
                    while (p.Loading) { Thread.Sleep(250); }
                    p.level.permissionbuild = LevelPermission.Nobody;
                    p.level.motd = "Welcome to the Countdown map!!!! -hax";
                    ushort x = System.Convert.ToUInt16(8);
                    ushort y = System.Convert.ToUInt16(23);
                    ushort z = System.Convert.ToUInt16(17);
                    x *= 32; x += 16;
                    y *= 32; y += 32;
                    z *= 32; z += 16;
                    unchecked { p.SendPos((byte)-1, x, y, z, p.rot[0], p.rot[1]); }
                }

                else if (par0 == "enable")
                {
                    if (CountdownGame.gamestatus == CountdownGameStatus.Disabled)
                    {
                        try
                        {
                            Command.all.Find("load").Use(null, "countdown");
                            CountdownGame.mapon = Level.Find("countdown");
                            CountdownGame.gamestatus = CountdownGameStatus.Enabled;
                            Player.GlobalMessage("Countdown has been enabled!!");
                        }
                        catch
                        {
                            Player.SendMessage(p, "Failed, have you downloaded the map yet??");
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "A Game is either already enabled or is already progress");
                        return;
                    }
                }

                else if (par0 == "disable")
                {

                    if (CountdownGame.gamestatus == CountdownGameStatus.AboutToStart || CountdownGame.gamestatus == CountdownGameStatus.InProgress)
                    {
                        Player.SendMessage(p, "Sorry, a game is currently in progress - please wait till its finished or use '/countdown cancel' to cancel the game");
                        return;
                    }
                    else if (CountdownGame.gamestatus == CountdownGameStatus.Disabled)
                    {
                        Player.SendMessage(p, "Already disabled!!");
                        return;
                    }
                    else
                    {
                        foreach (Player pl in CountdownGame.players)
                        {
                            Player.SendMessage(pl, "The countdown game was disabled.");
                        }
                        CountdownGame.gamestatus = CountdownGameStatus.Disabled;
                        CountdownGame.playersleft = 0;
                        CountdownGame.playersleftlist.Clear();
                        CountdownGame.players.Clear();
                        CountdownGame.squaresleft.Clear();
                        CountdownGame.Reset(p, true);
                        Player.SendMessage(p, "Countdown Disabled");
                        return;
                    }
                }

                else if (par0 == "cancel")
                {
                    if (CountdownGame.gamestatus == CountdownGameStatus.AboutToStart || CountdownGame.gamestatus == CountdownGameStatus.InProgress)
                    {
                        CountdownGame.cancel = true;
                        Thread.Sleep(1500);
                        Player.SendMessage(p, "Countdown has been canceled");
                        CountdownGame.gamestatus = CountdownGameStatus.Enabled;
                        return;
                    }
                    else
                    {
                        if (CountdownGame.gamestatus == CountdownGameStatus.Disabled)
                        {
                            Player.SendMessage(p, "The game is disabled!!");
                            return;
                        }
                        else
                        {
                            foreach (Player pl in CountdownGame.players)
                            {
                                Player.SendMessage(pl, "The countdown game was canceled");
                            }
                            CountdownGame.gamestatus = CountdownGameStatus.Enabled;
                            CountdownGame.playersleft = 0;
                            CountdownGame.playersleftlist.Clear();
                            CountdownGame.players.Clear();
                            CountdownGame.squaresleft.Clear();
                            CountdownGame.Reset(null, true);
                            return;
                        }
                    }
                }

                else if (par0 == "start" || par0 == "play")
                {
                    if (CountdownGame.gamestatus == CountdownGameStatus.Enabled)
                    {
                        if (CountdownGame.players.Count >= 2)
                        {
                            CountdownGame.playersleftlist = CountdownGame.players;
                            CountdownGame.playersleft = CountdownGame.players.Count;
                            switch (par1)
                            {
                                case "slow":
                                    CountdownGame.speed = 800;
                                    CountdownGame.speedtype = "slow";
                                    break;

                                case "normal":
                                    CountdownGame.speed = 650;
                                    CountdownGame.speedtype = "normal";
                                    break;

                                case "fast":
                                    CountdownGame.speed = 500;
                                    CountdownGame.speedtype = "fast";
                                    break;

                                case "extreme":
                                    CountdownGame.speed = 300;
                                    CountdownGame.speedtype = "extreme";
                                    break;

                                case "ultimate":
                                    CountdownGame.speed = 150;
                                    CountdownGame.speedtype = "ultimate";
                                    break;

                                default:
                                    p.SendMessage("You didn't specify a speed, resorting to 'normal'");
                                    goto case "normal"; //More efficient
                            }
                            if (par2 == null || par2.Trim() == "")
                            {
                                CountdownGame.freezemode = false;
                            }
                            else
                            {
                                if (par2 == "freeze" || par2 == "frozen")
                                {
                                    CountdownGame.freezemode = true;
                                }
                                else
                                {
                                    CountdownGame.freezemode = false;
                                }
                            }
                            CountdownGame.GameStart(p);
                        }
                        else
                        {
                            Player.SendMessage(p, "Sorry, there aren't enough players to play.");
                            return;
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "Either a game is already in progress or it hasn't been enabled");
                        return;
                    }
                }

                else if (par0 == "reset")
                {
                    switch (CountdownGame.gamestatus)
                    {
                        case CountdownGameStatus.Disabled:
                            Player.SendMessage(p, "Please enable countdown first.");
                            return;
                        case CountdownGameStatus.AboutToStart:
                            Player.SendMessage(p, "Sorry - The game is about to start");
                            return;
                        case CountdownGameStatus.InProgress:
                            Player.SendMessage(p, "Sorry - The game is already in progress.");
                            return;
                        default:
                            Player.SendMessage(p, "Reseting");
                            if (par1 == "map")
                            {
                                CountdownGame.Reset(p, false);
                            }
                            else if (par1 == "all")
                            {
                                CountdownGame.Reset(p, true);
                            }
                            else
                            {
                                Player.SendMessage(p, "Please specify whether it is 'map' or 'all'");
                                return;
                            }
                            break;
                    }
                }

                else if (par0 == "tutorial")
                {
                    p.SendMessage("First, download the map using /countdown download");
                    p.SendMessage("Next, type /countdown enable to enable the game mode");
                    p.SendMessage("Next, type /countdown join to join the game and tell other players to join aswell");
                    p.SendMessage("When some people have joined, type /countdown start [speed] to start it");
                    p.SendMessage("[speed] can be 'ultimate', 'extreme', 'fast', 'normal' or 'slow'");
                    p.SendMessage("When you are done, type /countdown reset [map/all]");
                    p.SendMessage("use map to reset only the map and all to reset everything.");
                    return;
                }
            }
            else
            {
                p.SendMessage("Sorry, you aren't a high enough rank or that wasn't a correct command addition.");
                return;
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/cd - Command shortcut.");
            p.SendMessage("/countdown join - join the game");
            p.SendMessage("/countdown leave - leave the game");
            p.SendMessage("/countdown goto - goto the countdown map");
            p.SendMessage("/countdown players - view players currently playing");
            {
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 1))
                {
                    p.SendMessage("/countdown rules <send> <all/map/player> - the rules of countdown. with send: all to send to all, map to send to map and have a players name to send to a player");
                }
                else
                {
                    p.SendMessage("/countdown rules - view the rules of countdown");
                }
                if ((int)p.group.Permission >= CommandOtherPerms.GetPerm(this, 2))
                {
                    p.SendMessage("/countdown download - download the countdown map");
                    p.SendMessage("/countdown enable - enable the game");
                    p.SendMessage("/countdown disable - disable the game");
                    p.SendMessage("/countdown cancel - cancels a game");
                    p.SendMessage("/countdown start [speed] [mode] - start the game, speeds are 'slow', 'normal', 'fast', 'extreme' and 'ultimate', modes are 'normal' and 'freeze'");
                    p.SendMessage("/countdown reset [all/map] - reset the whole game (all) or only the map (map)");
                    p.SendMessage("/countdown tutorial - a tutorial on how to setup countdown");
                }
            }
        }
    }
}
