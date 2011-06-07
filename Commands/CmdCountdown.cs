//--------------------------------------------------\\
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
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdCountdown() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (p == null)
            {
                Server.s.Log("'null' or console tried to use /countdown. This command is limited to ingame, sorry!!");
                return;
            }
            string par = message.Split(' ')[0].ToLower();
            string par1;
            try
            { par1 = message.Split(' ')[1].ToLower(); }
            catch
            { par1 = ""; }

            string par2;
            try
            { par2 = message.Split(' ')[2].ToLower(); }
            catch
            { par2 = ""; }

            string par3;
            try
            { par3 = message.Split(' ')[3].ToLower(); }
            catch
            { par3 = ""; }

            if (par == "goto")
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

            else if (par == "join")
            {
                switch (CountdownGame.gameprogress)
                {
                    case 0:
                        Player.SendMessage(p, "Sorry - Countdown isn't enabled yet");
                        return;
                    case 1:
                        if (!CountdownGame.players.Contains(p))
                        {
                            CountdownGame.players.Add(p);
                            Player.SendMessage(p, "You've joined the Countdown game!!");
                            Player.GlobalMessage(p.name + " has joined Countdown!!");
                            if (p.level != CountdownGame.mapon)
                            {
                                Player.SendMessage(p, "You can type '/countdown goto' to goto the countdown map!!");
                            }
                        }
                        else
                        {
                            Player.SendMessage(p, "Sorry, you have already joined!!, to leave please type /countdown leave");
                            return;
                        }
                        break;
                    case 2:
                        Player.SendMessage(p, "Sorry - The game is about to start");
                        return; ;
                    case 3:
                        Player.SendMessage(p, "Sorry - The game is already in progress.");
                        return;
                    case 4:
                        Player.SendMessage(p, "Sorry - The game has finished. Get an op to reset it.");
                        return;
                }
            }

            else if (par == "leave")
            {
                if (CountdownGame.players.Contains(p))
                {
                    switch (CountdownGame.gameprogress)
                    {
                        case 0:
                            Player.SendMessage(p, "Sorry - Countdown isn't enabled yet");
                            return;
                        case 1:
                            CountdownGame.players.Remove(p);
                            CountdownGame.playersleftlist.Remove(p);
                            Player.SendMessage(p, "You've left the game.");
                            break;
                        case 2:
                            Player.SendMessage(p, "Sorry - The game is about to start");
                            return; ;
                        case 3:
                            Player.SendMessage(p, "Sorry - you are in a game that is in progress, please wait till its finished or till you've died.");
                            return;
                        case 4:
                            CountdownGame.players.Remove(p);
                            CountdownGame.playersleftlist.Remove(p);
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

            else if (par == "players")
            {
                switch (CountdownGame.gameprogress)
                {
                    case 0:
                        Player.SendMessage(p, "The game has not been enabled yet.");
                        return;

                    case 1:
                        Player.SendMessage(p, "Players who have joined:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            Player.SendMessage(p, Server.DefaultColor + plya.name);
                        }
                        break;

                    case 2:
                        Player.SendMessage(p, "Players who are about to play:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            {
                                Player.SendMessage(p, Server.DefaultColor + plya.name);
                            }
                        }
                        break;

                    case 3:
                        Player.SendMessage(p, "Players left playing:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            {
                                if (CountdownGame.playersleftlist.Contains(plya))
                                {
                                    Player.SendMessage(p, Server.DefaultColor + plya.name + Server.DefaultColor + " who is &aIN");
                                }
                                else
                                {
                                    Player.SendMessage(p, Server.DefaultColor + plya.name + Server.DefaultColor + " who is &cOUT");
                                }
                            }
                        }
                        break;

                    case 4:
                        Player.SendMessage(p, "Players who were playing:");
                        foreach (Player plya in CountdownGame.players)
                        {
                            Player.SendMessage(p, Server.DefaultColor + plya.name);
                        }
                        break;
                }
            }

            else if (par == "rules")
            {
                if (par1 == null)
                {
                    Player.SendMessage(p, "The aim of the game is to stay alive the longest.");
                    Player.SendMessage(p, "Don't fall in the lava!!");
                    Player.SendMessage(p, "Blocks on the ground will disapear randomly, first going yellow, then orange, then red and finally disappering.");
                    Player.SendMessage(p, "The last person alive will win!!");
                }

                else if (par1 == "send")
                {
                    if (p.group.Permission >= LevelPermission.Operator)
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
                    else if (par2 != null)
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

            else if (p.group.Permission >= LevelPermission.Operator)
            {
                if (par == "download")
                {
                    WebClient WEB = new WebClient();
                    WEB.DownloadFile("http://db.tt/R0x1MFS", "levels/countdown.lvl");
                    Player.SendMessage(p, "Downloaded map, now loading map and sending you to it.");
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

                else if (par == "enable")
                {
                    if (CountdownGame.gameprogress == 0)
                    {
                        CountdownGame.mapon = Level.Find("countdown");
                        CountdownGame.gameprogress = 1;
                        Player.SendMessage(p, "Countdown has been enabled");
                        Player.GlobalMessage("Countdown has been enabled!!");
                    }
                    else
                    {
                        Player.SendMessage(p, "A Game is either already enabled or in progress");
                        return;
                    }
                }

                else if (par == "disable")
                {
                    if (CountdownGame.gameprogress != 1 || CountdownGame.gameprogress != 4)
                    {
                        Player.SendMessage(p, "Sorry, a game is currently in progress - please wait till its finished!!");
                        return;
                    }
                    else
                    {
                        foreach (Player pl in CountdownGame.players)
                        {
                            Player.SendMessage(pl, "The countdown game was canceled.");
                        }
                        CountdownGame.gameprogress = 0;
                        CountdownGame.playersleft = 0;
                        CountdownGame.playersleftlist.Clear();
                        CountdownGame.players.Clear();
                        CountdownGame.squaresleft.Clear();
                    }
                }

                else if (par == "start")
                {
                    if (CountdownGame.players.Count >= 2)
                    {
                        CountdownGame.playersleftlist = CountdownGame.players;
                        CountdownGame.playersleft = CountdownGame.players.Count;
                        switch (par1.ToLower())
                        {
                            case "slow":
                                CountdownGame.speed = 1000;
                                CountdownGame.speedtype = "slow";
                                break;

                            case "normal":
                                CountdownGame.speed = 750;
                                CountdownGame.speedtype = "normal";
                                break;

                            case "fast":
                                CountdownGame.speed = 500;
                                CountdownGame.speedtype = "fast";
                                break;

                            default:
                                p.SendMessage("You didn't specify a speed, resorting to 'normal'");
                                CountdownGame.speed = 750;
                                break;
                        }
                        CountdownGame.GameStart(p);
                    }
                    else
                    {
                        Player.SendMessage(p, "Sorry, there aren't enough players to play.");
                        return;
                    }
                }

                else if (par == "reset")
                {
                    switch (CountdownGame.gameprogress)
                    {
                        case 0:
                            Player.SendMessage(p, "Please enable countdown first!!");
                            return;
                        case 2:
                            Player.SendMessage(p, "Sorry - The game is about to start");
                            return;
                        case 3:
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

                else if (par == "tutorial")
                {
                    p.SendMessage("First, download the map using /countdown download");
                    p.SendMessage("Next, type /countdown enable to enable the game mode");
                    p.SendMessage("Next, type /countdown join to join the game and tell other players to join aswell");
                    p.SendMessage("When some people have joined, type /countdown start [speed] to start it");
                    p.SendMessage("[speed] can be 'fast', 'normal' or 'slow'");
                    p.SendMessage("When you are done, type /countdown reset [map/all]");
                    p.SendMessage("use map to reset only the map and all to reset everything.");
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
            p.SendMessage("/countdown join - join the game");
            p.SendMessage("/countdown leave - leave the game");
            p.SendMessage("/countdown goto - goto the countdown map");
            p.SendMessage("/countdown players - view players currently playing");
            if (p.group.Permission < LevelPermission.Operator)
            {
                p.SendMessage("/countdown rules - the rules of countdown");
            }
            else
            {
                p.SendMessage("The following commands are OP and above only!!");
                p.SendMessage("/countdown rules <send> <all/map/player> - the rules of countdown. with send: all to send to all, map to send to map and have a players name to send to a player");
                p.SendMessage("/countdown download - download the countdown map");
                p.SendMessage("/countdown enable - enable the game");
                p.SendMessage("/countdown disable - disable the game");
                p.SendMessage("/countdown start [speed] - start the game, speeds are 'slow', 'normal' and 'fast'");
                p.SendMessage("/countdown reset [all/map] - reset the whole game (all) or only the map (map)");
                p.SendMessage("/countdown tutorial - a tutorial on how to setup countdown");
            }
        }
    }
}
