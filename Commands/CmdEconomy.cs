using System;
using System.IO;
using MySql.Data.MySqlClient;
using System.Text;
using MCForge;
using MCForge_;
using System.Collections.Generic;

namespace MCForge
{
    public class CmdEconomy : Command
    {
        public override string name { get { return "economy"; } }
        public override string shortcut { get { return "eco"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            {
                #region - testing directories and files (.properties)...

                if (!Directory.Exists("text/Economy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!Directory.Exists("text/Economy/Buy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                # endregion
            }
            if (message == "")
            {
                Player.SendMessage(p, "&a" + "/economy stats" + "&3" + "- for help on /economystats");
                Player.SendMessage(p, "&a" + "/economy buytitle" + "&3" + "- for help on /economybuytitle");
                Player.SendMessage(p, "&a" + "/economy buymap" + "&3" + "- for help on /economybuymap");
                Player.SendMessage(p, "&a" + "/economy buyrank" + "&3" + "- for help on /economybuyrank");
                Player.SendMessage(p, "&a" + "/economy buycolor" + "&3" + "- for help on /economybuycolor");
                Player.SendMessage(p, "&a" + "/economy setup" + "&3" + "- for help on /economysetup");
            }

            else
            {
                Command.all.Find("help").Use(p, "economy" + " " + message);
            }
        }
        public override void Help(Player p)
        {

            Player.SendMessage(p, "/economy - shows help for all the economy commands");

        }
    }

	public class CmdEconomyStats : Command
	{
		public override string name { get { return "economystats"; } }
		public override string shortcut { get { return "ecostats"; } }
		public override string type { get { return "other"; } }
		public override bool museumUsable { get { return false; } }
		public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
		public override void Use(Player p, string message)
        {
            {
                #region - testing directories and files (.properties)...

                if (!Directory.Exists("text/Economy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!Directory.Exists("text/Economy/Buy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                # endregion
            }
            Player player = null;


            if (message == "")
            {
                Player.SendMessage(p, "Stats for: %4" + p.name);
                Player.SendMessage(p, "============================================================");
                Player.SendMessage(p, Server.moneys + ": &b$" + p.money);
                Player.SendMessage(p, "Blocks Modified Overall :: &b" + p.overallBlocks + "    &fSince Login :: &b" + p.loginBlocks);
            }

            else
            {
                player = Player.Find(message);

                if (player == null)
                {
                    Player.SendMessage(p, "Could not find the specified player.");
                }

                else
                {
                    Player.SendMessage(p, "Stats for: %4" + player.name);
                    Player.SendMessage(p, "============================================================");
                    Player.SendMessage(p, Server.moneys + ": &b$" + player.money);
                    Player.SendMessage(p, "Blocks Modified Overall :: &b" + player.overallBlocks + "    &fSince Login :: &b" + player.loginBlocks);
                    Player.SendMessage(p, "Blocks Modified Overall :: &b" + player.overallBlocks + "    &fSince Login :: &b" + player.loginBlocks);
					Player.SendMessage(player, "Player " + p.color + p.name + Server.DefaultColor + " has just viewed your stats.");
                }
            }
        }


		public override void Help(Player p)
		{
			Player.SendMessage(p, "/economystats - Used with /economy");
            Player.SendMessage(p, "Type /stats for your stats");
            Player.SendMessage(p, "Type /stats <playername> for other's stats");
		}
	}

    public class CmdEconomySetup : Command
    {
        public override string name { get { return "economysetup"; } }
        public override string shortcut { get { return "ecosetup"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            
#region - testing and creating directories and files (.properties)...  
              
                if (!Directory.Exists("text/Economy"))
                {
                    p.SendMessage("%4Could not locate the 'text/Economy' folder, creating one now.");
                    Directory.CreateDirectory("text/Economy");
                    Directory.CreateDirectory("text/Economy/Buy");
                    p.SendMessage("%2Adding the 'Economy' directory within 'text'!");
                    p.SendMessage("%2Adding the 'Buy' directory within 'text/Economy'!");

                }

                if (!Directory.Exists("text/Economy/Buy"))
                {
                    p.SendMessage("%4Could not locate the 'text/Economy/Buy' folder, creating one now.");
                    Directory.CreateDirectory("text/Economy/Buy");
                    p.SendMessage("%2Adding the 'Buy' directory within 'text/Economy'!");
                }

                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    p.SendMessage("%4The 'buyranks.properties' file could not be located, generating default file!");
                    StreamWriter SW;
                    SW = File.CreateText("text/Economy/Buy/buyranks.properties");
                    SW.WriteLine("!Skipable-ranks!False!");
                    SW.WriteLine("!guest!0!");
                    SW.WriteLine("!builder!10!");
                    SW.WriteLine("!advbuilder!100!");
                    SW.Close();
                    p.SendMessage("%2Adding the 'buyranks.properties' file within ");
                    p.SendMessage("%2'text/Economy/Buy'");
                    p.SendMessage("%2'buyranks.properties' file successfully created!");
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
				{
                    p.SendMessage("%4The 'buymap.properties' file could not be located, generating default file!");
                    StreamWriter SW;
                    SW = File.CreateText("text/Economy/Buy/buymap.properties");
                    SW.WriteLine("DO NOT EDIT FORMAT - ONLY EDIT THE NAMES/SIZES/PRICE");
                    #region
                    SW.WriteLine("THE MAXIMUM NUMBER OF DIFFERENT MAPS THAT YOU CAN BUY AT THE MOMENT IS 4!! ");
                    SW.WriteLine("Max #. of levels per person:");
                    SW.WriteLine("10");
                    SW.WriteLine(" ");
                    SW.WriteLine("sizename:");
                    SW.WriteLine("small");
                    SW.WriteLine("length:");
                    SW.WriteLine("64");
                    SW.WriteLine("height:");
                    SW.WriteLine("32");
                    SW.WriteLine("width:");
                    SW.WriteLine("64");
                    SW.WriteLine("price:");
                    SW.WriteLine("2500");
                    SW.WriteLine(" ");
                    SW.WriteLine("sizename:");
                    SW.WriteLine("medium");
                    SW.WriteLine("length:");
                    SW.WriteLine("128");
                    SW.WriteLine("height:");
                    SW.WriteLine("64");
                    SW.WriteLine("width:");
                    SW.WriteLine("128");
                    SW.WriteLine("price:");
                    SW.WriteLine("5000");
                    SW.WriteLine(" ");
                    SW.WriteLine("sizename:");
                    SW.WriteLine("large");
                    SW.WriteLine("length:");
                    SW.WriteLine("256");
                    SW.WriteLine("height:");
                    SW.WriteLine("128");
                    SW.WriteLine("width:");
                    SW.WriteLine("256");
                    SW.WriteLine("price:");
                    SW.WriteLine("7500");
                    SW.WriteLine(" ");
                    SW.WriteLine("sizename:");
                    SW.WriteLine("massive");
                    SW.WriteLine("length:");
                    SW.WriteLine("512");
                    SW.WriteLine("height:");
                    SW.WriteLine("256");
                    SW.WriteLine("width:");
                    SW.WriteLine("512");
                    SW.WriteLine("price:");
                    SW.WriteLine("10000");
                    SW.WriteLine(" ");
                    SW.Close();
                    #endregion
                    p.SendMessage("%2Adding the 'buymap.properties' file within ");
                    p.SendMessage("%2'text/Economy/Buy'");
                    p.SendMessage("%2'buymap.properties' file successfully created!");
                
				}
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    p.SendMessage("%4The 'buytitle.properties' file could not be located, generating default file!");
                    StreamWriter SW2;
                    SW2 = File.CreateText("text/Economy/Buy/buytitle.properties");
                    SW2.WriteLine("DO NOT EDIT FORMAT - ONLY EDIT PRICES/TRUE/FALSE!!! (also the true/false should be exactly true/false, NO CAPITALS!!)");
                    SW2.WriteLine("/EconomyBuyTitle Properties File");
                    SW2.WriteLine(" ");
                    SW2.WriteLine("perletter");
                    SW2.WriteLine("true");
                    SW2.WriteLine(" ");
                    SW2.WriteLine("startingprice");
                    SW2.WriteLine("100");
                    SW2.WriteLine(" ");
                    SW2.WriteLine("perletter");
                    SW2.WriteLine("25");
                    SW2.WriteLine(" ");
                    SW2.WriteLine("nonperletterprice");
                    SW2.WriteLine("300");
                    SW2.WriteLine(" ");
                    SW2.Close();
                    p.SendMessage("%2Adding the 'buytitle.properties' file within ");
                    p.SendMessage("%2'text/Economy/Buy'");
                    p.SendMessage("%2'buytitle.properties' file successfully created!");
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    p.SendMessage("%4The 'buycolor.properties' file could not be located, generating default file!");
                    StreamWriter SW;
                    SW = File.CreateText("text/Economy/Buy/buycolor.properties");
                    SW.WriteLine("DO NOT CHANGE FORMATTING!! ONLY CHANGE THE PRICE!!");
                    SW.WriteLine("color price:");
                    SW.WriteLine("150");
                    SW.Close();
                    p.SendMessage("%2Adding the 'buycolor.properties' file within ");
                    p.SendMessage("%2'text/Economy/Buy'");
                    p.SendMessage("%2'buycolor.properties' file successfully created!");
                }
            }

#endregion

        
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/economysetup - Sets up the files for /Economy");
        }
    }

    public class CmdEconomyBuyColor : Command
    {
        public override string name { get { return "economybuycolor"; } }
        public override string shortcut { get { return "ecobuycolor"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            {
                #region - testing directories and files (.properties)...

                if (!Directory.Exists("text/Economy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!Directory.Exists("text/Economy/Buy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                # endregion
            }

            if (message == "") { Help(p); return; }
            else
            {
                //Load our text file

                TextReader tr = new StreamReader("text/Economy/Buy/buycolor.properties");

                //How many lines should be loaded?

                int NumberOfLines = 5;

                //Make our array for each line

                string[] ListLines = new string[NumberOfLines];

                //Read the number of lines and put them in the array

                for (int i = 1; i < NumberOfLines; i++)
                {
                    ListLines[i] = tr.ReadLine();
                }
                string input = ListLines[3];
                int listlines3asint = 0;
                listlines3asint = Convert.ToInt32(input);
                tr.Close();
                {
                    if (p.money >= listlines3asint)
                    {
                        if (message.ToLower() == "BLACK" || message.ToLower() == "black")
                        {
                            p.color = "%0";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "NAVY" || message.ToLower() == "navy")
                        {
                            p.color = "%1";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "GREEN" || message.ToLower() == "green")
                        {
                            p.color = "%2";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "TEAL" || message.ToLower() == "teal")
                        {
                            p.color = "%3";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "MAROON" || message.ToLower() == "maroon")
                        {
                            p.color = "%4";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "PURPLE" || message.ToLower() == "purple")
                        {
                            p.color = "%5";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "GOLD" || message.ToLower() == "gold")
                        {
                            p.color = "%6";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "SILVER" || message.ToLower() == "silver")
                        {
                            p.color = "%7";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "GRAY" || message.ToLower() == "gray")
                        {
                            p.color = "%8";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "BLUE" || message.ToLower() == "blue")
                        {
                            p.color = "%9";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "LIME" || message.ToLower() == "lime")
                        {
                            p.color = "%a";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "AQUA" || message.ToLower() == "aqua")
                        {
                            p.color = "%b";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "RED" || message.ToLower() == "red")
                        {
                            p.color = "%c";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "PINK" || message.ToLower() == "pink")
                        {
                            p.color = "%d";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "YELLOW" || message.ToLower() == "yellow")
                        {
                            p.color = "%e";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        if (message.ToLower() == "WHITE" || message.ToLower() == "white")
                        {
                            p.color = "%f";
                            p.money = p.money - listlines3asint;
                            Player.SendMessage(p, "You have changed your color!");
                            Player.SendMessage(p, "You now have " + p.money + " left.");
                            return;
                        }
                        else
                        {
                            Player.SendMessage(p, "That is an invalid color please select from the following list.");
                            Player.SendMessage(p, "%0black %1navy %2green %3teal %4maroon %5purple %6gold %7silver %8gray %9blue %aline %baqua %cred %dpink %eyellow %fwhite");
                            return;
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "You do not have the " + listlines3asint + Server.moneys + " to use this command.");
                        return;
                    }

                }

            }
        }
        public override void Help(Player p)
        {

            {
                //Load our text file

                TextReader tr = new StreamReader("text/Economy/Buy/buycolor.properties");

                //How many lines should be loaded?

                int NumberOfLines = 3;

                //Make our array for each line

                string[] ListLines = new string[NumberOfLines];

                //Read the number of lines and put them in the array

                for (int i = 1; i < NumberOfLines; i++)
                {
                    ListLines[i] = tr.ReadLine();
                }
                string input = ListLines[3];
                int listlines3asint = 0;
                listlines3asint = Convert.ToInt32(input);

                {
                    Player.SendMessage(p, "/economybuycolor - purchases a color for " + listlines3asint + " " + Server.moneys + ".");
                    Player.SendMessage(p, "%0black %1navy %2green %3teal %4maroon %5purple %6gold %7silver %8gray %9blue %aline %baqua %cred %dpink %eyellow %fwhite");
                }

                tr.Close();
            }
        }
    }

    public class CmdEconomyBuyTitle : Command
    {
        public override string name { get { return "economybuytitle"; } }
        public override string shortcut { get { return "ecobuytitle"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            {
                #region - testing directories and files (.properties)...

                if (!Directory.Exists("text/Economy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!Directory.Exists("text/Economy/Buy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                # endregion
            }
            if (message == "") { Help(p); return; }
            else
            {
                //Load our text file

                TextReader tr = new StreamReader("text/Economy/Buy/buytitle.properties");

                //How many lines should be loaded?

                int NumberOfLines = 20;

                //Make our array for each line

                string[] ListLines = new string[NumberOfLines];

                //Read the number of lines and put them in the array

                for (int i = 1; i < NumberOfLines; i++)
                {
                    ListLines[i] = tr.ReadLine();
                }
				
				string input14 = ListLines[14];
				int listlines14asint = 0;
				listlines14asint = Convert.ToInt32(input14);
				
				string input8 = ListLines[8];
				int listlines8asint = 0;
				listlines8asint = Convert.ToInt32(input8);
				
				string input11 = ListLines[11];
				int listlines11asint = 0;
				listlines11asint = Convert.ToInt32(input11);

                {
                    if (message == "remove")
                    {
                        Command.all.Find("title").Use(p, p.name + " ");
                        Player.SendMessage(p, "You have removed your title.");
                        return;
                    }
                    
                        int msgcount = 0;
                        foreach (char c in message)
                            msgcount = msgcount + 1;

                    if (msgcount >= 17) 
                    {
                        Player.SendMessage(p, "Sorry, That title is too long - there is a limit of 17 characters!!");
                        return;
                    }
                    if (ListLines[5].Equals("False") || ListLines[5].Equals("false"))
                    {
                        if (p.money >= listlines14asint)
                        {
                            {
                                Command.all.Find("title").Use(p, p.name + " " + message);
                                p.money = p.money - listlines14asint;
                                Player.SendMessage(p, "You now have the title of " + message + " !");
                                Player.SendMessage(p, "You now have " + p.money + " left.");
                                return;
                            }
                        }
                        else
                        {
                            Player.SendMessage(p, "You do not have " + listlines14asint + " " + Server.moneys + " to use this command!");
                            return;
                        }
                    }

                    if (ListLines[5].Equals("True") || ListLines[5].Equals("true"))
                    {
                        //Character count
                        int count = 0;
                        foreach (char c in message)
                            count = count + 1;

                        //Price
                        int tprice = count * listlines11asint;
						int price = listlines8asint + tprice;

                        if (p.money >= tprice)
                        {
                            {
                                Command.all.Find("title").Use(p, p.name + " " + message);
                                p.money = p.money - price;
                                Player.SendMessage(p, "You now have the title of " + message + " !");
                                Player.SendMessage(p, "You now have " + p.money + " left.");
                                return;
                            }
                        }
                        else
                        {
                            Player.SendMessage(p, "You do not have " + price + " " + Server.moneys + " to use this command.");
                            return;
                        }
                    }

                    else
                    {
                        Player.SendMessage(p, "text/Economy/Buy/buytitle.properties is corrupt, please contact the server owner to ask to restore the file");
                    }

                }

                tr.Close();
            }
        }
        public override void Help(Player p)
        {
            //Load our text file

            TextReader tr = new StreamReader("text/Economy/Buy/buytitle.properties");

            //How many lines should be loaded?

            int NumberOfLines = 20;

            //Make our array for each line

            string[] ListLines = new string[NumberOfLines];

            //Read the number of lines and put them in the array

            for (int i = 1; i < NumberOfLines; i++)
            {
                ListLines[i] = tr.ReadLine();
            }
			
			string input8 = ListLines[8];
			int listlines8asint = 0;
			listlines8asint = Convert.ToInt32(input8);
			
			string input11 = ListLines[11];
			int listlines11asint = 0;
			listlines11asint = Convert.ToInt32(input11);
			
			string input14 = ListLines[14];
			int listlines14asint = 0;
			listlines14asint = Convert.ToInt32(input14);

            if (ListLines[5].Equals("True") || ListLines[5].Equals("true"))
            {
                Player.SendMessage(p, "/economybuytitle - buy a title for " + listlines8asint + " " + Server.moneys);
                Player.SendMessage(p, " and " + listlines11asint + " " + Server.moneys + " Per letter.");
            }
            if (ListLines[5].Equals("False") || ListLines[5].Equals("false"))
            {
                Player.SendMessage(p, "/economybuytitle - buy a title for " + listlines14asint + " " + Server.moneys  + ".");
            }

            else
            {
                Player.SendMessage(p, "text/Economy/Buy/buytitle.properties is corrupt, please contact the server owner to ask to restore the file");
                Player.SendMessage(p, "but usually this command would be used to buy a title!");
            }

        }
    }

    public class CmdEconomyBuyMap : Command
    {
        public override string name { get { return "economybuymap"; } }
        public override string shortcut { get { return "ecobuymap"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            {
                #region - testing directories and files (.properties)...

                if (!Directory.Exists("text/Economy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!Directory.Exists("text/Economy/Buy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                # endregion
            }

            if (message == "") { Help(p); return; }
            else
			{
            {
                //Load our text file

                TextReader tr = new StreamReader("text/Economy/Buy/buymap.properties");

                //How many lines should be loaded?

                int NumberOfLines = 60;

                //Make our array for each line

                string[] ListLines = new string[NumberOfLines];

                //Read the number of lines and put them in the array

                for (int i = 1; i < NumberOfLines; i++)
                {
                    ListLines[i] = tr.ReadLine();
                }
                {
						string input4 = ListLines[4];
				int listlines4asint = 0;
				listlines4asint = Convert.ToInt32(input4);
						
						string input15 = ListLines[15];
				int listlines15asint = 0;
				listlines15asint = Convert.ToInt32(input15);
						
						string input26 = ListLines[26];
				int listlines26asint = 0;
				listlines26asint = Convert.ToInt32(input26);
						
						string input37 = ListLines[37];
				int listlines37asint = 0;
				listlines37asint = Convert.ToInt32(input37);
						
						string input48 = ListLines[48];
				int listlines48asint = 0;
				listlines48asint = Convert.ToInt32(input48);
						

						
                    {
                        int levelno = 1;
                        int cont = 0;

                        while (cont == 0)
                        {

                            if (levelno < listlines4asint)
                            {

                                
                                if (!File.Exists("levels/" + p.name + "s" + "lvl" + levelno + ".lvl"))
                                {
                                    cont = 1;
                                }
                                if (File.Exists("levels/" + p.name + "s" + "lvl" + levelno + ".lvl"))
                                {
                                    Player.SendMessage(p, p.name + "s" + "lvl" + levelno + " " + "Already exists, moving onto next number (" + (levelno + 1) + ").");
                                    levelno = levelno + 1;
                                }
                            }
                            else
                            {
                                Player.SendMessage(p, "You have reached the maximum number of levels, speak to an Op for more levels");
                                return;
                            }

                        }
                        #region 
                        {
                            if (message == (ListLines[7]))
                            {

                                if (p.money >= listlines15asint)
                                {
                                    {
                                        Command.all.Find("newlvl").Use(p, p.name + "s" + "lvl" + levelno + " " + (ListLines[9]) + " " + (ListLines[11]) + " " + (ListLines[13]) + " " + "flat");
                                        p.money = p.money - listlines15asint;
                                        Command.all.Find("load").Use(p, p.name + "s" + "lvl" + levelno);

                                        {
                                            
                                                //if (p.name.ToUpper() == p.level.name.ToUpper()) { Command.all.Find("home").Use(p, ""); }
                                                Command.all.Find("unload").Use(p, p.name + "s" + "lvl" + levelno);

                                                
												
												
												if (runSQL(p, "INSERT INTO zone" + p.name + "s" + "lvl" + levelno + " (SmallX, SmallY, SmallZ, BigX, BigY, BigZ, Owner) VALUES (0,0,0," + (p.level.width - 1) + "," + (p.level.depth - 1) + "," + (p.level.height - 1) + ",'" + p.name + "');") ==(bool)true)
                                                {
                                                    //Command.all.Find("unload").Use(p, p.name);
                                                    Player.SendMessage(p, "Zoning Succesful!!" );
                                                }
                                                else { Player.SendMessage(p, "Unable to zone your map."); }

                                            
                                        }
                                        Player.SendMessage(p, "You now have your own map!");
                                        Player.SendMessage(p, "You now have " + p.money + " " + Server.moneys + " left.");
                                        return;
                                    }
                                }
                                else
                                {
                                    Player.SendMessage(p, "You do not have " + (ListLines[15]) + " " + Server.moneys + " to buy this map.");
                                    return;
                                }
                            }
                            if (message == (ListLines[18]))
                            {

                                if (p.money >= listlines26asint)
                                {
                                    {
                                        Command.all.Find("newlvl").Use(p, p.name + "s" + "lvl" + levelno + " " + (ListLines[20]) + " " + (ListLines[22]) + " " + (ListLines[24]) + " " + "flat");
                                        p.money = p.money - listlines26asint;
                                        Command.all.Find("load").Use(p, p.name + "s" + "lvl" + levelno);
                                        Command.all.Find("goto").Use(p, p.name + "s" + "lvl" + levelno);

                                        {

                                            //if (p.name.ToUpper() == p.level.name.ToUpper()) { Command.all.Find("home").Use(p, ""); }
                                            Command.all.Find("unload").Use(p, p.name + "s" + "lvl" + levelno);

                                            if (runSQL(p, "INSERT INTO zone" + p.name + "s" + "lvl" + levelno + " (SmallX, SmallY, SmallZ, BigX, BigY, BigZ, Owner) VALUES (0,0,0," + (p.level.width - 1) + "," + (p.level.depth - 1) + "," + (p.level.height - 1) + ",'" + p.name + "');") == true)
                                            {
                                                //Command.all.Find("unload").Use(p, p.name);
                                                Player.SendMessage(p, "Zoning Succesful!!");
                                            }
                                            else { Player.SendMessage(p, "Unable to zone your map."); }


                                        }

                                        Player.SendMessage(p, "You now have your own map!");
                                        Player.SendMessage(p, "You now have " + p.money + " " + Server.moneys + " left.");
                                        return;
                                    }
                                }
                                else
                                {
                                    Player.SendMessage(p, "You do not have " + listlines26asint + " " + Server.moneys + " to buy this map.");
                                    return;
                                }
                            }
                            if (message == (ListLines[29]))
                            {

                                if (p.money >= listlines37asint)
                                {
                                    {
                                        Command.all.Find("newlvl").Use(p, p.name + "s" + "lvl" + levelno + " " + (ListLines[31]) + " " + (ListLines[33]) + " " + (ListLines[35]) + " " + "flat");
                                        p.money = p.money - listlines37asint;
                                        Command.all.Find("load").Use(p, p.name + "s" + "lvl" + levelno);
                                        Command.all.Find("goto").Use(p, p.name + "s" + "lvl" + levelno);

                                        {

                                            //if (p.name.ToUpper() == p.level.name.ToUpper()) { Command.all.Find("home").Use(p, ""); }
                                            Command.all.Find("unload").Use(p, p.name + "s" + "lvl" + levelno);

                                            if (runSQL(p, "INSERT INTO zone" + p.name + "s" + "lvl" + levelno + " (SmallX, SmallY, SmallZ, BigX, BigY, BigZ, Owner) VALUES (0,0,0," + (p.level.width - 1) + "," + (p.level.depth - 1) + "," + (p.level.height - 1) + ",'" + p.name + "');") == true)
                                            {
                                                //Command.all.Find("unload").Use(p, p.name);
                                                Player.SendMessage(p, "Zoning Succesful!!");
                                            }
                                            else { Player.SendMessage(p, "Unable to zone your map."); }


                                        }

                                        Player.SendMessage(p, "You now have your own map!");
                                        Player.SendMessage(p, "You now have " + p.money + " " + Server.moneys + " left.");
                                        return;
                                    }
                                }
                                else
                                {
                                    Player.SendMessage(p, "You do not have " + (ListLines[26]) + " " + Server.moneys + " to buy this map.");
                                }
                            }
                            if (message == (ListLines[40]))
                            {

                                if (p.money >= listlines48asint)
                                {
                                    {
                                        Command.all.Find("newlvl").Use(p, p.name + "s" + "lvl" + levelno + " " + (ListLines[42]) + " " + (ListLines[44]) + " " + (ListLines[46]) + " " + "flat");
                                        p.money = p.money - listlines48asint;
                                        Command.all.Find("load").Use(p, p.name + "s" + "lvl" + levelno);
                                        Command.all.Find("goto").Use(p, p.name + "s" + "lvl" + levelno);

                                        {

                                            //if (p.name.ToUpper() == p.level.name.ToUpper()) { Command.all.Find("home").Use(p, ""); }
                                            Command.all.Find("unload").Use(p, p.name + "s" + "lvl" + levelno);

                                            if (runSQL(p, "INSERT INTO zone" + p.name + "s" + "lvl" + levelno + " (SmallX, SmallY, SmallZ, BigX, BigY, BigZ, Owner) VALUES (0,0,0," + (p.level.width - 1) + "," + (p.level.depth - 1) + "," + (p.level.height - 1) + ",'" + p.name + "');") == true)
                                            {
                                                //Command.all.Find("unload").Use(p, p.name);
                                                Player.SendMessage(p, "Zoning Succesful!!");
                                            }
                                            else { Player.SendMessage(p, "Unable to zone your map."); }


                                        }
                                        Player.SendMessage(p, "You now have your own map!");
                                        Player.SendMessage(p, "You now have " + p.money + " " + Server.moneys + " left.");
                                        return;
                                    }
                                }
                                else
                                {
                                    Player.SendMessage(p, "You do not have " + listlines48asint + " " + Server.moneys + " to buy this map.");
                                    return;
                                }
                            }
                            else
                            {
                                Player.SendMessage(p, "'" + message + "' is not a valid map size!");
                                Help(p);
                                return;

                            }





                        }
                        #endregion

                    }
			}
		}
		}
		}
	
	
			
            

        public override void Help(Player p)
		{
		                //Load our text file

                TextReader tr = new StreamReader("text/Economy/Buy/buymap.properties");

                //How many lines should be loaded?

                int NumberOfLines = 60;

                //Make our array for each line

                string[] ListLines = new string[NumberOfLines];

                //Read the number of lines and put them in the array

                for (int i = 1; i < NumberOfLines; i++)
                {
                    ListLines[i] = tr.ReadLine();
                }	
		
			{
			Player.SendMessage(p, "/economybuymap - Use this command to buy a map!");
			Player.SendMessage(p, "Sizes: ");
			Player.SendMessage(p, (ListLines[7]) + " - " + (ListLines[9]) + " * " + (ListLines[11]) + " * " + (ListLines[13]) + " for: " + (ListLines[15]) + Server.moneys);
			Player.SendMessage(p, (ListLines[18]) + " - " + (ListLines[20]) + " * " + (ListLines[22]) + " * " + (ListLines[24]) + " for: " + (ListLines[26]) + Server.moneys);
			Player.SendMessage(p, (ListLines[29]) + " - " + (ListLines[31]) + " * " + (ListLines[33]) + " * " + (ListLines[35]) + " for: " + (ListLines[37]) + Server.moneys);
			Player.SendMessage(p, (ListLines[40]) + " - " + (ListLines[42]) + " * " + (ListLines[44]) + " * " + (ListLines[46]) + " for: " + (ListLines[48]) + Server.moneys);				
			}
		}

        public bool runSQL(Player p, string strQuery)
        {
            /*
             * Very basic SQL Runner
             * 
             * Functionality exists in MCLawl/MCForge, revice to use any built-in MySQL Connection instead
             * of opening a new connection
             */
            bool res = false;

            try
            {
                MySqlConnection myConn = new MySqlConnection("user id=" + MCForge.Server.MySQLUsername + "; password=" + MCForge.Server.MySQLPassword + "; database=" + MCForge.Server.MySQLDatabaseName + "; server=" + MCForge.Server.MySQLHost);
                myConn.Open();
                try
                {
                    MySqlCommand myCommand = new MySqlCommand(strQuery, myConn);
                    myCommand.ExecuteNonQuery();
                    res = true;
                }
                catch (Exception excp)
                {
                    Player.SendMessage(p, "Unable to run SQL Query: " + excp.Message);
                }
                myConn.Close();
            }
            catch (Exception excp)
            {
                Player.SendMessage(p, "Unable to connect to MCForge: " + excp.Message);
            }
            return res;
        }

    }

    public class CmdEconomyBuyRank : Command
    {
        public override string name { get { return "economybuyrank"; } }
        public override string shortcut { get { return "ecobuyrank"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            {
                #region - testing directories and files (.properties)...

                if (!Directory.Exists("text/Economy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!Directory.Exists("text/Economy/Buy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                # endregion
            }

            string rank = message;

            String[] strArray_rankFileDetails = File.ReadAllLines("text/Economy/Buy/Buyranks.properties");

            if (strArray_rankFileDetails[0].IndexOf("false") > 0)
            {

                for (int x = 1; x < strArray_rankFileDetails.Length; x++)
                {

                    String[] strArray_rankDetails = strArray_rankFileDetails[x].Split('!');

                    if (rank == strArray_rankDetails[1])
                    {

                        Group group = Group.Find(rank);

                        if (group.Permission > p.group.Permission)
                        {

                            int y = x - 1;

                            String[] strArray_currRankDetails = strArray_rankFileDetails[y].Split('!');

                            if (strArray_currRankDetails[1] == p.group.name)
                            {

                                if (p.money >= int.Parse(strArray_rankDetails[2]))
                                {



                                    p.money = p.money - int.Parse(strArray_rankDetails[2]);

                                    p.group.playerList.Remove(p.name);

                                    p.group.playerList.Save();

                                    group.playerList.Add(p.name);

                                    group.playerList.Save();

                                    p.SendMessage(strArray_rankDetails[2].ToString() + " " + Server.moneys + " have been removed from your savings!");

                                    p.SendMessage("%3Transaction complete!");

                                    p.group = group;

                                    p.color = group.color;

                                    Player.GlobalMessage(string.Concat((string[])new string[] { p.color, p.name, Server.DefaultColor, " %3just purchased the " + group.color + group.name + " %3rank!" }));

                                    Player.GlobalMessage(string.Concat((string[])new string[] { "%3Be sure to congradulate them!" }));

                                    return;

                                }

                                else
                                {

                                    p.SendMessage("Not Enough " + Server.moneys + " to buy rank :: " + strArray_rankDetails[1]);

                                    p.SendMessage("%3You lack, %2" + (int.Parse(strArray_rankDetails[2]) - p.money) + " %3" + Server.moneys + "!");

                                    return;

                                }

                            }

                            else
                            {



                                Group nextRank = Group.Find(strArray_rankDetails[1]);

                                p.SendMessage("%3You may not skip ranks!");

                                p.SendMessage("%3Your next buyable rank is " + nextRank.color + nextRank.name + " for :: &a" + strArray_rankDetails[2] + " %3" + Server.moneys + "!");

                                return;

                            }

                        }

                        else
                        {

                            p.SendMessage("%3You cannot buy a rank that is lower or equal to that of your current!");

                        }

                    }

                    else
                    {

                        if (x == (strArray_rankFileDetails.Length - 1) && rank != strArray_rankDetails[1])
                        {

                            p.SendMessage("The rank specified does not exist!");

                        }

                    }

                }

            }

            else
            {



                for (int x = 0; x < strArray_rankFileDetails.Length; x++)
                {

                    String[] strArray_rankDetails = strArray_rankFileDetails[x].Split('!');

                    if (rank == strArray_rankDetails[1])
                    {

                        Group group = Group.Find(rank);

                        if (group.Permission > p.group.Permission)
                        {

                            if (p.money >= int.Parse(strArray_rankDetails[2]))
                            {



                                p.money = p.money - int.Parse(strArray_rankDetails[2]);

                                p.group.playerList.Remove(p.name);

                                p.group.playerList.Save();

                                group.playerList.Add(p.name);

                                group.playerList.Save();

                                p.SendMessage("%3Transaction complete! Thank you for your purchase.");

                                p.group = group;

                                p.color = group.color;

                                Player.GlobalMessage(p.color + p.name + " " + Server.DefaultColor + " just purchased the " + group.color + group.name + " " + Server.DefaultColor + " rank!");

                                return;

                            }

                            else
                            {

                                p.SendMessage("%3Insufficient " + Server.moneys + " to buy rank :: " + strArray_rankDetails[1]);

                                p.SendMessage("%3You lack, %2" + (int.Parse(strArray_rankDetails[2]) - p.money) + " %3" + Server.moneys + "!");

                                return;

                            }

                        }

                        else
                        {

                            p.SendMessage("%3You cannot buy a rank that is lower or equal to that of your current!");

                        }

                    }

                    else
                    {

                        if (x == (strArray_rankFileDetails.Length - 1) && rank != strArray_rankDetails[1])
                        {

                            p.SendMessage("The rank specified does not exist!");

                        }

                    }

                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/economybuyrank <rankname> - buy the next rank up from you!");
        }
    }

    public class CmdSetOwnMapSpawn : Command
    {
        public override string name { get { return "setownmapspawn"; } }
        public override string shortcut { get { return "onwmapspawn"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            {
                #region - testing directories and files (.properties)...

                if (!Directory.Exists("text/Economy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!Directory.Exists("text/Economy/Buy"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buyranks.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buymap.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buytitle.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                if (!File.Exists("text/Economy/Buy/buycolor.properties"))
                {
                    Player.SendMessage(p, "The Correct Files do not exist, please ask a Server op to run /economysetup to setup all the files and directories for all the /economy commands!");
                    return;
                }
                # endregion
            }

            if ((p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "1") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "2") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "3") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "4") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "5") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "6") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "7") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "8") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "9") || (p.name.ToUpper() == p.level.name.ToUpper() + "s" + "lvl" + "10"))
            {
                Command.all.Find("setspawn").Use(p, "");
                Player.SendMessage(p, "Spawn Set");
            }
            else 
            { 
                Player.SendMessage(p, "You can only change the Spawn Point when you are on your own map."); 
            }
        }
        public override void Help(Player p)
        {

            Player.SendMessage(p, "/setownmpaspawn - For setting the spawn of your own map!");

        }
    }



} //Do not delete - THE LAST '}'!!!