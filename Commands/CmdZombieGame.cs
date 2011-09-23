/*
	Copyright 2011 MCForge
    Created by Snowl (David D.) and Cazzar (Cayde D.)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
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
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Threading;

namespace MCForge
{
    public class CmdZombieGame : Command
    {
        public override string name { get { return "zombiegame"; } }
        public override string shortcut { get { return "zg"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        //Variables
        public static List<Player> players = new List<Player>();
        public static List<Player> infect = new List<Player>();
        public static bool playersWon = false;
        public static int timeMinute = 0;
        public static int timeSecond = 0;
        public static System.Timers.Timer timer;
        public static Player p = null;
        Random randomhalf = new Random();
        public static int half = 0;
        public static int playersInfected = 1;
        public static int amountOfRounds = 0;
        public static bool infiniteRounds = false;

        public void ZombieMob(Object person)
        {
            int cut = 1;
            while (cut == 1 && (Server.ZombieModeOn == true))
            {
                MyLocation:
                playersInfected = 1;
                zombieGame();
                //changeLevel();
                if (!infiniteRounds)
                {
                    if (amountOfRounds != cut)
                        amountOfRounds--;
                    else
                    {
                        cut = 2;
                        return;
                    }
                }
                if (Server.ZombieModeOn == true && cut == 1)
                {
                    goto MyLocation;
                }
            }
        }

        public void zombieGame()
        {
        StartGame:

            Player.GlobalMessage("%4Round Start:%f 2:00");
            Thread.Sleep(60000);
            if (!Server.ZombieModeOn) { return; }
            Player.GlobalMessage("%4Round Start:%f 1:00");
            Thread.Sleep(55000);
            if (!Server.ZombieModeOn) { return; }
            Player.GlobalMessage("%4Round Start:%f 5");
            Thread.Sleep(1000);
            if (!Server.ZombieModeOn) { return; }
            Player.GlobalMessage("%4Round Start:%f 4");
            Thread.Sleep(1000);
            if (!Server.ZombieModeOn) { return; }
            Player.GlobalMessage("%4Round Start:%f 3");
            Thread.Sleep(1000);
            if (!Server.ZombieModeOn) { return; }
            Player.GlobalMessage("%4Round Start:%f 2");
            Thread.Sleep(1000);
            if (!Server.ZombieModeOn) { return; }
            Player.GlobalMessage("%4Round Start:%f 1");
            Thread.Sleep(1000);
            playersWon = false;
            infect.Clear();
            players.Clear();
            //getting player list
            int playerscount = 0;
            int playerscountminusref = 0;

            if (!Server.ZombieModeOn) { return; }

            try
            {
                foreach (Player player in Player.players)
                {
                    playerscount++;
                    if (player.referee)
                    {
                        player.color = player.group.color;
                    }
                    else
                    {
                        players.Add(player);
                        playerscountminusref++;
                    }
                }
            }
            catch
            {
                Player.SendMessage(null, "ERROR: Syntax");
            }

            while (playerscountminusref < 2)
            {
                Player.GlobalMessage("ERROR: Need more than 2 players to play");
                Server.PlayedRounds++;
                playerscountminusref = 0;
                goto StartGame;
            }
            int humans = players.Count;
            Random random = new Random();
            int firstinfect = random.Next(humans);
            if (Server.queZombie == true)
            {
                Player player = Player.Find(Server.nextZombie);
                player.infected = true;
                players.Remove(player);
                Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " started the infection!");
                player.color = c.red;
                Player.GlobalDie(player, false);
                Player.GlobalSpawn(player, player.pos[0], player.pos[1], player.pos[2], player.rot[0], player.rot[1], false);
                infect.Add(player);
                Server.queZombie = false;
                Server.nextZombie = "";
            }
            else
            {
                Player player = players[firstinfect];
                Player.GlobalMessage(player.color + player.name + Server.DefaultColor + " started the infection!");
                player.infected = true;
                players.Remove(players[firstinfect]);
                player.color = c.red;
                Player.GlobalDie(player, false);
                Player.GlobalSpawn(player, player.pos[0], player.pos[1], player.pos[2], player.rot[0], player.rot[1], false);
                infect.Add(player);
            }

            string[] array = new string[] { " WIKIWOO'D ", " stuck their teeth into ", " licked ", " danubed ", " made ", " tripped ", " made some zombie babies with ", " made ", " tweeted ", " made ", " infected ", " iDotted ", "", "" };
            string[] array2 = new string[] { "", "", "'s brain", "", " meet their maker", "", "", " see the dark side", "", " open source", "", "", " got nommed on", " got $nameifyed by $name!" };
            Server.infection = true;
            timer = new System.Timers.Timer(420000);
            timeMinute = DateTime.Now.Minute + 7;
            timer.Elapsed += new ElapsedEventHandler(timer3);
            timer.Enabled = true;
            while (players.Count > 0)
            {
                if (!Server.ZombieModeOn)
                {
                    infect.ForEach(delegate(Player infected)
                    {
                        infected.infected = false;
                        infected.color = infected.group.color;
                        Player.GlobalDie(infected, false);
                        Player.GlobalSpawn(infected, infected.pos[0], infected.pos[1], infected.pos[2], infected.rot[0], infected.rot[1], false);
                    });
                    infect.Clear();
                    players.Clear();
                    return;
                }

                infect.ForEach(delegate(Player player1)
                {
                    players.ForEach(delegate(Player player2)
                    {
                        if (player2.pos[0] / 32 == player1.pos[0] / 32 || player2.pos[0] / 32 == player1.pos[0] / 32 + 1 || player2.pos[0] / 32 == player1.pos[0] / 32 - 1)
                        {
                            if (player2.pos[1] / 32 == player1.pos[1] / 32 || player2.pos[1] / 32 == player1.pos[1] / 32 - 1 || player2.pos[1] / 32 == player1.pos[1] / 32 + 1)
                            {
                                if (player2.pos[2] / 32 == player1.pos[2] / 32 || player2.pos[2] / 32 == player1.pos[2] / 32 + 1 || player2.pos[2] / 32 == player1.pos[2] / 32 - 1)
                                {
                                    if (!infect.Contains(player2) || infect.Contains(player1) && !player2.referee && !player1.referee && player1 != player2)
                                    {
                                        player2.infected = true;
                                        infect.Add(player2);
                                        players.Remove(player2);
                                        player2.blockCount = 25;
                                        if (Server.lastPlayerToInfect == player1.name)
                                        {
                                            Server.infectCombo++;
                                            if (Server.infectCombo >= 2)
                                            {
                                                player1.SendMessage("You gained " + (4 - Server.infectCombo) + " " + Server.moneys);
                                                player1.money = player1.money + 4 - Server.infectCombo;
                                                Player.GlobalMessage(player1.color + player1.name + " is on a rampage! " + (Server.infectCombo + 1) + " infections in a row!");
                                            }
                                        }
                                        else
                                        {
                                            Server.infectCombo = 0;
                                        }
                                        Server.lastPlayerToInfect = player1.name;
                                        player1.infectThisRound++;
                                        int cazzar = random.Next(0, 14);
                                        if (array2[cazzar] == "")
                                        {
                                            Player.GlobalMessage(c.red + player1.Original + c.yellow + array[cazzar] + c.red + player2.name);
                                        }
                                        else if (array[cazzar] == "")
                                        {
                                            Player.GlobalMessage(c.red + player2.name + c.yellow + array2[cazzar]);
                                        }
                                        else
                                        {
                                            Player.GlobalMessage(c.red + player1.Original + c.yellow + array[cazzar] + c.red + player2.name + c.yellow + array2[cazzar]);
                                        }
                                        player2.color = c.red;
                                        player2.playersInfected = playersInfected;
                                        playersInfected++;
                                        Player.GlobalDie(player2, false);
                                        Player.GlobalSpawn(player2, player2.pos[0], player2.pos[1], player2.pos[2], player2.rot[0], player2.rot[1], false);
                                        Thread.Sleep(500);
                                    }
                                }
                            }
                        }
                    });
                });
                Thread.Sleep(500);
            }

            if (players.Count == 0 && !playersWon)
            {
                timer.Enabled = false;
                Player.GlobalMessage(c.red + "The game has ended!");
                Player.GlobalMessage(c.maroon + "Zombie have won this round.");
                string playersString = "";
                infect.ForEach(delegate(Player winners)
                {
                    winners.infected = false;
                    winners.infectThisRound = 0;
                    winners.blockCount = 50;
                    playersString += winners.group.color + winners.name + c.white + ", ";
                });
                Player.GlobalMessage(playersString);
                infect.ForEach(delegate(Player winners)
                {
                    Random random2 = new Random();
                    int randomInt = random2.Next(1, 5);
                    if (winners.infectThisRound > 2)
                    {
                        randomInt = randomInt * (int)Math.Round((decimal)randomInt * (decimal)winners.infectThisRound / (decimal)10);
                    }
                    Player.SendMessage(winners, c.gold + "You gained " + randomInt + " " + Server.moneys);
                    winners.blockCount = 50;
                    winners.money = winners.money + randomInt;
                    winners.color = winners.group.color;
                    Player.GlobalDie(winners, false);
                    Player.GlobalSpawn(winners, winners.pos[0], winners.pos[1], winners.pos[2], winners.rot[0], winners.rot[1], false);
                    winners.infectThisRound = 0;
                });
                foreach (Player player in Player.players)
                {
                    if (player.referee)
                    {
                        player.SendMessage("You gained one " + Server.moneys + " because you're a ref. Would you like a medal as well?");
                        player.money++;
                    }
                }
                infect.Clear();
                players.Clear();
                playersInfected = 1;
                Server.PlayedRounds++;
                return;
            }
        }

        public void changeLevel()
        {
            if (Server.ZombieModeOn == false)
                return;
            if (amountOfRounds == 1)
                return;
            Server.lastPlayerToInfect = "";
            Server.infection = false;
            try
            {
                DirectoryInfo di = new DirectoryInfo("levels/");
                FileInfo[] fi = di.GetFiles("*.lvl");
                int levelCount = 0;

                foreach (FileInfo file in fi)
                {
                    levelCount = levelCount + 1;
                }

                if (levelCount < 2)
                {
                    Player.GlobalMessage("You need more than 2 levels to enable the change levels function!");
                    return;
                }

                if (Server.queLevel == true)
                {
                    foreach (FileInfo file in fi)
                    {

                        String next = Server.nextLevel.ToLower();
                        Server.queLevel = false;
                        Server.nextLevel = "";
                        Command.all.Find("load").Use(null, next.ToLower());
                        Player.GlobalMessage("The next map has been chosen - " + c.red + next.ToLower());
                        Player.GlobalMessage("Please wait while you are transfered.");
                        String oldLevel = Server.mainLevel.name;
                        Server.mainLevel = Level.Find(next.ToLower());
                        Server.infection = false;
                        Player.players.ForEach(delegate(Player player)
                        {
                            if (player.level.name != next)
                            {
                                player.SendMessage("Going to the next map!");
                                Command.all.Find("goto").Use(player, next);
                                Thread.Sleep(1000);
                                // Sleep for a bit while they load
                                while (player.Loading) { Thread.Sleep(250); }
                            }
                        });
                        Command.all.Find("unload").Use(null, oldLevel);
                        return;

                    }
                }
                Random nextLevel = new Random();
                int level = nextLevel.Next(0, levelCount);
                int count = 0;
                Level current = Server.mainLevel;
                String level1 = "";
                String level2 = "";
                while (level1 == "")
                {
                    foreach (FileInfo file in fi)
                    {
                        string hi = file.Name.Replace(".lvl", "").ToLower();

                        if (count == level && hi != current.name.ToLower() && level2 != hi && Server.lastLevelVote1 != hi && Server.lastLevelVote2 != hi)
                        {
                            level1 = file.Name.Replace(".lvl", "").ToLower();
                            Server.lastLevelVote1 = hi;
                        }
                        else if (count == levelCount)
                        {
                            level = nextLevel.Next(0, levelCount);
                            count = 0;
                        }
                        else
                        {
                            count = count + 1;
                        }
                    }
                }
                int level5 = nextLevel.Next(0, levelCount);
                count = 0;
                while (level2 == "")
                {
                    foreach (FileInfo file in fi)
                    {
                        string hi = file.Name.Replace(".lvl", "").ToLower();

                        if (count == level5 && hi != current.name.ToLower() && level1 != hi && Server.lastLevelVote1 != hi && Server.lastLevelVote2 != hi)
                        {
                            level2 = file.Name.Replace(".lvl", "").ToLower();
                            Server.lastLevelVote2 = hi;
                        }
                        else if (count == levelCount)
                        {
                            level5 = nextLevel.Next(0, levelCount);
                            count = 0;
                        }
                        else
                        {
                            count = count + 1;
                        }
                    }
                }
                Server.votingforlevel = true;
                Server.NoLevelVotes = 0;
                Server.YesLevelVotes = 0;
                Player.GlobalMessage(" " + c.black + "Next Level Vote: " + Server.DefaultColor + level1 + " or " + level2 + " " + "(" + c.lime + "1 " + Server.DefaultColor + "/ " + c.red + "2" + Server.DefaultColor + ")");
                System.Threading.Thread.Sleep(15000);
                Server.votingforlevel = false;
                Player.players.ForEach(delegate(Player winners)
                {
                    winners.voted = false;
                });
                String nextl = "main";
                if (Server.NoLevelVotes >= Server.YesLevelVotes)
                {
                    nextl = level2;
                }
                else
                {
                    nextl = level1;
                }
                if (Server.queLevel == true)
                {
                    nextl = Server.nextLevel.ToLower();
                    Server.queLevel = false;
                    Server.nextLevel = "";
                }
                Command.all.Find("load").Use(null, nextl.ToLower());
                Player.GlobalMessage("The next map has been chosen - " + c.red + nextl.ToLower());
                Player.GlobalMessage("Please wait while you are transfered.");
                String oldLevelL = Server.mainLevel.name;
                Server.mainLevel = Level.Find(nextl.ToLower());
                Server.infection = false;
                Player.players.ForEach(delegate(Player player)
                {
                    if (player.level.name != nextl)
                    {
                        player.infected = false;
                        player.SendMessage("Going to the next map!");
                        player.color = player.group.color;
                        Player.GlobalDie(player, false);
                        Player.GlobalSpawn(player, player.pos[0], player.pos[1], player.pos[2], player.rot[0], player.rot[1], false);
                        Command.all.Find("goto").Use(player, nextl);
                        Thread.Sleep(1000);
                        // Sleep for a bit while they load
                        while (player.Loading) { Thread.Sleep(250); }
                    }
                });
                Command.all.Find("unload").Use(null, oldLevelL);
                return;
            }
            catch
            {
                Server.s.Log("An error occured");
                changeLevel();
            }
            changeLevel();
        }

        public static void timer3(object sender, ElapsedEventArgs e)
        {
            Player.GlobalMessage("%4Round End:%f 5");
            Thread.Sleep(1000);
            Player.GlobalMessage("%4Round End:%f 4");
            Thread.Sleep(1000);
            Player.GlobalMessage("%4Round End:%f 3");
            Thread.Sleep(1000);
            Player.GlobalMessage("%4Round End:%f 2");
            Thread.Sleep(1000);
            Player.GlobalMessage("%4Round End:%f 1");
            Thread.Sleep(1000);
            IEndRound();
        }

        public static void IEndRound()
        {
            Player.GlobalMessage(c.lime + "The game has ended");
            Player.GlobalMessage(c.green + "Congratulations to our survivor(s)");
            Server.PlayedRounds++;
            playersWon = false;
            timer.Enabled = false;
            string playersString = "";
            players.ForEach(delegate(Player winners)
            {
                winners.blockCount = 50;
                winners.color = winners.group.color;
                winners.infected = false;
                winners.infectThisRound = 0;
                playersString += winners.group.color + winners.name + c.white + ", ";
            });
            Player.GlobalMessage(playersString);
            players.ForEach(delegate(Player winners)
            {
                if (!winners.CheckIfInsideBlock())
                {
                    Player.GlobalDie(winners, false);
                    Player.GlobalSpawn(winners, winners.pos[0], winners.pos[1], winners.pos[2], winners.rot[0], winners.rot[1], false);
                    Random random2 = new Random();
                    int randomInt = 0;
                    if (winners.playersInfected > 5)
                    {
                        randomInt = random2.Next(1, playersInfected);
                    }
                    else
                    {
                        randomInt = random2.Next(1, 5);
                    }
                    Player.SendMessage(winners, c.gold + "You gained " + randomInt + " " + Server.moneys);
                    winners.blockCount = 50;
                    winners.playersInfected = 0;
                    winners.money = winners.money + randomInt;
                }
                else
                {
                    winners.SendMessage("You may not hide inside a block! No " +Server.moneys + " for you!");
                }
            });
            try
            {
                playersInfected = 1;
                playersWon = true;
                infect.Clear();
                players.Clear();
            }
            catch
            {
            }
            foreach (Player player in Player.players)
            {
                if (player.referee)
                {
                    player.SendMessage("You gained one " + Server.moneys + " because you're a ref. Would you like a medal as well?");
                    player.money++;
                }
                else
                {

                }
            }
            return;
        }

        public static void InfectedPlayerDC()
        {
            //This is for when the first zombie disconnects
            Random random = new Random();
            if (Server.infection && infect.Count <= 0)
            {
                int firstinfect = random.Next(players.Count);
                while (players[firstinfect].referee == true)
                {
                    if (firstinfect == players.Count)
                    {
                        firstinfect = 0;
                    }
                    else
                    {
                        firstinfect++;
                    }
                }
                Player.GlobalMessage(players[firstinfect].color + players[firstinfect].name + Server.DefaultColor + " continued the infection!");
                players[firstinfect].color = c.red;
                Player.GlobalDie(players[firstinfect], false);
                Player.GlobalSpawn(players[firstinfect], players[firstinfect].pos[0], players[firstinfect].pos[1], players[firstinfect].pos[2], players[firstinfect].rot[0], players[firstinfect].rot[1], false);
                infect.Add(players[firstinfect]);
                players.Remove(players[firstinfect]);
            }
            return;
        }

        public static bool InfectedPlayerLogin(Player p)
        {
            p.SendMessage("You have joined in the middle of a round. You are now infected!");
            p.blockCount = 50;
            Command.all.Find("logininfect").Use(p, p.name);
            return true;
        }

        public override void Use(Player theP, string message)
        {
                int num = message.Split(' ').Length;
                if (num >= 1)
                {
                    string[] strings = message.Split(' ');

                    for (int i = 0; i < num; i++)
                    {
                        strings[i] = strings[i].ToLower();
                    }

                    if (strings[0].Equals("stop"))
                    {
                        Player.GlobalMessage("Stopping Zombie Survival...");
                        Server.ZombieModeOn = false;
                        infiniteRounds = false;
                        return;
                    }
                    else if (Server.ZombieModeOn)
                    {
                        Player.GlobalMessage("Zombie Survival is currently in progress!");
                        return;
                    }
                    else if (strings[0].Equals(""))
                    {
                        Player.GlobalMessage("Starting Zombie Survival!");
                        Server.ZombieModeOn = true;
                        infiniteRounds = true;
                    }
                    else
                    {
                        string Str = strings[0];

                        double Num;

                        bool isNum = double.TryParse(Str, out Num);

                        if (isNum)
                        {
                            Player.GlobalMessage("Starting Zombie Survival for " + strings[0] + " rounds!");
                            Server.ZombieModeOn = true;
                            amountOfRounds = Convert.ToInt32(strings[0]);
                        }
                        else
                        {
                            Help(theP);
                            return;
                        }
                    }
                }
                else
                {
                    Player.GlobalMessage("Starting Zombie Survival!");
                    Server.ZombieModeOn = true;
                    infiniteRounds = true;
                }
                int number = message.Split(' ').Length;
                String[] param = message.Split(' ');
                Thread t = new Thread(ZombieMob);
                p = theP;
                t.Start(theP);
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/zombiegame - Starts Zombie Survival");
            Player.SendMessage(p, "/zombiegame x - Starts x amount of rounds of Zombie Survival [need to add]");
            Player.SendMessage(p, "/zombiegame stop - Stops the Zombie Survival game");
        }
    }
}