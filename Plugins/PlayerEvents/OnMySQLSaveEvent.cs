using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    /// <summary>
    /// This event is called whenever the server saves data to MySQL or SQLite
    /// </summary>
    public class OnMySQLSaveEvent
    {
        internal static List<OnMySQLSaveEvent> events = new List<OnMySQLSaveEvent>();
        Plugin plugin;
        Player.OnMySQLSave method;
        Priority priority;
        internal OnMySQLSaveEvent(Player.OnMySQLSave method, Priority priority, Plugin plugin) { this.plugin = plugin; this.priority = priority; this.method = method; }
        internal static void Call(Player p, string mysqlcommand)
        {
            events.ForEach(delegate(OnMySQLSaveEvent p1)
            {
                try
                {
                    p1.method(p, mysqlcommand);
                }
                catch (Exception e) { Server.s.Log("The plugin " + p1.plugin.name + " errored when calling the MySQLSave Event!"); Server.ErrorLog(e); }
            });
        }
        static void Organize()
        {
            List<OnMySQLSaveEvent> temp = new List<OnMySQLSaveEvent>();
            List<OnMySQLSaveEvent> temp2 = events;
            OnMySQLSaveEvent temp3 = null;
            int i = 0;
            int ii = temp2.Count;
            while (i < ii)
            {
                foreach (OnMySQLSaveEvent p in temp2)
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
        public static OnMySQLSaveEvent Find(Plugin plugin)
        {
            foreach (OnMySQLSaveEvent p in events.ToArray())
            {
                if (p.plugin == plugin)
                    return p;
            }
            return null;
        }
        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">This is the delegate that will get called when this event occurs</param>
        /// <param name="priority">The priority (imporantce) of this call</param>
        /// <param name="plugin">The plugin object that is registering the event</param>
        public static void Register(Player.OnMySQLSave method, Priority priority, Plugin plugin)
        {
            if (Find(plugin) != null)
                throw new Exception("The user tried to register 2 of the same event!");
            events.Add(new OnMySQLSaveEvent(method, priority, plugin));
            Organize();
        }
        /// <summary>
        /// UnRegister this event
        /// </summary>
        /// <param name="plugin">The plugin object that has this event registered</param>
        public static void UnRegister(Plugin plugin)
        {
            if (Find(plugin) == null)
                throw new Exception("This plugin doesnt have this event registered!");
            else
                events.Remove(Find(plugin));
        }
    }
}
