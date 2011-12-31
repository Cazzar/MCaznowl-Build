using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class OnLevelUnloadEvent
    {
        internal static List<OnLevelUnloadEvent> events = new List<OnLevelUnloadEvent>();
        Plugin plugin;
        Level.OnLevelUnload method;
        Priority priority;
        internal OnLevelUnloadEvent(Level.OnLevelUnload method, Priority priority, Plugin plugin) { this.plugin = plugin; this.priority = priority; this.method = method; }
        internal static void Call(Level l)
        {
            events.ForEach(delegate(OnLevelUnloadEvent p1)
            {
                try
                {
                    p1.method(l);
                }
                catch (Exception e) { Server.s.Log("The plugin " + p1.plugin.name + " errored when calling the LevelUnload Event!"); Server.ErrorLog(e); }
            });
        }
        static void Organize()
        {
            List<OnLevelUnloadEvent> temp = new List<OnLevelUnloadEvent>();
            List<OnLevelUnloadEvent> temp2 = events;
            OnLevelUnloadEvent temp3 = null;
            int i = 0;
            int ii = temp2.Count;
            while (i < ii)
            {
                foreach (OnLevelUnloadEvent p in temp2)
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
        public static OnLevelUnloadEvent Find(Plugin plugin)
        {
            foreach (OnLevelUnloadEvent p in events.ToArray())
            {
                if (p.plugin == plugin)
                    return p;
            }
            return null;
        }
        public static void Register(Level.OnLevelUnload method, Priority priority, Plugin plugin)
        {
            if (Find(plugin) != null)
                throw new Exception("The user tried to register 2 of the same event!");
            events.Add(new OnLevelUnloadEvent(method, priority, plugin));
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
