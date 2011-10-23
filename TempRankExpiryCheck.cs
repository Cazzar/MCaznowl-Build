/* by BeMacized, not a developer */


﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Timers;
using MCForge;


public static class TempRankExpiryCheck
{
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
        Thread.Sleep(60000);
        TRExpiryCheck();
	}
}
