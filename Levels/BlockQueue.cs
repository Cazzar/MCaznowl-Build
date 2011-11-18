﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public static class BlockQueue
    {
        public static int time { get { return (int)blocktimer.Interval; } set { blocktimer.Interval = value; } }
        public static int blockupdates = 200;
        static block b = new block();
        static System.Timers.Timer blocktimer = new System.Timers.Timer(100);
        static byte started = 0;

        public static void Start()
        {
            blocktimer.Elapsed += delegate
            {
                if (started == 1) return;
                started = 1;
                Server.levels.ForEach((Level l) =>
                {
                    try
                    {
                        if (l.blockqueue.Count < 1) return;
                        int count;
                        if (l.blockqueue.Count < blockupdates || l.players.Count == 0) count = l.blockqueue.Count;
                        else count = blockupdates;

                        for (int c = 0; c < count; c++)
                        {
                            l.Blockchange(l.blockqueue[c].p, l.blockqueue[c].x, l.blockqueue[c].y, l.blockqueue[c].z, l.blockqueue[c].type);
                        }
                        l.blockqueue.RemoveRange(0, count);
                    }
                    catch (Exception e)
                    {
                        Server.s.ErrorCase("error:" + e);
                        Server.s.Log("Block cache failed for map: " + l.name + ". " + l.blockqueue.Count + "lost.");
                        l.blockqueue.Clear();
                    }
                });
                started = 0;
            };
            blocktimer.Start();
        }
        public static void pause() { blocktimer.Enabled = false; }
        public static void resume() { blocktimer.Enabled = true; }

        public static void Addblock(Player P, ushort X, ushort Y, ushort Z, byte Type)
        {
            b.x = X; b.y = Y; b.z = Z; b.type = Type; b.p = P;
            P.level.blockqueue.Add(b);
        }

        public struct block { public Player p; public ushort x; public ushort y; public ushort z; public byte type; }
    }
}
