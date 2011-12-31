///////--|----------------------------------|--\\\\\\\
//////---|  TNT WARS - Coded by edh649      |---\\\\\\
/////----|                                  |----\\\\\
////-----|  Note: Double click on // to see |-----\\\\
///------|        them in the sidebar!!     |------\\\
//-------|__________________________________|-------\\

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace MCForge
{
    public class TntWarsGame : IDisposable
    {
        //Vars
        public static List<TntWarsGame> GameList = new List<TntWarsGame>();
        private Level _lvl; 

        public Level lvl
        {
            get
            {
                return _lvl;
            }
            set
            {
                _lvl = value;
            }
        }
        private TntWarsGameStatus _gameStatus = TntWarsGameStatus.WaitingForPlayers; 

        public TntWarsGameStatus GameStatus
        {
            get
            {
                return _gameStatus;
            }
            set
            {
                _gameStatus = value;
            }
        }
        private int _backupNumber; 

        public int BackupNumber
        {
            get
            {
                return _backupNumber;
            }
            set
            {
                _backupNumber = value;
            }
        }
        private bool _allSetUp; 

        public bool AllSetUp
        {
            get
            {
                return _allSetUp;
            }
            set
            {
                _allSetUp = value;
            }
        }
        private TntWarsGameMode _gameMode = TntWarsGameMode.TDM; 

        public TntWarsGameMode GameMode
        {
            get
            {
                return _gameMode;
            }
            set
            {
                _gameMode = value;
            }
        }
        private TntWarsDifficulty _gameDifficulty = TntWarsDifficulty.Normal; 

        public TntWarsDifficulty GameDifficulty
        {
            get
            {
                return _gameDifficulty;
            }
            set
            {
                _gameDifficulty = value;
            }
        }
        private int _gameNumber; 

        public int GameNumber
        {
            get
            {
                return _gameNumber;
            }
            set
            {
                _gameNumber = value;
            }
        }
        private ushort[] _redSpawn; 

        public ushort[] RedSpawn
        {
            get
            {
                return _redSpawn;
            }
            set
            {
                _redSpawn = value;
            }
        }
        private ushort[] _blueSpawn; 

        public ushort[] BlueSpawn
        {
            get
            {
                return _blueSpawn;
            }
            set
            {
                _blueSpawn = value;
            }
        }
        //incase they don't want the default
        private int _tntPerPlayerAtATime = Properties.DefaultTntPerPlayerAtATime; 

        public int TntPerPlayerAtATime
        {
            get
            {
                return _tntPerPlayerAtATime;
            }
            set
            {
                _tntPerPlayerAtATime = value;
            }
        }
        private bool _gracePeriod = Properties.DefaultGracePeriodAtStart; 

        public bool GracePeriod
        {
            get
            {
                return _gracePeriod;
            }
            set
            {
                _gracePeriod = value;
            }
        }
        private int _gracePeriodSecs = Properties.DefaultGracePeriodSecs; 

        public int GracePeriodSecs
        {
            get
            {
                return _gracePeriodSecs;
            }
            set
            {
                _gracePeriodSecs = value;
            }
        }
        private bool _balanceTeams = Properties.DefaultBalanceTeams; 

        public bool BalanceTeams
        {
            get
            {
                return _balanceTeams;
            }
            set
            {
                _balanceTeams = value;
            }
        }
        //scores/streaks
        private int _scoreLimit = Properties.DefaultTDMmaxScore; 

        public int ScoreLimit
        {
            get
            {
                return _scoreLimit;
            }
            set
            {
                _scoreLimit = value;
            }
        }
        private bool _streaks = true; 

        public bool Streaks
        {
            get
            {
                return _streaks;
            }
            set
            {
                _streaks = value;
            }
        }
        private int _multiKillBonus = Properties.DefaultMultiKillBonus; 

        public int MultiKillBonus
        {
            get
            {
                return _multiKillBonus;
            }
            set
            {
                _multiKillBonus = value;
            }
        } //This is the amount of extra points per each player that is killed per 1 tnt (if playerskilledforthistnt > 1)
        private int _scorePerKill = Properties.DefaultScorePerKill; 

        public int ScorePerKill
        {
            get
            {
                return _scorePerKill;
            }
            set
            {
                _scorePerKill = value;
            }
        }
        private int _scorePerAssist = Properties.DefaultAssistScore; 

        public int ScorePerAssist
        {
            get
            {
                return _scorePerAssist;
            }
            set
            {
                _scorePerAssist = value;
            }
        }
        private bool _teamKills; 

        public bool TeamKills
        {
            get
            {
                return _teamKills;
            }
            set
            {
                _teamKills = value;
            }
        }
        private Thread _starter; 

        public Thread Starter
        {
            get
            {
                return _starter;
            }
            set
            {
                _starter = value;
            }
        }

        public static TntWarsGame GuiLoaded = null;
        //======PLUGIN EVENTS======
        public delegate void Starting(TntWarsGame t);
        public delegate void Started(TntWarsGame t);
        public delegate void Death(Player killer, List<Player> deadplayers);
        public delegate void End(TntWarsGame t);
        //======PLUGIN EVENTS======
        public TntWarsGame(Level level)
        {
            Starter = new Thread(Start);
            lvl = level;
        }

        //Player/Team stuff
        private List<player> _players = new List<player>(); 

        public List<player> Players
        {
            get
            {
                return _players;
            }
            set
            {
                _players = value;
            }
        }

        private int _redScore; 

        public int RedScore
        {
            get
            {
                return _redScore;
            }
            set
            {
                _redScore = value;
            }
        }
        private int _blueScore; 

        public int BlueScore
        {
            get
            {
                return _blueScore;
            }
            set
            {
                _blueScore = value;
            }
        }

        //Zones
        private List<Zone> _noTNTplacableZones = new List<Zone>(); 

        public List<Zone> NoTNTplacableZones
        {
            get
            {
                return _noTNTplacableZones;
            }
            set
            {
                _noTNTplacableZones = value;
            }
        }
        private List<Zone> _noBlockDeathZones = new List<Zone>(); 

        public List<Zone> NoBlockDeathZones
        {
            get
            {
                return _noBlockDeathZones;
            }
            set
            {
                _noBlockDeathZones = value;
            }
        }
        public class Zone
        {
            private ushort _bigX; 

            public ushort bigX
            {
                get
                {
                    return _bigX;
                }
                set
                {
                    _bigX = value;
                }
            }
            private ushort _bigY; 

            public ushort bigY
            {
                get
                {
                    return _bigY;
                }
                set
                {
                    _bigY = value;
                }
            }
            private ushort _bigZ; 

            public ushort bigZ
            {
                get
                {
                    return _bigZ;
                }
                set
                {
                    _bigZ = value;
                }
            }
            private ushort _smallX; 

            public ushort smallX
            {
                get
                {
                    return _smallX;
                }
                set
                {
                    _smallX = value;
                }
            }
            private ushort _smallY; 

            public ushort smallY
            {
                get
                {
                    return _smallY;
                }
                set
                {
                    _smallY = value;
                }
            }
            private ushort _smallZ; 

            public ushort smallZ
            {
                get
                {
                    return _smallZ;
                }
                set
                {
                    _smallZ = value;
                }
            }
        }

        //During Game Main Methods
        public void Start()
        {
            GameStatus = TntWarsGameStatus.AboutToStart;
            //Checking Backups & physics etc.
            {
                BackupNumber = lvl.Backup(true);
                if (BackupNumber <= 0)
                {
                    SendAllPlayersMessage(c.red + "Backing up Level for TNT Wars failed, Stopping game");
                    Player.GlobalMessageOps(c.red + "Backing up Level for TNT Wars failed, Stopping game");
                    GameStatus = TntWarsGameStatus.Finished;
                    return;
                }
                Server.s.Log("Backed up " + lvl.name + " (" + BackupNumber.ToString(CultureInfo.CurrentCulture) + ") for TNT Wars");
            }
            //Map stuff
            lvl.setPhysics(3);
            lvl.permissionbuild = Group.Find(Server.defaultRank).Permission;
            lvl.permissionvisit = Group.Find(Server.defaultRank).Permission;
            lvl.Killer = true;
            //Seting Up Some Player stuff
            {
                foreach (player p in Players)
                {
                    p.p.PlayingTntWars = true;
                    p.p.CurrentAmountOfTnt = 0;
                    p.p.CurrentTntGameNumber = GameNumber;
                    if (GameDifficulty == TntWarsDifficulty.Easy || GameDifficulty == TntWarsDifficulty.Normal) p.p.TntWarsHealth = 2;
                    else p.p.TntWarsHealth = 1;
                    p.p.HarmedBy = null;
                    if (GracePeriod)
                    {
                        p.p.canBuild = false;
                    }
                    if (p.spec)
                    {
                        p.p.canBuild = false;
                        Player.SendMessage(p.p, "TNT Wars: Disabled building because you are a spectator!");
                    }
                    p.p.TntWarsKillStreak = 0;
                    p.p.TntWarsScoreMultiplier = 1f;
                    p.p.TNTWarsLastKillStreakAnnounced = 0;
                    SetTitlesAndColor(p);
                }
                if (GracePeriod)
                {
                    SendAllPlayersMessage("TNT Wars: Disabled building during Grace Period!");
                }
            }
            //Spawn them (And if needed, move them to the correct level!)
            {
                foreach (player p in Players)
                {
                    if (p.p.level != lvl)
                    {
                        Command.all.Find("goto").Use(p.p, lvl.name);
                        while (p.p.Loading) { Thread.Sleep(250); }
                        p.p.inTNTwarsMap = true;
                    }
                }
                if (GameMode == TntWarsGameMode.TDM) { Command.all.Find("reveal").Use(null, "all " + lvl.name); }//So peoples names apear above their heads in the right color!
                foreach (player p in Players)
                {
                    Command.all.Find("spawn").Use(p.p, ""); //This has to be after reveal so that they spawn in the correct place!!
                    Thread.Sleep(250);
                }
            }
            //Announcing Etc.
            string Gamemode = "Free For All";
            if (GameMode == TntWarsGameMode.TDM) Gamemode = "Team Deathmatch";
            string Difficulty = "Normal";
            string HitsToDie = "2";
            string explosiontime = "medium";
            string explosionsize = "normal";
            switch (GameDifficulty)
            {
                case TntWarsDifficulty.Easy:
                    Difficulty = "Easy";
                    explosiontime = "long";
                    break;

                case TntWarsDifficulty.Normal:
                    Difficulty = "Normal";
                    break;

                case TntWarsDifficulty.Hard:
                    HitsToDie = "1";
                    Difficulty = "Hard";
                    break;

                case TntWarsDifficulty.Extreme:
                    HitsToDie = "1";
                    explosiontime = "short";
                    explosionsize = "big";
                    Difficulty = "Extreme";
                    break;
            }
            string teamkillling = "Disabled";
            if (TeamKills) teamkillling = "Enabled";
            Player.GlobalMessage(c.red + "TNT Wars " + Server.DefaultColor + "on '" + lvl.name + "' has started " + c.teal + Gamemode + Server.DefaultColor + " with a difficulty of " + c.teal + Difficulty + Server.DefaultColor + " (" + c.teal + HitsToDie + Server.DefaultColor + " hits to die, a " + c.teal + explosiontime + Server.DefaultColor + " explosion delay and with a " + c.teal + explosionsize + Server.DefaultColor + " explosion size)" + ", team killing is " + c.teal + teamkillling + Server.DefaultColor + " and you can place " + c.teal + TntPerPlayerAtATime.ToString(CultureInfo.CurrentCulture) + Server.DefaultColor + " TNT at a time and there is a score limit of " + c.teal + ScoreLimit.ToString(CultureInfo.CurrentCulture) + Server.DefaultColor + "!!");
            if (GameMode == TntWarsGameMode.TDM) SendAllPlayersMessage("TNT Wars: Start your message with ':' to send it as a team chat!");
            //GracePeriod
            if (GracePeriod) //Check This Grace Stuff
            {
                GameStatus = TntWarsGameStatus.GracePeriod;
                int GracePeriodSecsRemaining = GracePeriodSecs;
                SendAllPlayersMessage("TNT Wars: Grace Period of " + c.lime + GracePeriodSecsRemaining.ToString(CultureInfo.CurrentCulture) + Server.DefaultColor + " seconds");
                while (GracePeriodSecsRemaining > 0)
                {
                    switch (GracePeriodSecsRemaining)
                    {
                        case 300:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "5" + Server.DefaultColor + " minutes remaining!");
                            break;

                        case 240:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "4" + Server.DefaultColor + " minutes remaining!");
                            break;

                        case 180:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "3" + Server.DefaultColor + " minutes remaining!");
                            break;

                        case 120:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "2" + Server.DefaultColor + " minutes remaining!");
                            break;

                        case 90:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "1" + Server.DefaultColor + " minute and " + c.teal + "30" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 60:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "1" + Server.DefaultColor + " minute remaining!");
                            break;

                        case 45:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "45" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 30:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "30" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 15:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "15" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 10:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "10" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 9:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "9" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 8:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "8" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 7:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "7" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 6:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "6" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 5:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "5" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 4:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "4" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 3:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "3" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 2:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "2" + Server.DefaultColor + " seconds remaining!");
                            break;

                        case 1:
                            SendAllPlayersMessage("TNT Wars: " + c.teal + "1" + Server.DefaultColor + " second remaining!");
                            break;
                    }

                    Thread.Sleep(1000);
                    GracePeriodSecsRemaining -= 1;
                }
                SendAllPlayersMessage("TNT Wars: Grace Period is over!!!!!");
                SendAllPlayersMessage("TNT Wars: You may now place " + c.red + "TNT");
            }
            SendAllPlayersMessage("TNT Wars: " + c.white + "The Game Has Started!!!!!");
            GameStatus = TntWarsGameStatus.InProgress;
            foreach (player p in Players)
            {
                if (p.spec == false)
                {
                    p.p.canBuild = true;
                }
            }
            if (GracePeriod)
            {
                SendAllPlayersMessage("TNT Wars: You can now build!!");
            }
            //MainLoop
            while (!Finished())
            {
                int i = 1; //For making a top 5 (or whatever) players announcement every 3 loops (if TDM)
                Thread.Sleep(3 * 1000); if (Finished()) break;  //--\\
                Thread.Sleep(3 * 1000); if (Finished()) break;  //----\
                Thread.Sleep(3 * 1000); if (Finished()) break;  //-----> So that if it finsihes, we don't have to wait like 10 secs for the announcement!!
                Thread.Sleep(3 * 1000); if (Finished()) break;  //----/
                Thread.Sleep(3 * 1000); if (Finished()) break;  //--//
                if (GameMode == TntWarsGameMode.TDM)
                {
                    if (i < 3)
                    {
                        SendAllPlayersScore(true, true, false);
                    }
                    if (i >= 3)
                    {
                        SendAllPlayersScore(true, true, true);
                        i = 0;
                    }
                    i++;
                }
                else if (GameMode == TntWarsGameMode.FFA)
                {
                    SendAllPlayersScore(false, true, true);
                }
            }
            END();
        }
        public void END()
        {
            GameStatus = TntWarsGameStatus.Finished;
            //let them build and spawn them and change playingtntwars to false
            foreach (player p in Players)
            {
                p.p.canBuild = true;
                Command.all.Find("spawn").Use(p.p, "");
                p.p.PlayingTntWars = false;
            }
            //Message about winners etc.
            if (Players.Count <= 1)
            {
                Player.GlobalMessage(c.red + "TNT Wars " + Server.DefaultColor + "has ended because there are no longer enough players!");
            }
            else
            {
                Player.GlobalMessage(c.red + "TNT Wars" + Server.DefaultColor + " has ended!!");
            }
            if (GameMode == TntWarsGameMode.TDM)
            {
                if (RedScore >= BlueScore)
                {
                    Player.GlobalMessage("TNT Wars: Team " + c.red + "Red " + Server.DefaultColor + "won " + c.red + "TNT Wars " + Server.DefaultColor + "by " + (RedScore - BlueScore).ToString(CultureInfo.CurrentCulture) + " points!");
                }
                if (BlueScore >= RedScore)
                {
                    Player.GlobalMessage("TNT Wars: Team " + c.blue + "Blue " + Server.DefaultColor + "won " + c.red + "TNT Wars " + Server.DefaultColor + "by " + (BlueScore - RedScore).ToString(CultureInfo.CurrentCulture) + " points!");
                }
                try
                {
                    foreach (player p in Players)
                    {
                        if (!p.spec)
                        {
                            Player.SendMessage(p.p, "TNT Wars: You Scored " + p.Score.ToString(CultureInfo.CurrentCulture) + " points");
                        }
                    }
                }
                catch { }
                SendAllPlayersMessage("TNT Wars: Top Scores:");
                SendAllPlayersScore(false, false, true);
            }
            if (GameMode == TntWarsGameMode.FFA)
            {
                var pls = from pla in Players orderby pla.Score descending select pla; //LINQ FTW
                int count = 1;
                foreach (var pl in pls)
                {
                    if (count == 1)
                    {
                        Player.GlobalMessage(c.red + "TNT Wars " + Server.DefaultColor + "1st Place: " + pl.p.color + pl.p.name + Server.DefaultColor + " with a score of " + pl.p.color + pl.Score);
                    }
                    else if (count == 2)
                    {
                        SendAllPlayersMessage(c.red + "TNT Wars " + Server.DefaultColor + "2nd Place: " + pl.p.color + pl.p.name + Server.DefaultColor + " with a score of " + pl.p.color + pl.Score);
                    }
                    else if (count == 3)
                    {
                        SendAllPlayersMessage(c.red + "TNT Wars " + Server.DefaultColor + "3rd Place: " + pl.p.color + pl.p.name + Server.DefaultColor + " with a score of " + pl.p.color + pl.Score);
                    }
                    else if (count >= 4)
                    {
                        SendAllPlayersMessage(c.red + "TNT Wars " + Server.DefaultColor + count.ToString(CultureInfo.CurrentCulture) + "th Place: " + pl.p.color + pl.p.name + Server.DefaultColor + " with a score of " + pl.p.color + pl.Score);
                    }
                    if (count >= PlayingPlayers())
                    {
                        break;
                    }
                    count++;
                    Thread.Sleep(750); //Maybe, not sure (was 500)
                }
            }
            //Reset map
            Command.all.Find("restore").Use(null, BackupNumber.ToString(CultureInfo.CurrentCulture) + " " + lvl.name);
            if (lvl.overload == 2501)
            {
                lvl.overload = 1500;
                Server.s.Log("TNT Wars: Set level physics overload back to 1500");
            }
        }

        public void SendAllPlayersMessage(string Message)
        {
            try
            {
                foreach (player p in Players)
                {
                    Player.SendMessage(p.p, Message);

                }
            }
            catch { };
            Server.s.Log("[TNT Wars] [" + lvl.name + "] " + Message);
        }

        public void HandleKill(Player Killer, List<Player> Killed)
        {
            List<Player> Dead = new List<Player>();
            int HealthDamage = 1;
            int kills = 0;
            int minusfromscore = 0;
            if (GameDifficulty == TntWarsDifficulty.Hard || GameDifficulty == TntWarsDifficulty.Extreme)
            {
                HealthDamage = 2;
            }
            foreach (Player Kld in Killed)
            {
                if (FindPlayer(Kld).spec)
                {
                    continue;
                }
                if (TeamKill(Killer, Kld) && TeamKills == false) continue;
                if (Kld.TntWarsHealth - HealthDamage <= 0)
                {
                    Kld.TntWarsHealth = 0;
                    Dead.Add(Kld);
                    if (TeamKills && TeamKill(Killer, Kld))
                    {
                        minusfromscore += ScorePerKill;
                    }
                }
                else
                {
                    Kld.TntWarsHealth -= HealthDamage;
                    Kld.HarmedBy = Killer;
                    Player.SendMessage(Killer, "TNT Wars: You harmed " + Kld.color + Kld.name);
                    Player.SendMessage(Kld, "TNT Wars: You were harmed by " + Killer.color + Killer.name);
                }
            }
            foreach (Player Died in Dead)
            {
                Died.TntWarsKillStreak = 0;
                Died.TntWarsScoreMultiplier = 1f;
                Died.TNTWarsLastKillStreakAnnounced = 0;
                if (Died.HarmedBy == null || Died.HarmedBy == Killer)
                {
                    if (TeamKill(Killer, Died))
                    {
                        SendAllPlayersMessage("TNT Wars: " + Killer.color + Killer.name + Server.DefaultColor + " team killed " + Died.color + Died.name);
                    }
                    else
                    {
                        SendAllPlayersMessage("TNT Wars: " + Killer.color + Killer.name + Server.DefaultColor + " killed " + Died.color + Died.name);
                        kills += 1;
                    }
                }
                else
                {
                    {
                        if (TeamKill(Killer, Died))
                        {
                            SendAllPlayersMessage("TNT Wars: " + Killer.color + Killer.name + Server.DefaultColor + " team killed " + Died.color + Died.name + Server.DefaultColor + " (with help from " + Died.HarmedBy.color + Died.HarmedBy.name + ")");
                        }
                        else
                        {
                            SendAllPlayersMessage("TNT Wars: " + Killer.color + Killer.name + Server.DefaultColor + " killed " + Died.color + Died.name + Server.DefaultColor + " (with help from " + Died.HarmedBy.color + Died.HarmedBy.name + ")");
                            kills += 1;
                        }
                    }
                    {
                        if (TeamKill(Died.HarmedBy, Died))
                        {
                            Player.SendMessage(Died.HarmedBy, "TNT Wars: - " + ScorePerAssist.ToString(CultureInfo.CurrentCulture) + " point(s) for team kill assist!");
                            ChangeScore(Died.HarmedBy, -ScorePerAssist);
                        }
                        else
                        {
                            Player.SendMessage(Died.HarmedBy, "TNT Wars: + " + ScorePerAssist.ToString(CultureInfo.CurrentCulture) + " point(s) for assist!");
                            ChangeScore(Died.HarmedBy, ScorePerAssist);
                        }
                    }
                    Died.HarmedBy = null;
                }
                Command.all.Find("spawn").Use(Died, "");
                Died.TntWarsHealth = 2;
            }
            //Scoring
            int AddToScore = 0;
            //streaks
            Killer.TntWarsKillStreak += kills;
            if (kills >= 1 && Streaks)
            {
                if (Killer.TntWarsKillStreak >= Properties.DefaultStreakOneAmount && Killer.TntWarsKillStreak < Properties.DefaultStreakTwoAmount && Killer.TNTWarsLastKillStreakAnnounced != Properties.DefaultStreakOneAmount)
                {
                    Player.SendMessage(Killer, "TNT Wars: Kill streak of " + Killer.TntWarsKillStreak.ToString(CultureInfo.CurrentCulture) + " (Multiplier of " + Properties.DefaultStreakOneMultiplier.ToString(CultureInfo.CurrentCulture) + ")");
                    SendAllPlayersMessage("TNT Wars: " + Killer.color + Killer.name + Server.DefaultColor + " has a kill streak of " + Killer.TntWarsKillStreak.ToString(CultureInfo.CurrentCulture));
                    Killer.TntWarsScoreMultiplier = Properties.DefaultStreakOneMultiplier;
                    Killer.TNTWarsLastKillStreakAnnounced = Properties.DefaultStreakOneAmount;
                }
                else if (Killer.TntWarsKillStreak >= Properties.DefaultStreakTwoAmount && Killer.TntWarsKillStreak < Properties.DefaultStreakThreeAmount && Killer.TNTWarsLastKillStreakAnnounced != Properties.DefaultStreakTwoAmount)
                {
                    Player.SendMessage(Killer, "TNT Wars: Kill streak of " + Killer.TntWarsKillStreak.ToString(CultureInfo.CurrentCulture) + " (Multiplier of " + Properties.DefaultStreakTwoMultiplier.ToString(CultureInfo.CurrentCulture) + " and a bigger explosion!)");
                    SendAllPlayersMessage("TNT Wars: " + Killer.color + Killer.name + Server.DefaultColor + " has a kill streak of " + Killer.TntWarsKillStreak.ToString(CultureInfo.CurrentCulture) + " and now has a bigger explosion for their TNT!");
                    Killer.TntWarsScoreMultiplier = Properties.DefaultStreakTwoMultiplier;
                    Killer.TNTWarsLastKillStreakAnnounced = Properties.DefaultStreakTwoAmount;
                }
                else if (Killer.TntWarsKillStreak >= Properties.DefaultStreakThreeAmount && Killer.TNTWarsLastKillStreakAnnounced != Properties.DefaultStreakThreeAmount)
                {
                    Player.SendMessage(Killer, "TNT Wars: Kill streak of " + Killer.TntWarsKillStreak.ToString(CultureInfo.CurrentCulture) + " (Multiplier of " + Properties.DefaultStreakThreeMultiplier.ToString(CultureInfo.CurrentCulture) + " and you now have 1 extra health!)");
                    SendAllPlayersMessage("TNT Wars: " + Killer.color + Killer.name + Server.DefaultColor + " has a kill streak of " + Killer.TntWarsKillStreak.ToString(CultureInfo.CurrentCulture) + " and now has 1 extra health!");
                    Killer.TntWarsScoreMultiplier = Properties.DefaultStreakThreeMultiplier;
                    Killer.TNTWarsLastKillStreakAnnounced = Properties.DefaultStreakThreeAmount;
                    if (GameDifficulty == TntWarsDifficulty.Hard || GameDifficulty == TntWarsDifficulty.Extreme)
                    {
                        Killer.TntWarsHealth += 2;
                    }
                    else
                    {
                        Killer.TntWarsHealth += 1;
                    }
                }
                else
                {
                    Player.SendMessage(Killer, "TNT Wars: Kill streak of " + Killer.TntWarsKillStreak.ToString(CultureInfo.CurrentCulture));
                }
            }
            AddToScore += kills * ScorePerKill;
            //multikill
            if (kills > 1)
            {
                AddToScore += kills * MultiKillBonus;
            }
            //Add to score
            if (AddToScore > 0)
            {
                ChangeScore(Killer, AddToScore, Killer.TntWarsScoreMultiplier);
                Player.SendMessage(Killer, "TNT Wars: + " + ((int)(AddToScore * Killer.TntWarsScoreMultiplier)).ToString(CultureInfo.CurrentCulture) + " point(s) for " + kills.ToString(CultureInfo.CurrentCulture) + " kills");
            }
            if (minusfromscore != 0)
            {
                ChangeScore(Killer, -minusfromscore);
                Player.SendMessage(Killer, "TNT Wars: - " + minusfromscore.ToString(CultureInfo.CurrentCulture) + " point(s) for team kill(s)!");
            }
        }

        public void ChangeScore(Player p, int Amount, float multiplier = 1f)
        {
            ChangeScore(FindPlayer(p), Amount, multiplier);
        }

        public void ChangeScore(player p, int Amount, float multiplier = 1f)
        {
            p.Score += (int)(Amount * multiplier);
            if (GameMode == TntWarsGameMode.TDM)
            {
                if (p.Red)
                {
                    RedScore += (int)(Amount * multiplier);
                }
                if (p.Blue)
                {
                    BlueScore += (int)(Amount * multiplier);
                }
            }
        }

        public bool InZone(ushort x, ushort y, ushort z, bool CheckForPlacingTnt)
        {
            if (CheckForPlacingTnt)
            {
                foreach (Zone Zn in NoTNTplacableZones)
                {
                    if (Zn.smallX <= x && x <= Zn.bigX && Zn.smallY <= y && y <= Zn.bigY && Zn.smallZ <= z && z <= Zn.bigZ)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                foreach (Zone Zn in NoBlockDeathZones)
                {
                    if (Zn.smallX <= x && x <= Zn.bigX && Zn.smallY <= y && y <= Zn.bigY && Zn.smallZ <= z && z <= Zn.bigZ)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void DeleteZone(ushort x, ushort y, ushort z, bool NoTntZone, Player p = null)
        {
            try
            {
                Zone Z = new Zone();
                if (NoTntZone)
                {
                    foreach (Zone Zn in NoTNTplacableZones)
                    {
                        if (Zn.smallX <= x && x <= Zn.bigX && Zn.smallY <= y && y <= Zn.bigY && Zn.smallZ <= z && z <= Zn.bigZ)
                        {
                            Z = Zn;
                        }
                    }
                    NoTNTplacableZones.Remove(Z);
                    if (p != null)
                    {
                        Player.SendMessage(p, "TNT Wars: Zone Deleted!");
                    }
                    return;
                }
                else
                {
                    foreach (Zone Zn in NoBlockDeathZones)
                    {
                        if (Zn.smallX <= x && x <= Zn.bigX && Zn.smallY <= y && y <= Zn.bigY && Zn.smallZ <= z && z <= Zn.bigZ)
                        {
                            Z = Zn;
                        }
                    }
                    NoBlockDeathZones.Remove(Z);
                    if (p != null)
                    {
                        Player.SendMessage(p, "TNT Wars: Zone Deleted!");
                    }
                    return;
                }
            }
            catch
            {
                Player.SendMessage(p, "TNT Wars Error: Zone not deleted!");
            }

        }

        public bool TeamKill(Player p1, Player p2)
        {
            return TeamKill(FindPlayer(p1), FindPlayer(p2));
        }

        public bool TeamKill(player p1, player p2)
        {
            if (GameMode == TntWarsGameMode.TDM)
            {
                if (p1.Red == true && p2.Red) return true;
                if (p1.Blue == true && p2.Blue) return true;
            }
            return false;
        }

        public void SendAllPlayersScore(bool TotalTeamScores = false, bool TheirTotalScore = false, bool TopScores = false)
        {
            try
            {
                if (TotalTeamScores)
                {
                    SendAllPlayersMessage("TNT Wars Scores:");
                    SendAllPlayersMessage(c.red + "RED: " + c.white + RedScore + " " + c.red + "(" + (ScoreLimit - RedScore).ToString(CultureInfo.CurrentCulture) + " needed)");
                    SendAllPlayersMessage(c.blue + "BLUE: " + c.white + BlueScore + " " + c.red + "(" + (ScoreLimit - BlueScore).ToString(CultureInfo.CurrentCulture) + " needed)");
                    Thread.Sleep(1000);
                }
                if (TopScores)
                {
                    int max = 5;
                    if (PlayingPlayers() < 5)
                    {
                        max = PlayingPlayers();
                    }

                    var pls = from pla in Players orderby pla.Score descending select pla; //LINQ FTW

                    foreach (player p in Players)
                    {
                        int count = 1;
                        foreach (var pl in pls)
                        {
                            Player.SendMessage(p.p, count.ToString(CultureInfo.CurrentCulture) + ": " + pl.p.name + " - " + pl.Score.ToString(CultureInfo.CurrentCulture));
                            if (count >= max)
                            {
                                break;
                            }
                            count++;
                            Thread.Sleep(500); //Maybe, not sure (250??)
                        }
                    }
                    Thread.Sleep(1000);
                }
                if (TheirTotalScore)
                {
                    foreach (player p in Players)
                    {
                        if (!p.spec)
                        {
                            Player.SendMessage(p.p, "TNT Wars: Your Score: " + c.white + p.Score.ToString(CultureInfo.CurrentCulture));
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch { }
        }

        public bool Finished()
        {
            if (GameMode == TntWarsGameMode.TDM && (RedScore >= ScoreLimit || BlueScore >= ScoreLimit))
            {
                return true;
            }
            if (GameMode == TntWarsGameMode.FFA)
            {
                try
                {
                    foreach (player p in Players)
                    {
                        if (p.Score >= ScoreLimit)
                        {
                            return true;
                        }
                    }
                }
                catch { }
            }
            if (PlayingPlayers() <= 1)
            {
                return true;
            }
            if (GameStatus == TntWarsGameStatus.Finished)
            {
                return true;
            }
            return false;
        }

        //enums
        public enum TntWarsGameStatus : int
        {
            WaitingForPlayers = 0,
            AboutToStart = 1,
            GracePeriod = 2,
            InProgress = 3,
            Finished = 4
        }

        public enum TntWarsGameMode : int
        {
            FFA = 0,
            TDM = 1
        }

        public enum TntWarsDifficulty : int
        {
            Easy = 0,       //2 Hits to die, Tnt has long delay
            Normal = 1,     //2 Hits to die, Tnt has normal delay
            Hard = 2,       //1 Hit to die, Tnt has short delay
            Extreme = 3     //1 Hit to die, Tnt has short delay and BIG exlosion
        }

        //Other stuff
        public int RedTeam()
        {
            int count = 0;
            foreach (player p in Players)
            {
                if (p.Red) count++;
            }
            return count;
        }
        public int BlueTeam()
        {
            int count = 0;
            foreach (player p in Players)
            {
                if (p.Blue) count++;
            }
            return count;
        }
        public int PlayingPlayers()
        {
            int count = 0;
            foreach (player p in Players)
            {
                if (!p.spec) count++;
            }
            return count;
        }
        public static void SetTitlesAndColor(player p, bool reset = false)
        {
            try
            {
                if (reset)
                {
                    p.p.title = p.OldTitle;
                    p.p.color = p.OldTitleColor;
                    p.p.color = p.OldColor;
                    p.p.SetPrefix();
                }
                else
                {
                    p.p.title = "TNT Wars";
                    p.p.titlecolor = c.white;
                    if (p.Red) p.p.color = c.red;
                    if (p.Blue) p.p.color = c.blue;
                    p.p.SetPrefix();
                }
            }
            catch { }
        }

        public bool CheckAllSetUp(Player p, bool ReturnErrors = false, bool TellPlayerOnSuccess = true)
        {
            if (lvl != null
                && GameStatus == 0)
            {
                foreach (TntWarsGame g in GameList)
                {
                    if (g.lvl == this.lvl && g != this)
                    {
                        if (ReturnErrors) Player.SendMessage(p, "There is already a TNT Wars game on that map");
                        AllSetUp = false;
                        return false;
                    }
                }
                if (TellPlayerOnSuccess) Player.SendMessage(p, "TNT Wars setup is done!");
                AllSetUp = true;
                return true;
            }
            if (ReturnErrors) SendPlayerCheckSetupErrors(p);
            AllSetUp = false;
            return false;

        }

        public void SendPlayerCheckSetupErrors(Player p)
        {
            if (lvl == null)
            {
                Player.SendMessage(p, "TNT Wars Error: No Level Selected");
            }
            else if (GameStatus != 0)
            {
                Player.SendMessage(p, "Game is already in progress");
            }
        }

        public static TntWarsGame Find(Level level)
        {
            foreach (TntWarsGame g in GameList)
            {
                if (g.lvl == level)
                {
                    return g;
                }
            }
            return null;
        }

        public static TntWarsGame FindFromGameNumber(int Number)
        {
            foreach (TntWarsGame g in GameList)
            {
                if (g.GameNumber == Number)
                {
                    return g;
                }
            }
            return null;
        }

        public player FindPlayer(Player pla)
        {
            foreach (player p in Players)
            {
                if (p.p == pla)
                {
                    return p;
                }
            }
            return null;
        }

        public static TntWarsGame GetTntWarsGame(Player p)
        {
            TntWarsGame it = TntWarsGame.Find(p.level);
            if (it == null)
            {
                it = TntWarsGame.FindFromGameNumber(p.CurrentTntGameNumber);
                if (it == null)
                {
                    return null;
                }
                else
                {
                    return it;
                }
            }
            else
            {
                return it;
            }
        }

        //Static Stuff
        public static class Properties
        {
            public static bool Enable = true;
            public static bool DefaultGracePeriodAtStart = true;
            public static int DefaultGracePeriodSecs = 30;
            public static int DefaultTntPerPlayerAtATime = 1;
            public static bool DefaultBalanceTeams = true;
            public static int DefaultFFAmaxScore = 75;
            public static int DefaultTDMmaxScore = 150;
            public static int DefaultScorePerKill = 10;
            public static int DefaultMultiKillBonus = 5;
            public static int DefaultAssistScore = 5;
            public static int DefaultStreakOneAmount = 3;
            public static float DefaultStreakOneMultiplier = 1.25f;
            public static int DefaultStreakTwoAmount = 5;
            public static float DefaultStreakTwoMultiplier = 1.5f;
            public static int DefaultStreakThreeAmount = 7;
            public static float DefaultStreakThreeMultiplier = 2f;
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

                    if (lvl != null)
                        lvl.Dispose();
                    if (GuiLoaded != null)
                        GuiLoaded.Dispose();
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

    public class player
    {
        public Player p;
        public bool Red/* = false*/;
        public bool Blue/* = false*/;
        public bool spec/* = false*/;
        public int Score/* = 0*/;
        public string OldColor;
        public string OldTitle;
        public string OldTitleColor;
        public player(Player pl)
        {
            p = pl;
            OldColor = pl.color;
            OldTitle = pl.title;
            OldTitleColor = pl.titlecolor;
        }
    }
}