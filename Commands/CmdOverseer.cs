using System;
using System.IO;
using MySql.Data.MySqlClient;
namespace MCForge
{
    public class CmdOverseer : Command
    {
        public override string name { get { return "overseer"; } }
        public override string shortcut { get { return "os"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdOverseer() { }
        public override void Use(Player p, string message)
        {

            if (message == "") { Help(p); return; }
            Player who = Player.Find(message.Split(' ')[0]);
            string cmd = message.Split(' ')[0].ToUpper();

            /* This part can be optimized, but will do for now with limited C# skills */
            string par;
            try
            { par = message.Split(' ')[1].ToUpper(); }
            catch
            { par = ""; }

            string par2;
            try
            { par2 = message.Split(' ')[2]; }
            catch
            { par2 = ""; }

            string par3;
            try
            { par3 = message.Split(' ')[3]; }
            catch
            { par3 = ""; }

            // Go to Home Map (/OS GO or /OS X (inside Mac joke))
            if ((cmd == "GO") || (cmd == "X"))
            {
                Command.all.Find("goto").Use(p, properMapName(p, false));
            }
            // Set Spawn (if you are on your own map level)
            else if (cmd == "SPAWN")
            {
                if ( (p.name.ToUpper() == p.level.name.ToUpper()) || (p.name.ToUpper() + "00" == p.level.name.ToUpper()))
                {
                    Command.all.Find("setspawn").Use(p, "");
                }
                else { Player.SendMessage(p, "You can only change the Spawn Point when you are on your own map."); }
            }
            // Map Commands
            else if (cmd == "MAP")
            {
                // Add Map (/OS MAP ADD [type])
                if (par == "ADD")
                {
                    // Add User Map (if it doesn't already exist)
                    if ((File.Exists(@"levels\" + p.name + ".lvl")) || (File.Exists(@"levels\" + p.name + "00.lvl")))
                    {
                        Player.SendMessage(p, "I am sorry " + p.name + ", but you already have a map.");
                    }
                    else
                    {
                        string mType;
                        if (par2.ToUpper() == "" || par2.ToUpper() == "DESERT" || par2.ToUpper() == "FLAT" || par2.ToUpper() == "FOREST" || par2.ToUpper() == "ISLAND" || par2.ToUpper() == "MOUNTAINS" || par2.ToUpper() == "OCEAN" || par2.ToUpper() == "PIXEL")
                        {
                            if (par2 != "")
                            {
                                mType = par2;
                            }
                            else
                            {
                                mType = "flat";
                            }
                            Player.SendMessage(p, "Creating your map, " + p.name);
                            Command.all.Find("newlvl").Use(p, p.name + "00 " + mSize(p) + " " + mType);
                        }
                        else
                        {
                            Player.SendMessage(p, "A wrong map type was specified.");
                        }
                    }
                }
                // Delete your map
                else if (par == "DELETE")
                {
                    Command.all.Find("unload").Use(p, properMapName(p, false)); // Unload
                    Command.all.Find("deletelvl").Use(p, properMapName(p, false)); // Delete
                    Player.SendMessage(p, "Your map has been removed.");
                }
                else
                {
                    Player.SendMessage(p, "/overseer map add [type - default is flat] -- Creates your map");
                    Player.SendMessage(p, "/overseer map delete -- Deletes your map");
                    Player.SendMessage(p, "  Map Types: desert, flat, forest, island, mountains, ocean, pixel");
                }

            }
            else if (cmd == "ZONE")
            {
                // List zones on a single block
                if (par == "LIST")
                {
                    Command.all.Find("zone").Use(p, "");
                }
                // Add Zone to your personal map
                else if (par == "ADD")
                {
                    if (par2 != "")
                    {
                        //if (p.name.ToUpper() == p.level.name.ToUpper()) { Command.all.Find("home").Use(p, ""); }
                        Command.all.Find("unload").Use(p, properMapName(p, false));

                        if (runSQL(p, "INSERT INTO zone" + properMapName(p, false) + " (SmallX, SmallY, SmallZ, BigX, BigY, BigZ, Owner) VALUES (0,0,0," + (p.level.width - 1) + "," + (p.level.depth - 1) + "," + (p.level.height - 1) + ",'" + par2 + "');") == true)
                        {
                            //Command.all.Find("unload").Use(p, p.name);
                            Player.SendMessage(p, par2 + " has been allowed building on your map.");
                        }
                        else { Player.SendMessage(p, "Unable to zone your map."); }

                    }
                    else
                    {
                        // Missing a player name!
                        Player.SendMessage(p, "You did not specify a name to be added to your map.");
                    }
                }
                else if (par == "DELETE")
                {
                    // Delete zone
                    if (par2.ToUpper() == "ALL")
                    {
                        // Delete ALL zones
                        Command.all.Find("unload").Use(p, properMapName(p, false));

                        if (runSQL(p, "DELETE FROM zone" + properMapName(p, false)) == true)
                        {
                            Player.SendMessage(p, "All zones have been deleted. Remember to zone yourself or your map will become a public map.");
                        }
                        else
                        {
                            Player.SendMessage(p, "Unable to unzone for everyone.");
                        }
                    }
                    else if (par2 != "")
                    {
                        // Delete a single Zone
                        if (runSQL(p, "DELETE FROM zone" + properMapName(p, false) + " WHERE owner = '" + par2 + "'") == true)
                        {
                            Player.SendMessage(p, par2 + " has been allowing building on your map.");
                        }
                        else
                        {
                            Player.SendMessage(p, "Unable to unzone for " + par2 + ".");
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "Please specify a name (or ALL) to delete a zone.");
                    }
                }
                else
                {
                    // Unknown Zone Request
                    Player.SendMessage(p, "/overseer ZONE add [playername] -- Add zone to player");
                    Player.SendMessage(p, "/overseer ZONE delete [playername] -- Deletes a zone");
                    Player.SendMessage(p, "/overseer ZONE delete [all] -- Deletes all zones");
                    Player.SendMessage(p, "/overseer ZONE list -- show active zones on brick");
                }
            }
/* Spoof Commands, just for fun - can be left out :) */
            else if (cmd == "MAKEMEOP")
            {
                Player.SendMessage(p, "Come on, you didn't really think that would work? :)");
            } else
            {
                //Player.SendMessage(p, "I did not understand your request.");
                Help(p);
            }
        }

        public override void Help(Player p)
        {
            // Remember to include or exclude the spoof command(s) -- MakeMeOp
            Player.SendMessage(p, "/overseer [command string] - sends command to The Overseer");
            Player.SendMessage(p, "Accepted Commands:");
            Player.SendMessage(p, "  Go, Map, Spawn, Vote, Zone, MakeMeOP");
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
                catch(Exception excp)
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

        public string properMapName(Player p, bool Ext)
        {
            /* Returns the proper name of the User Level. By default the User Level will be named
             * "UserName00" but was earlier named "UserName". Therefore the Script checks if the old
             * map name exists before trying the new (and correct) name. All Operations will work with
             * both map names (UserName and UserName00)
             */
            string r = "";
            if (File.Exists(Directory.GetCurrentDirectory() + "\\levels\\" + p.name + "00.lvl"))
            {
                r = p.name + "00";
            }
            else
            {
                r = p.name;
            }
            if (Ext == true) { r = r + ".lvl"; }
            return r;
        }

        public string mSize(Player p)
        {
            // Later this map size may differ depending on the rank
            return "128 64 128";
        }
    }
}
