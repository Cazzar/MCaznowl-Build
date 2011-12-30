using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class OnPlayerDeathEvent
    {
        internal static List<OnPlayerDeathEvent> events = new List<OnPlayerDeathEvent>();
        Plugin plugin;
        Player.OnPlayerDeath method;
        Priority priority;
        internal OnPlayerDeathEvent(Player.OnPlayerDeath method, Priority priority, Plugin plugin) { this.plugin = plugin; this.priority = priority; this.method = method; }
        internal static void Call(Player p, byte type)
        {
            events.ForEach(delegate(OnPlayerDeathEvent p1)
            {
                try
                {
                    p1.method(p, type);
                }
                catch (Exception e) { Server.s.Log("The plugin " + p1.plugin.name + " errored when calling the PlayerDeath Event!"); Server.ErrorLog(e); }
            });
        }
        static void Organize()
        {
            List<OnPlayerDeathEvent> temp = new List<OnPlayerDeathEvent>();
            List<OnPlayerDeathEvent> temp2 = events;
            OnPlayerDeathEvent temp3 = null;
            int i = 0;
            int ii = temp2.Count;
            while (i < ii)
            {
                foreach (OnPlayerDeathEvent p in temp2)
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
        public static OnPlayerDeathEvent Find(Plugin plugin)
        {
            foreach (OnPlayerDeathEvent p in events.ToArray())
            {
                if (p.plugin == plugin)
                    return p;
            }
            return null;
        }
        public static void Register(Player.OnPlayerDeath method, Priority priority, Plugin plugin)
        {
            if (Find(plugin) != null)
                throw new Exception("The user tried to register 2 of the same event!");
            events.Add(new OnPlayerDeathEvent(method, priority, plugin));
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
