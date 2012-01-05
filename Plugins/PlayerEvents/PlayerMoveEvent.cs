using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    /// <summary>
    /// This event is called whenever a player moves
    /// </summary>
    public class PlayerMoveEvent
    {
        internal static List<PlayerMoveEvent> events = new List<PlayerMoveEvent>();
        Plugin plugin;
        Player.OnPlayerMove method;
        Priority priority;
        internal PlayerMoveEvent(Player.OnPlayerMove method, Priority priority, Plugin plugin) { this.plugin = plugin; this.priority = priority; this.method = method; }
        internal static void Call(Player p, ushort x, ushort y,  ushort z)
        {
            events.ForEach(delegate(PlayerMoveEvent p1)
            {
                try
                {
                    p1.method(p, x, y, z);
                }
                catch (Exception e) { Server.s.Log("The plugin " + p1.plugin.name + " errored when calling the PlayerMove Event!"); Server.ErrorLog(e); }
            });
        }
        static void Organize()
        {
            List<PlayerMoveEvent> temp = new List<PlayerMoveEvent>();
            List<PlayerMoveEvent> temp2 = events;
            PlayerMoveEvent temp3 = null;
            int i = 0;
            int ii = temp2.Count;
            while (i < ii)
            {
                foreach (PlayerMoveEvent p in temp2)
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
        public static PlayerMoveEvent Find(Plugin plugin)
        {
            foreach (PlayerMoveEvent p in events.ToArray())
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
        public static void Register(Player.OnPlayerMove method, Priority priority, Plugin plugin)
        {
            if (Find(plugin) != null)
                throw new Exception("The user tried to register 2 of the same event!");
            events.Add(new PlayerMoveEvent(method, priority, plugin));
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
