/* by BeMacized, not a developer */


﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Timers;
using MCForge;
using System.Threading;


public static class Checktimer
{
    static System.Timers.Timer t;
    public static void StartTimer()
    {
    t = new System.Timers.Timer();
    t.AutoReset = false;
    t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
    t.Interval = GetInterval();
    t.Start();
    }
    static double GetInterval()
    {
        DateTime now = DateTime.Now;
        return ((60 - now.Second) * 1000 - now.Millisecond);
    }

    static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        t.Interval = GetInterval();
        t.Start();

        // Voids to be executed every minute:
        TRExpiryCheck();
    }
	public static void TRExpiryCheck()
	{
        foreach (Player p in Player.players)
        {
            
            foreach (string line3 in File.ReadAllLines("text/tempranks.txt"))
            {
                if (line3.Contains(p.name))
                {
                    
                    string player = line3.Split(' ')[0];
                    int period = Convert.ToInt32(line3.Split(' ')[3]);
                    int minutes = Convert.ToInt32(line3.Split(' ')[4]);
                    int hours = Convert.ToInt32(line3.Split(' ')[5]);
                    int days = Convert.ToInt32(line3.Split(' ')[6]);
                    int months = Convert.ToInt32(line3.Split(' ')[7]);
                    int years = Convert.ToInt32(line3.Split(' ')[8]);
                    Player who = Player.Find(player);
                    DateTime ExpireDate = new DateTime(years, months, days, hours, minutes, 0);
                    DateTime tocheck = ExpireDate.AddHours(Convert.ToDouble(period));
                    DateTime tochecknow = DateTime.Now;
                    double datecompare = DateTime.Compare(tocheck, tochecknow);
                    if (datecompare <= 0)
                    {
                                                                   
                        string group = line3.Split(' ')[2];
                        Group newgroup = Group.Find(group);
                        Command.all.Find("deltemprank").Use(null, who.name);
                        
                    }
                    
                }

            }
        }
	}
}
