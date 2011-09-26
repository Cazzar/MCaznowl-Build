/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
Dual-licensed under the Educational Community License, Version 2.0 and
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MCForge.Gui
{
    public partial class PropertyWindow : Form
    {
        Form lavaMapBrowser;
        System.Timers.Timer lavaUpdateTimer;
        string lsLoadedMap = "";

        public PropertyWindow()
        {
            InitializeComponent();
            {
                ChkPortResult.Text = "Port Check Not Started";
            }
        }

        private void PropertyWindow_Load(object sender, EventArgs e)
        {
            Icon = Gui.Window.ActiveForm.Icon;
            lavaMapBrowser = new LavaMapBrowser();

            Object[] colors = new Object[16];
            colors[0] = ("black"); colors[1] = ("navy");
            colors[2] = ("green"); colors[3] = ("teal");
            colors[4] = ("maroon"); colors[5] = ("purple");
            colors[6] = ("gold"); colors[7] = ("silver");
            colors[8] = ("gray"); colors[9] = ("blue");
            colors[10] = ("lime"); colors[11] = ("aqua");
            colors[12] = ("red"); colors[13] = ("pink");
            colors[14] = ("yellow"); colors[15] = ("white");
            cmbDefaultColour.Items.AddRange(colors);
            cmbIRCColour.Items.AddRange(colors);
            cmbColor.Items.AddRange(colors);
            cmbGlobalChatColor.Items.AddRange(colors);
            button3.Enabled = Server.WomDirect;
            if (Server.irc == false)
            {
                grpIRC.BackColor = Color.LightGray;
            }
            if (Server.irc == true)
            {
                grpIRC.BackColor = Color.White;
            }

            if (Server.useMySQL == false)
            {
                grpSQL.BackColor = Color.LightGray;
            }
            if (Server.useMySQL == true)
            {
                grpSQL.BackColor = Color.White;
            }

            string opchatperm = "";
            string adminchatperm = "";
            string verifyadminsperm = "";
            string grieferstonerank = "";
            foreach (Group grp in Group.GroupList)
            {
                cmbDefaultRank.Items.Add(grp.name);
                cmbOpChat.Items.Add(grp.name);
                cmbAdminChat.Items.Add(grp.name);
                cmbVerificationRank.Items.Add(grp.name);
                lsCmbSetupRank.Items.Add(grp.name);
                lsCmbControlRank.Items.Add(grp.name);
                cmbGrieferStoneRank.Items.Add(grp.name);
                if (grp.Permission == Server.opchatperm)
                {
                    opchatperm = grp.name;
                }
                if (grp.Permission == Server.adminchatperm)
                {
                    adminchatperm = grp.name;
                }
                if (grp.Permission == Server.verifyadminsrank)
                {
                    verifyadminsperm = grp.name;
                }
                if (grp.Permission == Server.grieferStoneRank)
                {
                    grieferstonerank = grp.name;
                }
            }
            listPasswords.Items.Clear();
            if (Directory.Exists("extra/passwords"))
            {
                DirectoryInfo di = new DirectoryInfo("extra/passwords/");
                FileInfo[] fi = di.GetFiles("*.xml");
                Thread.Sleep(10);
                foreach (FileInfo file in fi)
                {
                    listPasswords.Items.Add(file.Name.Replace(".xml", ""));
                }
            }
            cmbDefaultRank.SelectedIndex = 1;
            cmbOpChat.SelectedIndex = (opchatperm != "") ? cmbOpChat.Items.IndexOf(opchatperm) : 1;
            cmbAdminChat.SelectedIndex = (adminchatperm != "") ? cmbAdminChat.Items.IndexOf(adminchatperm) : 1;
            cmbVerificationRank.SelectedIndex = (verifyadminsperm != "") ? cmbVerificationRank.Items.IndexOf(verifyadminsperm) : 1;
            cmbGrieferStoneRank.SelectedIndex = (grieferstonerank != "") ? cmbGrieferStoneRank.Items.IndexOf(grieferstonerank) : 1;

            for (byte b = 1; b < 50; b++)
                cmbGrieferStoneType.Items.Add(Block.Name(b));

            //Load server stuff
            LoadProp("properties/server.properties");
            LoadRanks();
            try
            {
                LoadCommands();
                LoadBlocks();
            }
            catch
            {
                Server.s.Log("Failed to load commands and blocks!");
            }

            try
            {
                LoadLavaSettings();
                UpdateLavaMapList();
                UpdateLavaControls();
            }
            catch
            {
                Server.s.Log("Failed to load Lava Survival settings!");
            }

            try
            {
                lavaUpdateTimer = new System.Timers.Timer(10000);
                lavaUpdateTimer.AutoReset = true;
                lavaUpdateTimer.Elapsed += new System.Timers.ElapsedEventHandler(delegate
                {
                    UpdateLavaControls();
                    UpdateLavaMapList(false);
                });
                lavaUpdateTimer.Start();
            }
            catch
            {
                Server.s.Log("Failed to start lava control update timer!");
            }
        }

        public static bool EditTextOpen = false;

        private void PropertyWindow_Unload(object sender, EventArgs e)
        {
            lavaUpdateTimer.Dispose();
            lavaMapBrowser.Dispose();
            Window.prevLoaded = false;
        }

        List<Group> storedRanks = new List<Group>();
        List<GrpCommands.rankAllowance> storedCommands = new List<GrpCommands.rankAllowance>();
        List<Block.Blocks> storedBlocks = new List<Block.Blocks>();

        public void LoadRanks()
        {
            txtCmdRanks.Text = "The following ranks are available: \r\n\r\n";
            listRanks.Items.Clear();
            storedRanks.Clear();
            storedRanks.AddRange(Group.GroupList);
            foreach (Group grp in storedRanks)
            {
                txtCmdRanks.Text += "\t" + grp.name + " (" + (int)grp.Permission + ")\r\n";
                listRanks.Items.Add(grp.trueName + " = " + (int)grp.Permission);
            }
            txtBlRanks.Text = txtCmdRanks.Text;
            listRanks.SelectedIndex = 0;
        }
        public void SaveRanks()
        {
            Group.saveGroups(storedRanks);
            Group.InitAll();
            LoadRanks();
        }

        public void LoadCommands()
        {
            listCommands.Items.Clear();
            storedCommands.Clear();
            foreach (GrpCommands.rankAllowance aV in GrpCommands.allowedCommands)
            {
                storedCommands.Add(aV);
                listCommands.Items.Add(aV.commandName);
            }
            if (listCommands.SelectedIndex == -1)
                listCommands.SelectedIndex = 0;
        }
        public void SaveCommands()
        {
            GrpCommands.Save(storedCommands);
            GrpCommands.fillRanks();
            LoadCommands();
        }

        public void LoadBlocks()
        {
            listBlocks.Items.Clear();
            storedBlocks.Clear();
            storedBlocks.AddRange(Block.BlockList);
            foreach (Block.Blocks bs in storedBlocks)
            {
                if (Block.Name(bs.type) != "unknown")
                    listBlocks.Items.Add(Block.Name(bs.type));
            }
            if (listBlocks.SelectedIndex == -1)
                listBlocks.SelectedIndex = 0;
        }
        public static bool prevLoaded = false;
        Form PropertyForm;
        //Form UpdateForm; // doesnt seem to be used, uncomment as needed.
        //Form EditTxtForm;

        public void SaveBlocks()
        {
            Block.SaveBlocks(storedBlocks);
            Block.SetBlocks();
            LoadBlocks();
        }

        public void LoadProp(string givenPath)
        {
            //int count = 0;
            if (File.Exists(givenPath))
            {
                string[] lines = File.ReadAllLines(givenPath);

                foreach (string line in lines)
                {
                    if (line != "" && line[0] != '#')
                    {
                        //int index = line.IndexOf('=') + 1; // not needed if we use Split('=')
                        string key = line.Split('=')[0].Trim();
                        string value = "";
                        if (line.IndexOf('=') >= 0)
                            value = line.Substring(line.IndexOf('=') + 1).Trim(); // allowing = in the values
                        string color = "";

                        switch (key.ToLower())
                        {
                            case "server-name":
                                if (ValidString(value, "![]:.,{}~-+()?_/\\' ")) txtName.Text = value;
                                else txtName.Text = "[MCForge] Minecraft server";
                                break;
                            case "motd":
                                if (ValidString(value, "=![]&:.,{}~-+()?_/\\' ")) txtMOTD.Text = value; // allow = in the motd
                                else txtMOTD.Text = "Welcome to my server!";
                                break;
                            case "port":
                                try { txtPort.Text = Convert.ToInt32(value).ToString(); }
                                catch { txtPort.Text = "25565"; }
                                break;
                            case "verify-names":
                                chkVerify.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "public":
                                chkPublic.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "world-chat":
                                chkWorld.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "max-players":
                                try
                                {
                                    if (Convert.ToByte(value) > 128)
                                    {
                                        value = "128";
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1";
                                    }
                                    numPlayers.Value = Convert.ToInt16(value);
                                }
                                catch
                                {
                                    Server.s.Log("max-players invalid! setting to default.");
                                    numPlayers.Value = 12;
                                }
                                numGuests.Maximum = numPlayers.Value;
                                break;
                            case "max-guests":
                                try
                                {
                                    if (Convert.ToByte(value) > numPlayers.Value)
                                    {
                                        value = numPlayers.Value.ToString();
                                    }
                                    else if (Convert.ToByte(value) < 0)
                                    {
                                        value = "0";
                                    }
                                    numGuests.Minimum = 0;
                                    numGuests.Maximum = numPlayers.Value;
                                    numGuests.Value = Convert.ToInt16(value);
                                }
                                catch
                                {
                                    Server.s.Log("max-guests invalid! setting to default.");
                                    numGuests.Value = 10;
                                }
                                break;
                            case "max-maps":
                                try
                                {
                                    if (Convert.ToByte(value) > 100)
                                    {
                                        value = "100";
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1";
                                    }
                                    txtMaps.Text = value;
                                }
                                catch
                                {
                                    Server.s.Log("max-maps invalid! setting to default.");
                                    txtMaps.Text = "5";
                                }
                                break;
                            case "irc":
                                chkIRC.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "irc-server":
                                txtIRCServer.Text = value;
                                break;
                            case "irc-port":
                                txtIRCPort.Text = value;
                                break;
                            case "irc-nick":
                                txtNick.Text = value;
                                break;
                            case "irc-channel":
                                txtChannel.Text = value;
                                break;
                            case "irc-opchannel":
                                txtOpChannel.Text = value;
                                break;
                            case "irc-identify":
                                chkIrcId.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "irc-password":
                                txtIrcId.Text = value;
                                break;
                            case "anti-tunnels":
                                ChkTunnels.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "max-depth":
                                txtDepth.Text = value;
                                break;

                            case "rplimit":
                                try { txtRP.Text = value; }
                                catch { txtRP.Text = "500"; }
                                break;
                            case "rplimit-norm":
                                try { txtNormRp.Text = value; }
                                catch { txtNormRp.Text = "10000"; }
                                break;

                            case "log-heartbeat":
                                chkLogBeat.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "force-cuboid":
                                chkForceCuboid.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "profanity-filter":
                                chkProfanityFilter.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "notify-on-join-leave":
                                chkNotifyOnJoinLeave.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "backup-time":
                                if (Convert.ToInt32(value) > 1) txtBackup.Text = value; else txtBackup.Text = "300";
                                break;

                            case "backup-location":
                                if (!value.Contains("System.Windows.Forms.TextBox, Text:"))
                                    txtBackupLocation.Text = value;
                                break;

                            case "physicsrestart":
                                chkPhysicsRest.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "deathcount":
                                chkDeath.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "defaultcolor":
                                color = c.Parse(value);
                                if (color == "")
                                {
                                    color = c.Name(value); if (color != "") color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                cmbDefaultColour.SelectedIndex = cmbDefaultColour.Items.IndexOf(c.Name(value)); break;

                            case "irc-color":
                                color = c.Parse(value);
                                if (color == "")
                                {
                                    color = c.Name(value); if (color != "") color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                cmbIRCColour.SelectedIndex = cmbIRCColour.Items.IndexOf(c.Name(value)); break;
                            case "default-rank":
                                try
                                {
                                    if (cmbDefaultRank.Items.IndexOf(value.ToLower()) != -1)
                                        cmbDefaultRank.SelectedIndex = cmbDefaultRank.Items.IndexOf(value.ToLower());
                                }
                                catch { cmbDefaultRank.SelectedIndex = 1; }
                                break;

                            case "old-help":
                                chkHelp.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "cheapmessage":
                                chkCheap.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "cheap-message-given":
                                txtCheap.Text = value;
                                break;

                            case "rank-super":
                                chkrankSuper.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "custom-ban":
                                chkBanMessage.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "custom-ban-message":
                                txtBanMessage.Text = value;
                                break;

                            case "custom-shutdown":
                                chkShutdown.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "custom-shutdown-message":
                                txtShutdown.Text = value;
                                break;

                            case "custom-griefer-stone":
                                chkGrieferStone.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "custom-griefer-stone-message":
                                txtGrieferStone.Text = value;
                                break;

                            case "auto-restart":
                                chkRestartTime.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "restarttime":
                                txtRestartTime.Text = value;
                                break;
                            case "afk-minutes":
                                try { txtafk.Text = Convert.ToInt16(value).ToString(); }
                                catch { txtafk.Text = "10"; }
                                break;

                            case "afk-kick":
                                try { txtAFKKick.Text = Convert.ToInt16(value).ToString(); }
                                catch { txtAFKKick.Text = "45"; }
                                break;

                            case "check-updates":
                                chkUpdates.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "autoload":
                                chkAutoload.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "parse-emotes":
                                chkSmile.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "allow-tp-to-higher-ranks":
                                chkTpToHigherRanks.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "agree-to-rules-on-entry":
                                chkAgreeToRules.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "admins-join-silent":
                                chkAdminsJoinSilent.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "main-name":
                                txtMain.Text = value;
                                break;
                            case "dollar-before-dollar":
                                chk17Dollar.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "money-name":
                                txtMoneys.Text = value;
                                break;
                            case "mono":
                                chkMono.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "restart-on-error":
                                chkRestart.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "repeat-messages":
                                chkRepeatMessages.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "host-state":
                                if (value != "") txtHost.Text = value;
                                break;
                            case "kick-on-hackrank":
                                hackrank_kick.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "hackrank-kick-time":
                                hackrank_kick_time.Text = value;
                                break;
                            case "server-owner":
                                txtServerOwner.Text = value;
                                break;
                            case "zombie-on-server-start":
                                chkZombieOnServerStart.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "no-respawning-during-zombie":
                                chkNoRespawnDuringZombie.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "no-level-saving-during-zombie":
                                chkNoLevelSavingDuringZombie.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "no-pillaring-during-zombie":
                                chkNoPillaringDuringZombie.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "zombie-name-while-infected":
                                ZombieName.Text = value;
                                break;
                            case "ignore-ops":
                                chkIgnoreGlobal.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "admin-verification":
                                 chkEnableVerification.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "usemysql":
                                chkUseSQL.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "username":
                                if (value != "") txtSQLUsername.Text = value;
                                break;
                            case "password":
                                if (value != "") txtSQLPassword.Text = value;
                                break;
                            case "databasename":
                                if (value != "") txtSQLDatabase.Text = value;
                                break;
                            case "host":
                                try
                                {
                                    IPAddress.Parse(value);
                                    txtSQLHost.Text = value;
                                }
                                catch
                                {
                                    txtSQLHost.Text = "127.0.0.1";
                                }
                                break;
                            case "mute-on-spam":
                                chkSpamControl.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                            case "spam-messages":
                                try
                                {
                                    numSpamMessages.Value = Convert.ToInt16(value);
                                }
                                catch
                                {
                                    numSpamMessages.Value = 8;
                                }
                                break;
                            case "spam-mute-time":
                                try
                                {
                                    numSpamMute.Value = Convert.ToInt16(value);
                                }
                                catch
                                {
                                    numSpamMute.Value = 60;
                                }
                                break;
                            case "show-empty-ranks":
                                chkShowEmptyRanks.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "global-chat-enabled":
                                chkGlobalChat.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "global-chat-nick":
                                if (value != "") txtGlobalChatNick.Text = value;
                                break;

                            case "global-chat-color":
                                color = c.Parse(value);
                                if (color == "")
                                {
                                    color = c.Name(value); if (color != "") color = value; else { Server.s.Log("Could not find " + value); return; }
                                }
                                cmbGlobalChatColor.SelectedIndex = cmbGlobalChatColor.Items.IndexOf(c.Name(value)); break;

                            case "griefer-stone-tempban":
                                chkGrieferStoneBan.Checked = (value.ToLower() == "true") ? true : false;
                                break;

                            case "griefer-stone-type":
                                try { cmbGrieferStoneType.SelectedIndex = cmbGrieferStoneType.Items.IndexOf(value); }
                                catch
                                {
                                    try { cmbGrieferStoneType.SelectedIndex = cmbGrieferStoneType.Items.IndexOf(Block.Name(Convert.ToByte(value))); }
                                    catch { Server.s.Log("Could not find " + value); }
                                }
                                break;
                            case "wom-direct":
                                chkWomDirect.Checked = (value.ToLower() == "true") ? true : false;
                                break;
                        }
                    }
                }
                //Save(givenPath);
            }
            //else Save(givenPath);
        }
        public bool ValidString(string str, string allowed)
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

        public void Save(string givenPath)
        {
            try
            {
                StreamWriter w = new StreamWriter(File.Create(givenPath));
                if (givenPath.IndexOf("server") != -1)
                {
                    w.WriteLine("# Edit the settings below to modify how your server operates. This is an explanation of what each setting does.");
                    w.WriteLine("# server-name\t=\tThe name which displays on minecraft.net");
                    w.WriteLine("# motd\t=\tThe message which displays when a player connects");
                    w.WriteLine("# port\t=\tThe port to operate from");
                    w.WriteLine("# console-only\t=\tRun without a GUI (useful for Linux servers with mono)");
                    w.WriteLine("# verify-names\t=\tVerify the validity of names");
                    w.WriteLine("# public\t=\tSet to true to appear in the public server list");
                    w.WriteLine("# max-players\t=\tThe maximum number of connections");
                    w.WriteLine("# max-guests\t=\tThe maximum number of guests allowed");
                    w.WriteLine("# max-maps\t=\tThe maximum number of maps loaded at once");
                    w.WriteLine("# world-chat\t=\tSet to true to enable world chat");
                    w.WriteLine("# guest-goto\t=\tSet to true to give guests goto and levels commands");
                    w.WriteLine("# irc\t=\tSet to true to enable the IRC bot");
                    w.WriteLine("# irc-nick\t=\tThe name of the IRC bot");
                    w.WriteLine("# irc-server\t=\tThe server to connect to");
                    w.WriteLine("# irc-channel\t=\tThe channel to join");
                    w.WriteLine("# irc-opchannel\t=\tThe channel to join (posts OpChat)");
                    w.WriteLine("# irc-port\t=\tThe port to use to connect");
                    w.WriteLine("# irc-identify\t=(true/false)\tDo you want the IRC bot to Identify itself with nickserv. Note: You will need to register it's name with nickserv manually.");
                    w.WriteLine("# irc-password\t=\tThe password you want to use if you're identifying with nickserv");
                    w.WriteLine("# anti-tunnels\t=\tStops people digging below max-depth");
                    w.WriteLine("# max-depth\t=\tThe maximum allowed depth to dig down");
                    w.WriteLine("# backup-time\t=\tThe number of seconds between automatic backups");
                    w.WriteLine("# overload\t=\tThe higher this is, the longer the physics is allowed to lag. Default 1500");
                    w.WriteLine("# use-whitelist\t=\tSwitch to allow use of a whitelist to override IP bans for certain players. Default false.");
                    w.WriteLine("# force-cuboid\t=\tRun cuboid until the limit is hit, instead of canceling the whole operation. Default false.");
                    w.WriteLine("# profanity-filter\t=\tReplace certain bad words in the chat. Default false.");
                    w.WriteLine("# notify-on-join-leave\t=\tShow a balloon popup in tray notification area when a player joins/leaves the server. Default false.");
                    w.WriteLine("# allow-tp-to-higher-ranks\t=\tAllows the teleportation to players of higher ranks");
                    w.WriteLine("# agree-to-rules-on-entry\t=\tForces all new players to the server to agree to the rules before they can build or use commands.");
                    w.WriteLine("# adminchat-perm\t=\tThe rank required to view adminchat. Default rank is superop.");
                    w.WriteLine("# admins-join-silent\t=\tPlayers who have adminchat permission join the game silently. Default true");
                    w.WriteLine("# server-owner\t=\tThe minecraft name, of the owner of the server.");
                    w.WriteLine("# zombie-on-server-start\t=\tStarts Zombie Survival when server is started.");
                    w.WriteLine("# no-respawning-during-zombie\t=\tDisables respawning (Pressing R) while Zombie is on.");
                    w.WriteLine("# no-level-saving-during-zombie\t=\tDisables level saving while Zombie Survival is activated.");
                    w.WriteLine("# no-pillaring-during-zombie\t=\tDisables pillaring while Zombie Survival is activated.");
                    w.WriteLine("# zombie-name-while-infected\t=\tSets the zombies name while actived if there is a value.");
                    w.WriteLine();
                    w.WriteLine("# Host\t=\tThe host name for the database (usually 127.0.0.1)");
                    w.WriteLine("# SQLPort\t=\tPort number to be used for MySQL. Unless you manually changed the port, leave this alone. Default 3306.");
                    w.WriteLine("# Username\t=\tThe username you used to create the database (usually root)");
                    w.WriteLine("# Password\t=\tThe password set while making the database");
                    w.WriteLine("# DatabaseName\t=\tThe name of the database stored (Default = MCZall)");
                    w.WriteLine();
                    w.WriteLine("# defaultColor\t=\tThe color code of the default messages (Default = &e)");
                    w.WriteLine("# ignore-ops\t=\tDecides whether or not an operator can be ignored. Default false.");
                    w.WriteLine();
                    w.WriteLine("# admin-verification\t=\tDetermines whether admins have to verify on entry to the server. Default true.");
                    w.WriteLine("# verify-admin-perm\t=\tThe minimum rank required for admin verification to occur.");
                    w.WriteLine();
                    w.WriteLine("# mute-on-spam\t=\tIf enabled it mutes a player for spamming. Default false.");
                    w.WriteLine("# spam-messages\t=\tThe amount of messages that have to be sent consecutively to be muted.");
                    w.WriteLine("# spam-mute-time\t=\tThe amount of seconds a player is muted for spam.");
                    w.WriteLine();
                    w.WriteLine("# Server options");
                    w.WriteLine("server-name = " + txtName.Text);
                    w.WriteLine("motd = " + txtMOTD.Text);
                    w.WriteLine("port = " + txtPort.Text);
                    w.WriteLine("verify-names = " + chkVerify.Checked.ToString().ToLower());
                    w.WriteLine("public = " + chkPublic.Checked.ToString().ToLower());
                    w.WriteLine("max-players = " + numPlayers.Value.ToString());
                    w.WriteLine("max-guests = " + numGuests.Value.ToString());
                    w.WriteLine("max-maps = " + txtMaps.Text);
                    w.WriteLine("world-chat = " + chkWorld.Checked.ToString().ToLower());
                    w.WriteLine("check-updates = " + chkUpdates.Checked.ToString().ToLower());
                    w.WriteLine("autoload = " + chkAutoload.Checked.ToString().ToLower());
                    w.WriteLine("auto-restart = " + chkRestartTime.Checked.ToString().ToLower());
                    w.WriteLine("restarttime = " + txtRestartTime.Text);
                    w.WriteLine("restart-on-error = " + chkRestart.Checked);
                    if (Player.ValidName(txtMain.Text)) w.WriteLine("main-name = " + txtMain.Text);
                    else w.WriteLine("main-name = main");
                    w.WriteLine();
                    w.WriteLine("# irc bot options");
                    w.WriteLine("irc = " + chkIRC.Checked.ToString());
                    w.WriteLine("irc-nick = " + txtNick.Text);
                    w.WriteLine("irc-server = " + txtIRCServer.Text);
                    w.WriteLine("irc-channel = " + txtChannel.Text);
                    w.WriteLine("irc-opchannel = " + txtOpChannel.Text);
                    w.WriteLine("irc-port = " + txtIRCPort.Text);
                    w.WriteLine("irc-identify = " + chkIrcId.Checked.ToString().ToLower());
                    w.WriteLine("irc-password = " + txtIrcId.Text);
                    w.WriteLine();
                    w.WriteLine("# other options");
                    w.WriteLine("anti-tunnels = " + ChkTunnels.Checked.ToString().ToLower());
                    w.WriteLine("max-depth = " + txtDepth.Text);
                    w.WriteLine("rplimit = " + txtRP.Text);
                    w.WriteLine("physicsrestart = " + chkPhysicsRest.Checked.ToString().ToLower());
                    w.WriteLine("old-help = " + chkHelp.Checked.ToString().ToLower());
                    w.WriteLine("deathcount = " + chkDeath.Checked.ToString().ToLower());
                    w.WriteLine("afk-minutes = " + txtafk.Text);
                    w.WriteLine("afk-kick = " + txtAFKKick.Text);
                    w.WriteLine("parse-emotes = " + chkSmile.Checked.ToString().ToLower());
                    w.WriteLine("dollar-before-dollar = " + chk17Dollar.Checked.ToString().ToLower());
                    w.WriteLine("use-whitelist = " + Server.useWhitelist.ToString().ToLower());
                    w.WriteLine("money-name = " + txtMoneys.Text);
                    w.WriteLine("opchat-perm = " + ((sbyte)Group.GroupList.Find(grp => grp.name == cmbOpChat.Items[cmbOpChat.SelectedIndex].ToString()).Permission).ToString());
                    w.WriteLine("adminchat-perm = " + ((sbyte)Group.GroupList.Find(grp => grp.name == cmbAdminChat.Items[cmbAdminChat.SelectedIndex].ToString()).Permission).ToString());
                    w.WriteLine("admins-join-silent = " + chkAdminsJoinSilent.Checked.ToString().ToLower());
                    w.WriteLine("log-heartbeat = " + chkLogBeat.Checked.ToString().ToLower());
                    w.WriteLine("force-cuboid = " + chkForceCuboid.Checked.ToString().ToLower());
                    w.WriteLine("profanity-filter = " + chkProfanityFilter.Checked.ToString().ToLower());
                    w.WriteLine("notify-on-join-leave = " + chkNotifyOnJoinLeave.Checked.ToString().ToLower());
                    w.WriteLine("repeat-messages = " + chkRepeatMessages.Checked.ToString());
                    w.WriteLine("host-state = " + txtHost.Text.ToString());
                    w.WriteLine("agree-to-rules-on-entry = " + chkAgreeToRules.Checked.ToString().ToLower());
                    w.WriteLine("kick-on-hackrank = " + hackrank_kick.Checked.ToString().ToLower());
                    w.WriteLine("hackrank-kick-time = " + hackrank_kick_time.Text);
                    w.WriteLine("ignore-ops = " + chkIgnoreGlobal.Checked.ToString().ToLower());
                    w.WriteLine("zombie-on-server-start = " + chkZombieOnServerStart.Checked.ToString().ToLower());
                    w.WriteLine("no-respawning-during-zombie = " + chkNoRespawnDuringZombie.Checked.ToString().ToLower());
                    w.WriteLine("no-level-saving-during-zombie = " + chkNoLevelSavingDuringZombie.Checked.ToString().ToLower());
                    w.WriteLine("no-pillaring-during-zombie = " + chkNoPillaringDuringZombie.Checked.ToString().ToLower());
                    w.WriteLine("zombie-name-while-infected = " + ZombieName.Text);
                    w.WriteLine();
                    w.WriteLine("# backup options");
                    w.WriteLine("backup-time = " + txtBackup.Text);
                    w.WriteLine("backup-location = " + txtBackupLocation.Text);
                    w.WriteLine();
                    w.WriteLine("#Error logging");
                    w.WriteLine("report-back = " + Server.reportBack.ToString().ToLower());
                    w.WriteLine();
                    w.WriteLine("#MySQL information");
                    w.WriteLine("UseMySQL = " + chkUseSQL.Checked.ToString().ToLower());
                    w.WriteLine("Host = " + txtSQLHost.Text);
                    w.WriteLine("SQLPort = " + Server.MySQLPort);
                    w.WriteLine("Username = " + txtSQLUsername.Text);
                    w.WriteLine("Password = " + txtSQLPassword.Text);
                    w.WriteLine("DatabaseName = " + txtSQLDatabase.Text);
                    w.WriteLine("Pooling = " + Server.MySQLPooling);
                    w.WriteLine();
                    w.WriteLine("#Colors");
                    w.WriteLine("defaultColor = " + cmbDefaultColour.Items[cmbDefaultColour.SelectedIndex].ToString());
                    w.WriteLine("irc-color = " + cmbIRCColour.Items[cmbIRCColour.SelectedIndex].ToString());
                    w.WriteLine();
                    w.WriteLine("#Running on mono?");
                    w.WriteLine("mono = " + chkMono.Checked.ToString().ToLower());
                    w.WriteLine();
                    w.WriteLine("#Custom Messages");
                    w.WriteLine("custom-ban = " + chkBanMessage.Checked.ToString().ToLower());
                    w.WriteLine("custom-ban-message = " + txtBanMessage.Text);
                    w.WriteLine("custom-shutdown = " + chkShutdown.Checked.ToString().ToLower());
                    w.WriteLine("custom-shutdown-message = " + txtShutdown.Text);
                    w.WriteLine("custom-griefer-stone = " + chkGrieferStone.Checked.ToString().ToLower());
                    w.WriteLine("custom-griefer-stone-message = " + txtGrieferStone.Text);
                    w.WriteLine("allow-tp-to-higher-ranks = " + chkTpToHigherRanks.Checked.ToString().ToLower());
                    w.WriteLine("server-owner = " + txtServerOwner.Text);
                    w.WriteLine();
                    w.WriteLine("cheapmessage = " + chkCheap.Checked.ToString().ToLower());
                    w.WriteLine("cheap-message-given = " + txtCheap.Text);
                    w.WriteLine("rank-super = " + chkrankSuper.Checked.ToString().ToLower());
                    w.WriteLine("default-rank = " + cmbDefaultRank.Items[cmbDefaultRank.SelectedIndex].ToString());
                    w.WriteLine();
                    w.WriteLine("#Admin Verification");
                    w.WriteLine("admin-verification = " + chkEnableVerification.Checked.ToString().ToLower());
                    w.WriteLine("verify-admin-perm = " + ((sbyte)Group.GroupList.Find(grp => grp.name == cmbVerificationRank.Items[cmbVerificationRank.SelectedIndex].ToString()).Permission).ToString());
                    w.WriteLine();
                    w.WriteLine("#Spam Control");
                    w.WriteLine("mute-on-spam = " + chkSpamControl.Checked.ToString().ToLower());
                    w.WriteLine("spam-messages = " + numSpamMessages.Value.ToString());
                    w.WriteLine("spam-mute-time = " + numSpamMute.Value.ToString());
                    w.WriteLine();
                    w.WriteLine("#Show Empty Ranks in /players");
                    w.WriteLine("show-empty-ranks = " + chkShowEmptyRanks.Checked.ToString().ToLower());
                    w.WriteLine();
                    w.WriteLine("#Global Chat Settings");
                    w.WriteLine("global-chat-enabled = " + chkGlobalChat.Checked.ToString().ToLower());
                    w.WriteLine("global-chat-nick = " + txtGlobalChatNick.Text);
                    w.WriteLine("global-chat-color = " + cmbGlobalChatColor.Items[cmbGlobalChatColor.SelectedIndex].ToString());
                    w.WriteLine();
                    w.WriteLine("#Griefer_stone Settings");
                    w.WriteLine("griefer-stone-tempban = " + chkGrieferStoneBan.Checked.ToString().ToLower());
                    w.WriteLine("griefer-stone-type = " + cmbGrieferStoneType.Items[cmbGrieferStoneType.SelectedIndex].ToString());
                    w.WriteLine("griefer-stone-rank = " + ((sbyte)Group.GroupList.Find(grp => grp.name == cmbGrieferStoneRank.Items[cmbGrieferStoneRank.SelectedIndex].ToString()).Permission).ToString());
                    w.WriteLine();
                    w.WriteLine("#WoM Direct thingy");
                    w.WriteLine("wom-direct = " + chkWomDirect.Checked.ToString().ToLower());
                }
                w.Flush();
                w.Close();
                w.Dispose();
            }
            catch
            {
                Server.s.Log("SAVE FAILED! " + givenPath);
            }
        }

        private void cmbDefaultColour_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDefault.BackColor = Color.FromName(cmbDefaultColour.Items[cmbDefaultColour.SelectedIndex].ToString());
        }

        private void cmbIRCColour_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblIRC.BackColor = Color.FromName(cmbIRCColour.Items[cmbIRCColour.SelectedIndex].ToString());
        }

        void removeDigit(TextBox foundTxt)
        {
            try
            {
                int lastChar = int.Parse(foundTxt.Text[foundTxt.Text.Length - 1].ToString());
            }
            catch
            {
                foundTxt.Text = "";
            }
        }

        private void txtPort_TextChanged(object sender, EventArgs e) { removeDigit(txtPort); }
        private void txtMaps_TextChanged(object sender, EventArgs e) { removeDigit(txtMaps); }
        private void txtBackup_TextChanged(object sender, EventArgs e) { removeDigit(txtBackup); }
        private void txtDepth_TextChanged(object sender, EventArgs e) { removeDigit(txtDepth); }

        private void btnSave_Click(object sender, EventArgs e) { saveStuff(); Dispose(); }
        private void btnApply_Click(object sender, EventArgs e) { saveStuff(); }

        void saveStuff()
        {
            foreach (Control tP in tabControl.Controls)
                if (tP is TabPage && tP != tabPage3 && tP != tabPage5)
                    foreach (Control ctrl in tP.Controls)
                        if (ctrl is TextBox)
                            if (ctrl.Text == "")
                            {
                                MessageBox.Show("A textbox has been left empty. It must be filled.\n" + ctrl.Name);
                                return;
                            }

            Save("properties/server.properties");
            SaveRanks();
            SaveCommands();
            SaveBlocks();
            try { SaveLavaSettings(); }
            catch { Server.s.Log("Error saving Lava Survival settings!"); }

            Properties.Load("properties/server.properties", true);
            GrpCommands.fillRanks();

            // Trigger profanity filter reload
            // Not the best way of doing things, but it kinda works
            ProfanityFilter.Init();
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void chkPhysicsRest_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkGC_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void chkIRC_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIRC.Checked.ToString().ToLower() == "false")
            {
                grpIRC.BackColor = Color.LightGray;
            }
            if (chkIRC.Checked.ToString().ToLower() == "true")
            {
                grpIRC.BackColor = Color.White;
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            /*FolderBrowserDialog folderDialog = new FolderBrowserDialog();
folderDialog.Description = "Select Folder";
if (folderDialog.ShowDialog() == DialogResult.OK) {
txtBackupLocation.Text = folderDialog.SelectedPath;
}*/
            MessageBox.Show("Currently glitchy! Just type in the location by hand.");
        }

        #region rankTab
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblColor.BackColor = Color.FromName(cmbColor.Items[cmbColor.SelectedIndex].ToString());
            storedRanks[listRanks.SelectedIndex].color = c.Parse(cmbColor.Items[cmbColor.SelectedIndex].ToString());
        }

        bool skip = false;
        private void listRanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (skip) return;
            Group foundRank = storedRanks.Find(grp => grp.trueName == listRanks.Items[listRanks.SelectedIndex].ToString().Split('=')[0].Trim());
            if (foundRank.Permission == LevelPermission.Nobody) { listRanks.SelectedIndex = 0; return; }

            txtRankName.Text = foundRank.trueName;
            txtPermission.Text = ((int)foundRank.Permission).ToString();
            txtLimit.Text = foundRank.maxBlocks.ToString();
            txtMaxUndo.Text = foundRank.maxUndo.ToString();
            cmbColor.SelectedIndex = cmbColor.Items.IndexOf(c.Name(foundRank.color));
            txtFileName.Text = foundRank.fileName;
        }

        private void txtRankName_TextChanged(object sender, EventArgs e)
        {
            if (txtRankName.Text != "" && txtRankName.Text.ToLower() != "nobody")
            {
                storedRanks[listRanks.SelectedIndex].trueName = txtRankName.Text;
                skip = true;
                listRanks.Items[listRanks.SelectedIndex] = txtRankName.Text + " = " + (int)storedRanks[listRanks.SelectedIndex].Permission;
                skip = false;
            }
        }

        private void txtPermission_TextChanged(object sender, EventArgs e)
        {
            if (txtPermission.Text != "")
            {
                int foundPerm;
                try
                {
                    foundPerm = int.Parse(txtPermission.Text);
                }
                catch
                {
                    if (txtPermission.Text != "-")
                        txtPermission.Text = txtPermission.Text.Remove(txtPermission.Text.Length - 1);
                    return;
                }

                if (foundPerm < -50) { txtPermission.Text = "-50"; return; }
                else if (foundPerm > 119) { txtPermission.Text = "119"; return; }

                storedRanks[listRanks.SelectedIndex].Permission = (LevelPermission)foundPerm;
                skip = true;
                listRanks.Items[listRanks.SelectedIndex] = storedRanks[listRanks.SelectedIndex].trueName + " = " + foundPerm;
                skip = false;
            }
        }

        private void txtLimit_TextChanged(object sender, EventArgs e)
        {
            if (txtLimit.Text != "")
            {
                int foundLimit;
                try
                {
                    foundLimit = int.Parse(txtLimit.Text);
                }
                catch
                {
                    txtLimit.Text = txtLimit.Text.Remove(txtLimit.Text.Length - 1);
                    return;
                }

                if (foundLimit < 1) { txtLimit.Text = "1"; return; }

                storedRanks[listRanks.SelectedIndex].maxBlocks = foundLimit;
            }
        }

        private void txtMaxUndo_TextChanged(object sender, EventArgs e) {
            if (txtMaxUndo.Text != "") {
                long foundMax;
                try {
                    foundMax = long.Parse(txtMaxUndo.Text);
                } catch {
                    txtMaxUndo.Text = txtMaxUndo.Text.Remove(txtMaxUndo.Text.Length - 1);
                    return;
                }

                if (foundMax < -1) { txtMaxUndo.Text = "0"; return; }

                storedRanks[listRanks.SelectedIndex].maxUndo = foundMax;
            }

        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (txtFileName.Text != "")
            {
                storedRanks[listRanks.SelectedIndex].fileName = txtFileName.Text;
            }
        }

        private void btnAddRank_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            Group newGroup = new Group((LevelPermission)5, 600, 30, "CHANGEME", '1', "CHANGEME.txt");
            storedRanks.Add(newGroup);
            listRanks.Items.Add(newGroup.trueName + " = " + (int)newGroup.Permission);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listRanks.Items.Count > 1)
            {
                storedRanks.RemoveAt(listRanks.SelectedIndex);
                skip = true;
                listRanks.Items.RemoveAt(listRanks.SelectedIndex);
                skip = false;

                listRanks.SelectedIndex = 0;
            }
        }
        #endregion

        #region commandTab
        private void listCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            Command cmd = Command.all.Find(listCommands.SelectedItem.ToString());
            GrpCommands.rankAllowance allowVar = storedCommands.Find(aV => aV.commandName == cmd.name);

            if (Group.findPerm(allowVar.lowestRank) == null) allowVar.lowestRank = cmd.defaultRank;
            txtCmdLowest.Text = (int)allowVar.lowestRank + "";

            bool foundOne = false;
            txtCmdDisallow.Text = "";
            foreach (LevelPermission perm in allowVar.disallow)
            {
                foundOne = true;
                txtCmdDisallow.Text += "," + (int)perm;
            }
            if (foundOne) txtCmdDisallow.Text = txtCmdDisallow.Text.Remove(0, 1);

            foundOne = false;
            txtCmdAllow.Text = "";
            foreach (LevelPermission perm in allowVar.allow)
            {
                foundOne = true;
                txtCmdAllow.Text += "," + (int)perm;
            }
            if (foundOne) txtCmdAllow.Text = txtCmdAllow.Text.Remove(0, 1);
        }
        private void txtCmdLowest_TextChanged(object sender, EventArgs e)
        {
            fillLowest(ref txtCmdLowest, ref storedCommands[listCommands.SelectedIndex].lowestRank);
        }
        private void txtCmdDisallow_TextChanged(object sender, EventArgs e)
        {
            fillAllowance(ref txtCmdDisallow, ref storedCommands[listCommands.SelectedIndex].disallow);
        }
        private void txtCmdAllow_TextChanged(object sender, EventArgs e)
        {
            fillAllowance(ref txtCmdAllow, ref storedCommands[listCommands.SelectedIndex].allow);
        }
        #endregion

        #region BlockTab
        private void listBlocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte b = Block.Byte(listBlocks.SelectedItem.ToString());
            Block.Blocks bs = storedBlocks.Find(bS => bS.type == b);

            txtBlLowest.Text = (int)bs.lowestRank + "";

            bool foundOne = false;
            txtBlDisallow.Text = "";
            foreach (LevelPermission perm in bs.disallow)
            {
                foundOne = true;
                txtBlDisallow.Text += "," + (int)perm;
            }
            if (foundOne) txtBlDisallow.Text = txtBlDisallow.Text.Remove(0, 1);

            foundOne = false;
            txtBlAllow.Text = "";
            foreach (LevelPermission perm in bs.allow)
            {
                foundOne = true;
                txtBlAllow.Text += "," + (int)perm;
            }
            if (foundOne) txtBlAllow.Text = txtBlAllow.Text.Remove(0, 1);
        }
        private void txtBlLowest_TextChanged(object sender, EventArgs e)
        {
            fillLowest(ref txtBlLowest, ref storedBlocks[Block.Byte(listBlocks.SelectedItem.ToString())].lowestRank);
        }
        private void txtBlDisallow_TextChanged(object sender, EventArgs e)
        {
            fillAllowance(ref txtBlDisallow, ref storedBlocks[listBlocks.SelectedIndex].disallow);
        }
        private void txtBlAllow_TextChanged(object sender, EventArgs e)
        {
            fillAllowance(ref txtBlAllow, ref storedBlocks[listBlocks.SelectedIndex].allow);
        }
        #endregion
        private void fillAllowance(ref TextBox txtBox, ref List<LevelPermission> addTo)
        {
            addTo.Clear();
            if (txtBox.Text != "")
            {
                string[] perms = txtBox.Text.Split(',');
                for (int i = 0; i < perms.Length; i++)
                {
                    perms[i] = perms[i].Trim().ToLower();
                    int foundPerm;
                    try
                    {
                        foundPerm = int.Parse(perms[i]);
                    }
                    catch
                    {
                        Group foundGroup = Group.Find(perms[i]);
                        if (foundGroup != null) foundPerm = (int)foundGroup.Permission;
                        else { Server.s.Log("Could not find " + perms[i]); continue; }
                    }
                    addTo.Add((LevelPermission)foundPerm);
                }

                txtBox.Text = "";
                foreach (LevelPermission p in addTo)
                {
                    txtBox.Text += "," + (int)p;
                }
                if (txtBox.Text != "") txtBox.Text = txtBox.Text.Remove(0, 1);
            }
        }
        private void fillLowest(ref TextBox txtBox, ref LevelPermission toChange)
        {
            if (txtBox.Text != "")
            {
                txtBox.Text = txtBox.Text.Trim().ToLower();
                int foundPerm = -100;
                try
                {
                    foundPerm = int.Parse(txtBox.Text);
                }
                catch
                {
                    Group foundGroup = Group.Find(txtBox.Text);
                    if (foundGroup != null) foundPerm = (int)foundGroup.Permission;
                    else { Server.s.Log("Could not find " + txtBox.Text); }
                }

                txtBox.Text = "";
                if (foundPerm < -99) txtBox.Text = (int)toChange + "";
                else txtBox.Text = foundPerm + "";

                toChange = (LevelPermission)Convert.ToInt16(txtBox.Text);
            }
        }

        private void btnBlHelp_Click(object sender, EventArgs e)
        {
            getHelp(listBlocks.SelectedItem.ToString());
        }
        private void btnCmdHelp_Click(object sender, EventArgs e)
        {
            getHelp(listCommands.SelectedItem.ToString());
        }
        private void getHelp(string toHelp)
        {
            Player.storedHelp = "";
            Player.storeHelp = true;
            Command.all.Find("help").Use(null, toHelp);
            Player.storeHelp = false;
            string messageInfo = "Help information for " + toHelp + ":\r\n\r\n";
            messageInfo += Player.storedHelp;
            MessageBox.Show(messageInfo);
        }


        private void ChkPort_Click(object sender, EventArgs e)
        {
            ChkPortResult.Text = "Testing Port!";
            ChkPortResult.BackColor = SystemColors.Control;

            int nPort = 0;
            nPort = Int32.Parse(txtPort.Text);

            TcpListener listener = null;
            try
            {
                // Try to open the port. If it fails, the port is probably open already.
                try
                {
                    listener = new TcpListener(IPAddress.Any, (int)nPort);
                    listener.Start();
                }
                catch
                {
                    // Port is probably open already by the server, so let's just continue :)
                    listener = null;
                }

               

               HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://headdetect.tk/port.php?port=" + nPort);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream)){



                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line == "") { continue; }
                            
                            if (line == "open")
                            {
                                ChkPortResult.Text = "Port Open!";
                                ChkPortResult.BackColor = Color.Lime;
                                MessageBox.Show("Port " + nPort + " is open!", "Port check successful");
                                return;
                            }

                        MessageBox.Show("Port " + nPort + " seems to be closed. You may need to set up port forwarding.", "Port check failed");
                        ChkPortResult.Text = "Port Not Open";
                        ChkPortResult.BackColor = Color.Red;
                        
                        
                        }

                        }
                    }
                }
                else { MessageBox.Show("Could Not connect to site, try again later."); }


            }
            catch (Exception ex)
            {
                ChkPortResult.Text = "Testing Port Failed!";
                ChkPortResult.BackColor = Color.Red;
                MessageBox.Show("Could not start listening on port " + nPort + ". Another program may be using the port.", "Port check failed");
                MessageBox.Show(ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }

        }

        private void CrtCustCmd_Click(object sender, EventArgs e)
        {
if (CustCmdtxtBox.Text != null)
{
             if (File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs"))
             {
                 MessageBox.Show("Sorry, That command already exists!!");
             }
             else
             {
             Command.all.Find("cmdcreate").Use(null, CustCmdtxtBox.Text);
MessageBox.Show("Command Created!!");
             }
}
else
{
MessageBox.Show("You didnt specify a name for the command!!");
}
        }

        private void CompileCustCmd_Click(object sender, EventArgs e)
        {
if (CustCmdtxtBox.Text != null)
{
                if (File.Exists("extra/commands/dll/Cmd" + CustCmdtxtBox.Text + ".dll"))
                {
                    MessageBox.Show("Sorry, That command already exists!!");
                }
                else
                {
                    Command.all.Find("compile").Use(null, CustCmdtxtBox.Text);
MessageBox.Show("Command Compiled!!");
                }
}
else
{
MessageBox.Show("You didnt specify a name for the command!!");
}
        }

        private void LoadCustCmd_Click(object sender, EventArgs e)
        {
            Command.all.Find("cmdload").Use(null, CustCmdtxtBox.Text);
        }

        private void LoadIntoTxtBox_Click(object sender, EventArgs e)
        {
if (CustCmdtxtBox.Text != null)
{
                if (!File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs"))
                {
                    MessageBox.Show("Sorry, That command doesn't exist yet - click Create Custom Command Above to create it.");
                }
                else
                {
                    CustCmdTxtBox2.Text = null;
                    CustCmdTxtBox2.Text = File.ReadAllText("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs");
}
            }
else
{
MessageBox.Show("You didnt specify a name for the command to be loaded!!");
}
        }

        private void SaveCustCmd_Click(object sender, EventArgs e)
        {
if (CustCmdtxtBox.Text != null)
{
                File.WriteAllText("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs", null);
                File.WriteAllText("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs", CustCmdTxtBox2.Text);
                CustCmdTxtBox2.Text = null;
                MessageBox.Show("Saved Succesfully!!");
}
else
{
MessageBox.Show("You didnt specify a name for the command to be saved as!!");
}
        }

        private void ClrCustCmdTxtBox_Click(object sender, EventArgs e)
        {
            CustCmdTxtBox2.Text = null;
MessageBox.Show("Text Box Cleared!!");
        }

        private void CancelCustCmdTxtBox_Click(object sender, EventArgs e)
        {
            CustCmdTxtBox2.Text = null;
        }

        private void numPlayers_ValueChanged(object sender, EventArgs e)
        {
            // Ensure that number of guests is never more than number of players
            if (numGuests.Value > numPlayers.Value)
            {
                numGuests.Value = numPlayers.Value;
            }
            numGuests.Maximum = numPlayers.Value;
        }

        private void editTxtsBt_Click_1(object sender, EventArgs e)
        {
            if (EditTextOpen == true)
            {
                return;
            }
            PropertyForm = new EditText();
            PropertyForm.Show();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".vb") || File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs"))
                {
                    MessageBox.Show("Command already exists", "", MessageBoxButtons.OK);
                }
                else
                {
                    Command.all.Find("cmdcreate").Use(null, CustCmdtxtBox.Text.ToLower() + " vb");
                    MessageBox.Show("New Command Created: " + CustCmdtxtBox.Text.ToLower() + " Created.");
                }
            }
            else
            {



                if (File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs") || File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".vb"))
                {
                    MessageBox.Show("Command already exists", "", MessageBoxButtons.OK);
                }
                else
                {
                    Command.all.Find("cmdcreate").Use(null, CustCmdtxtBox.Text.ToLower());
                    MessageBox.Show("New Command Created: " + CustCmdtxtBox.Text.ToLower() + " Created.");
                }
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            if (CustCmdtxtBox.Text == "" || CustCmdtxtBox.Text.IndexOf(' ') != -1) { MessageBox.Show("Please Put a Command in Textbox", "no text"); }
            bool success = false;
            if (radioButton1.Checked)
            {
                try
                {
                    success = ScriptingVB.Compile(CustCmdtxtBox.Text);
                }
                catch (Exception y)
                {
                    Server.ErrorLog(y);
                    MessageBox.Show("An exception was thrown during compilation.", "");
                    return;
                }
                if (success)
                {
                    MessageBox.Show("Compiled successfully.", "");
                }
                else
                {
                    MessageBox.Show("Compilation error.  Please check compile.log for more information.", "Error");
                }
            }
            else
            {
                try
                {
                    success = Scripting.Compile(CustCmdtxtBox.Text);
                }
                catch (Exception y)
                {
                    Server.ErrorLog(y);
                    MessageBox.Show("An exception was thrown during compilation.", "");
                    return;
                }
                if (success)
                {
                    MessageBox.Show("Compiled successfully.", "");
                }
                else
                {
                    MessageBox.Show("Compilation error.  Please check compile.log for more information.", "Error");
                }
            }

        }

        private void btnIntextbox_Click(object sender, EventArgs e)
        {
            try
            {
                string filepath;

                if (CustCmdtxtBox.Text == null)
                {
                    filepath = null;
                    MessageBox.Show("No Command entered");
                }
                else
                {
                    if (!radioButton1.Checked)
                    {

                        filepath = "extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs";
                        StreamReader reader = new StreamReader(filepath);
                        if (File.Exists(filepath))
                        {
                            try
                            {
                                CustCmdTxtBox2.Text = "";

                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    CustCmdTxtBox2.Text = CustCmdTxtBox2.Text + line + Environment.NewLine;
                                }

                            }

                            catch (Exception y)
                            {
                                MessageBox.Show(y.Message);
                            }
                            finally
                            {
                                if (reader != null)
                                    reader.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("command doesnt exist");
                        }
                    }
                    else
                    {

                        filepath = "extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".vb";
                        StreamReader reader = new StreamReader(filepath);
                        if (File.Exists(filepath))
                        {
                            try
                            {
                                CustCmdTxtBox2.Text = "";

                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    CustCmdTxtBox2.Text = CustCmdTxtBox2.Text + line + Environment.NewLine;
                                }

                            }

                            catch (Exception y)
                            {
                                MessageBox.Show(y.Message);
                            }
                            finally
                            {
                                if (reader != null)
                                    reader.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("command doesnt exist");
                        }
                    }

                }
            }

            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (CustCmdtxtBox.Text == "") { MessageBox.Show("Please Put a Command in Textbox", "no text"); }
            if (Command.all.Contains(CustCmdtxtBox.Text))
            {
                MessageBox.Show("Command is already loaded", "");
                return;
            }
            if (radioButton1.Checked)
            {
                string message;

                message = "Cmd" + CustCmdtxtBox.Text;
                string error = ScriptingVB.Load(message);
                if (error != null)
                {
                    MessageBox.Show(error, "error");
                    return;
                }
                GrpCommands.fillRanks();
                MessageBox.Show("Command was successfully loaded.", "");
            }
            else
            {
                string message;

                message = "Cmd" + CustCmdtxtBox.Text; ;
                string error = Scripting.Load(message);
                if (error != null)
                {
                    MessageBox.Show(error, "error");
                    return;
                }
                GrpCommands.fillRanks();
                MessageBox.Show("Command was successfully loaded.", "");
            }
        }

        private void btnUnload_Click(object sender, EventArgs e)
        {
            if (CustCmdtxtBox.Text == "") { MessageBox.Show("Please Put a Command in Textbox", "no text"); }
            if (Command.core.Contains(CustCmdtxtBox.Text))
            {
                MessageBox.Show(CustCmdtxtBox.Text + " is a core command, you cannot unload it!", "Error");
                return;
            }
            Command foundCmd = Command.all.Find(CustCmdtxtBox.Text);
            if (foundCmd == null)
            {
                MessageBox.Show(CustCmdtxtBox.Text + " is not a valid or loaded command.", "");
                return;
            }
            Command.all.Remove(foundCmd);
            GrpCommands.fillRanks();
            MessageBox.Show("Command was successfully unloaded.", "");
        }

        private void btnSavecmd_Click(object sender, EventArgs e)
        {
            if (CustCmdTxtBox2.Text != null)
            {
                if (radioButton1.Checked)
                {
                    try
                    {

                        File.WriteAllText("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".vb", null);
                        File.WriteAllText("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".vb", CustCmdTxtBox2.Text);

                        CustCmdTxtBox2.Text = null;
                        MessageBox.Show("Saved Successfully, as a VB file");
                    }
                    catch (Exception y)
                    {
                        MessageBox.Show(y.Message);

                    }
                }
                else
                {
                    try
                    {

                        File.WriteAllText("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs", null);
                        File.WriteAllText("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs", CustCmdTxtBox2.Text);
                        CustCmdTxtBox2.Text = null;
                        MessageBox.Show("Saved Successfully, as a C# File");
                    }
                    catch (Exception y)
                    {
                        MessageBox.Show(y.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("You didnt specify a name for the command to be saved as!!");
            }
        }

        private void btnDiscardcmd_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("Are you sure you want to discard this whole file?", "Discard?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    if (radioButton1.Checked)
                    {

                        if (File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".vb"))
                        {
                            File.Delete("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".vb");
                        }
                        else { MessageBox.Show("File: " + CustCmdtxtBox.Text + ".vb Doesnt Exist."); }
                    }
                    else
                    {
                        if (File.Exists("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs"))
                        {
                            File.Delete("extra/commands/source/Cmd" + CustCmdtxtBox.Text + ".cs");
                        }
                        else { MessageBox.Show("File: " + CustCmdtxtBox.Text + ".cs Doesnt Exist."); }
                    }
                    break;

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (listPasswords.Text == "")
            {
                MessageBox.Show("You have not selected a user's password to reset!");
                return;
            }
            try
            {
                File.Delete("extra/passwords/" + listPasswords.Text + ".xml");
                listPasswords.Items.Clear();
                DirectoryInfo di = new DirectoryInfo("extra/passwords/");
                FileInfo[] fi = di.GetFiles("*.xml");
                Thread.Sleep(10);
                foreach (FileInfo file in fi)
                {
                    listPasswords.Items.Add(file.Name.Replace(".xml", ""));
                }
            }
            catch
            {
                MessageBox.Show("Failed to reset password!");
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://dev.mysql.com/downloads/");
            }
            catch
            {
                MessageBox.Show("Failed to open link!");
            }
        }

        private void chkUseSQL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseSQL.Checked.ToString().ToLower() == "false")
            {
                grpSQL.BackColor = Color.LightGray;
            }
            if (chkUseSQL.Checked.ToString().ToLower() == "true")
            {
                grpSQL.BackColor = Color.White;
            }
        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void chkZombieOnServerStart_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtNick_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*if (chkIrcId.Checked)
            {
                textBox1.Enabled = true;
                textBox1.BackColor = Color.White;
            }
            else
            {
                textBox1.Enabled = false;
                textBox1.BackColor = Color.LightGray;
            }*/
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists("extra/images"))
                    Directory.CreateDirectory("extra/images");
                if (!File.Exists("extra/images/mcpony.png"))
                    using (WebClient WEB = new WebClient())
                        WEB.DownloadFile("http://mcforge.net/uploads/images/mcpony.png", "extra/images/mcpony.png");

                Image img = Image.FromFile("extra/images/mcpony.png");
                pictureBox1.Image = img;
            }
            catch { }
        }

        private void cmbGlobalChatColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGlobalChatColor.BackColor = Color.FromName(cmbGlobalChatColor.Items[cmbGlobalChatColor.SelectedIndex].ToString());
        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void LoadLavaSettings()
        {
            lsCmbSetupRank.SelectedIndex = (Group.findPerm(Server.lava.setupRank) == null) ? 0 : lsCmbSetupRank.Items.IndexOf(Group.findPerm(Server.lava.setupRank).name);
            lsCmbControlRank.SelectedIndex = (Group.findPerm(Server.lava.controlRank) == null) ? 0 : lsCmbControlRank.Items.IndexOf(Group.findPerm(Server.lava.controlRank).name);
            lsChkStartOnStartup.Checked = Server.lava.startOnStartup;
            lsChkSendAFKMain.Checked = Server.lava.sendAfkMain;
            lsNudVoteCount.Value = Server.lava.voteCount;
            lsNudVoteTime.Value = (decimal)MathHelper.Clamp(Server.lava.voteTime, 1, 1000);
        }

        private void SaveLavaSettings()
        {
            Server.lava.setupRank = Group.GroupList.Find(grp => grp.name == lsCmbSetupRank.Items[lsCmbSetupRank.SelectedIndex].ToString()).Permission;
            Server.lava.controlRank = Group.GroupList.Find(grp => grp.name == lsCmbControlRank.Items[lsCmbControlRank.SelectedIndex].ToString()).Permission;
            Server.lava.startOnStartup = lsChkStartOnStartup.Checked;
            Server.lava.sendAfkMain = lsChkSendAFKMain.Checked;
            Server.lava.voteCount = (byte)lsNudVoteCount.Value;
            Server.lava.voteTime = (double)lsNudVoteTime.Value;
            Server.lava.SaveSettings();
        }

        private void UpdateLavaControls()
        {
            try
            {
                lsBtnStartGame.Enabled = !Server.lava.active;
                lsBtnStopGame.Enabled = Server.lava.active;
                lsBtnEndRound.Enabled = Server.lava.roundActive;
                lsBtnEndVote.Enabled = Server.lava.voteActive;
            }
            catch { }
        }

        private void lsBtnStartGame_Click(object sender, EventArgs e)
        {
            if (!Server.lava.active) Server.lava.Start();
            UpdateLavaControls();
        }

        private void lsBtnStopGame_Click(object sender, EventArgs e)
        {
            if (Server.lava.active) Server.lava.Stop();
            UpdateLavaControls();
        }

        private void lsBtnEndRound_Click(object sender, EventArgs e)
        {
            if (Server.lava.roundActive) Server.lava.EndRound();
            UpdateLavaControls();
        }

        private void UpdateLavaMapList(bool useList = true, bool noUseList = true)
        {
            if (!useList && !noUseList) return;
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { UpdateLavaMapList(useList, noUseList); }));
                    return;
                }

                int useIndex = lsMapUse.SelectedIndex, noUseIndex = lsMapNoUse.SelectedIndex;
                if (useList) lsMapUse.Items.Clear();
                if (noUseList) lsMapNoUse.Items.Clear();

                if (useList)
                {
                    lsMapUse.Items.AddRange(Server.lava.GetMaps().ToArray());
                    try { if (useIndex > -1) lsMapUse.SelectedIndex = useIndex; }
                    catch { }
                }
                if (noUseList)
                {
                    string name;
                    FileInfo[] fi = new DirectoryInfo("levels/").GetFiles("*.lvl");
                    foreach (FileInfo file in fi)
                    {
                        name = file.Name.Replace(".lvl", "");
                        if (name.ToLower() != Server.mainLevel.name && !Server.lava.HasMap(name))
                            lsMapNoUse.Items.Add(name);
                    }
                    try { if (noUseIndex > -1) lsMapNoUse.SelectedIndex = noUseIndex; }
                    catch { }
                }
            }
            catch (Exception ex) { Server.ErrorLog(ex); }
        }

        private void lsAddMap_Click(object sender, EventArgs e)
        {
            try
            {
                Server.lava.Stop(); // Doing this so we don't break something...
                UpdateLavaControls();

                string name;
                try { name = lsMapNoUse.Items[lsMapNoUse.SelectedIndex].ToString(); }
                catch { return; }

                if (Level.Find(name) == null)
                    Command.all.Find("load").Use(null, name);
                Level level = Level.Find(name);
                if (level == null) return;

                Server.lava.AddMap(name);

                LavaSurvival.MapSettings settings = Server.lava.LoadMapSettings(level.name);
                settings.blockFlood = new LavaSurvival.Pos((ushort)(level.width / 2), (ushort)(level.depth - 1), (ushort)(level.height / 2));
                settings.blockLayer = new LavaSurvival.Pos(0, (ushort)(level.depth / 2), 0);
                ushort x = (ushort)(level.width / 2), y = (ushort)(level.depth / 2), z = (ushort)(level.height / 2);
                settings.safeZone = new LavaSurvival.Pos[] { new LavaSurvival.Pos((ushort)(x - 3), y, (ushort)(z - 3)), new LavaSurvival.Pos((ushort)(x + 3),(ushort)(y + 4), (ushort)(z + 3)) };
                Server.lava.SaveMapSettings(settings);

                level.motd = "Lava Survival: " + level.name.Capitalize();
                level.overload = 1000000;
                level.unload = false;
                level.loadOnGoto = false;
                Level.SaveSettings(level);
                level.Unload(true);

                UpdateLavaMapList();
            }
            catch (Exception ex) { Server.ErrorLog(ex); }
        }

        private void lsRemoveMap_Click(object sender, EventArgs e)
        {
            try
            {
                Server.lava.Stop(); // Doing this so we don't break something...
                UpdateLavaControls();

                string name;
                try { name = lsMapUse.Items[lsMapUse.SelectedIndex].ToString(); }
                catch { return; }

                if (Level.Find(name) == null)
                    Command.all.Find("load").Use(null, name);
                Level level = Level.Find(name);
                if (level == null) return;

                Server.lava.RemoveMap(name);
                level.motd = "ignore";
                level.overload = 1500;
                level.unload = true;
                level.loadOnGoto = true;
                Level.SaveSettings(level);
                level.Unload(true);

                UpdateLavaMapList();
            }
            catch (Exception ex) { Server.ErrorLog(ex); }
        }

        private void lsMapUse_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name;
            try { name = lsMapUse.Items[lsMapUse.SelectedIndex].ToString(); }
            catch { return; }

            lsLoadedMap = name;
            try
            {
                LavaSurvival.MapSettings settings = Server.lava.LoadMapSettings(name);
                lsNudFastLava.Value = MathHelper.Clamp((decimal)settings.fast, 0, 100);
                lsNudKiller.Value = MathHelper.Clamp((decimal)settings.killer, 0, 100);
                lsNudDestroy.Value = MathHelper.Clamp((decimal)settings.destroy, 0, 100);
                lsNudWater.Value = MathHelper.Clamp((decimal)settings.water, 0, 100);
                lsNudLayer.Value = MathHelper.Clamp((decimal)settings.layer, 0, 100);
                lsNudLayerHeight.Value = MathHelper.Clamp((decimal)settings.layerHeight, 1, 1000);
                lsNudLayerCount.Value = MathHelper.Clamp((decimal)settings.layerCount, 1, 1000);
                lsNudLayerTime.Value = (decimal)MathHelper.Clamp(settings.layerInterval, 1, 1000);
                lsNudRoundTime.Value = (decimal)MathHelper.Clamp(settings.roundTime, 1, 1000);
                lsNudFloodTime.Value = (decimal)MathHelper.Clamp(settings.floodTime, 1, 1000);
            }
            catch (Exception ex) { Server.ErrorLog(ex); }
        }

        private void lsBtnEndVote_Click(object sender, EventArgs e)
        {
            if (Server.lava.voteActive) Server.lava.EndVote();
            UpdateLavaControls();
        }

        private void lsBtnSaveSettings_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(lsLoadedMap)) return;

            try
            {
                LavaSurvival.MapSettings settings = Server.lava.LoadMapSettings(lsLoadedMap);
                settings.fast = (byte)lsNudFastLava.Value;
                settings.killer = (byte)lsNudKiller.Value;
                settings.destroy = (byte)lsNudDestroy.Value;
                settings.water = (byte)lsNudWater.Value;
                settings.layer = (byte)lsNudLayer.Value;
                settings.layerHeight = (int)lsNudLayerHeight.Value;
                settings.layerCount = (int)lsNudLayerCount.Value;
                settings.layerInterval = (double)lsNudLayerTime.Value;
                settings.roundTime = (double)lsNudRoundTime.Value;
                settings.floodTime = (double)lsNudFloodTime.Value;
                Server.lava.SaveMapSettings(settings);
            }
            catch (Exception ex) { Server.ErrorLog(ex); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                lavaMapBrowser.Show();
                lavaMapBrowser.Focus();
            }
            catch (ObjectDisposedException)
            {
                lavaMapBrowser = new LavaMapBrowser();
                lavaMapBrowser.Show();
                lavaMapBrowser.Focus();
            }
            catch (Exception ex) { Server.ErrorLog(ex); }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new GUI.WoM().Show();
        }

        private void chkWomDirect_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = chkWomDirect.Checked;
        }
    }

}