/*
Copyright 2011 MCForge
Coded by GamezGalaxy (hypereddie10)
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.osedu.org/licenses/ECL-2.0
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using System;
using System.Threading;
using System.Timers;
using System.Collections.Generic;

namespace MCForge
{
    class CmdInfection : Command
    {
        #region Declaring
        //Command stuff...
        public override string name { get { return "infection"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdInfection() { }
        //CAN I GO BOOM!?!?!?!
        public static Boolean Theybecreepen;
        //Infection Core timer
        public static System.Timers.Timer Infect = new System.Timers.Timer(500);
        //Timer timer
        public static System.Timers.Timer timer = new System.Timers.Timer(1000);
        //Some random stuff...
        Random random = new Random();
        //WHOS INFECTED :D
        public static List<Player> infect = new List<Player>();
        //Who isn't
        public static List<Player> People = new List<Player>();
        //Game name??
        public string gamename = "";
        //WHATS THE GAME PLAYING ON ?!?!?!
        public static Level INFECTEDLEVEL;
        //It wont let me use time D:
        public static int minute;
        public static int seconds;
        public static string time1;
        // Extra fields:
        private static string _oldtitle = "CmdInfection_oldtitle";
        private static string _oldname = "CmdInfection_oldname";
        private static string _lives = "CmdInfection_lives";
        private static string _creeper = "CmdInfection_Creeper";
        #endregion
        #region command
        public override void Use(Player p, string message)
        {
            if (message.ToLower() == "STOP") { Player.SendMessage(p, "Stopping infection..."); Infect.Enabled = false; timer.Enabled = false; END(); }
            Player.SendMessage(p, "Starting Infection...");
            //Picks a random game
            if (random.Next(5) == 3)
            {
                gamename = "CREEPER";
                Theybecreepen = true;
            }
            else
            {
                gamename = "ZOMBIE";
                Theybecreepen = false;
            }
            minute = random.Next(5, 11);
            seconds = 60;
            timer.Elapsed += new ElapsedEventHandler(TIMERCORE);
            timer.Enabled = true;
            INFECTEDLEVEL = p.level;
            Player.GlobalMessage(gamename + " INFECTION HAS STARTED ON MAP " + p.level.name);
            //Gets player in the map
            Player.players.ForEach(delegate(Player player)
            {
                if (player.level == p.level)
                {
                    People.Add(player);
                    player.Extras.PutString(_oldtitle, player.title);
                    player.title = "Playing";
                    //player.SendPos(0xff, 3682, 4179, 3303, player.rot[0], 0);
                }
            });
            //wait, theres less than 3 players?
            if (People.Count <= 3)
            {
                Player.SendMessage(p, "You can't play with only " + People.Count);
                Player.SendMessage(p, "Ending the game...");
                END();
                return;
            }
            //Count down...
            Player.GlobalMessageLevel(p.level, "%4Choosing in...");
            int time = 10;
            while (time > 0)
            {
                Player.GlobalMessageLevel(p.level, "%4" + time);
                time--;
                Thread.Sleep(1000);
            }
            //Enable Defense TNT
            Server.AllowTNT = true;
            //declaring stuff...
            int firstinfect = random.Next(People.Count);
            //pick someone
            Player infectedPlayer = People[firstinfect];
            Player.GlobalMessage("%4" + infectedPlayer.name + " is infected RUN AWAY!!!");
            infectedPlayer.Extras.PutString(_oldname, infectedPlayer.name);
            Player.GlobalDie(infectedPlayer, true);
            infectedPlayer.name = "zombie";
            Thread.Sleep(5);
            Player.GlobalSpawn(infectedPlayer, infectedPlayer.pos[0], infectedPlayer.pos[1], infectedPlayer.pos[2], infectedPlayer.rot[0], infectedPlayer.rot[1], false);
            infect.Add(infectedPlayer);
            People.Remove(infectedPlayer);
            Thread.Sleep(500);
            //Start the infection timer
            Infect.Elapsed += new ElapsedEventHandler(INFECTCORE);
            Infect.Enabled = true;
        }
        #endregion
        #region TIMECORE
        public static void TIMERCORE(object sender, ElapsedEventArgs e)
        {
            seconds--;
            if (minute <= 0 && seconds <= 0) { Infect.Enabled = false; END(); return; }
            else if (seconds <= 0) { seconds = 60; minute--; Player.GlobalMessage("There is " + minute + ":" + seconds + " left in the infection game"); }
            time1 = "" + minute + ":" + seconds;
        }
        public static void SaveInfo()
        {
            Server.Time = "%2There is " + time1 + " minutes remaining in this round";
        }
        #endregion
        #region INFECTCORE
        public static void INFECTCORE(object sender, ElapsedEventArgs e)
        {
            //Whats the infected name?
            string name;
            if (Theybecreepen == true)
            {
                name = "creeper";
            }
            else
            {
                name = "zombie";
            }
            //Everyone whos infected
            infect.ForEach(delegate(Player player1)
            {
                //Everyone on the server
                Player.players.ForEach(delegate(Player player2)
                {
                    //if the player is the same as the infected player and the player's name doesnt = an infected name, and the player is playing and the player is touching an ifected player
                    if (player2.level == player1.level && player2.name != name && player2.title == "Playing" && Math.Abs((player2.pos[0] / 32) - (player1.pos[0] / 32)) <= 1 && Math.Abs((player2.pos[1] / 32) - (player1.pos[1] / 32)) <= 1 && Math.Abs((player2.pos[2] / 32) - (player1.pos[2] / 32)) <= 1)
                    {
                        //if its creeper infetion
                        if (Theybecreepen == true)
                        {
                            //KABOOM
                            player1.level.MakeExplosion((ushort)(player1.pos[0] / 32), (ushort)(player1.pos[1] / 32), (ushort)(player1.pos[2] / 32), 1);
                        }
                        //Send message
                        Player.GlobalMessage(player2.color + player2.name + " %2WAS EATED BY " + player1.color + player1.Extras.GetString(_oldname));
                        //Give some advice
                        Player.SendMessage(player2, "&fYou can kill human " + name + " by placing tnt blocks.");
                        //Take away his lives
                        int p2lives = player2.Extras.GetInt(_lives, 10);
                        p2lives--;
                        player2.Extras.PutInt(_lives, p2lives);
                        //Spawn
                        Command.all.Find("spawn").Use(player2, "");
                        //Alert of him of how many lives he has left
                        Player.SendMessage(player2, "%2You have " + p2lives + " live(s) left!");
                        //O NOES HES DEAD
                        if (p2lives == 0)
                        {
                            //Tell the public
                            Player.GlobalMessage("%4" + player2.name + " is infected!!");
                            //Save his name
                            player2.Extras.PutString(_oldname, player2.name);
                            //KILL HIM
                            Player.GlobalDie(player2, true);
                            //Give him a new name
                            player2.name = name;
                            //Pause...(Just cause)
                            Thread.Sleep(5);
                            //Bring back to life
                            Player.GlobalSpawn(player2, player2.pos[0], player2.pos[1], player2.pos[2], player2.rot[0], player2.rot[1], false);
                            //O NOES HES INFECTED!!!
                            infect.Add(player2);
                            //Ok lets make him invincible
                            player2.invincible = true;
                            //And let the server know hes infected (Just because it says creeper doesnt mean it means he's a creeper >.>)
                            player2.Extras.PutBoolean(_creeper, true);
                        }
                    }
                    //IS THIS PERSON ON NO TEAM!?!?!?!
                    else if (player2.title != "Playing" && player2.name != name && player2.level == INFECTEDLEVEL)
                    {
                        //Lets put him on the good team
                        People.Add(player2);
                        player2.Extras.PutString(_oldtitle, player2.title);
                        player2.title = "Playing";
                    }
                });
            });
            //Lets see if someone got pass my dead code
            //**Might not be needed**
            Player.players.ForEach(delegate(Player tester)
            {
                if (tester.Extras.GetInt("CmdInfection_lives", 10) <= 0 && tester.name != name)
                {
                    People.Remove(tester);
                    Player.GlobalMessage("%4" + tester.name + " is infected!!");
                    tester.Extras.PutString(_oldname, tester.name);
                    Player.GlobalDie(tester, true);
                    tester.name = name;

                    Thread.Sleep(5);

                    Player.GlobalSpawn(tester, tester.pos[0], tester.pos[1], tester.pos[2], tester.rot[0], tester.rot[1], false);
                    infect.Add(tester);
                    tester.invincible = true;

                    Thread.Sleep(500);

                    tester.Extras.PutBoolean(_creeper, true);
                }
            });
            //IS EVERYONE DEAD?!?!
            if (People.Count == 1)
            {
                Infect.Enabled = false;
                END();
            }
        }
        #endregion
        #region END
        public static void END()
        {
            //yay the game is over
            Player.GlobalMessage("The infection game ended!");
            //Who won??
            Player.GlobalMessage("The winners are");
            Player.players.ForEach(delegate(Player p)
            {
                if (p.title == "Playing")
                    Player.GlobalMessage(p.color + p.Extras.GetString(_oldname));
                Thread.Sleep(500);
            });
            //ok lets reset everything...
            minute = 0;
            seconds = 0;
            timer.Enabled = false;
            Infect.Enabled = false;
            Theybecreepen = false;
            infect.ForEach(delegate(Player p) { PlayerReset(p, true); });
            infect.Clear();
            People.ForEach(delegate(Player p) { PlayerReset(p, false); });
            People.Clear();
        }
        #endregion
        private static void PlayerReset(Player player1, bool resetname)
        {
            if (resetname)
                player1.name = player1.Extras.GetString(_oldname, player1.name);
            player1.title = player1.Extras.GetString(_oldtitle, player1.title);
            player1.Extras.PutBoolean(_creeper, false);
            player1.Extras.PutInt(_lives, 10);
        }
        #region TNT EXPLODE
        public static void Death(Player killer, Player zombie, ushort x, ushort y, ushort z)
        {
            Player.GlobalMessage(killer.color + killer.name + Server.DefaultColor + " EXPLODED " + zombie.color + zombie.Extras.GetString(_oldname));
            Command.all.Find("spawn").Use(zombie, "");
            killer.level.MakeExplosion(x, y, z, 1);
        }
        #endregion
        #region help
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/infection - Play infection on the current map you are on");
        }
        #endregion
    }
}