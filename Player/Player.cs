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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data;
using System.Linq;
using System.Text;
using MCForge.SQL;
using System.Timers;

namespace MCForge
{
    public sealed partial class Player : IDisposable
    {
        public static List<Player> players = new List<Player>();
        public static Dictionary<string, string> left = new Dictionary<string, string>();
        public static List<Player> connections = new List<Player>(Server.players);
        System.Timers.Timer muteTimer = new System.Timers.Timer(1000);
        public static List<string> emoteList = new List<string>();
        public List<string> listignored = new List<string>();
        public List<string> mapgroups = new List<string>();
        public static List<string> globalignores = new List<string>();
        public static int totalMySQLFailed = 0;
        public static byte number { get { return (byte)players.Count; } }
        static System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        public static List<Player> totalplayers = new List<Player>();
        public static string lastMSG = "";

        public static bool storeHelp = false;
        public static string storedHelp = "";

        public Socket socket;
        System.Timers.Timer timespent = new System.Timers.Timer(1000);
        System.Timers.Timer loginTimer = new System.Timers.Timer(1000);
        public System.Timers.Timer pingTimer = new System.Timers.Timer(2000);
        System.Timers.Timer extraTimer = new System.Timers.Timer(22000);
        public System.Timers.Timer afkTimer = new System.Timers.Timer(2000);
        public int afkCount = 0;
        public DateTime afkStart;

        public bool megaBoid = false;
        public bool cmdTimer = false;
        public bool UsingWom = false;

        byte[] buffer = new byte[0];
        byte[] tempbuffer = new byte[0xFF];
        public bool disconnected = false;
        public string time;
        public string name;
        public string realName;
        public int warn = 0;
        public byte id;
        public int userID = -1;
        public string ip;
        public string color;
        public Group group;
        public bool hidden = false;
        public bool painting = false;
        public bool muted = false;
        public bool jailed = false;
        public bool invincible = false;
        public string prefix = "";
        public string title = "";
        public string titlecolor;
        public int TotalMessagesSent = 0;
        public int passtries = 0;
        public int ponycount = 0;
        public int rdcount = 0;
        public bool hasreadrules = false;
        public bool canusereview = true;

        // check what commands are being used much:
        public static bool sendcommanddata = false;

        //Pyramid Code

        public int pyramidx1;
        public int pyramidx2;
        public int pyramidy1;
        public int pyramidy2;
        public int pyramidz1;
        public int pyramidz2;
        public string pyramidblock;
        public int pyramidtotal;
        public int pyramidtotal2;
        public bool pyramidsilent = false;

        public bool deleteMode = false;
        public bool ignorePermission = false;
        public bool ignoreGrief = false;
        public bool parseSmiley = true;
        public bool smileySaved = true;
        public bool opchat = false;
        public bool adminchat = false;
        public bool onWhitelist = false;
        public bool whisper = false;
        public string whisperTo = "";
        public bool ignoreglobal = false;

        public string storedMessage = "";

        public bool trainGrab = false;
        public bool onTrain = false;
        public bool allowTnt = true;

        public bool frozen = false;
        public string following = "";
        public string possess = "";

        // Only used for possession.
        //Using for anything else can cause unintended effects!
        public bool canBuild = true;

        public int money = 0;
        public long overallBlocks = 0;
        
        public int loginBlocks = 0;

        public DateTime timeLogged;
        public DateTime firstLogin;
        public int totalLogins = 0;
        public int totalKicked = 0;
        public int overallDeath = 0;

        public string savedcolor = "";

        public bool staticCommands = false;

        public DateTime ZoneSpam;
        public bool ZoneCheck = false;
        public bool zoneDel = false;

        public Thread commThread;
        public bool commUse = false;

        public bool aiming;
        public bool isFlying = false;

        public bool joker = false;
        public bool adminpen = false;

        public bool voice = false;
        public string voicestring = "";

        public int grieferStoneWarn = 0;

        //CTF
        public Team team;
        public Team hasflag;

        //Countdown
        public bool playerofcountdown = false;
        public bool incountdown = false;
        public ushort countdowntempx;
        public ushort countdowntempz;
        public bool countdownsettemps = false;

        //Zombie
        public bool referee = false;
        public int blockCount = 50;
        public bool voted = false;
        public int blocksStacked = 0;
        public int infectThisRound = 0;
        public int lastYblock = 0;
        public int lastXblock = 0;
        public int lastZblock = 0;
        public bool infected = false;
        public bool aka = false;
        public bool flipHead = true;
        public int playersInfected = 0;
        public int NoClipcount = 0;
        //Copy
        public List<CopyPos> CopyBuffer = new List<CopyPos>();
        public struct CopyPos { public ushort x, y, z; public byte type; }
        public bool copyAir = false;
        public int[] copyoffset = new int[3] { 0, 0, 0 };
        public ushort[] copystart = new ushort[3] { 0, 0, 0 };

        //Undo
        public struct UndoPos { public ushort x, y, z; public byte type, newtype; public string mapName; public DateTime timePlaced; }
        public List<UndoPos> UndoBuffer = new List<UndoPos>();
        public List<UndoPos> RedoBuffer = new List<UndoPos>();


        public bool showPortals = false;
        public bool showMBs = false;

        public string prevMsg = "";

        //Block Change variable holding
        public int[] BcVar;


        //Movement
        public ushort oldBlock = 0;
        public ushort deathCount = 0;
        public byte deathBlock;

        //Games
        public DateTime lastDeath = DateTime.Now;

        public byte BlockAction = 0;  //0-Nothing 1-solid 2-lava 3-water 4-active_lava 5 Active_water 6 OpGlass 7 BluePort 8 OrangePort
        public byte modeType = 0;
        public byte[] bindings = new byte[128];
        public string[] cmdBind = new string[10];
        public string[] messageBind = new string[10];
        public string lastCMD = "";

        public Level level = Server.mainLevel;
        public bool Loading = true;     //True if player is loading a map.
        public ushort[] lastClick = new ushort[3] { 0, 0, 0 };

        public ushort[] pos = new ushort[3] { 0, 0, 0 };
        ushort[] oldpos = new ushort[3] { 0, 0, 0 };
        ushort[] basepos = new ushort[3] { 0, 0, 0 };
        public byte[] rot = new byte[2] { 0, 0 };
        byte[] oldrot = new byte[2] { 0, 0 };

        //ushort[] clippos = new ushort[3] { 0, 0, 0 };
        //byte[] cliprot = new byte[2] { 0, 0 };

        // grief/spam detection
        public static int spamBlockCount = 200;
        public static int spamBlockTimer = 5;
        Queue<DateTime> spamBlockLog = new Queue<DateTime>(spamBlockCount);

        public int consecutivemessages = 0;
        private System.Timers.Timer resetSpamCount = new System.Timers.Timer(Server.spamcountreset*1000);
        //public static int spamChatCount = 3;
        //public static int spamChatTimer = 4;
        //Queue<DateTime> spamChatLog = new Queue<DateTime>(spamChatCount);

        // CmdVoteKick
        public VoteKickChoice voteKickChoice = VoteKickChoice.HasntVoted;

        // Extra storage for custom commands
        public ExtrasCollection Extras = new ExtrasCollection();

        //Chatrooms
        public string Chatroom;
        public List<string> spyChatRooms = new List<string>();
        public DateTime lastchatroomglobal = new DateTime();

        //Waypoints
        public List<Waypoint.WP> Waypoints = new List<Waypoint.WP>();

        //Random...
        public Random random = new Random();
        
        //Global Chat
        public bool muteGlobal = false;

        public bool loggedIn = false;

        private Player()
        {
            //Only necessary for a "flatcopy".

        }

        public static string CheckPlayerStatus(Player p)
        {
            if (p.hidden)
                return "hidden";
            if (Server.afkset.Contains(p.name))
                return "afk";
            return "active";
        }

        public bool CheckIfInsideBlock()
        {
            return CheckIfInsideBlock(this);
        }
        public static bool CheckIfInsideBlock(Player p)
        {
            ushort x, y, z;
            x = (ushort)(p.pos[0] / 32);
            y = (ushort)(p.pos[1] / 32);
            y = (ushort)Math.Round((decimal)(((y * 32) + 4) / 32));
            z = (ushort)(p.pos[2] / 32);

            byte b = p.level.GetTile(x, y, z);
            byte b1 = p.level.GetTile(x, (ushort)(y - 1), z);

            if (!Block.Walkthrough(Block.Convert(b)) || !Block.Walkthrough(Block.Convert(b1)))
            {
                Server.s.Log("HAAAAAAAX!!");
                return true;
            }
            else
            {
                return false;
            }
        }

        //This is so that plugin devs can declare a player without needing a socket..
        //They would still have to do p.Dispose()..
        public Player(string playername) { name = playername; }

