using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;

namespace MCForge
{
    public static class Economy
    {
        public static class Settings
        {
            public static bool Enabled/* = false*/;

            //Maps
            public static bool Levels/* = false*/;
            public static List<Level> LevelsList = new List<Level>();
            public class Level
            {
                private int _price; 

                public int price
                {
                    get
                    {
                        return _price;
                    }
                    set
                    {
                        _price = value;
                    }
                }
                private string _name; 

                public string name
                {
                    get
                    {
                        return _name;
                    }
                    set
                    {
                        _name = value;
                    }
                }
                private string _x; 

                public string x
                {
                    get
                    {
                        return _x;
                    }
                    set
                    {
                        _x = value;
                    }
                }
                private string _y; 

                public string y
                {
                    get
                    {
                        return _y;
                    }
                    set
                    {
                        _y = value;
                    }
                }
                private string _z; 

                public string z
                {
                    get
                    {
                        return _z;
                    }
                    set
                    {
                        _z = value;
                    }
                }
                private string _type; 

                public string type
                {
                    get
                    {
                        return _type;
                    }
                    set
                    {
                        _type = value;
                    }
                }
            }

            //Titles
            public static bool Titles/* = false*/;
            public static int TitlePrice = 100;

            //Colors
            public static bool Colors/* = false*/;
            public static int ColorPrice = 100;

            //Ranks
            public static bool Ranks/* = false*/;
            public static LevelPermission MaxRank = LevelPermission.AdvBuilder;
            public static List<Rank> RanksList = new List<Rank>();
            public class Rank
            {
                private Group _group; 

                public Group group
                {
                    get
                    {
                        return _group;
                    }
                    set
                    {
                        _group = value;
                    }
                }
                private int _price = 1000; 

                public int price
                {
                    get
                    {
                        return _price;
                    }
                    set
                    {
                        _price = value;
                    }
                }
            }
        }

        public static void Load()
        {
            if (!File.Exists("properties/economy.properties")) { Server.s.Log("Economy properties don't exist, creating"); File.Create("properties/economy.properties").Close(); Save(); }
            using (StreamReader r = File.OpenText("properties/economy.properties"))
            {
                string line;
                while (!r.EndOfStream)
                {
                    line = r.ReadLine().ToLower(CultureInfo.CurrentCulture).Trim();
                    string[] linear = line.ToLower(CultureInfo.CurrentCulture).Trim().Split(':');
                    try
                    {
                        switch (linear[0])
                        {
                            case "enabled":
                                if (linear[1] == "true") { Settings.Enabled = true; }
                                else if (linear[1] == "false") { Settings.Enabled = false; }
                                break;

                            case "title":
                                if (linear[1] == "price") { Settings.TitlePrice = int.Parse(linear[2], CultureInfo.CurrentCulture); }
                                if (linear[1] == "enabled")
                                {
                                    if (linear[2] == "true") { Settings.Titles = true; }
                                    else if (linear[2] == "false") { Settings.Titles = false; }
                                }
                                break;

                            case "color":
                                if (linear[1] == "price") { Settings.ColorPrice = int.Parse(linear[2], CultureInfo.CurrentCulture); }
                                if (linear[1] == "enabled")
                                {
                                    if (linear[2] == "true") { Settings.Colors = true; }
                                    else if (linear[2] == "false") { Settings.Colors = false; }
                                }
                                break;

                            case "rank":
                                if (linear[1] == "price")
                                {
                                    Economy.Settings.Rank rnk = new Economy.Settings.Rank();
                                    rnk = Economy.FindRank(linear[2]);
                                    if (rnk == null)
                                    {
                                        rnk = new Economy.Settings.Rank();
                                        rnk.group = Group.Find(linear[2]);
                                        rnk.price = int.Parse(linear[3], CultureInfo.CurrentCulture);
                                        Economy.Settings.RanksList.Add(rnk);
                                    }
                                    else
                                    {
                                        Economy.Settings.RanksList.Remove(rnk);
                                        rnk.price = int.Parse(linear[3], CultureInfo.CurrentCulture);
                                        Economy.Settings.RanksList.Add(rnk);
                                    }
                                }
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
                                            lvl.price = int.Parse(linear[4], CultureInfo.CurrentCulture);
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
                    catch { }
                }
                r.Close();
            }
            Save();
        }

        public static void Save()
        {
            if (!File.Exists("properties/economy.properties")) { Server.s.Log("Economy properties don't exist, creating"); }
            //Thread.Sleep(2000);
            File.Delete("properties/economy.properties");
            //Thread.Sleep(2000);
            using (StreamWriter w = File.CreateText("properties/economy.properties"))
            {
                //enabled
                w.WriteLine("enabled:" + Settings.Enabled);
                //title
                w.WriteLine();
                w.WriteLine("title:enabled:" + Settings.Titles);
                w.WriteLine("title:price:" + Settings.TitlePrice);
                //color
                w.WriteLine();
                w.WriteLine("color:enabled:" + Settings.Colors);
                w.WriteLine("color:price:" + Settings.ColorPrice);
                //rank
                w.WriteLine();
                w.WriteLine("rank:enabled:" + Settings.Ranks);
                w.WriteLine("rank:maxrank:" + Settings.MaxRank);
                foreach (Settings.Rank rnk in Settings.RanksList)
                {
                    w.WriteLine("rank:price:" + rnk.group.name + ":" + rnk.price);
                }
                //maps
                w.WriteLine();
                w.WriteLine("level:enabled:" + Settings.Levels);
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
                    if (lvl.name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture))
                    {
                        found = lvl;
                    }
                }
                catch { }
            }
            return found;
        }

        public static Settings.Rank FindRank(string name)
        {
            Settings.Rank found = null;
            foreach (Settings.Rank rnk in Settings.RanksList)
            {
                try
                {
                    if (rnk.group.name.ToLower(CultureInfo.CurrentCulture) == name.ToLower(CultureInfo.CurrentCulture))
                    {
                        found = rnk;
                    }
                }
                catch { }
            }
            return found;
        }

        public static Economy.Settings.Rank NextRank(Player p)
        {
            Group foundGroup = p.group;
            Group nextGroup = null; bool nextOne = false;
            for (int i = 0; i < Group.GroupList.Count; i++)
            {
                Group grp = Group.GroupList[i];
                if (nextOne)
                {
                    if (grp.Permission >= LevelPermission.Nobody) break;
                    nextGroup = grp;
                    break;
                }
                if (grp == foundGroup)
                    nextOne = true;
            }
            return Economy.FindRank(nextGroup.name);
        }

    }
}
