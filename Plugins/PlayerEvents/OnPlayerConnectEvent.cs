﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class OnPlayerConnectEvent
    {
        internal static List<OnPlayerConnectEvent> events = new List<OnPlayerConnectEvent>();
        Plugin plugin;
        Player.OnPlayerConnect method;
        Priority priority;
        internal OnPlayerConnectEvent(Player.OnPlayerConnect method, Priority priority, Plugin plugin) { this.plugin = plugin; this.priority = priority; this.method = method; }
        internal static void Call(Player p)
        {
            events.ForEach(delegate(OnPlayerConnectEvent p1)
            {
                try
                {
                    p1.method(p);
                }
                catch (Exception e) { Server.s.Log("The plugin " + p1.plugin.name + " errored when calling the PlayerConnect Event!"); Server.ErrorLog(e); }
            });
        }
        static void Organize()
        {
            List<OnPlayerConnectEvent> temp = new List<OnPlayerConnectEvent>();
            List<OnPlayerConnectEvent> temp2 = events;
            OnPlayerConnectEvent temp3 = null;
            int i = 0;
            int ii = temp2.Count;
            while (i < ii)
            {
                foreach (OnPlayerConnectEvent p in temp2)
                {
                    if (temp3 == null)
                        temp3 = p;
                    else if (temp3.priority < p.priority)
                        temp3 = p;
                }
                temp.Add(temp3);
                temp2.Remove(temp3);
                temp3 = null;
                i++;
            }
            events = temp;
        }
        public static OnPlayerConnectEvent Find(Plugin plugin)
        {
            foreach (OnPlayerConnectEvent p in events.ToArray())
            {
                if (p.plugin == plugin)
                    return p;
            }
            return null;
        }
        public static void Register(Player.OnPlayerConnect method, Priority priority, Plugin plugin)
        {
            if (Find(plugin) != null)
                throw new Exception("The user tried to register 2 of the same event!");
            events.Add(new OnPlayerConnectEvent(method, priority, plugin));
            Organize();
        }
        public static void UnRegister(Plugin plugin)
        {
            if (Find(plugin) == null)
                throw new Exception("This plugin doesnt have this event registered!");
            else
                events.Remove(Find(plugin));
        }
    }
}
