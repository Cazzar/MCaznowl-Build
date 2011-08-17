using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace MCForge
{
    public static class Economy
    {
        public static class Settings
        {
            public static bool Enabled = false;

            //Maps
            public static bool Levels = false;
            public static List<Level> LevelsList = new List<Level>();
            public class Level
            {
                public int price;
                public string name;
                public string x;
                public string y;
                public string z;
                public string type;
            }

            //Titles
            public static bool Titles = false;
            public static int TitlePrice = 100;

            //Colors
            public static bool Colors = false;
            public static int ColorPrice = 100;

            //Ranks
            public static bool Ranks = false;
            public static LevelPermission MaxRank = LevelPermission.AdvBuilder;
            public static int RankPrice = 100;
        }

        public static void Load()
        {
            if (!File.Exists("properties/economy.properties")) { Server.s.Log("Economy properties don't exist, creating"); File.Create("properties/economy.properties").Close(); Save(); }
            using (StreamReader r = File.OpenText("properties/economy.properties"))
            {
                string line;
                while (!r.EndOfStream)
                {
                    line = r.ReadLine().ToLower().Trim();
                    string[] linear = line.ToLower().Trim().Split(':');
                    switch (linear[0])
                    {
                        case "enabled":
                            if (linear[1] == "true") { Settings.Enabled = true; }
                            else if (linear[1] == "false") { Settings.Enabled = false; }
                            break;

                        case "title":
                            if (linear[1] == "price") { Settings.TitlePrice = int.Parse(linear[2]); }
                            if (linear[1] == "enabled")
                            {
                                if (linear[2] == "true") { Settings.Titles = true; }
                                else if (linear[2] == "false") { Settings.Titles = false; }
                            }
                            break;

                        case "color":
                            if (linear[1] == "price") { Settings.ColorPrice = int.Parse(linear[2]); }
                            if (linear[1] == "enabled")
                            {
                                if (linear[2] == "true") { Settings.Colors = true; }
                                else if (linear[2] == "false") { Settings.Colors = false; }
                            }
                            break;

                        case "rank":
                            if (linear[1] == "price") { Settings.RankPrice = int.Parse(linear[2]); }
                            if (linear[1] == "maxrank")
                            {
                                Group grp = Group.Find(linear[2]);
                                if (grp != null) { Settings.MaxRank = grp.Permission; }
                            }
                            if (linear[1] == "enabled")
                            {
                                if (linear[2] == "true") { Settings.Ranks = true; }
                                else if (linear[2] == "false") { Settings.Ranks = false; }
                            }
                            break;

                        case "level":
                            if (linear[1] == "enabled")
                            {
                                if (linear[2] == "true") { Settings.Levels = true; }
                                else if (linear[2] == "false") { Settings.Levels = false; }
                            }
                            if (linear[1] == "levels")
                            {
                                Settings.Level lvl = new Settings.Level();
                                if (FindLevel(linear[2]) != null) { lvl = FindLevel(linear[2]); Settings.LevelsList.Remove(lvl); }
                                switch (linear[3])
                                {
                                    case "name":
                                        lvl.name = linear[4];
                                        break;

                                    case "price":
                                        lvl.price = int.Parse(linear[4]);
                                        break;

                                    case "x":
                                        lvl.x = linear[4];
                                        break;

                                    case "y":
                                        lvl.y = linear[4];
                                        break;

                                    case "z":
                                        lvl.z = linear[4];
                                        break;

                                    case "type":
                                        lvl.type = linear[4];
                                        break;
                                }
                                Settings.LevelsList.Add(lvl);
                            }
                            break;
                    }
                }
                r.Close();
            }
            Save();
        }

        public static void Save()
        {
            if (!File.Exists("properties/economy.properties")) { Server.s.Log("Economy properties don't exist, creating"); }
            Thread.Sleep(2000);
            File.Delete("properties/economy.properties");
            Thread.Sleep(2000);
            using (StreamWriter w = File.CreateText("properties/economy.properties"))
            {
                //enabled
                w.WriteLine("enabled:" + Settings.Enabled.ToString());
                //title
                w.WriteLine();
                w.WriteLine("title:enabled:" + Settings.Titles.ToString());
                w.WriteLine("title:price:" + Settings.TitlePrice.ToString());
                //color
                w.WriteLine();
                w.WriteLine("color:enabled:" + Settings.Colors.ToString());
                w.WriteLine("color:price:" + Settings.ColorPrice.ToString());
                //rank
                w.WriteLine();
                w.WriteLine("rank:enabled:" + Settings.Ranks.ToString());
                w.WriteLine("rank:price:" + Settings.RankPrice.ToString());
                w.WriteLine("rank:maxrank:" + Settings.MaxRank.ToString());
                //maps
                w.WriteLine();
                w.WriteLine("level:enabled:" + Settings.Levels.ToString());
                foreach (Settings.Level lvl in Settings.LevelsList)
                {
                    w.WriteLine();
                    w.WriteLine("level:levels:" + lvl.name + ":name:" + lvl.name);
                    w.WriteLine("level:levels:" + lvl.name + ":price:" + lvl.price);
                    w.WriteLine("level:levels:" + lvl.name + ":x:" + lvl.x);
                    w.WriteLine("level:levels:" + lvl.name + ":y:" + lvl.y);
                    w.WriteLine("level:levels:" + lvl.name + ":z:" + lvl.z);
                    w.WriteLine("level:levels:" + lvl.name + ":type:" + lvl.type);
                }
                w.Close();
            }
        }

        public static Settings.Level FindLevel(string name)
        {
            Settings.Level found = null;
            foreach (Settings.Level lvl in Settings.LevelsList)
            {
                try
                {
                    if (lvl.name.ToLower() == name.ToLower())
                    {
                        found = lvl;
                    }
                }
                catch { }
            }
            return found;
        }

    }
}