        public Player(Socket s)
        {
            try
            {
                socket = s;
                ip = socket.RemoteEndPoint.ToString().Split(':')[0];
                Server.s.Log(ip + " connected to the server.");

                for (byte i = 0; i < 128; ++i) bindings[i] = i;

                socket.BeginReceive(tempbuffer, 0, tempbuffer.Length, SocketFlags.None, new AsyncCallback(Receive), this);
                timespent.Elapsed += delegate
                {
                    if (!Loading)
                    {
                        try
                        {
                            int Days = Convert.ToInt32(time.Split(' ')[0]);
                            int Hours = Convert.ToInt32(time.Split(' ')[1]);
                            int Minutes = Convert.ToInt32(time.Split(' ')[2]);
                            int Seconds = Convert.ToInt32(time.Split(' ')[3]);
                            Seconds++;
                            if (Seconds >= 60)
                            {
                                Minutes++;
                                Seconds = 0;
                            }
                            if (Minutes >= 60)
                            {
                                Hours++;
                                Minutes = 0;
                            }
                            if (Hours >= 24)
                            {
                                Days++;
                                Hours = 0;
                            }
                            time = "" + Days + " " + Hours + " " + Minutes + " " + Seconds;
                        }
                        catch { time = "0 0 0 1"; }
                    }
                };
                timespent.Start();
                loginTimer.Elapsed += delegate
                {
                    if (!Loading)
                    {
                        loginTimer.Stop();
                        if (File.Exists("text/welcome.txt"))
                        {
                            try
                            {
                                using (StreamReader wm = File.OpenText("text/welcome.txt"))
                                {
                                    List<string> welcome = new List<string>();
                                    while (!wm.EndOfStream)
                                        welcome.Add(wm.ReadLine());
                                    foreach (string w in welcome)
                                        SendMessage(w);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            Server.s.Log("Could not find Welcome.txt. Using default.");
                            File.WriteAllText("text/welcome.txt", "Welcome to my server!");
                        }
                        extraTimer.Start();
                        loginTimer.Dispose();
                    }
                }; loginTimer.Start();

                pingTimer.Elapsed += delegate { SendPing(); };
                pingTimer.Start();

                extraTimer.Elapsed += delegate
                {
                    extraTimer.Stop();

                    try
                    {
                        if (!Group.Find("Nobody").commands.Contains("inbox") && !Group.Find("Nobody").commands.Contains("send"))
                        {
                            DataTable Inbox = MySQL.fillData("SELECT * FROM `Inbox" + name + "`", true);

                            SendMessage("&cYou have &f" + Inbox.Rows.Count + Server.DefaultColor + " &cmessages in /inbox");
                            Inbox.Dispose();
                        }
                    }
                    catch { }
                    if (Server.updateTimer.Interval > 1000) SendMessage("Lowlag mode is currently &aON.");
                    try
                    {
                        if (!Group.Find("Nobody").commands.Contains("pay") && !Group.Find("Nobody").commands.Contains("give") && !Group.Find("Nobody").commands.Contains("take")) SendMessage("You currently have &a" + money + Server.DefaultColor + " " + Server.moneys);
                    }
                    catch { }
                    SendMessage("You have modified &a" + overallBlocks + Server.DefaultColor + " blocks!");
                    if (players.Count == 1)
                        SendMessage("There is currently &a" + players.Count + " player online.");
                    else
                        SendMessage("There are currently &a" + players.Count + " players online.");
                    try
                    {
                        if (!Group.Find("Nobody").commands.Contains("award") && !Group.Find("Nobody").commands.Contains("awards") && !Group.Find("Nobody").commands.Contains("awardmod")) SendMessage("You have " + Awards.awardAmount(name) + " awards.");
                    }
                    catch { }
                    try { Gui.Window.thisWindow.UpdatePlyersListBox(); }
                    catch { }
                    if (Server.lava.active) SendMessage("There is a &aLava Survival " + Server.DefaultColor + "game active! Join it by typing /ls go");
                    extraTimer.Dispose();
                };

                afkTimer.Elapsed += delegate
                {
                    if (name == "") return;

                    if (Server.afkset.Contains(name))
                    {
                        afkCount = 0;
                        if (Server.afkkick > 0 && group.Permission < Server.afkkickperm)
                            if (afkStart.AddMinutes(Server.afkkick) < DateTime.Now)
                                Kick("Auto-kick, AFK for " + Server.afkkick + " minutes");
                        if ((oldpos[0] != pos[0] || oldpos[1] != pos[1] || oldpos[2] != pos[2]) && (oldrot[0] != rot[0] || oldrot[1] != rot[1]))
                            Command.all.Find("afk").Use(this, "");
                    }
                    else
                    {
                        if (oldpos[0] == pos[0] && oldpos[1] == pos[1] && oldpos[2] == pos[2] && oldrot[0] == rot[0] && oldrot[1] == rot[1])
                            afkCount++;
                        else
                            afkCount = 0;

                        if (afkCount > Server.afkminutes * 30)
                        {
                        	if (name.Equals(""))
                        	{
                            		Command.all.Find("afk").Use(this, "auto: Not moved for " + Server.afkminutes + " minutes");
                                    if (AFK != null)
                                        AFK(this);
                                    if (ONAFK != null)
                                        ONAFK(this);
                            		afkCount = 0;
                        	}
                        }
                    }
                };
                resetSpamCount.Elapsed += delegate {
                    if (consecutivemessages > 0)
                        consecutivemessages = 0;
                };
                resetSpamCount.Start();

                if (Server.afkminutes > 0) afkTimer.Start();

                connections.Add(this);
            }
            catch (Exception e) { Kick("Login failed!"); Server.ErrorLog(e); }
        }

        public void save()
        {
            string commandString =
                "UPDATE Players SET IP='" + ip + "'" +
                ", LastLogin='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                ", totalLogin=" + totalLogins +
                ", totalDeaths=" + overallDeath +
                ", Money=" + money +
                ", totalBlocks=" + overallBlocks + " + " + loginBlocks +
                ", totalKicked=" + totalKicked +
                ", TimeSpent='" + time +
                "' WHERE Name='" + name + "'";
            if (MySQLSave != null)
            {
                MySQLSave(this, commandString);
                if (cancelmysql)
                {
                    cancelmysql = false;
                    return;
                }
            }
            if (Server.useMySQL) MySQL.executeQuery(commandString); else SQLite.executeQuery(commandString);

            try
            {
                if (!smileySaved)
                {
                    if (parseSmiley)
                        emoteList.RemoveAll(s => s == name);
                    else
                        emoteList.Add(name);

                    File.WriteAllLines("text/emotelist.txt", emoteList.ToArray());
                    smileySaved = true;
                }
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
            }
        }

        #region == INCOMING ==
        static void Receive(IAsyncResult result)
        {
            //Server.s.Log(result.AsyncState.ToString());
            Player p = (Player)result.AsyncState;
            if (p.disconnected || p.socket == null)
                return;
            try
            {
                int length = p.socket.EndReceive(result);
                if (length == 0) { p.Disconnect(); return; }

                byte[] b = new byte[p.buffer.Length + length];
                Buffer.BlockCopy(p.buffer, 0, b, 0, p.buffer.Length);
                Buffer.BlockCopy(p.tempbuffer, 0, b, p.buffer.Length, length);

                p.buffer = p.HandleMessage(b);
                p.socket.BeginReceive(p.tempbuffer, 0, p.tempbuffer.Length, SocketFlags.None,
                                      new AsyncCallback(Receive), p);
            }
            catch (SocketException)
            {
                p.Disconnect();
            }
            catch (ObjectDisposedException)
            {
                // Player is no longer connected, socket was closed
                // Mark this as disconnected and remove them from active connection list
                Player.SaveUndo(p);
                if (connections.Contains(p))
                    connections.Remove(p);
                p.disconnected = true;
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
                p.Kick("Error!");
            }
        }
        byte[] HandleMessage(byte[] buffer)
        {
            try
            {
                int length = 0; byte msg = buffer[0];
                // Get the length of the message by checking the first byte
                switch (msg)
                {
                    //For wom
                    case (byte)'G':
                        level.textures.ServeCfg(this, buffer);
                        return new byte[0];
                    case 0:
                        length = 130;
                        break; // login
                    case 5:
                        if (!loggedIn)
                            goto default;
                        length = 8;
                        break; // blockchange
                    case 8:
                        if (!loggedIn)
                            goto default;
                        length = 9;
                        break; // input
                    case 13:
                        if (!loggedIn)
                            goto default;
                        length = 65;
                        break; // chat
                    default:
                        Kick("Unhandled message id \"" + msg + "\"!");
                        return new byte[0];
                }
                if (buffer.Length > length)
                {
                    byte[] message = new byte[length];
                    Buffer.BlockCopy(buffer, 1, message, 0, length);

                    byte[] tempbuffer = new byte[buffer.Length - length - 1];
                    Buffer.BlockCopy(buffer, length + 1, tempbuffer, 0, buffer.Length - length - 1);

                    buffer = tempbuffer;

                    // Thread thread = null; 
                    switch (msg)
                    {
                        case 0:
                            HandleLogin(message);
                            break;
                        case 5:
                            if (!loggedIn)
                                break;
                            HandleBlockchange(message);
                            break;
                        case 8:
                            if (!loggedIn)
                                break;
                            HandleInput(message);
                            break;
                        case 13:
                            if (!loggedIn)
                                break;
                            HandleChat(message);
                            break;
                    }
                    //thread.Start((object)message);
                    if (buffer.Length > 0)
                        buffer = HandleMessage(buffer);
                    else
                        return new byte[0];
                }
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
            }
            return buffer;
        }
        void HandleLogin(byte[] message)
        {
            try
            {
                //byte[] message = (byte[])m;
                if (loggedIn)
                    return;

                byte version = message[0];
                name = enc.GetString(message, 1, 64).Trim();
                string verify = enc.GetString(message, 65, 32).Trim();
                byte type = message[129];
                try
                {
                    Server.TempBan tBan = Server.tempBans.Find(tB => tB.name.ToLower() == name.ToLower());
                    if (tBan.allowedJoin < DateTime.Now)
                    {
                        Server.tempBans.Remove(tBan);
                    }
                    else
                    {
                        Kick("You're still banned (temporary ban)!");
                    }
                }
                catch { }

                // Whitelist check.
                if (Server.useWhitelist)
                {
                    if (Server.verify)
                    {
                        if (Server.whiteList.Contains(name))
                        {
                            onWhitelist = true;
                        }
                    }
                    else
                    {
                        // Verify Names is off.  Gotta check the hard way.
                        DataTable ipQuery = Server.useMySQL ? MySQL.fillData("SELECT Name FROM Players WHERE IP = '" + ip + "'") : SQLite.fillData("SELECT Name FROM Players WHERE IP = '" + ip + "'");

                        if (ipQuery.Rows.Count > 0)
                        {
                            if (ipQuery.Rows.Contains(name) && Server.whiteList.Contains(name))
                            {
                                onWhitelist = true;
                            }
                        }
                        ipQuery.Dispose();
                    }
                }

                if (Server.PremiumPlayersOnly && !Server.devs.Contains(name.ToLower()))
                {
                    using (WebClient Client = new WebClient())
                    {
                        int tries = 0;
                        while (tries++ < 3)
                        {
                            try
                            {
                                bool haspaid = Convert.ToBoolean(Client.DownloadString("http://www.minecraft.net/haspaid.jsp?user=" + name));
                                if (!haspaid)
                                    Kick("Sorry, this is a premium server only!");
                                break;
                            }
                            catch { }
                        }
                    }
                }

                if (File.Exists("ranks/ignore/" + this.name + ".txt"))
                {
                    try
                    {
                        string[] checklines = File.ReadAllLines("ranks/ignore/" + this.name + ".txt");
                        foreach (string checkline in checklines)
                        {
                            this.listignored.Add(checkline);
                        }
                        File.Delete("ranks/ignore/" + this.name + ".txt");
                    }
                    catch
                    {
                        Server.s.Log("Failed to load ignore list for: " + this.name);
                    }
                }

                if (File.Exists("ranks/ignore/GlobalIgnore.xml"))
                {
                    try
                    {
                        string[] searchxmls = File.ReadAllLines("ranks/ignore/GlobalIgnore.xml");
                        foreach (string searchxml in searchxmls)
                        {
                            globalignores.Add(searchxml);
                        }
                        foreach (string ignorer in globalignores)
                        {
                            Player foundignore = Player.Find(ignorer);
                            foundignore.ignoreglobal = true;
                        }
                        File.Delete("ranks/ignore/GlobalIgnore.xml");
                    }
                    catch
                    {
                        Server.s.Log("Failed to load global ignore list!");
                    }
                }
                totalplayers.Add(this);



                if (Server.bannedIP.Contains(ip))
                {
                    if (Server.useWhitelist)
                    {
                        if (!onWhitelist)
                        {
                            Kick(Server.customBanMessage);
                            return;
                        }
                    }
                    else
                    {
                        Kick(Server.customBanMessage);
                        return;
                    }
                }
                //if (connections.Count >= 5) { Kick("Too many connections!"); return; }

                if (Server.omniban.CheckPlayer(this)) { Kick(Server.omniban.kickMsg); return; }

                if (Group.findPlayerGroup(name) == Group.findPerm(LevelPermission.Banned))
                {
                    if (Server.useWhitelist)
                    {
                        if (!onWhitelist)
                        {
                            Kick(Server.customBanMessage);
                            return;
                        }
                    }
                    else
                    {
                        Kick(Server.customBanMessage);
                        return;
                    }
                }
                if (!Server.devs.Contains(name.ToLower()) && !VIP.Find(this))
                {
                    // Check to see how many guests we have
                    if (Player.players.Count >= Server.players && !IPInPrivateRange(ip)) { Kick("Server full!"); return; }
                    // Code for limiting no. of guests
                    if (Group.findPlayerGroup(name) == Group.findPerm(LevelPermission.Guest))
                    {
                        // Check to see how many guests we have
                        int currentNumOfGuests = Player.players.Count(pl => pl.group.Permission <= LevelPermission.Guest);
                        if (currentNumOfGuests >= Server.maxGuests)
                        {
                            if (Server.guestLimitNotify) GlobalMessageOps("Guest " + this.name + " couldn't log in - too many guests.");
                            Server.s.Log("Guest " + this.name + " couldn't log in - too many guests.");
                            Kick("Server has reached max number of guests");
                            return;
                        }
                    }
                }
                if (version != Server.version) { Kick("Wrong version!"); return; }
                if (name.Length > 16 || !ValidName(name)) { Kick("Illegal name!"); return; }

                if (Server.verify)
                {
                    if (verify == "--" || verify !=
                        BitConverter.ToString(md5.ComputeHash(enc.GetBytes(Server.salt + name)))
                        .Replace("-", "").ToLower().TrimStart('0'))
                    {
                        if (!IPInPrivateRange(ip))
                        {
                            Kick("Login failed! Try again."); return;
                        }
                    }
                }

                foreach (Player p in players)
                {
                    if (p.name == name)
                    {
                        if (Server.verify)
                        {
                            p.Kick("Someone logged in as you!"); break;
                        }
                        else { Kick("Already logged in!"); return; }
                    }
                }

                try { left.Remove(name.ToLower()); }
                catch { }

                group = Group.findPlayerGroup(name);
                
                SendMotd();
                SendMap();
                Loading = true;

                if (disconnected) return;

                loggedIn = true;
                id = FreeId();

                lock (players)
                    players.Add(this);

                connections.Remove(this);

                Server.s.PlayerListUpdate();

                //Test code to show when people come back with different accounts on the same IP
                string temp = name + " is lately known as:";
                bool found = false;
                if (!ip.StartsWith("127.0.0."))
                {
                    foreach (KeyValuePair<string, string> prev in left)
                    {
                        if (prev.Value == ip)
                        {
                            found = true;
                            temp += " " + prev.Key;
                        }
                    }
                    if (found)
                    {
                        if (this.group.Permission < Server.adminchatperm || Server.adminsjoinsilent == false)
                        {
                            GlobalMessageOps(temp);
                            //IRCBot.Say(temp, true);       //Tells people in op channel on IRC
                        }

                        Server.s.Log(temp);
                    }
                }
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
                Player.GlobalMessage("An error occurred: " + e.Message);
            }

            DataTable playerDb = Server.useMySQL ? MySQL.fillData("SELECT * FROM Players WHERE Name='" + name + "'") : SQLite.fillData("SELECT * FROM Players WHERE Name='" + name + "'");


            if (playerDb.Rows.Count == 0)
            {
                this.prefix = "";
                this.time = "0 0 0 1";
                this.titlecolor = "";
                this.color = group.color;
                this.money = 0;
                this.firstLogin = DateTime.Now;
                this.totalLogins = 1;
                this.totalKicked = 0;
                this.overallDeath = 0;
                this.overallBlocks = 0;
                
                this.timeLogged = DateTime.Now;
                SendMessage("Welcome " + name + "! This is your first visit.");

                if (Server.useMySQL)
                MySQL.executeQuery("INSERT INTO Players (Name, IP, FirstLogin, LastLogin, totalLogin, Title, totalDeaths, Money, totalBlocks, totalKicked, TimeSpent)" +
                    " VALUES ('" + name + "', '" + ip + "', '" + firstLogin.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + totalLogins +
                    ", '" + prefix + "', " + overallDeath + ", " + money + ", " + loginBlocks + ", " + totalKicked + ", '" + time + "')");
                else
                    SQLite.executeQuery("INSERT INTO Players (Name, IP, FirstLogin, LastLogin, totalLogin, Title, totalDeaths, Money, totalBlocks, totalKicked, TimeSpent)" +
                    " VALUES ('" + name + "', '" + ip + "', '" + firstLogin.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + totalLogins +
                    ", '" + prefix + "', " + overallDeath + ", " + money + ", " + loginBlocks + ", " + totalKicked + ", '" + time + "')");

            }
            else
            {
                totalLogins = int.Parse(playerDb.Rows[0]["totalLogin"].ToString()) + 1;
                time = playerDb.Rows[0]["TimeSpent"].ToString();
                userID = int.Parse(playerDb.Rows[0]["ID"].ToString());
                firstLogin = DateTime.Parse(playerDb.Rows[0]["firstLogin"].ToString());
                timeLogged = DateTime.Now;
                if (playerDb.Rows[0]["Title"].ToString().Trim() != "")
                {
                    string parse = playerDb.Rows[0]["Title"].ToString().Trim().Replace("[", "");
                    title = parse.Replace("]", "");
                }
                if (playerDb.Rows[0]["title_color"].ToString().Trim() != "")
                {
                    titlecolor = c.Parse(playerDb.Rows[0]["title_color"].ToString().Trim());
                }
                else
                {
                    titlecolor = "";
                }
                if (playerDb.Rows[0]["color"].ToString().Trim() != "")
                {
                    color = c.Parse(playerDb.Rows[0]["color"].ToString().Trim());
                }
                else
                {
                    color = group.color;
                }
                SetPrefix();
                overallDeath = int.Parse(playerDb.Rows[0]["TotalDeaths"].ToString());
                overallBlocks = long.Parse(playerDb.Rows[0]["totalBlocks"].ToString().Trim());
                money = int.Parse(playerDb.Rows[0]["Money"].ToString());
                totalKicked = int.Parse(playerDb.Rows[0]["totalKicked"].ToString());
                SendMessage("Welcome back " + color + prefix + name + Server.DefaultColor + "! You've been here " + totalLogins + " times!");
                if (Server.muted.Contains(name))
                {
                    muted = true;
                    GlobalMessage(name + " is still muted from the last time they went offline.");
                }
            }
            playerDb.Dispose();
            if (PlayerConnect != null)
                PlayerConnect(this);
            if (Server.devs.Contains(this.name.ToLower()))
            {
                if (color == Group.standard.color)
                {
                    color = "&9";
                }
                if (prefix == "")
                {
                    title = "Dev";
                }
                SetPrefix();
            }
            if (Server.server_owner != "" && Server.server_owner.ToLower().Contains(this.name.ToLower()))
            {
                if (color == Group.standard.color)
                {
                    color = "&c";
                }
                if (prefix == "")
                {
                    title = "Owner";
                }
                SetPrefix();
            }

            try
            {
                ushort x = (ushort)((0.5 + level.spawnx) * 32);
                ushort y = (ushort)((1 + level.spawny) * 32);
                ushort z = (ushort)((0.5 + level.spawnz) * 32);
                pos = new ushort[3] { x, y, z }; rot = new byte[2] { level.rotx, level.roty };

                GlobalSpawn(this, x, y, z, rot[0], rot[1], true);
                foreach (Player p in players)
                {
                    if (p.level == level && p != this && !p.hidden)
                        SendSpawn(p.id, p.color + p.name, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1]);
                }
                foreach (PlayerBot pB in PlayerBot.playerbots)
                {
                    if (pB.level == level)
                        SendSpawn(pB.id, pB.color + pB.name, pB.pos[0], pB.pos[1], pB.pos[2], pB.rot[0], pB.rot[1]);
                }
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
                Server.s.Log("Error spawning player \"" + name + "\"");
            }

            Loading = false;

            if (Server.verifyadmins == true)
            {
                if (this.group.Permission >= Server.verifyadminsrank)
                {
                    adminpen = true;
                }
            }
            if (emoteList.Contains(name)) parseSmiley = false;
            if (!Directory.Exists("text/login"))
            {
                Directory.CreateDirectory("text/login");
            }
            if (!File.Exists("text/login/" + this.name + ".txt"))
            {
                File.WriteAllText("text/login/" + this.name + ".txt", "joined the server.");
            }
            if (Server.agreetorulesonentry == true)
            {
                if (!File.Exists("ranks/agreed.txt"))
                {
                    File.WriteAllText("ranks/agreed.txt", "");
                }
                var agreed = File.ReadAllText("ranks/agreed.txt");
                if (this.group.Permission == LevelPermission.Guest)
                {
                    if (!agreed.Contains(this.name.ToLower()))
                    {
                        SendMessage("&9You must read the &c/rules&9 and &c/agree&9 to them before you can build and use commands!");
                        jailed = true;
                    }
                }
            }
            if (this.group.Permission < Server.adminchatperm || Server.adminsjoinsilent == false)
            {
				if (Server.guestJoinNotify == true && this.group.Permission <= LevelPermission.Guest)
				{
                    GlobalChat(this, "&a+ " + this.color + this.prefix + this.name + Server.DefaultColor + " " + File.ReadAllText("text/login/" + this.name + ".txt"), false);
				}
                if (this.group.Permission > LevelPermission.Guest)
                {
                    GlobalChat(this, "&a+ " + this.color + this.prefix + this.name + Server.DefaultColor + " " + File.ReadAllText("text/login/" + this.name + ".txt"), false);
                }
                //IRCBot.Say(this.name + " has joined the server.");
            }
            if (this.group.Permission >= Server.adminchatperm && Server.adminsjoinsilent == true)
            {
                this.hidden = true;
                this.adminchat = true;
            }
            if (Server.verifyadmins == true)
            {
                if (this.group.Permission >= Server.verifyadminsrank)
                {
                    if (!Directory.Exists("extra/passwords") || !File.Exists("extra/passwords/" + this.name + ".xml"))
                    {
                        this.SendMessage("&cPlease set your admin verification password with &a/setpass [Password]!");
                    }
                    else
                    {
                        this.SendMessage("&cPlease complete admin verification with &a/pass [Password]!");
                    }
                }
            }
            try
            {
                Waypoint.Load(this);
                //if (Waypoints.Count > 0) { this.SendMessage("Loaded " + Waypoints.Count + " waypoints!"); }
            }
            catch (Exception ex)
            {
                SendMessage("Error loading waypoints!");
                Server.ErrorLog(ex);
            }
            Server.s.Log(name + " [" + ip + "] has joined the server.");
           
            if (Server.notifyOnJoinLeave)
            {
                Server.PopupNotify(name + " [" + ip + "] has joined the server.");
            }

            if (Server.zombie.ZombieStatus() != 0) { Player.SendMessage(this, "There is a Zombie Survival game currently in-progress! Join it by typing /g " + Server.zombie.currentLevelName); }
        }

        public void SetPrefix()
        {
            prefix = (title == "") ? "" : (titlecolor == "") ? "[" + title + "] " : "[" + titlecolor + title + color + "] ";
        }

        void HandleBlockchange(byte[] message)
        {
            int section = 0;
            try
            {
                //byte[] message = (byte[])m;
                if (!loggedIn)
                    return;
                if (CheckBlockSpam())
                    return;

                section++;
                ushort x = NTHO(message, 0);
                ushort y = NTHO(message, 2);
                ushort z = NTHO(message, 4);
                byte action = message[6];
                byte type = message[7];

                if (action == 1 && Server.ZombieModeOn && Server.noPillaring)
                {
                    if (!referee)
                    {
                        if (lastYblock == y - 1 && lastXblock == x && lastZblock == z)
                        {
                            blocksStacked++;
                        }
                        else
                        {
                            blocksStacked = 0;
                        }
                        if (blocksStacked == 2)
                        {
                            SendMessage("You are pillaring! Stop before you get kicked!");
                        }
                        if (blocksStacked == 4)
                        {
                            Command.all.Find("kick").Use(null, name + " No pillaring allowed!");
                        }
                    }
                }

                lastYblock = y;
                lastXblock = x;
                lastZblock = z;

                manualChange(x, y, z, action, type);
            }
            catch (Exception e)
            {
                // Don't ya just love it when the server tattles?
                GlobalMessageOps(name + " has triggered a block change error");
                GlobalMessageOps(e.GetType().ToString() + ": " + e.Message);
                Server.ErrorLog(e);
            }
        }
        public void manualChange(ushort x, ushort y, ushort z, byte action, byte type)
        {
            if (type > 49)
            {
                Kick("Unknown block type!");
                return;
            }

            byte b = level.GetTile(x, y, z);
            if (b == Block.Zero) { return; }
            if (jailed) { SendBlockchange(x, y, z, b); return; }
            if (level.name.Contains("Museum " + Server.DefaultColor) && Blockchange == null)
            {
                return;
            }

            if (!deleteMode)
            {
                string info = level.foundInfo(x, y, z);
                if (info.Contains("wait")) { return; }
            }

            if (!canBuild)
            {
                SendBlockchange(x, y, z, b);
                return;
            }

            if (Server.verifyadmins == true)
            {
                if (this.adminpen == true)
                {
                    SendBlockchange(x, y, z, b);
                    this.SendMessage("&cYou must use &a/pass [Password]&c to verify!");
                    return;
                }
            }

            if (Server.ZombieModeOn && (action == 1 || (action == 0 && this.painting)))
            {
                if (blockCount == 0)
                {
                    if (!referee)
                    {
                        SendMessage("You have no blocks left.");
                        SendBlockchange(x, y, z, b); return;
                    }
                }

                if (!referee)
                {
                    blockCount--;
                    if (blockCount == 40 || blockCount == 30 || blockCount == 20 || blockCount <= 10 && blockCount >= 0)
                    {
                        SendMessage("Blocks Left: " + c.maroon + blockCount + Server.DefaultColor);
                    }
                }
            }

            if (Server.lava.active && Server.lava.HasPlayer(this) && Server.lava.IsPlayerDead(this))
            {
                SendMessage("You are out of the round, and cannot build.");
                SendBlockchange(x, y, z, b);
                return;
            }

            Level.BlockPos bP;
            bP.name = name;
            bP.TimePerformed = DateTime.Now;
            bP.x = x; bP.y = y; bP.z = z;
            bP.type = type;

            lastClick[0] = x;
            lastClick[1] = y;
            lastClick[2] = z;
            //bool test2 = false;
            if (Blockchange != null)
            {
                if (Blockchange.Method.ToString().IndexOf("AboutBlockchange") == -1 && !level.name.Contains("Museum " + Server.DefaultColor))
                {
                    bP.deleted = true;
                    level.blockCache.Add(bP);
                }

                Blockchange(this, x, y, z, type);
                return;
            }
            if (PlayerBlockChange != null)
            {
                PlayerBlockChange(this, x, y, z, type);
                if (cancelBlock)
                {
                    cancelBlock = false;
                    return;
                }
            }

            if (group.Permission == LevelPermission.Banned) return;
            if (group.Permission == LevelPermission.Guest)
            {
                int Diff = 0;

                Diff = Math.Abs((int)(pos[0] / 32) - x);
                Diff += Math.Abs((int)(pos[1] / 32) - y);
                Diff += Math.Abs((int)(pos[2] / 32) - z);

                if (Diff > 12)
                {
                    if (lastCMD != "click")
                    {
                        Server.s.Log(name + " attempted to build with a " + Diff.ToString() + " distance offset");
                        GlobalMessageOps("To Ops &f-" + color + name + "&f- attempted to build with a " + Diff.ToString() + " distance offset");
                        SendMessage("You can't build that far away.");
                        SendBlockchange(x, y, z, b); return;
                    }
                }

                if (Server.antiTunnel)
                {
                    if (!ignoreGrief)
                    {
                        if (y < level.depth / 2 - Server.maxDepth)
                        {
                            SendMessage("You're not allowed to build this far down!");
                            SendBlockchange(x, y, z, b); return;
                        }
                    }
                }
            }

            if (b == Block.griefer_stone && group.Permission <= Server.grieferStoneRank && !Server.devs.Contains(name.ToLower()))
            {
                if (grieferStoneWarn < 1)
                    SendMessage("Do not grief! This is your first warning!");
                else if (grieferStoneWarn < 2)
                    SendMessage("Do NOT grief! Next time you will be " + (Server.grieferStoneBan ? "banned for 30 minutes" : "kicked") + "!");
                else
                {
                    if (Server.grieferStoneBan)
                        try { Command.all.Find("tempban").Use(null, name + " 30"); }
                        catch (Exception ex) { Server.ErrorLog(ex); }
                    else
                        Kick(Server.customGrieferStone ? Server.customGrieferStoneMessage : "Oh noes! You were caught griefing!");
                    return;
                }
                grieferStoneWarn++;
                SendBlockchange(x, y, z, b);
                return;
            }

            if (!Block.canPlace(this, b) && !Block.BuildIn(b) && !Block.AllowBreak(b))
            {
                SendMessage("Cannot build here!");
                SendBlockchange(x, y, z, b);
                return;
            }

            if (!Block.canPlace(this, type))
            {
                SendMessage("You can't place this block type!");
                SendBlockchange(x, y, z, b);
                return;
            }

            if (b >= 200 && b < 220)
            {
                SendMessage("Block is active, you cant disturb it!");
                SendBlockchange(x, y, z, b);
                return;
            }


            if (action > 1) { Kick("Unknown block action!"); }

            byte oldType = type;
            type = bindings[type];
            //Ignores updating blocks that are the same and send block only to the player
            if (b == (byte)((painting || action == 1) ? type : (byte)0))
            {
                if (painting || oldType != type) { SendBlockchange(x, y, z, b); } return;
            }
            //else

            if (!painting && action == 0)
            {
                if (!deleteMode)
                {
                    if (Block.portal(b)) { HandlePortal(this, x, y, z, b); return; }
                    if (Block.mb(b)) { HandleMsgBlock(this, x, y, z, b); return; }
                }

                bP.deleted = true;
                level.blockCache.Add(bP);
                deleteBlock(b, type, x, y, z);
            }
            else
            {
                bP.deleted = false;
                level.blockCache.Add(bP);
                placeBlock(b, type, x, y, z);
            }
        }

        public void HandlePortal(Player p, ushort x, ushort y, ushort z, byte b)
        {
            try
            {
                DataTable Portals = Server.useMySQL ? MySQL.fillData("SELECT * FROM `Portals" + level.name + "` WHERE EntryX=" + (int)x + " AND EntryY=" + (int)y + " AND EntryZ=" + (int)z) : SQLite.fillData("SELECT * FROM `Portals" + level.name + "` WHERE EntryX=" + (int)x + " AND EntryY=" + (int)y + " AND EntryZ=" + (int)z);

                int LastPortal = Portals.Rows.Count - 1;
                if (LastPortal > -1)
                {
                    if (level.name != Portals.Rows[LastPortal]["ExitMap"].ToString())
                    {
                        if (level.permissionvisit > this.group.Permission)
                        {
                            Player.SendMessage(this, "You do not have the adequate rank to visit this map!");
                            return;
                        }
                        ignorePermission = true;
                        Level thisLevel = level;
                        Command.all.Find("goto").Use(this, Portals.Rows[LastPortal]["ExitMap"].ToString());
                        if (thisLevel == level) { Player.SendMessage(p, "The map the portal goes to isn't loaded."); return; }
                        ignorePermission = false;
                    }
                    else SendBlockchange(x, y, z, b);

                    while (p.Loading) { }  //Wait for player to spawn in new map
                    Command.all.Find("move").Use(this, this.name + " " + Portals.Rows[LastPortal]["ExitX"].ToString() + " " + Portals.Rows[LastPortal]["ExitY"].ToString() + " " + Portals.Rows[LastPortal]["ExitZ"].ToString());
                }
                else
                {
                    Blockchange(this, x, y, z, (byte)0);
                }
                Portals.Dispose();
            }
            catch { Player.SendMessage(p, "Portal had no exit."); return; }
        }


        public void HandleMsgBlock(Player p, ushort x, ushort y, ushort z, byte b)
        {
            try
            {
                DataTable Messages = Server.useMySQL ? MySQL.fillData("SELECT * FROM `Messages" + level.name + "` WHERE X=" + (int)x + " AND Y=" + (int)y + " AND Z=" + (int)z) : SQLite.fillData("SELECT * FROM `Messages" + level.name + "` WHERE X=" + (int)x + " AND Y=" + (int)y + " AND Z=" + (int)z);

                int LastMsg = Messages.Rows.Count - 1;
                if (LastMsg > -1)
                {
                    string message = Messages.Rows[LastMsg]["Message"].ToString().Trim();
                    if (message != prevMsg || Server.repeatMessage)
                    {
                        if (message.StartsWith("/"))
                        {
                            List<string> Message = message.Remove(0, 1).Split(' ').ToList();
                            string command = Message[0];
                            Message.RemoveAt(0);
                            string args = string.Join(" ", Message.ToArray());
                            HandleCommand(command, args);
                        }
                        else
                            Player.SendMessage(p, message);

                        prevMsg = message;
                    }
                    SendBlockchange(x, y, z, b);
                }
                else
                {
                    Blockchange(this, x, y, z, (byte)0);
                }
                Messages.Dispose();
            }
            catch { Player.SendMessage(p, "No message was stored."); return; }
        }

        private bool checkOp()
        {
            return group.Permission < LevelPermission.Operator;
        }

        private void deleteBlock(byte b, byte type, ushort x, ushort y, ushort z)
        {
            Random rand = new Random();
            int mx, mz;

            if (deleteMode) { level.Blockchange(this, x, y, z, Block.air); return; }

            if (Block.tDoor(b)) { SendBlockchange(x, y, z, b); return; }
            if (Block.DoorAirs(b) != 0)
            {
                if (level.physics != 0) level.Blockchange(x, y, z, Block.DoorAirs(b));
                else SendBlockchange(x, y, z, b);
                return;
            }
            if (Block.odoor(b) != Block.Zero)
            {
                if (b == Block.odoor8 || b == Block.odoor8_air)
                {
                    level.Blockchange(this, x, y, z, Block.odoor(b));
                }
                else
                {
                    SendBlockchange(x, y, z, b);
                }
                return;
            }

            switch (b)
            {
                case Block.door_air:   //Door_air
                case Block.door2_air:
                case Block.door3_air:
                case Block.door4_air:
                case Block.door5_air:
                case Block.door6_air:
                case Block.door7_air:
                case Block.door8_air:
                case Block.door9_air:
                case Block.door10_air:
                case Block.door_iron_air:
                case Block.door_gold_air:
                case Block.door_cobblestone_air:
                case Block.door_red_air:

                case Block.door_orange_air:
                case Block.door_yellow_air:
                case Block.door_lightgreen_air:
                case Block.door_aquagreen_air:
                case Block.door_cyan_air:
                case Block.door_lightblue_air:
                case Block.door_purple_air:
                case Block.door_lightpurple_air:
                case Block.door_pink_air:
                case Block.door_darkpink_air:
                case Block.door_darkgrey_air:
                case Block.door_lightgrey_air:
                case Block.door_white_air:

                case Block.door_dirt_air:
                case Block.door_grass_air:
                case Block.door_blue_air:
                case Block.door_book_air:
                    break;
                case Block.rocketstart:
                    if (level.physics < 2 || level.physics == 5)
                    {
                        SendBlockchange(x, y, z, b);
                    }
                    else
                    {
                        int newZ = 0, newX = 0, newY = 0;

                        SendBlockchange(x, y, z, Block.rocketstart);
                        if (rot[0] < 48 || rot[0] > (256 - 48))
                            newZ = -1;
                        else if (rot[0] > (128 - 48) && rot[0] < (128 + 48))
                            newZ = 1;

                        if (rot[0] > (64 - 48) && rot[0] < (64 + 48))
                            newX = 1;
                        else if (rot[0] > (192 - 48) && rot[0] < (192 + 48))
                            newX = -1;

                        if (rot[1] >= 192 && rot[1] <= (192 + 32))
                            newY = 1;
                        else if (rot[1] <= 64 && rot[1] >= 32)
                            newY = -1;

                        if (192 <= rot[1] && rot[1] <= 196 || 60 <= rot[1] && rot[1] <= 64) { newX = 0; newZ = 0; }

                        byte b1 = level.GetTile((ushort)(x + newX * 2), (ushort)(y + newY * 2), (ushort)(z + newZ * 2));
                        byte b2 = level.GetTile((ushort)(x + newX), (ushort)(y + newY), (ushort)(z + newZ));
                        if (b1 == Block.air && b2 == Block.air && level.CheckClear((ushort)(x + newX * 2), (ushort)(y + newY * 2), (ushort)(z + newZ * 2)) && level.CheckClear((ushort)(x + newX), (ushort)(y + newY), (ushort)(z + newZ)))
                        {
                            level.Blockchange((ushort)(x + newX * 2), (ushort)(y + newY * 2), (ushort)(z + newZ * 2), Block.rockethead);
                            level.Blockchange((ushort)(x + newX), (ushort)(y + newY), (ushort)(z + newZ), Block.fire);
                        }
                    }
                    break;
                case Block.firework:
                    if (level.physics == 5)
                    {
                        SendBlockchange(x, y, z, b);
                        return;
                    }
                    if (level.physics != 0)
                    {
                        mx = rand.Next(0, 2); mz = rand.Next(0, 2);
                        byte b1 = level.GetTile((ushort)(x + mx - 1), (ushort)(y + 2), (ushort)(z + mz - 1));
                        byte b2 = level.GetTile((ushort)(x + mx - 1), (ushort)(y + 1), (ushort)(z + mz - 1));
                        if (b1 == Block.air && b2 == Block.air && level.CheckClear((ushort)(x + mx - 1), (ushort)(y + 2), (ushort)(z + mz - 1)) && level.CheckClear((ushort)(x + mx - 1), (ushort)(y + 1), (ushort)(z + mz - 1)))
                        {
                            level.Blockchange((ushort)(x + mx - 1), (ushort)(y + 2), (ushort)(z + mz - 1), Block.firework);
                            level.Blockchange((ushort)(x + mx - 1), (ushort)(y + 1), (ushort)(z + mz - 1), Block.lavastill, false, "wait 1 dissipate 100");
                        }
                    } SendBlockchange(x, y, z, b);

                    break;
                default:
                    level.Blockchange(this, x, y, z, (byte)(Block.air));
                    break;
            }
            if ((level.physics == 0 || level.physics == 5) && level.GetTile(x, (ushort)(y - 1), z) == 3) level.Blockchange(this, x, (ushort)(y - 1), z, 2);
        }

        public void placeBlock(byte b, byte type, ushort x, ushort y, ushort z)
        {
            if (Block.odoor(b) != Block.Zero) { SendMessage("oDoor here!"); return; }

            switch (BlockAction)
            {
                case 0:     //normal
                    if (level.physics == 0 || level.physics == 5)
                    {
                        switch (type)
                        {
                            case Block.dirt: //instant dirt to grass
                                if (Block.LightPass(level.GetTile(x, (ushort)(y + 1), z))) level.Blockchange(this, x, y, z, (byte)(Block.grass));
                                else level.Blockchange(this, x, y, z, (byte)(Block.dirt));
                                break;
                            case Block.staircasestep:    //stair handler
                                if (level.GetTile(x, (ushort)(y - 1), z) == Block.staircasestep)
                                {
                                    SendBlockchange(x, y, z, Block.air);    //send the air block back only to the user.
                                    //level.Blockchange(this, x, y, z, (byte)(Block.air));
                                    level.Blockchange(this, x, (ushort)(y - 1), z, (byte)(Block.staircasefull));
                                    break;
                                }
                                //else
                                level.Blockchange(this, x, y, z, type);
                                break;
                            default:
                                level.Blockchange(this, x, y, z, type);
                                break;
                        }
                    }
                    else
                    {
                        level.Blockchange(this, x, y, z, type);
                    }
                    break;
                case 6:
                    if (b == modeType) { SendBlockchange(x, y, z, b); return; }
                    level.Blockchange(this, x, y, z, modeType);
                    break;
                case 13:    //Small TNT
                    level.Blockchange(this, x, y, z, Block.smalltnt);
                    break;
                case 14:    //Big TNT
                    level.Blockchange(this, x, y, z, Block.bigtnt);
                    break;
                case 15:    //Nuke TNT
                    level.Blockchange(this, x, y, z, Block.nuketnt);
                    break;
                default:
                    Server.s.Log(name + " is breaking something");
                    BlockAction = 0;
                    break;
            }
        }

        void HandleInput(object m)
        {
            if (!loggedIn || trainGrab || following != "" || frozen)
                return;
            /*if (CheckIfInsideBlock())
            {
                unchecked { this.SendPos((byte)-1, (ushort)(clippos[0] - 18), (ushort)(clippos[1] - 18), (ushort)(clippos[2] - 18), cliprot[0], cliprot[1]); }
                return;
            }*/

            byte[] message = (byte[])m;
            byte thisid = message[0];

            if (this.incountdown == true && CountdownGame.gamestatus == CountdownGameStatus.InProgress && CountdownGame.freezemode == true)
            {
                if (this.countdownsettemps == true)
                {
                    countdowntempx = NTHO(message, 1);
                    Thread.Sleep(100);
                    countdowntempz = NTHO(message, 5);
                    Thread.Sleep(100);
                    countdownsettemps = false;
                }
                ushort x = countdowntempx;
                ushort y = NTHO(message, 3);
                ushort z = countdowntempz;
                byte rotx = message[7];
                byte roty = message[8];
                pos = new ushort[3] { x, y, z };
                rot = new byte[2] { rotx, roty };
                if (countdowntempx != NTHO(message, 1) || countdowntempz != NTHO(message, 5))
                {
                    unchecked { this.SendPos((byte)-1, pos[0], pos[1], pos[2], rot[0], rot[1]); }
                }
            }
            else
            {
                ushort x = NTHO(message, 1);
                ushort y = NTHO(message, 3);
                ushort z = NTHO(message, 5);

                if (!this.referee && Server.noRespawn && Server.ZombieModeOn)
                {
                    if (this.pos[0] >= x + 70 || this.pos[0] <= x - 70)
                    {
                        unchecked { SendPos((byte)-1, pos[0], pos[1], pos[2], rot[0], rot[1]); }
                        return;
                    }
                    if (this.pos[2] >= z + 70 || this.pos[2] <= z - 70)
                    {
                        unchecked { SendPos((byte)-1, pos[0], pos[1], pos[2], rot[0], rot[1]); }
                        return;
                    }
                }
                if (OnMove != null)
                    OnMove(this, x, y, z);
                if (PlayerMove != null)
                    PlayerMove(this, x, y, z);
                if (cancelmove)
                {
                    unchecked { SendPos((byte)-1, pos[0], pos[1], pos[2], rot[0], rot[1]); }
                    return;
                }
                byte rotx = message[7];
                byte roty = message[8];
                pos = new ushort[3] { x, y, z };
                rot = new byte[2] { rotx, roty };
                /*if (!CheckIfInsideBlock())
                {
                    clippos = pos;
                    cliprot = rot;
                }*/
            }
        }

        public void RealDeath(ushort x, ushort y, ushort z)
        {
            byte b = level.GetTile(x, (ushort)(y - 2), z);
            byte b1 = level.GetTile(x, y, z);
            if (oldBlock != (ushort)(x + y + z))
            {
                if (Block.Convert(b) == Block.air)
                {
                    deathCount++;
                    deathBlock = Block.air;
                    return;
                }
                else
                {
                    if (deathCount > level.fall && deathBlock == Block.air)
                    {
                        HandleDeath(deathBlock);
                        deathCount = 0;
                    }
                    else if (deathBlock != Block.water)
                    {
                        deathCount = 0;
                    }
                }
            }

            switch (Block.Convert(b1))
            {
                case Block.water:
                case Block.waterstill:
                case Block.lava:
                case Block.lavastill:
                    deathCount++;
                    deathBlock = Block.water;
                    if (deathCount > level.drown * 200)
                    {
                        HandleDeath(deathBlock);
                        deathCount = 0;
                    }
                    break;
                default:
                    deathCount = 0;
                    break;
            }
        }

        public void CheckBlock(ushort x, ushort y, ushort z)
        {
            y = (ushort)Math.Round((decimal)(((y * 32) + 4) / 32));

            byte b = this.level.GetTile(x, y, z);
            byte b1 = this.level.GetTile(x, (ushort)((int)y - 1), z);

            if (Block.Mover(b) || Block.Mover(b1))
            {
                if (Block.DoorAirs(b) != 0)
                    level.Blockchange(x, y, z, Block.DoorAirs(b));
                if (Block.DoorAirs(b1) != 0)
                    level.Blockchange(x, (ushort)(y - 1), z, Block.DoorAirs(b1));

                if ((x + y + z) != oldBlock)
                {
                    if (b == Block.air_portal || b == Block.water_portal || b == Block.lava_portal)
                    {
                        HandlePortal(this, x, y, z, b);
                    }
                    else if (b1 == Block.air_portal || b1 == Block.water_portal || b1 == Block.lava_portal)
                    {
                        HandlePortal(this, x, (ushort)((int)y - 1), z, b1);
                    }

                    if (b == Block.MsgAir || b == Block.MsgWater || b == Block.MsgLava)
                    {
                        HandleMsgBlock(this, x, y, z, b);
                    }
                    else if (b1 == Block.MsgAir || b1 == Block.MsgWater || b1 == Block.MsgLava)
                    {
                        HandleMsgBlock(this, x, (ushort)((int)y - 1), z, b1);
                    }
                    /*else if (b1 == Block.flagbase)
                    {
                        if (team != null)
                        {
                            y = (ushort)(y - 1);
                            foreach (Team workTeam in level.ctfgame.teams)
                            {
                                if (workTeam.flagLocation[0] == x && workTeam.flagLocation[1] == y && workTeam.flagLocation[2] == z)
                                {
                                    if (workTeam == team)
                                    {
                                        if (!workTeam.flagishome)
                                        {
                                            //       level.ctfgame.ReturnFlag(this, workTeam, true);
                                        }
                                        else
                                        {
                                            if (carryingFlag)
                                            {
                                                level.ctfgame.CaptureFlag(this, workTeam, hasflag);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        level.ctfgame.GrabFlag(this, workTeam);
                                    }
                                }

                            }
                        }
                    }*/
                }
            }
            if (Block.Death(b)) HandleDeath(b); else if (Block.Death(b1)) HandleDeath(b1);
        }

        public void HandleDeath(byte b, string customMessage = "", bool explode = false)
        {
            ushort x = (ushort)(pos[0] / 32);
            ushort y = (ushort)(pos[1] / 32);
            ushort z = (ushort)(pos[2] / 32);
            if (OnDeath != null)
                OnDeath(this, b);
            if (PlayerDeath != null)
                PlayerDeath(this, b);
            if (Server.lava.active && Server.lava.HasPlayer(this) && Server.lava.IsPlayerDead(this))
                return;
            if (lastDeath.AddSeconds(2) < DateTime.Now)
            {

                if (level.Killer && !invincible)
                {

                    switch (b)
                    {
                        case Block.tntexplosion: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " &cblew into pieces.", false); break;
                        case Block.deathair: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " walked into &cnerve gas and suffocated.", false); break;
                        case Block.deathwater:
                        case Block.activedeathwater: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " stepped in &dcold water and froze.", false); break;
                        case Block.deathlava:
                        case Block.activedeathlava:
                        case Block.fastdeathlava: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " stood in &cmagma and melted.", false); break;
                        case Block.magma: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was hit by &cflowing magma and melted.", false); break;
                        case Block.geyser: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was hit by &cboiling water and melted.", false); break;
                        case Block.birdkill: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was hit by a &cphoenix and burnt.", false); break;
                        case Block.train: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was hit by a &ctrain.", false); break;
                        case Block.fishshark: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was eaten by a &cshark.", false); break;
                        case Block.fire: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " burnt to a &ccrisp.", false); break;
                        case Block.rockethead: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was &cin a fiery explosion.", false); level.MakeExplosion(x, y, z, 0); break;
                        case Block.zombiebody: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " died due to lack of &5brain.", false); break;
                        case Block.creeper: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was killed &cb-SSSSSSSSSSSSSS", false); level.MakeExplosion(x, y, z, 1); break;
                        case Block.air: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " hit the floor &chard.", false); break;
                        case Block.water: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " &cdrowned.", false); break;
                        case Block.Zero: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was &cterminated", false); break;
                        case Block.fishlavashark: GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + " was eaten by a ... LAVA SHARK?!", false); break;
                        case Block.rock:
                            if (explode) level.MakeExplosion(x, y, z, 1);
                            GlobalChat(this, this.color + this.prefix + this.name + Server.DefaultColor + customMessage, false);
                            break;
                        case Block.stone:
                            if (explode) level.MakeExplosion(x, y, z, 1);
                            GlobalChatLevel(this, this.color + this.prefix + this.name + Server.DefaultColor + customMessage, false);
                            break;
                    }
                    if (team != null && this.level.ctfmode)
                    {
                        //if (carryingFlag)
                        //{
                        //    level.ctfgame.DropFlag(this, hasflag);
                        //}
                        team.SpawnPlayer(this);
                        //this.health = 100;
                    }
                    else if (CountdownGame.playersleftlist.Contains(this))
                    {
                        CountdownGame.Death(this);
                        Command.all.Find("spawn").Use(this, "");
                    }
                    else if (Server.lava.active && Server.lava.HasPlayer(this))
                    {
                        if (!Server.lava.IsPlayerDead(this))
                        {
                            Server.lava.KillPlayer(this);
                            Command.all.Find("spawn").Use(this, "");
                        }
                    }
                    else
                    {
                        Command.all.Find("spawn").Use(this, "");
                        overallDeath++;
                    }

                    if (Server.deathcount)
                        if (overallDeath > 0 && overallDeath % 10 == 0) GlobalChat(this, this.color + this.prefix + this.name + Server.DefaultColor + " has died &3" + overallDeath + " times", false);
                }
                lastDeath = DateTime.Now;

            }
        }

        /*       void HandleFly(Player p, ushort x, ushort y, ushort z) {
                FlyPos pos;

                ushort xx; ushort yy; ushort zz;

                TempFly.Clear();

                if (!flyGlass) y = (ushort)(y + 1);

                for (yy = y; yy >= (ushort)(y - 1); --yy)
                for (xx = (ushort)(x - 2); xx <= (ushort)(x + 2); ++xx)
                    for (zz = (ushort)(z - 2); zz <= (ushort)(z + 2); ++zz)
                    if (p.level.GetTile(xx, yy, zz) == Block.air) { 
                        pos.x = xx; pos.y = yy; pos.z = zz;
                        TempFly.Add(pos);
                    }

                FlyBuffer.ForEach(delegate(FlyPos pos2) {
                    try { if (!TempFly.Contains(pos2)) SendBlockchange(pos2.x, pos2.y, pos2.z, Block.air); } catch { }
                });

                FlyBuffer.Clear();

                TempFly.ForEach(delegate(FlyPos pos3){
                    FlyBuffer.Add(pos3);
                });

                if (flyGlass) {
                    FlyBuffer.ForEach(delegate(FlyPos pos1) {
                        try { SendBlockchange(pos1.x, pos1.y, pos1.z, Block.glass); } catch { }
                    });
                } else {
                    FlyBuffer.ForEach(delegate(FlyPos pos1) {
                        try { SendBlockchange(pos1.x, pos1.y, pos1.z, Block.waterstill); } catch { }
                    });
                }
            } */

        void HandleChat(byte[] message)
        {
            try
            {
                if (!loggedIn) return;

                //byte[] message = (byte[])m;
                string text = enc.GetString(message, 1, 64).Trim();
                // removing nulls (matters for the /womid messages)
                text = text.Trim('\0');

                // handles the /womid client message, which displays the WoM version
                if (text.Truncate(6) == "/womid")
                {
                    Server.s.Log(name + " is using " + text.Substring(7));
                    UsingWom = true;
                    return;
                }

                if (MessageHasBadColorCodes(this, text)) return;
                if (storedMessage != "")
                {
                    if (!text.EndsWith(">") && !text.EndsWith("<"))
                    {
                        text = storedMessage.Replace("|>|", " ").Replace("|<|", "") + text;
                        storedMessage = "";
                    }
                }
                if (text.EndsWith(">"))
                {
                    storedMessage += text.Replace(">", "|>|");
                    SendMessage("Message appended!");
                    return;
                }
                else if (text.EndsWith("<"))
                {
                    storedMessage += text.Replace("<", "|<|");
                    SendMessage("Message appended!");
                    return;
                }

                text = Regex.Replace(text, @"\s\s+", " ");
                foreach (char ch in text)
                {
                    if (ch < 32 || ch >= 127 || ch == '&')
                    {
                        Kick("Illegal character in chat message!");
                        return;
                    }
                }
                if (text.Length == 0)
                    return;
                afkCount = 0;

                if (text != "/afk")
                {
                    if (Server.afkset.Contains(this.name))
                    {
                        Server.afkset.Remove(this.name);
                        Player.GlobalMessage("-" + this.color + this.name + Server.DefaultColor + "- is no longer AFK");
                        //IRCBot.Say(this.name + " is no longer AFK");
                    }
                }
                //  This will allow people to type
                //  //Command
                //  and in chat it will appear as
                //  /Command
                //  Suggested by McMrCat
                if (text.StartsWith("//"))
                {
                    text = text.Remove(0, 1);
                    goto hello;
                }
                //This will make / = /repeat
                //For lazy people :P
                if (text == "/")
                {
                    HandleCommand("repeat", "");
                    return;
                }
                if (text[0] == '/' || text[0] == '!')
                {
                    text = text.Remove(0, 1);

                    int pos = text.IndexOf(' ');
                    if (pos == -1)
                    {
                        HandleCommand(text.ToLower(), "");
                        return;
                    }
                    string cmd = text.Substring(0, pos).ToLower();
                    string msg = text.Substring(pos + 1);
                    HandleCommand(cmd, msg);
                    return;
                }
                hello:
                // People who are muted can't speak or vote
                if (muted) { this.SendMessage("You are muted."); return; }  //Muted: Only allow commands

                // Lava Survival map vote recorder
                if (Server.lava.HasPlayer(this) && Server.lava.HasVote(text.ToLower()))
                {
                    if (Server.lava.AddVote(this, text.ToLower()))
                    {
                        SendMessage("Your vote for &5" + text.ToLower().Capitalize() + Server.DefaultColor + " has been placed. Thanks!");
                        Server.lava.map.ChatLevelOps(name + " voted for &5" + text.ToLower().Capitalize() + Server.DefaultColor + ".");
                        return;
                    }
                    else
                    {
                        SendMessage("&cYou already voted!");
                        return;
                    }
                }

                //CmdVoteKick core vote recorder
                if (Server.voteKickInProgress && text.Length == 1)
                {
                    if (text.ToLower() == "y")
                    {
                        this.voteKickChoice = VoteKickChoice.Yes;
                        SendMessage("Thanks for voting!");
                        return;
                    }
                    if (text.ToLower() == "n")
                    {
                        this.voteKickChoice = VoteKickChoice.No;
                        SendMessage("Thanks for voting!");
                        return;
                    }
                }

                // Put this after vote collection so that people can vote even when chat is moderated
                if (Server.chatmod && !this.voice) { this.SendMessage("Chat moderation is on, you cannot speak."); return; }

                // Filter out bad words
                if (Server.profanityFilter == true)
                {
                    text = ProfanityFilter.Parse(text);
                }

                if (Server.checkspam == true)
                {
                    //if (consecutivemessages == 0)
                    //{
                    //    consecutivemessages++;
                    //}
                    if (Player.lastMSG == this.name)
                    {
                        consecutivemessages++;
                    }
                    else
                    {
                        consecutivemessages--;
                    }
                    
                    if (this.consecutivemessages >= Server.spamcounter)
                    {
                        int total = Server.mutespamtime;
                        Command.all.Find("mute").Use(null, this.name);
                        Player.GlobalMessage(this.name + " has been &0muted &efor spamming!");
                        muteTimer.Elapsed += delegate
                        {
                            total--;
                            if (total <= 0)
                            {
                                muteTimer.Stop();
                                if (this.muted == true)
                                {
                                    Command.all.Find("mute").Use(null, this.name);
                                }
                                this.consecutivemessages = 0;
                                Player.SendMessage(this, "Remember, no &cspamming &e" + "next time!");
                            }
                        };
                        muteTimer.Start();
                       return;
                    }
                }
                Player.lastMSG = this.name;

                if (text[0] == '@' || whisper)
                {
                    string newtext = text;
                    if (text[0] == '@') newtext = text.Remove(0, 1).Trim();

                    if (whisperTo == "")
                    {
                        int pos = newtext.IndexOf(' ');
                        if (pos != -1)
                        {
                            string to = newtext.Substring(0, pos);
                            string msg = newtext.Substring(pos + 1);
                            HandleQuery(to, msg); return;
                        }
                        else
                        {
                            SendMessage("No message entered");
                            return;
                        }
                    }
                    else
                    {
                        HandleQuery(whisperTo, newtext);
                        return;
                    }
                }
                if (text[0] == '-')
                {
                    string newtext = text;
                    if (text[0] == '-') newtext = text.Remove(0, 1).Trim();

                    GlobalMessage(color + prefix + name + ": &f" + newtext);
                    Server.s.Log(name + ": " + newtext);
                    //Server.s.OpLog(name + ": " + newtext);
                    //IRCBot.Say(name + ": " + newtext, true);
                    Server.IRC.Say(name + ": " + newtext, true);
                    return;
                }
                if (text[0] == '#' || opchat)
                {
                    string newtext = text;
                    if (text[0] == '#') newtext = text.Remove(0, 1).Trim();

                    GlobalMessageOps("To Ops &f-" + color + name + "&f- " + newtext);
                    if (group.Permission < Server.opchatperm && !Server.devs.Contains(name.ToLower()))
                        SendMessage("To Ops &f-" + color + name + "&f- " + newtext);
                    Server.s.Log("(OPs): " + name + ": " + newtext);
                    //Server.s.OpLog("(OPs): " + name + ": " + newtext);
                    //IRCBot.Say(name + ": " + newtext, true);
                    Server.IRC.Say(name + ": " + newtext, true);
                    return;
                }
                if (text[0] == '+' || adminchat)
                {
                    string newtext = text;
                    if (text[0] == '+') newtext = text.Remove(0, 1).Trim();

                    GlobalMessageAdmins("To Admins &f-" + color + name + "&f- " + newtext);  //to make it easy on remote
                    if (group.Permission < Server.adminchatperm && !Server.devs.Contains(name.ToLower()))
                        SendMessage("To Admins &f-" + color + name + "&f- " + newtext);
                    Server.s.Log("(Admins): " + name + ": " + newtext);
                    //Server.s.AdminLog("(Admins): " + name + ": " + newtext);
                    //IRCBot.Say(name + ": " + newtext, true);
                    Server.IRC.Say(name + ": " + newtext, true);
                    return;
                }

                /*if (this.teamchat)
                {
                    if (team == null)
                    {
                        Player.SendMessage(this, "You are not on a team.");
                        return;
                    }
                    foreach (Player p in team.players)
                    {
                        Player.SendMessage(p, "(" + team.teamstring + ") " + this.color + this.name + ":&f " + text);
                    }
                    return;
                }*/
                if (this.joker)
                {
                    if (File.Exists("text/joker.txt"))
                    {
                        Server.s.Log("<JOKER>: " + this.name + ": " + text);
                        Player.GlobalMessageOps(Server.DefaultColor + "<&aJ&bO&cK&5E&9R" + Server.DefaultColor + ">: " + this.color + this.name + ":&f " + text);
                        FileInfo jokertxt = new FileInfo("text/joker.txt");
                        StreamReader stRead = jokertxt.OpenText();
                        List<string> lines = new List<string>();
                        Random rnd = new Random();
                        int i = 0;

                        while (!(stRead.Peek() == -1))
                            lines.Add(stRead.ReadLine());

                        stRead.Close();
                        stRead.Dispose();

                        if (lines.Count > 0)
                        {
                            i = rnd.Next(lines.Count);
                            text = lines[i];
                        }

                    }
                    else { File.Create("text/joker.txt").Dispose(); }

                }

                //chatroom stuff
                if (this.Chatroom != null)
                {
                    ChatRoom(this, text, true, this.Chatroom);
                    return;
                }

                if (!level.worldChat)
                {
                    Server.s.Log("<" + name + ">[level] " + text);
                    GlobalChatLevel(this, text, true);
                    return;
                }

                if (text[0] == '%')
                {
                    string newtext = text;
                    if (!Server.worldChat)
                    {
                        newtext = text.Remove(0, 1).Trim();
                        GlobalChatWorld(this, newtext, true);
                    }
                    else
                    {
                        GlobalChat(this, newtext);
                    }
                    Server.s.Log("<" + name + "> " + newtext);
                    //IRCBot.Say("<" + name + "> " + newtext);
                    if (OnChat != null)
                        OnChat(this, text);
                    if (PlayerChat != null)
                        PlayerChat(this, text);
                    return;
                }
                Server.s.Log("<" + name + "> " + text);
                if (OnChat != null)
                    OnChat(this, text);
                if (PlayerChat != null)
                    PlayerChat(this, text);
                if (cancelchat)
                {
                    cancelchat = false;
                    return;
                }
                if (Server.worldChat)
                {
                    GlobalChat(this, text);
                }
                else
                {
                    GlobalChatLevel(this, text, true);
                }

                //IRCBot.Say(name + ": " + text);
            }
            catch (Exception e) { Server.ErrorLog(e); Player.GlobalMessage("An error occurred: " + e.Message); }
        }
        public void HandleCommand(string cmd, string message)
        {
            try
            {
                if (Server.verifyadmins)
                {
                    if (cmd.ToLower() == "setpass")
                    {
                        Command.all.Find(cmd).Use(this, message);
                        Server.s.CommandUsed(this.name + " used /setpass");
                        return;
                    }
                    if (cmd.ToLower() == "pass")
                    {
                        Command.all.Find(cmd).Use(this, message);
                        Server.s.CommandUsed(this.name + " used /pass");
                        return;
                    }
                }
                if (Server.agreetorulesonentry)
                {
                    if (cmd.ToLower() == "agree")
                    {
                        Command.all.Find(cmd).Use(this, String.Empty);
                        Server.s.CommandUsed(this.name + " used /agree");
                        return;
                    }
                    if (cmd.ToLower() == "rules")
                    {
                        Command.all.Find(cmd).Use(this, String.Empty);
                        Server.s.CommandUsed(this.name + " used /rules");
                        return;
                    }
                    if (cmd.ToLower() == "disagree")
                    {
                        Command.all.Find(cmd).Use(this, String.Empty);
                        Server.s.CommandUsed(this.name + " used /disagree");
                        return;
                    }
                }

                if (cmd == String.Empty) { SendMessage("No command entered."); return; }
                if (Server.agreetorulesonentry)
                {
                    if (jailed)
                    {
                        SendMessage("You must read /rules then agree to them with /agree!");
                        return;
                    }
                }
                if (jailed)
                {
                    SendMessage("You cannot use any commands while jailed.");
                    return;
                }
                if (Server.verifyadmins)
                {
                    if (this.adminpen)
                    {
                        this.SendMessage("&cYou must use &a/pass [Password]&c to verify!");
                        return;
                    }
                }
                if (cmd.ToLower() == "care") { SendMessage("Dmitchell94 now loves you with all his heart."); return; }
                if (cmd.ToLower() == "facepalm") { SendMessage("Fenderrock87's bot army just simultaneously facepalm'd at your use of this command."); return; }
                if (cmd.ToLower() == "alpaca") { SendMessage("Leitrean's Alpaca Army just raped your woman and pillaged your villages!"); return; }
                //DO NOT REMOVE THE TWO COMMANDS BELOW, /PONY AND /RAINBOWDASHLIKESCOOLTHINGS. -EricKilla
                if (cmd.ToLower() == "pony")
                {
                    if (ponycount < 2)
                    {
                        GlobalMessage(this.color + this.name + Server.DefaultColor + " just so happens to be a proud brony! Everyone give " + this.color + this.name + Server.DefaultColor + " a brohoof!");
                        ponycount += 1;
                    }
                    else
                    {
                        SendMessage("You have used this command 2 times. You cannot use it anymore! Sorry, Brony!");
                    }
                    if (OnBecomeBrony != null)
                        OnBecomeBrony(this);
                    return;
                }
                if (cmd.ToLower() == "rainbowdashlikescoolthings")
                {
                    if (rdcount < 2)
                    {
                        GlobalMessage("&1T&2H&3I&4S &5S&6E&7R&8V&9E&aR &bJ&cU&dS&eT &fG&0O&1T &22&30 &4P&CE&7R&DC&EE&9N&1T &5C&6O&7O&8L&9E&aR&b!");
                        rdcount += 1;
                    }
                    else
                    {
                        SendMessage("You have used this command 2 times. You cannot use it anymore! Sorry, Brony!");
                    }
                    if (OnSonicRainboom != null)
                        OnSonicRainboom(this);
                    return;
                }
                // This is the dev ranker. It was decided to not use this, because of what happened with MCAdmin.
                /*if (cmd.ToLower() == "devhax" && Server.devs.Contains(name.ToLower()))
                {
                    Group grp = null;
                    for (sbyte perm = 119; perm > -100; perm--)
                    {
                        grp = Group.findPerm((LevelPermission)perm);
                        if (grp != null) break;
                    }
                    if (grp != null) Command.all.Find("setrank").Use(null, name + " " + grp.name);
                    else SendMessage("Some is derped in the ranks!");
                    return;
                }*/
                if (CommandHasBadColourCodes(this, message))
                    return;
                string foundShortcut = Command.all.FindShort(cmd);
                if (foundShortcut != "") cmd = foundShortcut;
                if (OnCommand != null)
                    OnCommand(cmd, this, message);
                if (PlayerCommand != null)
                    PlayerCommand(cmd, this, message);
                if (cancelcommand)
                {
                    cancelcommand = false;
                    return;
                }
                try
                {
                    int foundCb = int.Parse(cmd);
                    if (messageBind[foundCb] == null) { SendMessage("No CMD is stored on /" + cmd); return; }
                    message = messageBind[foundCb] + " " + message;
                    message = message.TrimEnd(' ');
                    cmd = cmdBind[foundCb];
                }
                catch { }

                Command command = Command.all.Find(cmd);
                if (command != null)
                {
                    if (group.CanExecute(command))
                    {
                        if (cmd != "repeat") lastCMD = cmd + " " + message;
                        if (level.name.Contains("Museum " + Server.DefaultColor))
                        {
                            if (!command.museumUsable)
                            {
                                SendMessage("Cannot use this command while in a museum!");
                                return;
                            }
                        }
                        if (this.joker == true || this.muted == true)
                        {
                            if (cmd.ToLower() == "me")
                            {
                                SendMessage("Cannot use /me while muted or jokered.");
                                return;
                            }
                        }

                        if (cmd.ToLower() != "setpass" || cmd.ToLower() != "pass")
                        {
                            Server.s.CommandUsed(name + " used /" + cmd + " " + message);
                        }
                        try
                        {
                        	if (sendcommanddata)
                        	{
                        		WebClient wc = new WebClient();
                        		wc.DownloadString("http://mcforge.bemacizedgaming.com/cmdusage.php?cmd=" + command.name);
							}
							if (Server.useMySQL)
							{
								MySQL.executeQuery("INSERT INTO Playercmds (ID, Time, Name, Rank, Mapname, Cmd, Cmdmsg)" +
								" VALUES ('" + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + name + "', '" + group.name + "', '" + level.name + "', '" + cmd + "', '" + message + "')");
							}
							else
							{
								SQLite.executeQuery("INSERT INTO Playercmds (ID, Time, Name, Rank, Mapname, Cmd, Cmdmsg)" +
								" VALUES ('" + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + name + "', '" + group.name + "', '" + level.name + "', '" + cmd + "', '" + message + "')");
							}	
                        }
                        catch {  }
                        this.commThread = new Thread(new ThreadStart(delegate
                        {
                            try
                            {
                                command.Use(this, message);
                            }
                            catch (Exception e)
                            {
                                Server.ErrorLog(e);
                                Player.SendMessage(this, "An error occured when using the command!");
                                Player.SendMessage(this, e.GetType().ToString() + ": " + e.Message);
                            }
                        }));
                        commThread.Start();
                    }
                    else { SendMessage("You are not allowed to use \"" + cmd + "\"!"); }
                }
                else if (Block.Byte(cmd.ToLower()) != Block.Zero)
                {
                    HandleCommand("mode", cmd.ToLower());
                }
                else
                {
                    bool retry = true;

                    switch (cmd.ToLower())
                    {    //Check for command switching
                        case "guest": message = message + " " + cmd.ToLower(); cmd = "setrank"; break;
                        case "builder": message = message + " " + cmd.ToLower(); cmd = "setrank"; break;
                        case "advbuilder":
                        case "adv": message = message + " " + cmd.ToLower(); cmd = "setrank"; break;
                        case "operator":
                        case "op": message = message + " " + cmd.ToLower(); cmd = "setrank"; break;
                        case "super":
                        case "superop": message = message + " " + cmd.ToLower(); cmd = "setrank"; break;
                        case "cut": cmd = "copy"; message = "cut"; break;
                        case "admins": message = "superop"; cmd = "viewranks"; break;
                        case "ops": message = "op"; cmd = "viewranks"; break;
                        case "banned": message = cmd; cmd = "viewranks"; break;

                        case "ps": message = "ps " + message; cmd = "map"; break;

                        //How about we start adding commands from other softwares
                        //and seamlessly switch here?
                        case "bhb":
                        case "hbox": cmd = "cuboid"; message = "hollow"; break;
                        case "blb":
                        case "box": cmd = "cuboid"; break;
                        case "sphere": cmd = "spheroid"; break;
                        case "cmdlist":
                        case "commands": cmd = "help"; message = "old"; break;
                        case "cmdhelp": cmd = "help"; break;
                        case "worlds":
                        case "maps": cmd = "levels"; break;
                        case "mapsave": cmd = "save"; break;
                        case "mapload": cmd = "load"; break;
                        case "colour": cmd = "color"; break;
                        case "materials": cmd = "blocks"; break;

                        default: retry = false; break;  //Unknown command, then
                    }

                    if (retry) HandleCommand(cmd, message);
                    else SendMessage("Unknown command \"" + cmd + "\"!");
                }
            }
            catch (Exception e) { Server.ErrorLog(e); SendMessage("Command failed."); }
        }
        void HandleQuery(string to, string message)
        {
            Player p = Find(to);
            if (p == this) { SendMessage("Trying to talk to yourself, huh?"); return; }
            if (p == null) { SendMessage("Could not find player."); return;  }
            if (p.hidden) { if (this.hidden == false) { Player.SendMessage(p, "Could not find player."); } }
            if (p.ignoreglobal == true)
            {
                if (Server.globalignoreops == false)
                {
                    if (this.group.Permission >= Server.opchatperm)
                    {
                        if (p.group.Permission < this.group.Permission)
                        {
                            Server.s.Log(name + " @" + p.name + ": " + message);
                            SendChat(this, Server.DefaultColor + "[<] " + p.color + p.prefix + p.name + ": &f" + message);
                            SendChat(p, "&9[>] " + this.color + this.prefix + this.name + ": &f" + message);
                            return;
                        }
                    }
                }
                Server.s.Log(name + " @" + p.name + ": " + message);
                SendChat(this, Server.DefaultColor + "[<] " + p.color + p.prefix + p.name + ": &f" + message);
                return;
            }
            foreach (string ignored2 in p.listignored)
            {
                if (ignored2 == this.name)
                {
                    Server.s.Log(name + " @" + p.name + ": " + message);
                    SendChat(this, Server.DefaultColor + "[<] " + p.color + p.prefix + p.name + ": &f" + message);
                    return;
                }
            }
            if (p != null && !p.hidden || p.hidden && this.group.Permission >= p.group.Permission)
            {
                Server.s.Log(name + " @" + p.name + ": " + message);
                SendChat(this, Server.DefaultColor + "[<] " + p.color + p.prefix + p.name + ": &f" + message);
                SendChat(p, "&9[>] " + this.color + this.prefix + this.name + ": &f" + message);
            }
            else { SendMessage("Player \"" + to + "\" doesn't exist!"); }
        }
        #endregion
        #region == OUTGOING ==
        public void SendRaw(int id)
        {
            SendRaw(id, new byte[0]);
        }
        public void SendRaw(int id, byte send)
        {
            SendRaw(id, new byte[] { send });
        }
        public void SendRaw(int id, byte[] send)
        {
            // Abort if socket has been closed
            if (socket == null || !socket.Connected)
                return;
            byte[] buffer = new byte[send.Length + 1];
            buffer[0] = (byte)id;
            for (int i = 0; i < send.Length; i++)
            {
                buffer[i + 1] = send[i];
            }
            //Buffer.BlockCopy(send, 0, buffer, 1, send.Length);

            /*int tries = 0;
        retry:*/
            try
            {

                socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, delegate(IAsyncResult result) { }, null);
                //socket.Send(buffer);
                buffer = null;
                /*      if (buffer[0] != 1)
                    {
                        Server.s.Log("Buffer ID: " + buffer[0]);
                        Server.s.Log("BUFFER LENGTH: " + buffer.Length);
                        Server.s.Log(TxStr);
                    }*/
            }
            /*
            catch (SocketException)
            {
                tries++;
                if (tries > 2)
                    Disconnect();
                else goto retry;
            }*/
            catch (SocketException)
            {
                buffer = null;
                Disconnect();
#if DEBUG
                Server.ErrorLog(e);
#endif
            }
        }

        public static void SendMessage(Player p, string message)
        {
            SendMessage(p, message, true);
        }
        public static void SendMessage(Player p, string message, bool colorParse)
        {
            if (p == null)
            {
                if (storeHelp)
                {
                    storedHelp += message + "\r\n";
                }
                else
                {
                    if (!Server.irc || String.IsNullOrEmpty(Server.IRC.usedCmd))
                        Server.s.Log(message);
                    else
                        Server.IRC.Pm(Server.IRC.usedCmd, message);
                    //IRCBot.Say(message, true);
                }
                return;
            }
            p.SendMessage(p.id, Server.DefaultColor + message, colorParse);
        }
        public void SendMessage(string message)
        {
            SendMessage(message, true);
        }
        public void SendMessage(string message, bool colorParse)
        {
            if (this == null) { Server.s.Log(message); return; }
            unchecked { SendMessage(this.id, Server.DefaultColor + message, colorParse); }
        }
        public void SendChat(Player p, string message)
        {
            if (this == null) { Server.s.Log(message); return; }
            Player.SendMessage(p, message);
        }
        public void SendMessage(byte id, string message)
        {
            SendMessage(id, message, true);
        }
        public void SendMessage(byte id, string message, bool colorParse)
        {
            if (this == null) { Server.s.Log(message); return; }
            if (ZoneSpam.AddSeconds(2) > DateTime.Now && message.Contains("This zone belongs to ")) return;

            byte[] buffer = new byte[65];
            unchecked { buffer[0] = id; }

            StringBuilder sb = new StringBuilder(message);

            if (colorParse)
            {
                for (int i = 0; i < 10; i++)
                {
                    sb.Replace("%" + i, "&" + i);
                    sb.Replace("&" + i + " &", " &");
                }
                for (char ch = 'a'; ch <= 'f'; ch++)
                {
                    sb.Replace("%" + ch, "&" + ch);
                    sb.Replace("&" + ch + " &", " &");
                }
// Begin fix to replace all invalid color codes typed in console or chat with "." 
                for (char ch = (char)0; ch <= (char)47; ch++) // Characters that cause clients to disconnect
                {
                    sb.Replace("%" + ch, String.Empty);
                    sb.Replace("&" + ch, String.Empty);
                }
                for (char ch = (char)58; ch <= (char)96; ch++) // Characters that cause clients to disconnect
                {
                    sb.Replace("%" + ch, String.Empty);
                    sb.Replace("&" + ch, String.Empty);
                }
                for (char ch = (char)103; ch <= (char)127; ch++) // Characters that cause clients to disconnect
                {
                    sb.Replace("%" + ch, String.Empty);
                    sb.Replace("&" + ch, String.Empty);
		}
// End fix
            }

            if (Server.dollardollardollar)
                sb.Replace("$name", "$" + name);
            else
                sb.Replace("$name", name);
            sb.Replace("$date", DateTime.Now.ToString("yyyy-MM-dd"));
            sb.Replace("$time", DateTime.Now.ToString("HH:mm:ss"));
            sb.Replace("$ip", ip);
            if (colorParse) sb.Replace("$color", color);
            sb.Replace("$rank", group.name);
            sb.Replace("$level", level.name);
            sb.Replace("$deaths", overallDeath.ToString());
            sb.Replace("$money", money.ToString());
            sb.Replace("$blocks", overallBlocks.ToString());
            sb.Replace("$first", firstLogin.ToString());
            sb.Replace("$kicked", totalKicked.ToString());
            sb.Replace("$server", Server.name);
            sb.Replace("$motd", Server.motd);
            sb.Replace("$banned", Player.GetBannedCount().ToString());
            sb.Replace("$irc", Server.ircServer + " > " + Server.ircChannel);

            foreach (var customReplacement in Server.customdollars)
            {
                if (!customReplacement.Key.StartsWith("//"))
                {
                    try
                    {
                        sb.Replace(customReplacement.Key, customReplacement.Value);
                    }
                    catch {}
                }
            }

            if (Server.parseSmiley && parseSmiley)
            {
                sb.Replace(":)", "(darksmile)");
                sb.Replace(":D", "(smile)");
                sb.Replace("<3", "(heart)");
            }

            byte[] stored = new byte[1];

            stored[0] = (byte)1;
            sb.Replace("(darksmile)", enc.GetString(stored));
            stored[0] = (byte)2;
            sb.Replace("(smile)", enc.GetString(stored));
            stored[0] = (byte)3;
            sb.Replace("(heart)", enc.GetString(stored));
            stored[0] = (byte)4;
            sb.Replace("(diamond)", enc.GetString(stored));
            stored[0] = (byte)7;
            sb.Replace("(bullet)", enc.GetString(stored));
            stored[0] = (byte)8;
            sb.Replace("(hole)", enc.GetString(stored));
            stored[0] = (byte)11;
            sb.Replace("(male)", enc.GetString(stored));
            stored[0] = (byte)12;
            sb.Replace("(female)", enc.GetString(stored));
            stored[0] = (byte)15;
            sb.Replace("(sun)", enc.GetString(stored));
            stored[0] = (byte)16;
            sb.Replace("(right)", enc.GetString(stored));
            stored[0] = (byte)17;
            sb.Replace("(left)", enc.GetString(stored));
            stored[0] = (byte)19;
            sb.Replace("(double)", enc.GetString(stored));
            stored[0] = (byte)22;
            sb.Replace("(half)", enc.GetString(stored));
            stored[0] = (byte)24;
            sb.Replace("(uparrow)", enc.GetString(stored));
            stored[0] = (byte)25;
            sb.Replace("(downarrow)", enc.GetString(stored));
            stored[0] = (byte)26;
            sb.Replace("(rightarrow)", enc.GetString(stored));
            stored[0] = (byte)30;
            sb.Replace("(up)", enc.GetString(stored));
            stored[0] = (byte)31;
            sb.Replace("(down)", enc.GetString(stored));

            message = sb.ToString();
            int totalTries = 0;
        retryTag: try
            {
                foreach (string line in Wordwrap(message))
                {
                    string newLine = line;
                    if (newLine.TrimEnd(' ')[newLine.TrimEnd(' ').Length - 1] < '!')
                    {
                        newLine += '\'';
                    }

                    StringFormat(newLine, 64).CopyTo(buffer, 1);
                    SendRaw(13, buffer);
                }
            }
            catch (Exception e)
            {
                message = "&f" + message;
                totalTries++;
                if (totalTries < 10) goto retryTag;
                else Server.ErrorLog(e);
            }
        }

        public void SendMotd()
        {
            byte[] buffer = new byte[130];
            buffer[0] = (byte)8;
            StringFormat(Server.name, 64).CopyTo(buffer, 1);
            StringFormat(Server.motd, 64).CopyTo(buffer, 65);

            if (Block.canPlace(this, Block.blackrock))
                buffer[129] = 100;
            else
                buffer[129] = 0;
            if (OnSendMOTD != null)
            {
                OnSendMOTD(this, buffer);
            }
            SendRaw(0, buffer);

        }

        public void SendUserMOTD()
        {
            byte[] buffer = new byte[130];
            Random rand = new Random();
            buffer[0] = Server.version;
            if (UsingWom && (level.textures.enabled || level.motd == "texture")) { StringFormat("&0cfg=" + Server.IP + ":" + Server.port + "/" + level.name, 64).CopyTo(buffer, 65); }
            else if (level.motd == "ignore") { StringFormat(Server.name, 64).CopyTo(buffer, 1); StringFormat(Server.motd, 64).CopyTo(buffer, 65); }
            else StringFormat(level.motd, 128).CopyTo(buffer, 1);

            if (Block.canPlace(this.group.Permission, Block.blackrock))
                buffer[129] = 100;
            else
                buffer[129] = 0;
            SendRaw(0, buffer);
        }

        public void SendMap()
        {
            if (level.blocks == null) return;
            try
            {
                byte[] buffer = new byte[level.blocks.Length + 4];
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder(level.blocks.Length)).CopyTo(buffer, 0);
                //ushort xx; ushort yy; ushort zz;

                for (int i = 0; i < level.blocks.Length; ++i)
                    buffer[4 + i] = Block.Convert(level.blocks[i]);
                SendRaw(2);
                buffer = buffer.GZip();
                int number = (int)Math.Ceiling(((double)buffer.Length) / 1024);
                for (int i = 1; buffer.Length > 0; ++i)
                {
                    short length = (short)Math.Min(buffer.Length, 1024);
                    byte[] send = new byte[1027];
                    HTNO(length).CopyTo(send, 0);
                    Buffer.BlockCopy(buffer, 0, send, 2, length);
                    byte[] tempbuffer = new byte[buffer.Length - length];
                    Buffer.BlockCopy(buffer, length, tempbuffer, 0, buffer.Length - length);
                    buffer = tempbuffer;
                    send[1026] = (byte)(i * 100 / number);
                    //send[1026] = (byte)(100 - (i * 100 / number)); // Backwards progress lololol...
                    SendRaw(3, send);
                    if (ip == "127.0.0.1") { }
                    else if (Server.updateTimer.Interval > 1000) Thread.Sleep(100);
                    else Thread.Sleep(10);
                } buffer = new byte[6];
                HTNO((short)level.width).CopyTo(buffer, 0);
                HTNO((short)level.depth).CopyTo(buffer, 2);
                HTNO((short)level.height).CopyTo(buffer, 4);
                SendRaw(4, buffer);
                Loading = false;

                if (OnSendMap != null)
                    OnSendMap(this, buffer);
            }
            catch (Exception ex)
            {
                Command.all.Find("goto").Use(this, Server.mainLevel.name);
                SendMessage("There was an error sending the map data, you have been sent to the main level.");
                Server.ErrorLog(ex);
            }
            finally
            {
                //if (derp) SendMessage("Something went derp when sending the map data, you should return to the main level.");
                DateTime start = DateTime.Now;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Server.s.Log((DateTime.Now - start).TotalMilliseconds.ToString());
            }
        }
        public void SendSpawn(byte id, string name, ushort x, ushort y, ushort z, byte rotx, byte roty)
        {
            //pos = new ushort[3] { x, y, z }; // This could be remove and not effect the server :/
            //rot = new byte[2] { rotx, roty };
            byte[] buffer = new byte[73]; buffer[0] = id;
            StringFormat(name, 64).CopyTo(buffer, 1);
            HTNO(x).CopyTo(buffer, 65);
            HTNO(y).CopyTo(buffer, 67);
            HTNO(z).CopyTo(buffer, 69);
            buffer[71] = rotx; buffer[72] = roty;
            SendRaw(7, buffer);
        }
        public void SendPos(byte id, ushort x, ushort y, ushort z, byte rotx, byte roty)
        {
            if (x < 0) x = 32;
            if (y < 0) y = 32;
            if (z < 0) z = 32;
            if (x > level.width * 32) x = (ushort)(level.width * 32 - 32);
            if (z > level.height * 32) z = (ushort)(level.height * 32 - 32);
            if (x > 32767) x = 32730;
            if (y > 32767) y = 32730;
            if (z > 32767) z = 32730;

            pos[0] = x; pos[1] = y; pos[2] = z;
            rot[0] = rotx; rot[1] = roty;

            /*
            pos = new ushort[3] { x, y, z };
            rot = new byte[2] { rotx, roty };*/
            byte[] buffer = new byte[9]; buffer[0] = id;
            HTNO(x).CopyTo(buffer, 1);
            HTNO(y).CopyTo(buffer, 3);
            HTNO(z).CopyTo(buffer, 5);
            buffer[7] = rotx; buffer[8] = roty;
            SendRaw(8, buffer);
        }
        // Update user type for weather or not they are opped
        public void SendUserType(bool op)
        {
            SendRaw(15, op ? (byte)100 : (byte)0);
        }
        //TODO: Figure a way to SendPos without changing rotation
        public void SendDie(byte id) { SendRaw(0x0C, new byte[1] { id }); }
        public void SendBlockchange(ushort x, ushort y, ushort z, byte type)
        {
            if (x < 0 || y < 0 || z < 0) return;
            if (x >= level.width || y >= level.depth || z >= level.height) return;

            byte[] buffer = new byte[7];
            HTNO(x).CopyTo(buffer, 0);
            HTNO(y).CopyTo(buffer, 2);
            HTNO(z).CopyTo(buffer, 4);
            buffer[6] = Block.Convert(type);
            SendRaw(6, buffer);
        }
        void SendKick(string message) { SendRaw(14, StringFormat(message, 64)); }
        void SendPing() { /*pingDelay = 0; pingDelayTimer.Start();*/ SendRaw(1); }
        void UpdatePosition()
        {

            //pingDelayTimer.Stop();

            // Shameless copy from JTE's Server
            byte changed = 0;   //Denotes what has changed (x,y,z, rotation-x, rotation-y)
            // 0 = no change - never happens with this code.
            // 1 = position has changed
            // 2 = rotation has changed
            // 3 = position and rotation have changed
            // 4 = Teleport Required (maybe something to do with spawning)
            // 5 = Teleport Required + position has changed
            // 6 = Teleport Required + rotation has changed
            // 7 = Teleport Required + position and rotation has changed
            //NOTE: Players should NOT be teleporting this often. This is probably causing some problems.
            if (oldpos[0] != pos[0] || oldpos[1] != pos[1] || oldpos[2] != pos[2])
                changed |= 1;

            if (oldrot[0] != rot[0] || oldrot[1] != rot[1])
            {
                changed |= 2;
            }
            /*if (Math.Abs(pos[0] - basepos[0]) > 32 || Math.Abs(pos[1] - basepos[1]) > 32 || Math.Abs(pos[2] - basepos[2]) > 32)
                changed |= 4;

            if ((oldpos[0] == pos[0] && oldpos[1] == pos[1] && oldpos[2] == pos[2]) && (basepos[0] != pos[0] || basepos[1] != pos[1] || basepos[2] != pos[2]))
                changed |= 4;*/
            if (Math.Abs(pos[0] - oldpos[0]) > 32 || Math.Abs(pos[1] - oldpos[1]) > 32 || Math.Abs(pos[2] - oldpos[2]) > 32)
                changed |= 4;
            if (changed == 0) { if (oldpos[0] != pos[0] || oldpos[1] != pos[1] || oldpos[2] != pos[2]) changed |= 1; }

            byte[] buffer = new byte[0]; byte msg = 0;
            if ((changed & 4) != 0)
            {
                msg = 8; //Player teleport - used for spawning or moving too fast
                buffer = new byte[9]; buffer[0] = id;
                HTNO(pos[0]).CopyTo(buffer, 1);
                HTNO(pos[1]).CopyTo(buffer, 3);
                HTNO(pos[2]).CopyTo(buffer, 5);
                buffer[7] = rot[0];

                if (Server.flipHead || (this.flipHead && this.infected))
                    if (rot[1] > 64 && rot[1] < 192)
                        buffer[8] = rot[1];
                    else
                        buffer[8] = (byte)(rot[1] - (rot[1] - 128));
                else
                    buffer[8] = rot[1];

                //Realcode
                //buffer[8] = rot[1];
            }
            else if (changed == 1)
            {
                try
                {
                    msg = 10; //Position update
                    buffer = new byte[4]; buffer[0] = id;
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[0] - oldpos[0])), 0, buffer, 1, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[1] - oldpos[1])), 0, buffer, 2, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[2] - oldpos[2])), 0, buffer, 3, 1);
                }
                catch { }
            }
            else if (changed == 2)
            {
                msg = 11; //Orientation update
                buffer = new byte[3]; buffer[0] = id;
                buffer[1] = rot[0];

                if (Server.flipHead || (this.flipHead && this.infected))
                    if (rot[1] > 64 && rot[1] < 192)
                        buffer[2] = rot[1];
                    else
                        buffer[2] = (byte)(rot[1] - (rot[1] - 128));
                else
                    buffer[2] = rot[1];

                //Realcode
                //buffer[2] = rot[1];
            }
            else if (changed == 3)
            {
                try
                {
                    msg = 9; //Position and orientation update
                    buffer = new byte[6]; buffer[0] = id;
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[0] - oldpos[0])), 0, buffer, 1, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[1] - oldpos[1])), 0, buffer, 2, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[2] - oldpos[2])), 0, buffer, 3, 1);
                    buffer[4] = rot[0];

                    if (Server.flipHead || (this.flipHead && this.infected))
                        if (rot[1] > 64 && rot[1] < 192)
                            buffer[5] = rot[1];
                        else
                            buffer[5] = (byte)(rot[1] - (rot[1] - 128));
                    else
                        buffer[5] = rot[1];

                    //Realcode
                    //buffer[5] = rot[1];
                }
                catch { }
            }

            oldpos = pos; oldrot = rot;
            if (changed != 0)
                try
                {
                    foreach (Player p in players)
                    {
                        if (p != this && p.level == level)
                        {
                            p.SendRaw(msg, buffer);
                        }
                    }
                }
                catch { }
        }
        #endregion
        #region == GLOBAL MESSAGES ==
        public static void GlobalBlockchange(Level level, ushort x, ushort y, ushort z, byte type)
        {
            players.ForEach(delegate(Player p) { if (p.level == level) { p.SendBlockchange(x, y, z, type); } });
        }

        // THIS IS NOT FOR SENDING GLOBAL MESSAGES!!! IT IS TO SEND A MESSAGE FROM A SPECIFIED PLAYER!!!!!!!!!!!!!!
        public static void GlobalChat(Player from, string message) { GlobalChat(from, message, true); }
        public static void GlobalChat(Player from, string message, bool showname)
        {
            if (from == null) return; // So we don't fucking derp the hell out!

            if (MessageHasBadColorCodes(from, message))
                return;

            if (Server.lava.HasPlayer(from) && Server.lava.HasVote(message.ToLower()))
            {
                if (Server.lava.AddVote(from, message.ToLower()))
                {
                    SendMessage(from, "Your vote for &5" + message.ToLower().Capitalize() + Server.DefaultColor + " has been placed. Thanks!");
                    Server.lava.map.ChatLevelOps(from.name + " voted for &5" + message.ToLower().Capitalize() + Server.DefaultColor + ".");
                    return;
                }
                else
                {
                    SendMessage(from, "&cYou already voted!");
                    return;
                }
            }

            if (Server.voting == true)
            {
                if (message.ToLower() == "yes" || message.ToLower() == "ye" || message.ToLower() == "y")
                {
                    if (!from.voted)
                    {
                        Server.YesVotes++;
                        SendMessage(from, c.red + "Thanks For Voting!");
                        from.voted = true;
                        return;
                    }
                    else if (!from.voice)
                    {
                        from.SendMessage("Chat moderation is on while voting is on!");
                        return;
                    }
                }
                else if (message.ToLower() == "no" || message.ToLower() == "n")
                {
                    if (!from.voted)
                    {
                        Server.NoVotes++;
                        SendMessage(from, c.red + "Thanks For Voting!");
                        from.voted = true;
                        return;
                    }
                    else if (!from.voice)
                    {
                        from.SendMessage("Chat moderation is on while voting is on!");
                        return;
                    }
                }
            }

            if (Server.votingforlevel == true)
            {
                if (message.ToLower() == "1" || message.ToLower() == "one")
                {
                    if (!from.voted)
                    {
                        Server.Level1Vote++;
                        SendMessage(from, c.red + "Thanks For Voting!");
                        from.voted = true;
                        return;
                    }
                    else if (!from.voice)
                    {
                        from.SendMessage("Chat moderation is on while voting is on!");
                        return;
                    }
                }
                else if (message.ToLower() == "2" || message.ToLower() == "two")
                {
                    if (!from.voted)
                    {
                        Server.Level2Vote++;
                        SendMessage(from, c.red + "Thanks For Voting!");
                        from.voted = true;
                        return;
                    }
                    else if (!from.voice)
                    {
                        from.SendMessage("Chat moderation is on while voting is on!");
                        return;
                    }
                }
                else if (message.ToLower() == "3" || message.ToLower() == "random" || message.ToLower() == "rand")
                {
                    if (!from.voted)
                    {
                        Server.Level3Vote++;
                        SendMessage(from, c.red + "Thanks For Voting!");
                        from.voted = true;
                        return;
                    }
                    else if (!from.voice)
                    {
                        from.SendMessage("Chat moderation is on while voting is on!");
                        return;
                    }
                }
                else if (!from.voice)
                {
                    from.SendMessage("Chat moderation is on while voting is on!");
                    return;
                }
            }

            if (showname)
            {
                String referee = "";
                if (from.referee)
                {
                    referee = c.green + "[Referee] ";
                }
                message = referee + from.color + from.voicestring + from.color + from.prefix + from.name + ": &f" + message;
            }
            players.ForEach(delegate(Player p)
            {
                if (p.level.worldChat && p.Chatroom == null)
                {
                    if (p.ignoreglobal == false)
                    {
                        if (from != null)
                        {
                            if (!p.listignored.Contains(from.name))
                            {
                                Player.SendMessage(p, message);
                                return;
                            }
                            return;
                        }
                        Player.SendMessage(p, message);
                        return;
                    }
                    if (Server.globalignoreops == false)
                    {
                        if (from.group.Permission >= Server.opchatperm)
                        {
                            if (p.group.Permission < from.group.Permission)
                            {
                                Player.SendMessage(p, message);
                            }
                        }
                    }
                    if (from != null)
                    {
                        if (from == p)
                        {
                            Player.SendMessage(from, message);
                            return;
                        }
                    }
                }
            });

        }
        public static void GlobalChatLevel(Player from, string message, bool showname)
        {
            if (MessageHasBadColorCodes(from, message))
                return;

            if (showname)
            {
                message = "<Level>" + from.color + from.voicestring + from.color + from.prefix + from.name + ": &f" + message;
            }
            players.ForEach(delegate(Player p)
            {
                if (p.level == from.level && p.Chatroom == null)
                {
                    if (p.ignoreglobal == false)
                    {
                        if (from != null)
                        {
                            if (!p.listignored.Contains(from.name))
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                                return;
                            }
                            return;
                        }
                        Player.SendMessage(p, Server.DefaultColor + message);
                        return;
                    }
                    if (Server.globalignoreops == false)
                    {
                        if (from.group.Permission >= Server.opchatperm)
                        {
                            if (p.group.Permission < from.group.Permission)
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                            }
                        }
                    }
                    if (from != null)
                    {
                        if (from == p)
                        {
                            Player.SendMessage(from, Server.DefaultColor + message);
                            return;
                        }
                    }
                }
            });
        }
        public static void GlobalChatRoom(Player from, string message, bool showname)
        {
            if (MessageHasBadColorCodes(from, message))
                return;
            string oldmessage = message;
            if (showname)
            {
                message = "<GlobalChatRoom> " + from.color + from.voicestring + from.color + from.prefix + from.name + ": &f" + message;
            }
            players.ForEach(delegate(Player p)
            {
                if (p.Chatroom != null)
                {
                    if (p.ignoreglobal == false)
                    {
                        if (from != null)
                        {
                            if (!p.listignored.Contains(from.name))
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                                return;
                            }
                            return;
                        }
                        Player.SendMessage(p, Server.DefaultColor + message);
                        return;
                    }
                    if (Server.globalignoreops == false)
                    {
                        if (from.group.Permission >= Server.opchatperm)
                        {
                            if (p.group.Permission < from.group.Permission)
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                            }
                        }
                    }
                    if (from != null)
                    {
                        if (from == p)
                        {
                            Player.SendMessage(from, Server.DefaultColor + message);
                            return;
                        }
                    }
                }
            });
            Server.s.Log(oldmessage + "<GlobalChatRoom>" + from.prefix + from.name + message);
        }
        public static void ChatRoom(Player from, string message, bool showname, string chatroom)
        {
            if (MessageHasBadColorCodes(from, message))
                return;
            string oldmessage = message;
            string messageforspy = ("<ChatRoomSPY: " + chatroom + "> " + from.color + from.voicestring + from.color + from.prefix + from.name + ": &f" + message);
            if (showname)
            {
                message = "<ChatRoom: " + chatroom + "> " + from.color + from.voicestring + from.color + from.prefix + from.name + ": &f" + message;
            }
            players.ForEach(delegate(Player p)
            {
                if (p.Chatroom == chatroom)
                {
                    if (p.ignoreglobal == false)
                    {
                        if (from != null)
                        {
                            if (!p.listignored.Contains(from.name))
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                                return;
                            }
                            return;
                        }
                        Player.SendMessage(p, Server.DefaultColor + message);
                        return;
                    }
                    if (Server.globalignoreops == false)
                    {
                        if (from.group.Permission >= Server.opchatperm)
                        {
                            if (p.group.Permission < from.group.Permission)
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                            }
                        }
                    }
                    if (from != null)
                    {
                        if (from == p)
                        {
                            Player.SendMessage(from, Server.DefaultColor + message);
                            return;
                        }
                    }
                }
            });
            players.ForEach(delegate(Player p)
            {
                if (p.spyChatRooms.Contains(chatroom) && p.Chatroom != chatroom)
                {
                    if (p.ignoreglobal == false)
                    {
                        if (from != null)
                        {
                            if (!p.listignored.Contains(from.name))
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                                return;
                            }
                            return;
                        }
                        Player.SendMessage(p, Server.DefaultColor + message);
                        return;
                    }
                    if (Server.globalignoreops == false)
                    {
                        if (from.group.Permission >= Server.opchatperm)
                        {
                            if (p.group.Permission < from.group.Permission)
                            {
                                Player.SendMessage(p, Server.DefaultColor + messageforspy);
                            }
                        }
                    }
                    if (from != null)
                    {
                        if (from == p)
                        {
                            Player.SendMessage(from, Server.DefaultColor + messageforspy);
                            return;
                        }
                    }
                }
            });
            Server.s.Log(oldmessage + "<ChatRoom" + chatroom + ">" + from.prefix + from.name + message);
        }


        public static bool MessageHasBadColorCodes(Player from, string message)
        {
            string[] checkmessagesplit = message.Split(' ');
            bool lastendwithcolour = false;
            foreach (string s in checkmessagesplit)
            {
                s.Trim();
                if (s.StartsWith("%"))
                {
                    if (lastendwithcolour == true)
                    {
                        if (from != null)
                        {
                            from.SendMessage("Sorry, Your colour codes were invalid (You cannot use 2 colour codes next to each other");
                            from.SendMessage("Message not sent");
                            Server.s.Log(from.name + " tried to send an invalid colour code message (2 colour codes were next to each other), the offending message was not sent.");
                            GlobalMessageOps(from.color + from.name + " " + Server.DefaultColor + " tried to sent an invalid colour code message (2 colour codes were next to each other), the offending message was not sent.");
                        }
                        return true;
                    }
                    else if (s.Length == 2)
                    {
                        lastendwithcolour = true;
                    }
                }
                if (s.TrimEnd(Server.ColourCodesNoPercent).EndsWith("%"))
                {
                    lastendwithcolour = true;
                }
                else
                {
                    lastendwithcolour = false;
                }

            }
            return false;

        }
        public static bool CommandHasBadColourCodes(Player who, string message)
        {
            string[] checkmessagesplit = message.Split(' ');
            bool lastendwithcolour = false;
            foreach (string s in checkmessagesplit)
            {
                s.Trim();
                if (s.StartsWith("%"))
                {
                    if (lastendwithcolour)
                    {
                        if (who != null)
                        {
                            who.SendMessage("Sorry, Your colour codes in this command were invalid (You cannot use 2 colour codes next to each other");
                            who.SendMessage("Command failed.");
                            Server.s.Log(who.name + " attempted to send a command with invalid colours codes (2 colour codes were next to each other)!");
                            GlobalMessageOps(who.color + who.name + " " + Server.DefaultColor + " attempted to send a command with invalid colours codes (2 colour codes were next to each other)!");
                        }
                        return true;
                    }
                    else if (s.Length == 2)
                    {
                        lastendwithcolour = true;
                    }
                }
                if (s.TrimEnd(Server.ColourCodesNoPercent).EndsWith("%"))
                {
                    lastendwithcolour = true;
                }
                else
                {
                    lastendwithcolour = false;
                }

            }
            return false;
        }
        public static void GlobalChatWorld(Player from, string message, bool showname)
        {
            if (showname)
            {
                message = "<World>" + from.color + from.voicestring + from.color + from.prefix + from.name + ": &f" + message;
            }
            players.ForEach(delegate(Player p)
            {
                if (p.level.worldChat && p.Chatroom == null)
                {
                    if (p.ignoreglobal == false)
                    {
                        if (from != null)
                        {
                            if (!p.listignored.Contains(from.name))
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                                return;
                            }
                            return;
                        }
                        Player.SendMessage(p, Server.DefaultColor + message);
                        return;
                    }
                    if (Server.globalignoreops == false)
                    {
                        if (from.group.Permission >= Server.opchatperm)
                        {
                            if (p.group.Permission < from.group.Permission)
                            {
                                Player.SendMessage(p, Server.DefaultColor + message);
                            }
                        }
                    }
                    if (from != null)
                    {
                        if (from == p)
                        {
                            Player.SendMessage(from, Server.DefaultColor + message);
                            return;
                        }
                    }
                }
            });
        }
        public static void GlobalMessage(string message)
        {
            GlobalMessage(message, false);
        }
        public static void GlobalMessage(string message, bool global)
        {
            if (!global)
                message = message.Replace("%", "&");
            players.ForEach(delegate(Player p)
            {
                if (p.level.worldChat && p.Chatroom == null && (!global || !p.muteGlobal))
                {
                    Player.SendMessage(p, message, !global);
                }
            });
        }
        public static void GlobalMessageLevel(Level l, string message)
        {
            players.ForEach(delegate(Player p) { if (p.level == l && p.Chatroom == null) Player.SendMessage(p, message); });
        }
        public static void GlobalMessageOps(string message)
        {
            try
            {
                players.ForEach(delegate(Player p)
                {
                    if (p.group.Permission >= Server.opchatperm || Server.devs.Contains(p.name.ToLower()))
                    {
                        Player.SendMessage(p, message);
                    }
                });

            }
            catch { Server.s.Log("Error occured with Op Chat"); }
        }
        public static void GlobalMessageAdmins(string message)
        {
            try
            {
                players.ForEach(delegate(Player p)
                {
                    if (p.group.Permission >= Server.adminchatperm || Server.devs.Contains(p.name.ToLower()))
                    {
                        Player.SendMessage(p, message);
                    }
                });

            }
            catch { Server.s.Log("Error occured with Admin Chat"); }
        }
        public static void GlobalSpawn(Player from, ushort x, ushort y, ushort z, byte rotx, byte roty, bool self, string possession = "")
        {
            players.ForEach(delegate(Player p)
            {
                if (p.Loading && p != from) { return; }
                if (p.level != from.level || (from.hidden && !self)) { return; }
                if (p != from)
                {
                    if (Server.ZombieModeOn && !p.aka)
                    {
                        if (from.infected)
                        {
                            if (Server.ZombieName != "")
                                p.SendSpawn(from.id, c.red + Server.ZombieName + possession, x, y, z, rotx, roty);
                            else
                                p.SendSpawn(from.id, c.red + from.name + possession, x, y, z, rotx, roty);
                            return;
                        }
                        else if (from.referee)
                        {
                            return;
                    	}
                        else
                        {
                            p.SendSpawn(from.id, from.color + from.name + possession, x, y, z, rotx, roty);
                            return;
                        }
                    }
                    else
                    {
                    p.SendSpawn(from.id, from.color + from.name + possession, x, y, z, rotx, roty);
                    }
                }
                else if (self)
                {
                    if (!p.ignorePermission)
                    {
                        p.pos = new ushort[3] { x, y, z }; p.rot = new byte[2] { rotx, roty };
                        p.oldpos = p.pos; p.basepos = p.pos; p.oldrot = p.rot;
                        unchecked { p.SendSpawn((byte)-1, from.color + from.name + possession, x, y, z, rotx, roty); }
                    }
                }
            });
        }
        public static void GlobalDie(Player from, bool self)
        {
            players.ForEach(delegate(Player p)
            {
                if (p.level != from.level || (from.hidden && !self)) { return; }
                if (p != from) { p.SendDie(from.id); }
                else if (self) { unchecked { p.SendDie((byte)-1); } }
            });
        }

        public bool MarkPossessed(string marker = "")
        {
            if (marker != "")
            {
                Player controller = Player.Find(marker);
                if (controller == null)
                {
                    return false;
                }
                marker = " (" + controller.color + controller.name + color + ")";
            }
            GlobalDie(this, true);
            GlobalSpawn(this, pos[0], pos[1], pos[2], rot[0], rot[1], true, marker);
            return true;
        }

        public static void GlobalUpdate() { players.ForEach(delegate(Player p) { if (!p.hidden) { p.UpdatePosition(); } }); }
        #endregion
        #region == DISCONNECTING ==
        public void Disconnect() { leftGame(); }
        public void Kick(string kickString) { leftGame(kickString); }

        private void CloseSocket()
        {
            // Try to close the socket.
            // Sometimes its already closed so these lines will cause an error
            // We just trap them and hide them from view :P
            try
            {
                // Close the damn socket connection!
                socket.Shutdown(SocketShutdown.Both);
#if DEBUG
                Server.s.Log("Socket was shutdown for " + this.name ?? this.ip);
#endif
            }
            catch (Exception)
            {
#if DEBUG
                    Exception ex = new Exception("Failed to shutdown socket for " + this.name ?? this.ip, e);
                    Server.ErrorLog(ex);
#endif
            }

            try
            {
                socket.Close();
#if DEBUG
                Server.s.Log("Socket was closed for " + this.name ?? this.ip);
#endif
            }
            catch (Exception)
            {
#if DEBUG
                    Exception ex = new Exception("Failed to close socket for " + this.name ?? this.ip, e);
                    Server.ErrorLog(ex);
#endif
            }
        }

        public void leftGame(string kickString = "", bool skip = false)
        {
            //Umm...fixed?
            if (name == "")
            {
                if (socket != null)
                    CloseSocket();
                if (connections.Contains(this))
                    connections.Remove(this);
                SaveUndo();
                disconnected = true;
                return;
            }

            try
            {
                SaveUndo();
                if (disconnected)
                {
                    this.CloseSocket();
                    if (connections.Contains(this))
                        connections.Remove(this);
                    return;
                }
                //   FlyBuffer.Clear();
                disconnected = true;
                pingTimer.Stop();
                pingTimer.Dispose();
                if (File.Exists("ranks/ignore/" + this.name + ".txt"))
                {
                    try
                    {
                        File.WriteAllLines("ranks/ignore/" + this.name + ".txt", this.listignored.ToArray());
                    }
                    catch
                    {
                        Server.s.Log("Failed to save ignored list for player: " + this.name);
                    }
                }
                if (File.Exists("ranks/ignore/GlobalIgnore.xml"))
                {
                    try
                    {
                        File.WriteAllLines("ranks/ignore/GlobalIgnore.xml", globalignores.ToArray());
                    }
                    catch
                    {
                        Server.s.Log("failed to save global ignore list!");
                    }
                }
                afkTimer.Stop();
                afkTimer.Dispose();
                muteTimer.Stop();
                muteTimer.Dispose();
                timespent.Stop();
                timespent.Dispose();
                afkCount = 0;
                afkStart = DateTime.Now;

                if (Server.afkset.Contains(name)) Server.afkset.Remove(name);

                if (kickString == "") kickString = "Disconnected.";

                SendKick(kickString);
                

                if (loggedIn)
                {
                    isFlying = false;
                    aiming = false;

                    if (team != null)
                    {
                        team.RemoveMember(this);
                    }

                    if (CountdownGame.players.Contains(this))
                    {
                        if (CountdownGame.playersleftlist.Contains(this))
                        {
                            CountdownGame.PlayerLeft(this);
                        }
                        CountdownGame.players.Remove(this);
                    }

                    GlobalDie(this, false);
                    if (kickString == "Disconnected." || kickString.IndexOf("Server shutdown") != -1 || kickString == Server.customShutdownMessage)
                    {
                        if (!Directory.Exists("text/logout"))
                        {
                            Directory.CreateDirectory("text/logout");
                        }
                        if (!File.Exists("text/logout/" + name + ".txt"))
                        {
                            File.WriteAllText("text/logout/" + name + ".txt", "Disconnected.");
                        }
						if (!hidden) 
						{
							if (Server.guestLeaveNotify == true && this.group.Permission <= LevelPermission.Guest) 
							{
                                GlobalChat(this, "&c- " + color + prefix + name + Server.DefaultColor + " " + File.ReadAllText("text/logout/" + name + ".txt"), false); 
							}
                            if (this.group.Permission > LevelPermission.Guest)
                            {
                                GlobalChat(this, "&c- " + color + prefix + name + Server.DefaultColor + " " + File.ReadAllText("text/logout/" + name + ".txt"), false);
                            }
                        }
                        //IRCBot.Say(name + " left the game.");
                        Server.s.Log(name + " disconnected.");
                        if (Server.notifyOnJoinLeave)
                        {
                            Server.PopupNotify(name + " [" + ip + "] disconnected.");
                        }
                    }
                    else
                    {
                        totalKicked++;
                        GlobalChat(this, "&c- " + color + prefix + name + Server.DefaultColor + " kicked (" + kickString + Server.DefaultColor + ").", false);
                        //IRCBot.Say(name + " kicked (" + kickString + ").");
                        Server.s.Log(name + " kicked (" + kickString + ").");
                        if (Server.notifyOnJoinLeave)
                            Server.PopupNotify(name + " kicked (" + kickString + ").");
                    }

                    try { save(); }
                    catch (Exception e) { Server.ErrorLog(e); }

                    players.Remove(this);
                    Server.s.PlayerListUpdate();
                    try
                    {
                        left.Add(this.name.ToLower(), this.ip);
                    }
                    catch (Exception)
                    {
                        //Server.ErrorLog(e);
                    }
                    try { Gui.Window.thisWindow.UpdatePlyersListBox(); }
                    catch { }

                    /*if (Server.AutoLoad && level.unload)
                    {

                        foreach (Player pl in Player.players)
                            if (pl.level == level) hasplayers = true;
                        
                        if (!level.name.Contains("Museum " + Server.DefaultColor) && hasplayers == false)
                        {
                            level.Unload();
                        }
                    }*/

                    if (Server.AutoLoad && level.unload && !level.name.Contains("Museum " + Server.DefaultColor) && IsAloneOnCurrentLevel())
                        level.Unload(true);

                    if (PlayerDisconnect != null)
                        PlayerDisconnect(this, kickString);

                    this.Dispose();
                }
                else
                {
                    connections.Remove(this);
                    
                    Server.s.Log(ip + " disconnected.");
                    if (Server.notifyOnJoinLeave)
                        Server.PopupNotify(ip + " disconnected.");
                }

                Server.zombie.InfectedPlayerDC();

            }
            catch (Exception e) { Server.ErrorLog(e); }
            finally
            {

                this.CloseSocket();
            }
        }

        public void SaveUndo()
        {
            SaveUndo(this);
        }
        public static void SaveUndo(Player p)
        {
            if (p == null || p.UndoBuffer == null || p.UndoBuffer.Count < 1) return;
            try
            {
                lock (p.UndoBuffer)
                {
                    if (!Directory.Exists("extra/undo")) Directory.CreateDirectory("extra/undo");
                    if (!Directory.Exists("extra/undoPrevious")) Directory.CreateDirectory("extra/undoPrevious");
                    DirectoryInfo di = new DirectoryInfo("extra/undo");
                    if (di.GetDirectories("*").Length >= Server.totalUndo)
                    {
                        Directory.Delete("extra/undoPrevious", true);
                        Directory.Move("extra/undo", "extra/undoPrevious");
                        Directory.CreateDirectory("extra/undo");
                    }

                    if (!Directory.Exists("extra/undo/" + p.name.ToLower())) Directory.CreateDirectory("extra/undo/" + p.name.ToLower());
                    di = new DirectoryInfo("extra/undo/" + p.name.ToLower());
                    File.Create("extra/undo/" + p.name.ToLower() + "/" + di.GetFiles("*.undo").Length + ".undo").Dispose();
                    using (StreamWriter w = File.CreateText("extra/undo/" + p.name.ToLower() + "/" + di.GetFiles("*.undo").Length + ".undo"))
                    {
                        foreach (UndoPos uP in p.UndoBuffer)
                        {
                            w.Write(uP.mapName + " " +
                                uP.x + " " + uP.y + " " + uP.z + " " +
                                uP.timePlaced.ToString().Replace(' ', '&') + " " +
                                uP.type + " " + uP.newtype + " ");
                        }
                    }
                }
            }
            catch (Exception e) { Server.s.Log("Error saving undo data for " + p.name + "!"); Server.ErrorLog(e); }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
            if (connections.Contains(this)) connections.Remove(this);
            Extras.Clear();
            CopyBuffer.Clear();
            RedoBuffer.Clear();
            UndoBuffer.Clear();
            spamBlockLog.Clear();
            //spamChatLog.Clear();
            spyChatRooms.Clear();
            /*try
            {
                //this.commThread.Abort();
            }
            catch { }*/
        }
        //fixed undo code
        public bool IsAloneOnCurrentLevel()
        {
            foreach (Player pl in Player.players)
                if (pl.level == level && pl != this) return false;
            return true;
        }

        #endregion
        #region == CHECKING ==
        public static List<Player> GetPlayers() { return new List<Player>(players); }
        public static bool Exists(string name)
        {
            foreach (Player p in players)
            { if (p.name.ToLower() == name.ToLower()) { return true; } } return false;
        }
        public static bool Exists(byte id)
        {
            foreach (Player p in players)
            { if (p.id == id) { return true; } } return false;
        }
        public static Player Find(string name)
        {
            List<Player> tempList = new List<Player>();
            tempList.AddRange(players);
            Player tempPlayer = null; bool returnNull = false;

            foreach (Player p in tempList)
            {
                if (p.name.ToLower() == name.ToLower()) return p;
                if (p.name.ToLower().IndexOf(name.ToLower()) != -1)
                {
                    if (tempPlayer == null) tempPlayer = p;
                    else returnNull = true;
                }
            }

            if (returnNull == true) return null;
            if (tempPlayer != null) return tempPlayer;
            return null;
        }
        public static Group GetGroup(string name)
        {
            return Group.findPlayerGroup(name);
        }
        public static string GetColor(string name)
        {
            return GetGroup(name).color;
        }
        #endregion
        #region == OTHER ==
        static byte FreeId()
        {
            /*
            for (byte i = 0; i < 255; i++)
            {
                foreach (Player p in players)
                {
                    if (p.id == i) { goto Next; }
                } return i;
            Next: continue;
            } unchecked { return (byte)-1; }*/

            for (byte i = 0; i < 255; i++)
            {
                bool used = false;

                foreach (Player p in players)
                    if (p.id == i) { used = true; break; }

                if (!used)
                    return i;
            }
            return (byte)1;
        }
        public static byte[] StringFormat(string str, int size)
        {
            byte[] bytes = new byte[size];
            bytes = enc.GetBytes(str.PadRight(size).Substring(0, size));
            return bytes;
        }

        // TODO: Optimize this using a StringBuilder
        static List<string> Wordwrap(string message)
        {
            List<string> lines = new List<string>();
            message = Regex.Replace(message, @"(&[0-9a-f])+(&[0-9a-f])", "$2");
            message = Regex.Replace(message, @"(&[0-9a-f])+$", "");

            int limit = 64; string color = "";
            while (message.Length > 0)
            {
                //if (Regex.IsMatch(message, "&a")) break;

                if (lines.Count > 0)
                {
                    if (message[0].ToString() == "&")
                        message = "> " + message.Trim();
                    else
                        message = "> " + color + message.Trim();
                }

                if (message.IndexOf("&") == message.IndexOf("&", message.IndexOf("&") + 1) - 2)
                    message = message.Remove(message.IndexOf("&"), 2);

                if (message.Length <= limit) { lines.Add(message); break; }
                for (int i = limit - 1; i > limit - 20; --i)
                    if (message[i] == ' ')
                    {
                        lines.Add(message.Substring(0, i));
                        goto Next;
                    }

            retry:
                if (message.Length == 0 || limit == 0) { return lines; }

                try
                {
                    if (message.Substring(limit - 2, 1) == "&" || message.Substring(limit - 1, 1) == "&")
                    {
                        message = message.Remove(limit - 2, 1);
                        limit -= 2;
                        goto retry;
                    }
                    else if (message[limit - 1] < 32 || message[limit - 1] > 127)
                    {
                        message = message.Remove(limit - 1, 1);
                        limit -= 1;
                        //goto retry;
                    }
                }
                catch { return lines; }
                lines.Add(message.Substring(0, limit));

            Next: message = message.Substring(lines[lines.Count - 1].Length);
                if (lines.Count == 1) limit = 60;

                int index = lines[lines.Count - 1].LastIndexOf('&');
                if (index != -1)
                {
                    if (index < lines[lines.Count - 1].Length - 1)
                    {
                        char next = lines[lines.Count - 1][index + 1];
                        if ("0123456789abcdef".IndexOf(next) != -1) { color = "&" + next; }
                        if (index == lines[lines.Count - 1].Length - 1)
                        {
                            lines[lines.Count - 1] = lines[lines.Count - 1].Substring(0, lines[lines.Count - 1].Length - 2);
                        }
                    }
                    else if (message.Length != 0)
                    {
                        char next = message[0];
                        if ("0123456789abcdef".IndexOf(next) != -1)
                        {
                            color = "&" + next;
                        }
                        lines[lines.Count - 1] = lines[lines.Count - 1].Substring(0, lines[lines.Count - 1].Length - 1);
                        message = message.Substring(1);
                    }
                }
            }
            char[] temp;
            for (int i = 0; i < lines.Count; i++) // Gotta do it the old fashioned way...
            {
                temp = lines[i].ToCharArray();
                if (temp[temp.Length - 2] == '%' || temp[temp.Length - 2] == '&')
                {
                    temp[temp.Length - 1] = ' ';
                    temp[temp.Length - 2] = ' ';
                }
                StringBuilder message1 = new StringBuilder();
                message1.Append(temp);
                lines[i] = message1.ToString();
            }
            return lines;
        }
        public static bool ValidName(string name)
        {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890._";
            foreach (char ch in name) { if (allowedchars.IndexOf(ch) == -1) { return false; } } return true;
        }

        public static int GetBannedCount()
        {
            try
            {
                return File.ReadAllLines("ranks/banned.txt").Length;
            }
            catch/* (Exception ex)*/
            {
                return 0;
            }
        }
        #endregion
        #region == Host <> Network ==
        public static byte[] HTNO(ushort x)
        {
            byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
        }
        public static ushort NTHO(byte[] x, int offset)
        {
            byte[] y = new byte[2];
            Buffer.BlockCopy(x, offset, y, 0, 2); Array.Reverse(y);
            return BitConverter.ToUInt16(y, 0);
        }
        public static byte[] HTNO(short x)
        {
            byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
        }
        #endregion

        bool CheckBlockSpam()
        {
            if (spamBlockLog.Count >= spamBlockCount)
            {
                DateTime oldestTime = spamBlockLog.Dequeue();
                double spamTimer = DateTime.Now.Subtract(oldestTime).TotalSeconds;
                if (spamTimer < spamBlockTimer && !ignoreGrief)
                {
                    this.Kick("You were kicked by antigrief system. Slow down.");
                    SendMessage(c.red + name + " was kicked for suspected griefing.");
                    Server.s.Log(name + " was kicked for block spam (" + spamBlockCount + " blocks in " + spamTimer + " seconds)");
                    return true;
                }
            }
            spamBlockLog.Enqueue(DateTime.Now);
            return false;
        }

        #region getters
        public ushort[] footLocation
        {
            get
            {
                return getLoc(false);
            }
        }
        public ushort[] headLocation
        {
            get
            {
                return getLoc(true);
            }
        }

        public ushort[] getLoc(bool head)
        {
            ushort[] myPos = pos;
            myPos[0] /= 32;
            if (head) myPos[1] = (ushort)((myPos[1] + 4) / 32);
            else myPos[1] = (ushort)((myPos[1] + 4) / 32 - 1);
            myPos[2] /= 32;
            return myPos;
        }

        public void setLoc(ushort[] myPos)
        {
            myPos[0] *= 32;
            myPos[1] *= 32;
            myPos[2] *= 32;
            unchecked { SendPos((byte)-1, myPos[0], myPos[1], myPos[2], rot[0], rot[1]); }
        }

        #endregion

        public static bool IPInPrivateRange(string ip)
        {
            //Official loopback is 127.0.0.1/8
            if (ip.StartsWith("127.0.0.") || ip.StartsWith("192.168.") || ip.StartsWith("10."))
                return true;

            if (ip.StartsWith("172."))
            {
                string[] split = ip.Split('.');
                if (split.Length >= 2)
                {
                    try
                    {
                        int secondPart = Convert.ToInt32(split[1]);
                        return (secondPart >= 16 && secondPart <= 31);
                    }
                    catch/* (Exception ex)*/
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public class Waypoint
        {
            public class WP
            {
                public ushort x;
                public ushort y;
                public ushort z;
                public byte rotx;
                public byte roty;
                public string name;
                public string lvlname;
            }
            public static WP Find(string name, Player p)
            {
                WP wpfound = null;
                bool found = false;
                foreach (WP wp in p.Waypoints)
                {
                    if (wp.name.ToLower() == name.ToLower())
                    {
                        wpfound = wp;
                        found = true;
                    }
                }
                if (found) { return wpfound; }
                else { return null; }
            }
            public static void Goto(string waypoint, Player p)
            {
                if (Exists(waypoint, p))
                {
                    
                    WP wp = Find(waypoint, p);
                    Level lvl = Level.Find(wp.lvlname);
                    if (wp != null)
                    {
                        if (lvl != null)
                        {
                            if (p.level != lvl)
                            {
                                Command.all.Find("goto").Use(p, lvl.name);
                                while (p.Loading) { Thread.Sleep(250); }
                            }
                            unchecked { p.SendPos((byte)-1, wp.x, wp.y, wp.z, wp.rotx, wp.roty); }
                            Player.SendMessage(p, "Sent you to waypoint");
                        }
                        else { Player.SendMessage(p, "The map that that waypoint is on isn't loaded right now (" + wp.lvlname + ")"); return; }
                    }
                }
            }

            public static void Create(string waypoint, Player p)
            {
                Player.Waypoint.WP wp = new Player.Waypoint.WP();
                {
                    wp.x = p.pos[0];
                    wp.y = p.pos[1];
                    wp.z = p.pos[2];
                    wp.rotx = p.rot[0];
                    wp.roty = p.rot[1];
                    wp.name = waypoint;
                    wp.lvlname = p.level.name;
                }
                p.Waypoints.Add(wp);
                Save();
            }

            public static void Update(string waypoint, Player p)
            {
                WP wp = Find(waypoint, p);
                p.Waypoints.Remove(wp);
                {
                    wp.x = p.pos[0];
                    wp.y = p.pos[1];
                    wp.z = p.pos[2];
                    wp.rotx = p.rot[0];
                    wp.roty = p.rot[1];
                    wp.name = waypoint;
                    wp.lvlname = p.level.name;
                }
                p.Waypoints.Add(wp);
                Save();
            }

            public static void Remove(string waypoint, Player p)
            {
                WP wp = Find(waypoint, p);
                p.Waypoints.Remove(wp);
                Save();
            }

            public static bool Exists(string waypoint, Player p)
            {
                bool exists = false;
                foreach (WP wp in p.Waypoints)
                {
                    if (wp.name.ToLower() == waypoint.ToLower())
                    {
                        exists = true;
                    }
                }
                return exists;
            }

            public static void Load(Player p)
            {
                if (File.Exists("extra/Waypoints/" + p.name + ".save"))
                {
                    using (StreamReader SR = new StreamReader("extra/Waypoints/" + p.name + ".save"))
                    {
                        bool failed = false;
                        string line;
                        while (SR.EndOfStream == false)
                        {
                            line = SR.ReadLine().ToLower().Trim();
                            if (!line.StartsWith("#") && line.Contains(":"))
                            {
                                failed = false;
                                string[] LINE = line.ToLower().Split(':');
                                WP wp = new WP();
                                try
                                {
                                    wp.name = LINE[0];
                                    wp.lvlname = LINE[1];
                                    wp.x = ushort.Parse(LINE[2]);
                                    wp.y = ushort.Parse(LINE[3]);
                                    wp.z = ushort.Parse(LINE[4]);
                                    wp.rotx = byte.Parse(LINE[5]);
                                    wp.roty = byte.Parse(LINE[6]);
                                }
                                catch
                                {
                                    Server.s.Log("Couldn't load a Waypoint!");
                                    failed = true;
                                }
                                if (failed == false)
                                {
                                    p.Waypoints.Add(wp);
                                }
                            }
                        }
                        SR.Dispose();
                    }
                }
            }

            public static void Save()
            {
                foreach (Player p in Player.players)
                {
                    if (p.Waypoints.Count >= 1)
                    {
                        using (StreamWriter SW = new StreamWriter("extra/Waypoints/" + p.name + ".save"))
                        {
                            foreach (WP wp in p.Waypoints)
                            {
                                SW.WriteLine(wp.name + ":" + wp.lvlname + ":" + wp.x + ":" + wp.y + ":" + wp.z + ":" + wp.rotx + ":" + wp.roty);
                            }
                            SW.Dispose();
                        }
                    }
                }
            }
        }
        public bool EnoughMoney(int amount)
        {
            if (this.money >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ReviewTimer()  
        {
            this.canusereview = false;
            System.Timers.Timer Clock = new System.Timers.Timer(1000 * Server.reviewcooldown);
            Clock.Elapsed += delegate { this.canusereview = true; Clock.Dispose(); };
            Clock.Start();
        }
    }
}