/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
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
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace MCForge
{
    public static class SrvProperties
    {
        public static void Load(string givenPath, bool skipsalt = false)
        {
            /*
            if (!skipsalt)
            {
                Server.salt = "";
                string rndchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random rnd = new Random();
                for (int i = 0; i < 16; ++i) { Server.salt += rndchars[rnd.Next(rndchars.Length)]; }
            }*/
            if (!skipsalt)
            {
                bool gotSalt = false;
                if (File.Exists("text/salt.txt"))
                {
                    string salt = File.ReadAllText("text/salt.txt");
                    if (salt.Length != 16)
                        Server.s.Log("Invalid salt in salt.txt!");
                    else
                    {
                        Server.salt = salt;
                        gotSalt = true;
                    }
                }
                if (!gotSalt)
                {
                    RandomNumberGenerator prng = RandomNumberGenerator.Create();
                    StringBuilder sb = new StringBuilder();
                    byte[] oneChar = new byte[1];
                    while (sb.Length < 16)
                    {
                        prng.GetBytes(oneChar);
                        if (Char.IsLetterOrDigit((char)oneChar[0]))
                        {
                            sb.Append((char)oneChar[0]);
                        }
                    }
                    Server.salt = sb.ToString();
                }
            }

            if (File.Exists(givenPath))
            {
                string[] lines = File.ReadAllLines(givenPath);

                foreach (string line in lines)
                {
                    if (!(line != null && String.IsNullOrEmpty(line)) && line[0] != '#')
                    {
                        //int index = line.IndexOf('=') + 1; // not needed if we use Split('=')
                        string key = line.Split('=')[0].Trim();
                        string value = "";
                        if (line.IndexOf('=') >= 0)
                            value = line.Substring(line.IndexOf('=') + 1).Trim(); // allowing = in the values
                        string color = "";

                        switch (key.ToLower(CultureInfo.CurrentCulture))
                        {
                            case "server-name":
                                if (ValidString(value, "![]:.,{}~-+()?_/\\' "))
                                {
                                    Server.name = value;
                                }
                                else { Server.s.Log("server-name invalid! setting to default."); }
                                break;
                            case "motd":
                                if (ValidString(value, "=![]&:.,{}~-+()?_/\\' ")) // allow = in the motd
                                {
                                    Server.motd = value;
                                }
                                else { Server.s.Log("motd invalid! setting to default."); }
                                break;
                            case "port":
                                try { Server.port = Convert.ToInt32(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("port invalid! setting to default."); }
                                break;
                            case "verify-names":
                                Server.verify = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "public":
                                Server.pub = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "world-chat":
                                Server.worldChat = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            //case "guest-goto":
                            //    Server.guestGoto = (value.ToLower() == "true") ? true : false;
                            //    break;
                            case "max-players":
                                try
                                {
                                    if (Convert.ToByte(value, CultureInfo.CurrentCulture) > 128)
                                    {
                                        value = "128"; Server.s.Log("Max players has been lowered to 128.");
                                    }
                                    else if (Convert.ToByte(value, CultureInfo.CurrentCulture) < 1)
                                    {
                                        value = "1"; Server.s.Log("Max players has been increased to 1.");
                                    }
                                    Server.players = Convert.ToByte(value, CultureInfo.CurrentCulture);
                                }
                                catch { Server.s.Log("max-players invalid! setting to default."); }
                                break;
                            case "max-guests":
                                try
                                {
                                    if (Convert.ToByte(value, CultureInfo.CurrentCulture) > Server.players)
                                    {
                                        value = Server.players.ToString(CultureInfo.CurrentCulture); Server.s.Log("Max guests has been lowered to " + Server.players.ToString(CultureInfo.CurrentCulture));
                                    }
                                    else if (Convert.ToByte(value, CultureInfo.CurrentCulture) < 0)
                                    {
                                        value = "0"; Server.s.Log("Max guests has been increased to 0.");
                                    }
                                    Server.maxGuests = Convert.ToByte(value, CultureInfo.CurrentCulture);
                                }
                                catch { Server.s.Log("max-guests invalid! setting to default."); }
                                break;
                            case "max-maps":
                                try
                                {
                                    if (Convert.ToByte(value, CultureInfo.CurrentCulture) > 100)
                                    {
                                        value = "100";
                                        Server.s.Log("Max maps has been lowered to 100.");
                                    }
                                    else if (Convert.ToByte(value, CultureInfo.CurrentCulture) < 1)
                                    {
                                        value = "1";
                                        Server.s.Log("Max maps has been increased to 1.");
                                    }
                                    Server.maps = Convert.ToByte(value, CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    Server.s.Log("max-maps invalid! setting to default.");
                                }
                                break;
                            case "irc":
                                Server.irc = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "irc-colorsenable":
                                Server.ircColorsEnable = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "irc-server":
                                Server.ircServer = value;
                                break;
                            case "irc-nick":
                                Server.ircNick = value;
                                break;
                            case "irc-channel":
                                Server.ircChannel = value;
                                break;
                            case "irc-opchannel":
                                Server.ircOpChannel = value;
                                break;
                            case "irc-port":
                                try
                                {
                                    Server.ircPort = Convert.ToInt32(value, CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    Server.s.Log("irc-port invalid! setting to default.");
                                }
                                break;
                            case "irc-identify":
                                try
                                {
                                    Server.ircIdentify = Convert.ToBoolean(value, CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    Server.s.Log("irc-identify boolean value invalid! Setting to the default of: " + Server.ircIdentify + ".");
                                }
                                break;
                            case "irc-password":
                                Server.ircPassword = value;
                                break;
                            case "anti-tunnels":
                                Server.antiTunnel = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "max-depth":
                                try
                                {
                                    Server.maxDepth = Convert.ToByte(value, CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    Server.s.Log("maxDepth invalid! setting to default.");
                                }
                                break;

                            case "rplimit":
                                try { Server.rpLimit = Convert.ToInt16(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("rpLimit invalid! setting to default."); }
                                break;
                            case "rplimit-norm":
                                try { Server.rpNormLimit = Convert.ToInt16(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("rpLimit-norm invalid! setting to default."); }
                                break;


                            case "report-back":
                                Server.reportBack = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "backup-time":
                                if (Convert.ToInt32(value, CultureInfo.CurrentCulture) > 1) { Server.backupInterval = Convert.ToInt32(value, CultureInfo.CurrentCulture); }
                                break;
                            case "backup-location":
                                if (!value.Contains("System.Windows.Forms.TextBox, Text:"))
                                    Server.backupLocation = value;
                                break;

                            //case "console-only": // Never used
                            //    Server.console = (value.ToLower() == "true") ? true : false;
                            //    break;

                            case "physicsrestart":
                                Server.physicsRestart = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "deathcount":
                                Server.deathcount = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;

                            case "usemysql":
                                Server.useMySQL = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "host":
                                Server.MySQLHost = value;
                                break;
                            case "sqlport":
                                Server.MySQLPort = value;
                                break;
                            case "username":
                                Server.MySQLUsername = value;
                                break;
                            case "password":
                                Server.MySQLPassword = value;
                                break;
                            case "databasename":
                                Server.MySQLDatabaseName = value;
                                break;
                            case "pooling":
                                try { Server.DatabasePooling = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "defaultcolor":
                                color = c.Parse(value);
                                if ((color != null && String.IsNullOrEmpty(color)))
                                {
                                    color = c.Name(value); if (!(color != null && String.IsNullOrEmpty(color))) color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                Server.DefaultColor = color;
                                break;
                            case "irc-color":
                                color = c.Parse(value);
                                if ((color != null && String.IsNullOrEmpty(color)))
                                {
                                    color = c.Name(value); if (!(color != null && String.IsNullOrEmpty(color))) color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                Server.IRCColour = color;
                                break;
                            case "old-help":
                                try { Server.oldHelp = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "opchat-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.opchatperm = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "adminchat-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.adminchatperm = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "log-heartbeat":
                                try { Server.logbeat = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "force-cuboid":
                                try { Server.forceCuboid = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "profanity-filter":
                                try { Server.profanityFilter = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "notify-on-join-leave":
                                try { Server.notifyOnJoinLeave = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "cheapmessage":
                                try { Server.cheapMessage = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "cheap-message-given":
                                if (!(value != null && String.IsNullOrEmpty(value))) Server.cheapMessageGiven = value;
                                break;
                            case "custom-ban":
                                try { Server.customBan = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "custom-ban-message":
                                if (!(value != null && String.IsNullOrEmpty(value))) Server.customBanMessage = value;
                                break;
                            case "custom-shutdown":
                                try { Server.customShutdown = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "custom-shutdown-message":
                                if (!(value != null && String.IsNullOrEmpty(value))) Server.customShutdownMessage = value;
                                break;
                            case "custom-griefer-stone":
                                try { Server.customGrieferStone = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "custom-griefer-stone-message":
                                if (!(value != null && String.IsNullOrEmpty(value))) Server.customGrieferStoneMessage = value;
                                break;
                            case "custom-promote-message":
                                if (!(value != null && String.IsNullOrEmpty(value))) Server.customPromoteMessage = value;
                                break;
                            case "custom-demote-message":
                                if (!(value != null && String.IsNullOrEmpty(value))) Server.customDemoteMessage = value;
                                break;
                            case "rank-super":
                                try { Server.rankSuper = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "default-rank":
                                try { Server.defaultRank = value.ToLower(CultureInfo.CurrentCulture); }
                                catch { }
                                break;
                            case "afk-minutes":
                                try
                                {
                                    Server.afkminutes = Convert.ToInt32(value, CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    Server.s.Log("irc-port invalid! setting to default.");
                                }
                                break;
                            case "afk-kick":
                                try { Server.afkkick = Convert.ToInt32(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("irc-port invalid! setting to default."); }
                                break;
                            case "afk-kick-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.afkkickperm = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "check-updates":
                                try { Server.autonotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "auto-update":
                                Server.autoupdate = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "in-game-update-notify":
                                Server.notifyPlayers = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "update-countdown":
                                try { Server.restartcountdown = Convert.ToInt32(value, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture); }
                                catch { Server.restartcountdown = "10"; }
                                break;
                            case "autoload":
                                try { Server.AutoLoad = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "auto-restart":
                                try { Server.autorestart = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "restarttime":
                                try { Server.restarttime = DateTime.Parse(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("Invalid " + key + ". Using defualt."); break; }
                                break;
                            case "parse-emotes":
                                try { Server.parseSmiley = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "use-whitelist":
                                Server.useWhitelist = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "premium-only":
                                Server.PremiumPlayersOnly = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "allow-tp-to-higher-ranks":
                                Server.higherranktp = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "agree-to-rules-on-entry":
                                try { Server.agreetorulesonentry = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "admins-join-silent":
                                try { Server.adminsjoinsilent = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "main-name":
                                if (Player.ValidName(value)) Server.level = value;
                                else Server.s.Log("Invalid main name");
                                break;
                            case "dollar-before-dollar":
                                try { Server.dollardollardollar = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "money-name":
                                if (!(value != null && String.IsNullOrEmpty(value))) Server.moneys = value;
                                break;
                            /*case "mono":
                                try { Server.mono = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;*/
                            case "restart-on-error":
                                try { Server.restartOnError = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "repeat-messages":
                                try { Server.repeatMessage = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "host-state":
                                if (!(value != null && String.IsNullOrEmpty(value)))
                                    Server.ZallState = value;
                                break;
                            case "kick-on-hackrank":
                                try { Server.hackrank_kick = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "hackrank-kick-time":
                                try { Server.hackrank_kick_time = int.Parse(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "server-owner":
                                if (!(value != null && String.IsNullOrEmpty(value)))
                                    Server.server_owner = value;
                                break;
                            case "zombie-on-server-start":
                                try { Server.startZombieModeOnStartup = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "no-respawning-during-zombie":
                                try { Server.noRespawn = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "no-level-saving-during-zombie":
                                try { Server.noLevelSaving = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "no-pillaring-during-zombie":
                                try { Server.noPillaring = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "zombie-name-while-infected":
                                if (!(value != null && String.IsNullOrEmpty(value)))
                                    Server.ZombieName = value;
                                break;
                            case "enable-changing-levels":
                                try { Server.ChangeLevels = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "zombie-survival-only-server":
                                try { Server.ZombieOnlyServer = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "use-level-list":
                                try { Server.UseLevelList = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "zombie-level-list":
                                if (!(value != null && String.IsNullOrEmpty(value)))
                                {

                                    string input = value.Replace(" ", "").ToString(CultureInfo.CurrentCulture);
                                        int itndex = input.IndexOf("#", StringComparison.CurrentCulture);
                                    if (itndex > 0)
                                        input = input.Substring(0, itndex);

                                    Server.LevelList = input.Split(',').ToList<string>();
                                }
                                break;
                            case "guest-limit-notify":
                                try { Server.guestLimitNotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "guest-join-notify":
                                try { Server.guestJoinNotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "guest-leave-notify":
                                try { Server.guestLeaveNotify = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "ignore-ops":
                                try { Server.globalignoreops = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "admin-verification":
                                try { Server.verifyadmins = bool.Parse(value); }
                                catch { Server.s.Log("invalid " + key + ". Using default"); }
                                break;
                            case "verify-admin-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.verifyadminsrank = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ".  Using default."); break; }
                                break;
                            case "mute-on-spam":
                                try { Server.checkspam = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "spam-messages":
                                try { Server.spamcounter = int.Parse(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "spam-mute-time":
                                try { Server.mutespamtime = int.Parse(value, CultureInfo.CurrentCulture); } catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "spam-counter-reset-time":
                                try { Server.spamcountreset = int.Parse(value, CultureInfo.CurrentCulture); } catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "show-empty-ranks":
                                try { Server.showEmptyRanks = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "global-chat-enabled":
                                try { Server.UseGlobalChat = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;

                            case "global-chat-nick":
                                if (!(value != null && String.IsNullOrEmpty(value)))
                                    Server.GlobalChatNick = value;
                                break;

                            case "global-chat-color":
                                color = c.Parse(value);
                                if ((color != null && String.IsNullOrEmpty(color)))
                                {
                                    color = c.Name(value); if (!(color != null && String.IsNullOrEmpty(color))) color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                Server.GlobalChatColor = color;
                                break;

                            case "total-undo":
                                try { Server.totalUndo = int.Parse(value, CultureInfo.CurrentCulture); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;

                            case "griefer-stone-tempban":
                                try { Server.grieferStoneBan = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;

                            case "griefer-stone-type":
                                try { Server.grieferStoneType = (byte)MathHelper.Clamp((decimal)Block.Byte(value), 1, 49); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "griefer-stone-rank":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.grieferStoneRank = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); break; }
                                break;
                            case "wom-direct":
                                try { Server.WomDirect = bool.Parse(value); }
                                catch { Server.s.Log("Invalid " + key + ". Using default"); }
                                break;
                            case "wom-serveralt":
                                Server.Server_ALT = value;
                                break;
                            case "wom-serverdis":
                                Server.Server_Disc = value;
                                break;
                            case "wom-serverflag":
                                Server.Server_Flag = value;
                                break;
                            case "wom-textures":
                                Server.UseTextures = bool.Parse(value);
                                break;
                            case "send-command-usage":
                                Player.sendcommanddata = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "review-view-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.reviewview = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "review-enter-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.reviewenter = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "review-leave-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.reviewleave = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "review-cooldown":
                                try
                                {
                                    Server.reviewcooldown = Convert.ToInt32(value.ToLower(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "review-clear-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.reviewclear = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "review-next-perm":
                                try
                                {
                                    sbyte parsed = sbyte.Parse(value, CultureInfo.CurrentCulture);
                                    if (parsed < -50 || parsed > 120)
                                    {
                                        throw new FormatException();
                                    }
                                    Server.reviewnext = (LevelPermission)parsed;
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "bufferblocks":
                                try
                                {
                                    Server.bufferblocks = bool.Parse(value);
                                }
                                catch { Server.s.Log("Invalid " + key + ". Using default."); }
                                break;
                            case "translation-enabled":
                                Server.transenabled = (value.ToLower(CultureInfo.CurrentCulture) == "true") ? true : false;
                                break;
                            case "translation-language":
                                string langcode = value;
                            switch (langcode)
                                {
                                    case "af":
                                        langcode = "Afrikaans";
                                        break;
                                    case "ar-sa":
                                        langcode = "Arabic (Saudi Arabia)";
                                        break;
                                    case "ar-eg":
                                        langcode = "Arabic (Egypt)";
                                        break;
                                    case "ar-dz":
                                        langcode = "Arabic (Algeria)";
                                        break;
                                    case "ar-tn":
                                        langcode = "Arabic (Tunisia)";
                                        break;
                                    case "ar-ye":
                                        langcode = "Arabic (Yemen)";
                                        break;
                                    case "ar-jo":
                                        langcode = "Arabic (Jordan)";
                                        break;
                                    case "ar-kw":
                                        langcode = "Arabic (Kuwait)";
                                        break;
                                    case "ar-bh":
                                        langcode = "Arabic (Bahrain)";
                                        break;
                                    case "eu":
                                        langcode = "Basque";
                                        break;
                                    case "be":
                                        langcode = "Belarusian";
                                        break;
                                    case "zh-tw":
                                        langcode = "Chinese (Taiwan)";
                                        break;
                                    case "zh-hk":
                                        langcode = "Chinese (Hong Kong SAR)";
                                        break;
                                    case "hr":
                                        langcode = "Croatian";
                                        break;
                                    case "da":
                                        langcode = "Danish";
                                        break;
                                    case "nl-be":
                                        langcode = "Dutch (Belgium)";
                                        break;
                                    case "en-us":
                                        langcode = "English (United States)";
                                        break;
                                    case "en-au":
                                        langcode = "English (Australia)";
                                        break;
                                    case "en-nz":
                                        langcode = "English (New Zealand)";
                                        break;
                                    case "en-za":
                                        langcode = "English (South Africa)";
                                        break;
                                    case "en-tt":
                                        langcode = "English (Trinidad)";
                                        break;
                                    case "fo":
                                        langcode = "Faeroese";
                                        break;
                                    case "fi":
                                        langcode = "Finnish";
                                        break;
                                    case "fr-be":
                                        langcode = "French (Belgium)";
                                        break;
                                    case "fr-ch":
                                        langcode = "French (Switzerland)";
                                        break;
                                    case "gd":
                                        langcode = "Gaelic (Scotland)";
                                        break;
                                    case "de":
                                        langcode = "German (Standard)";
                                        break;
                                    case "de-at":
                                        langcode = "German (Austria)";
                                        break;
                                    case "de-li":
                                        langcode = "German (Liechtenstein)";
                                        break;
                                    case "he":
                                        langcode = "Hebrew";
                                        break;
                                    case "hu":
                                        langcode = "Hungarian";
                                        break;
                                    case "id":
                                        langcode = "Indonesian";
                                        break;
                                    case "it-ch":
                                        langcode = "Italian (Switzerland)";
                                        break;
                                    case "ko":
                                        langcode = "Korean";
                                        break;
                                    case "lv":
                                        langcode = "Latvian";
                                        break;
                                    case "mk":
                                        langcode = "Macedonian (FYROM)";
                                        break;
                                    case "mt":
                                        langcode = "Maltese";
                                        break;
                                    case "no":
                                        langcode = "Norwegian (Nynorsk)";
                                        break;
                                    case "pt-br":
                                        langcode = "Portuguese (Brazil)";
                                        break;
                                    case "rm":
                                        langcode = "Rhaeto-Romanic";
                                        break;
                                    case "ro-mo":
                                        langcode = "Romanian (Republic of Moldova)";
                                        break;
                                    case "ru-mo":
                                        langcode = "Russian (Republic of Moldova)";
                                        break;
                                    case "sr":
                                        langcode = "Serbian (Cyrillic)";
                                        break;
                                    case "sk":
                                        langcode = "Slovak";
                                        break;
                                    case "sb":
                                        langcode = "Sorbian";
                                        break;
                                    case "es-mx":
                                        langcode = "Spanish (Mexico)";
                                        break;
                                    case "es-cr":
                                        langcode = "Spanish (Costa Rica)";
                                        break;
                                    case "es-do":
                                        langcode = "Spanish (Dominican Republic)";
                                        break;
                                    case "es-co":
                                        langcode = "Spanish (Colombia)";
                                        break;
                                    case "es-ar":
                                        langcode = "Spanish (Argentina)";
                                        break;
                                    case "es-cl":
                                        langcode = "Spanish (Chile)";
                                        break;
                                    case "es-py":
                                        langcode = "Spanish (Paraguay)";
                                        break;
                                    case "es-sv":
                                        langcode = "Spanish (El Salvador)";
                                        break;
                                    case "es-ni":
                                        langcode = "Spanish (Nicaragua)";
                                        break;
                                    case "sx":
                                        langcode = "Sutu";
                                        break;
                                    case "sv-fi":
                                        langcode = "Swedish (Finland)";
                                        break;
                                    case "ts":
                                        langcode = "Tsonga";
                                        break;
                                    case "tr":
                                        langcode = "Turkish";
                                        break;
                                    case "ur":
                                        langcode = "Urdu";
                                        break;
                                    case "vi":
                                        langcode = "Vietnamese";
                                        break;
                                    case "ji":
                                        langcode = "Yiddish";
                                        break;
                                    case "sq":
                                        langcode = "Albanian";
                                        break;
                                    case "ar-iq":
                                        langcode = "Arabic (Iraq)";
                                        break;
                                    case "ar-ly":
                                        langcode = "Arabic (Libya)";
                                        break;
                                    case "ar-ma":
                                        langcode = "Arabic (Morocco)";
                                        break;
                                    case "ar-om":
                                        langcode = "Arabic (Oman)";
                                        break;
                                    case "ar-sy":
                                        langcode = "Arabic (Syria)";
                                        break;
                                    case "ar-lb":
                                        langcode = "Arabic (Lebanon)";
                                        break;
                                    case "ar-ae":
                                        langcode = "Arabic (U.A.E.)";
                                        break;
                                    case "ar-qa":
                                        langcode = "Arabic (Qatar)";
                                        break;
                                    case "bg":
                                        langcode = "Bulgarian";
                                        break;
                                    case "ca":
                                        langcode = "Catalan";
                                        break;
                                    case "zh-cn":
                                        langcode = "Chinese (PRC)";
                                        break;
                                    case "zh-sg":
                                        langcode = "Chinese (Singapore)";
                                        break;
                                    case "cs":
                                        langcode = "Czech";
                                        break;
                                    case "nl":
                                        langcode = "Dutch (Standard)";
                                        break;
                                    case "en":
                                        langcode = "English";
                                        break;
                                    case "en-gb":
                                        langcode = "English (United Kingdom)";
                                        break;
                                    case "en-ca":
                                        langcode = "English (Canada)";
                                        break;
                                    case "en-ie":
                                        langcode = "English (Ireland)";
                                        break;
                                    case "en-jm":
                                        langcode = "English (Jamaica)";
                                        break;
                                    case "en-bz":
                                        langcode = "English (Belize)";
                                        break;
                                    case "et":
                                        langcode = "Farsi";
                                        break;
                                    case "fr":
                                        langcode = "French (Standard)";
                                        break;
                                    case "ga":
                                        langcode = "Irish";
                                        break;
                                    case "el":
                                        langcode = "Greek";
                                        break;
                                    case "hi":
                                        langcode = "Hindi";
                                        break;
                                    case "it":
                                        langcode = "Italian (Standard)";
                                        break;
                                    case "is":
                                        langcode = "Icelandic";
                                        break;
                                    case "ja":
                                        langcode = "Japanese";
                                        break;
                                    case "lt":
                                        langcode = "Lithuanian";
                                        break;
                                    case "ms":
                                        langcode = "Malaysian";
                                        break;
                                    case "pl":
                                        langcode = "Polish";
                                        break;
                                    case "pt":
                                        langcode = "Portuguese";
                                        break;
                                    case "ro":
                                        langcode = "Romanian";
                                        break;
                                    case "ru":
                                        langcode = "Russian";
                                        break;
                                    case "sz":
                                        langcode = "Sami (Lappish) ";
                                        break;
                                    case "sl":
                                        langcode = "Slovenian ";
                                        break;
                                    case "es":
                                        langcode = "Spanish";
                                        break;
                                    case "sv":
                                        langcode = "Swedish";
                                        break;
                                    case "th":
                                        langcode = "Thai";
                                        break;
                                    case "tn":
                                        langcode = "Tswana";
                                        break;
                                    case "uk":
                                        langcode = "Ukrainian";
                                        break;
                                    case "ve":
                                        langcode = "Venda";
                                        break;
                                    case "xh":
                                        langcode = "Xhosa";
                                        break;
                                    case "zu":
                                        langcode = "Zulu";
                                        break;
                                    default:
                                        langcode = "!ERROR!";
                                        break;
                                }
                            if (langcode != "!ERROR!")
                            {
                                Server.translang = value.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture);
                            }
                            else
                            {
                                Server.translang = "en";
                            }
                                break;

                        }
                    }
                }
                Server.s.SettingsUpdate();
                Save(givenPath);
            }
            else Save(givenPath);
        }
        public static bool ValidString(string str, string allowed)
        {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890" + allowed;
            foreach (char ch in str)
            {
                if (allowedchars.IndexOf(ch) == -1)
                {
                    return false;
                }
            } return true;
        }

       public static void Save(string givenPath)
        {
            try
            {
                File.Create(givenPath).Dispose();
                using (StreamWriter w = File.CreateText(givenPath))
                {
                    if (givenPath.IndexOf("server", StringComparison.CurrentCulture) != -1)
                    {
                        SaveProps(w);
                    }
                }
            }
            catch
            {
                Server.s.Log("SAVE FAILED! " + givenPath);
            }
        }
        public static void SaveProps(StreamWriter w)
        {
            w.WriteLine("#   Edit the settings below to modify how your server operates. This is an explanation of what each setting does.");
            w.WriteLine("#   server-name\t=\tThe name which displays on minecraft.net");
            w.WriteLine("#   motd\t=\tThe message which displays when a player connects");
            w.WriteLine("#   port\t=\tThe port to operate from");
            w.WriteLine("#   console-only\t=\tRun without a GUI (useful for Linux servers with mono)");
            w.WriteLine("#   verify-names\t=\tVerify the validity of names");
            w.WriteLine("#   public\t=\tSet to true to appear in the public server list");
            w.WriteLine("#   max-players\t=\tThe maximum number of connections");
            w.WriteLine("#   max-guests\t=\tThe maximum number of guests allowed");
            w.WriteLine("#   max-maps\t=\tThe maximum number of maps loaded at once");
            w.WriteLine("#   world-chat\t=\tSet to true to enable world chat");
            w.WriteLine("#   guest-goto\t=\tSet to true to give guests goto and levels commands (Not implemented yet)");
            w.WriteLine("#   irc\t=\tSet to true to enable the IRC bot");
            w.WriteLine("#   irc-nick\t=\tThe name of the IRC bot");
            w.WriteLine("#   irc-server\t=\tThe server to connect to");
            w.WriteLine("#   irc-channel\t=\tThe channel to join");
            w.WriteLine("#   irc-opchannel\t=\tThe channel to join (posts OpChat)");
            w.WriteLine("#   irc-port\t=\tThe port to use to connect");
            w.WriteLine("#   irc-identify\t=(true/false)\tDo you want the IRC bot to Identify itself with nickserv. Note: You will need to register it's name with nickserv manually.");
            w.WriteLine("#   irc-password\t=\tThe password you want to use if you're identifying with nickserv");
            w.WriteLine("#   anti-tunnels\t=\tStops people digging below max-depth");
            w.WriteLine("#   max-depth\t=\tThe maximum allowed depth to dig down");
            w.WriteLine("#   backup-time\t=\tThe number of seconds between automatic backups");
            w.WriteLine("#   overload\t=\tThe higher this is, the longer the physics is allowed to lag.  Default 1500");
            w.WriteLine("#   use-whitelist\t=\tSwitch to allow use of a whitelist to override IP bans for certain players.  Default false.");
            w.WriteLine("#   premium-only\t=\tOnly allow premium players (paid for minecraft) to access the server. Default false.");
            w.WriteLine("#   force-cuboid\t=\tRun cuboid until the limit is hit, instead of canceling the whole operation.  Default false.");
            w.WriteLine("#   profanity-filter\t=\tReplace certain bad words in the chat.  Default false.");
            w.WriteLine("#   notify-on-join-leave\t=\tShow a balloon popup in tray notification area when a player joins/leaves the server.  Default false.");
            w.WriteLine("#   allow-tp-to-higher-ranks\t=\tAllows the teleportation to players of higher ranks");
            w.WriteLine("#   agree-to-rules-on-entry\t=\tForces all new players to the server to agree to the rules before they can build or use commands.");
            w.WriteLine("#   adminchat-perm\t=\tThe rank required to view adminchat. Default rank is superop.");
            w.WriteLine("#   admins-join-silent\t=\tPlayers who have adminchat permission join the game silently. Default true");
            w.WriteLine("#   server-owner\t=\tThe minecraft name, of the owner of the server.");
            w.WriteLine("#   zombie-on-server-start\t=\tStarts Zombie Survival when server is started.");
            w.WriteLine("#   no-respawning-during-zombie\t=\tDisables respawning (Pressing R) while Zombie is on.");
            w.WriteLine("#   no-level-saving-during-zombie\t=\tDisables level saving while Zombie Survival is activated.");
            w.WriteLine("#   no-pillaring-during-zombie\t=\tDisables pillaring while Zombie Survival is activated.");
            w.WriteLine("#   zombie-name-while-infected\t=\tSets the zombies name while actived if there is a value.");
            w.WriteLine("#   enable-changing-levels\t=\tAfter a Zombie Survival round has finished, will change the level it is running on.");
            w.WriteLine("#   zombie-survival-only-server\t=\tiEXPERIMENTAL! Makes the server only for Zombie Survival (etc. changes main level)");
            w.WriteLine("#   use-level-list\t=\tOnly gets levels for changing levels in Zombie Survival from zombie-level-list.");
            w.WriteLine("#   zombie-level-list\t=\tList of levels for changing levels (Must be comma seperated, no spaces. Must have changing levels and use level list enabled.)");
            w.WriteLine("#   total-undo\t=\tTrack changes made by the last X people logged on for undo purposes. Folder is rotated when full, so when set to 200, will actually track around 400.");
            w.WriteLine("#   guest-limit-notify\t=\tShow -Too Many Guests- message in chat when maxGuests has been reached. Default false");
            w.WriteLine("#   guest-join-notify\t=\tShows when guests and lower ranks join server in chat and IRC. Default true");
            w.WriteLine("#   guest-leave-notify\t=\tShows when guests and lower ranks leave server in chat and IRC. Default true");
            w.WriteLine();
            w.WriteLine("#   UseMySQL\t=\tUse MySQL (true) or use SQLite (false)");
            w.WriteLine("#   Host\t=\tThe host name for the database (usually 127.0.0.1)");
            w.WriteLine("#   SQLPort\t=\tPort number to be used for MySQL.  Unless you manually changed the port, leave this alone.  Default 3306.");
            w.WriteLine("#   Username\t=\tThe username you used to create the database (usually root)");
            w.WriteLine("#   Password\t=\tThe password set while making the database");
            w.WriteLine("#   DatabaseName\t=\tThe name of the database stored (Default = MCZall)");
            w.WriteLine();
            w.WriteLine("#   defaultColor\t=\tThe color code of the default messages (Default = &e)");
            w.WriteLine();
            w.WriteLine("#   Super-limit\t=\tThe limit for building commands for SuperOPs");
            w.WriteLine("#   Op-limit\t=\tThe limit for building commands for Operators");
            w.WriteLine("#   Adv-limit\t=\tThe limit for building commands for AdvBuilders");
            w.WriteLine("#   Builder-limit\t=\tThe limit for building commands for Builders");
            w.WriteLine();
            w.WriteLine("#   kick-on-hackrank\t=\tSet to true if hackrank should kick players");
            w.WriteLine("#   hackrank-kick-time\t=\tNumber of seconds until player is kicked");
            w.WriteLine("#   custom-rank-welcome-messages\t=\tDecides if different welcome messages for each rank is enabled. Default true.");
            w.WriteLine("#   ignore-ops\t=\tDecides whether or not an operator can be ignored. Default false.");
            w.WriteLine();
            w.WriteLine("#   admin-verification\t=\tDetermines whether admins have to verify on entry to the server.  Default true.");
            w.WriteLine("#   verify-admin-perm\t=\tThe minimum rank required for admin verification to occur.");
            w.WriteLine();
            w.WriteLine("#   mute-on-spam\t=\tIf enabled it mutes a player for spamming.  Default false.");
            w.WriteLine("#   spam-messages\t=\tThe amount of messages that have to be sent \"consecutively\" to be muted.");
            w.WriteLine("#   spam-mute-time\t=\tThe amount of seconds a player is muted for spam.");
            w.WriteLine("#   spam-counter-reset-time\t=\tThe amount of seconds the \"consecutive\" messages have to fall between to be considered spam.");
            w.WriteLine();
            w.WriteLine("#   As an example, if you wanted the spam to only mute if a user posts 5 messages in a row within 2 seconds, you would use the folowing:");
            w.WriteLine("#   mute-on-spam\t=\ttrue");
            w.WriteLine("#   spam-messages\t=\t5");
            w.WriteLine("#   spam-mute-time\t=\t60");
            w.WriteLine("#   spam-counter-reset-time\t=\t2");
            w.WriteLine("#   bufferblocks - Should buffer blocks by default for maps?");
            w.WriteLine();
            w.WriteLine("# Server options");
            w.WriteLine("server-name = " + Server.name);
            w.WriteLine("motd = " + Server.motd);
            w.WriteLine("port = " + Server.port.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("verify-names = " + Server.verify.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("public = " + Server.pub.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("max-players = " + Server.players.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("max-guests = " + Server.maxGuests.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("max-maps = " + Server.maps.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("world-chat = " + Server.worldChat.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("check-updates = " + Server.checkUpdates.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("auto-update = " + Server.autoupdate.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("in-game-update-notify = " + Server.notifyPlayers.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("update-countdown = " + Server.restartcountdown.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("autoload = " + Server.AutoLoad.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("auto-restart = " + Server.autorestart.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("restarttime = " + Server.restarttime.ToShortTimeString());
            w.WriteLine("restart-on-error = " + Server.restartOnError);
            w.WriteLine("main-name = " + Server.level);
            //w.WriteLine("guest-goto = " + Server.guestGoto);
            w.WriteLine();
            w.WriteLine("# irc bot options");
            w.WriteLine("irc = " + Server.irc.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("irc-colorsenable = " + Server.ircColorsEnable.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("irc-nick = " + Server.ircNick);
            w.WriteLine("irc-server = " + Server.ircServer);
            w.WriteLine("irc-channel = " + Server.ircChannel);
            w.WriteLine("irc-opchannel = " + Server.ircOpChannel);
            w.WriteLine("irc-port = " + Server.ircPort.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("irc-identify = " + Server.ircIdentify.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("irc-password = " + Server.ircPassword);
            w.WriteLine();
            w.WriteLine("# other options");
            w.WriteLine("anti-tunnels = " + Server.antiTunnel.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("max-depth = " + Server.maxDepth.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("rplimit = " + Server.rpLimit.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("rplimit-norm = " + Server.rpNormLimit.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("physicsrestart = " + Server.physicsRestart.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("old-help = " + Server.oldHelp.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("deathcount = " + Server.deathcount.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("afk-minutes = " + Server.afkminutes.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("afk-kick = " + Server.afkkick.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("afk-kick-perm = " + ((sbyte)Server.afkkickperm).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("parse-emotes = " + Server.parseSmiley.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("dollar-before-dollar = " + Server.dollardollardollar.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("use-whitelist = " + Server.useWhitelist.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("premium-only = " + Server.PremiumPlayersOnly.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("money-name = " + Server.moneys);
            w.WriteLine("opchat-perm = " + ((sbyte)Server.opchatperm).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("adminchat-perm = " + ((sbyte)Server.adminchatperm).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("log-heartbeat = " + Server.logbeat.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("force-cuboid = " + Server.forceCuboid.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("profanity-filter = " + Server.profanityFilter.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("notify-on-join-leave = " + Server.notifyOnJoinLeave.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("repeat-messages = " + Server.repeatMessage.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("host-state = " + Server.ZallState.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("agree-to-rules-on-entry = " + Server.agreetorulesonentry.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("admins-join-silent = " + Server.adminsjoinsilent.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("server-owner = " + Server.server_owner.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("zombie-on-server-start = " + Server.startZombieModeOnStartup);
            w.WriteLine("no-respawning-during-zombie = " + Server.noRespawn);
            w.WriteLine("no-level-saving-during-zombie = " + Server.noLevelSaving);
            w.WriteLine("no-pillaring-during-zombie = " + Server.noPillaring);
            w.WriteLine("zombie-name-while-infected = " + Server.ZombieName);
            w.WriteLine("enable-changing-levels = " + Server.ChangeLevels);
            w.WriteLine("zombie-survival-only-server = " + Server.ZombieOnlyServer);
            w.WriteLine("use-level-list = " + Server.UseLevelList);
            string dogCsv = string.Join(",", Server.LevelList.ToArray());
            w.WriteLine("zombie-level-list = " + dogCsv + "#(Must be comma seperated, no spaces. Must have changing levels and use level list enabled.)");
            w.WriteLine("guest-limit-notify = " + Server.guestLimitNotify.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("guest-join-notify = " + Server.guestJoinNotify.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("guest-leave-notify = " + Server.guestLeaveNotify.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("send-command-usage = " + Player.sendcommanddata.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("total-undo = " + Server.totalUndo.ToString(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("# backup options");
            w.WriteLine("backup-time = " + Server.backupInterval.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("backup-location = " + Server.backupLocation);
            w.WriteLine();
            w.WriteLine("#Error logging");
            w.WriteLine("report-back = " + Server.reportBack.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("#MySQL information");
            w.WriteLine("UseMySQL = " + Server.useMySQL);
            w.WriteLine("Host = " + Server.MySQLHost);
            w.WriteLine("SQLPort = " + Server.MySQLPort);
            w.WriteLine("Username = " + Server.MySQLUsername);
            w.WriteLine("Password = " + Server.MySQLPassword);
            w.WriteLine("DatabaseName = " + Server.MySQLDatabaseName);
            w.WriteLine("Pooling = " + Server.DatabasePooling);
            w.WriteLine();
            w.WriteLine("#Colors");
            w.WriteLine("defaultColor = " + Server.DefaultColor);
            w.WriteLine("irc-color = " + Server.IRCColour);
            w.WriteLine();
            /*w.WriteLine("#Running on mono?");
            w.WriteLine("mono = " + Server.mono);
            w.WriteLine();*/
            w.WriteLine("#Custom Messages");
            w.WriteLine("custom-ban = " + Server.customBan.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("custom-ban-message = " + Server.customBanMessage);
            w.WriteLine("custom-shutdown = " + Server.customShutdown.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("custom-shutdown-message = " + Server.customShutdownMessage);
            w.WriteLine("custom-griefer-stone = " + Server.customGrieferStone.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("custom-griefer-stone-message = " + Server.customGrieferStoneMessage);
            w.WriteLine("custom-promote-message = " + Server.customPromoteMessage);
            w.WriteLine("custom-demote-message = " + Server.customDemoteMessage);
            w.WriteLine("allow-tp-to-higher-ranks = " + Server.higherranktp.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("ignore-ops = " + Server.globalignoreops.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("cheapmessage = " + Server.cheapMessage.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("cheap-message-given = " + Server.cheapMessageGiven);
            w.WriteLine("rank-super = " + Server.rankSuper.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            try { w.WriteLine("default-rank = " + Server.defaultRank); }
            catch { w.WriteLine("default-rank = guest"); }
            w.WriteLine();
            w.WriteLine("kick-on-hackrank = " + Server.hackrank_kick.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("hackrank-kick-time = " + Server.hackrank_kick_time.ToString(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("#Admin Verification");
            w.WriteLine("admin-verification = " + Server.verifyadmins.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("verify-admin-perm = " + ((sbyte)Server.verifyadminsrank).ToString(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("#Spam Control");
            w.WriteLine("mute-on-spam = " + Server.checkspam.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("spam-messages = " + Server.spamcounter.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("spam-mute-time = " + Server.mutespamtime.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("spam-counter-reset-time = " + Server.spamcountreset.ToString(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("#Show Empty Ranks in /players");
            w.WriteLine("show-empty-ranks = " + Server.showEmptyRanks.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("#Global Chat Settings");
            w.WriteLine("global-chat-enabled = " + Server.UseGlobalChat.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("global-chat-nick = " + Server.GlobalChatNick);
            w.WriteLine("global-chat-color = " + Server.GlobalChatColor);
            w.WriteLine();
            w.WriteLine("#Griefer_stone Settings");
            w.WriteLine("griefer-stone-tempban = " + Server.grieferStoneBan.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("griefer-stone-type = " + Block.Name(Server.grieferStoneType));
            w.WriteLine("griefer-stone-rank = " + ((sbyte)Server.grieferStoneRank).ToString(CultureInfo.CurrentCulture));
            w.WriteLine();
            w.WriteLine("#WoM Settings");
            w.WriteLine("wom-direct = " + Server.WomDirect.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("wom-serveralt = " + Server.Server_ALT);
            w.WriteLine("wom-serverdis = " + Server.Server_Disc);
            w.WriteLine("wom-serverflag = " + Server.Server_Flag);
            w.WriteLine("wom-textures = " + Server.UseTextures);
            w.WriteLine("");
            w.WriteLine("#Review settings");
            w.WriteLine("review-view-perm = " + ((sbyte)Server.reviewview).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("review-enter-perm = " + ((sbyte)Server.reviewenter).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("review-leave-perm = " + ((sbyte)Server.reviewleave).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("review-cooldown = " + Server.reviewcooldown.ToString(CultureInfo.CurrentCulture));
            w.WriteLine("review-clear-perm = " + ((sbyte)Server.reviewclear).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("review-next-perm = " + ((sbyte)Server.reviewnext).ToString(CultureInfo.CurrentCulture));
            w.WriteLine("bufferblocks = " + Server.bufferblocks);
            w.WriteLine("");
            w.WriteLine("#Translation settings");
            w.WriteLine("translation-enabled = " + Server.transenabled.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
            w.WriteLine("translation-language = " + Server.translang.ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture));
        }
    }
}