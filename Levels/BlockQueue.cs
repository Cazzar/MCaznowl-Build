using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public static class BlockQueue
    {
        public static int time { get { return (int)blocktimer.Interval; } set { blocktimer.Interval = value; } }
        public static int blockupdates = 200;
        static List<block> blocks = new List<block>();
        static block b = new block();
        static System.Timers.Timer blocktimer = new System.Timers.Timer(100);

        public static void Start()
        {
            blocktimer.Elapsed += delegate
            {
                try
                {
                    if (blocks.Count < 1) 
                        return;
                    int count;
                    if (blocks.Count < blockupdates) count = blocks.Count;
                    else count = blockupdates;

                    for (int c = 0; c < count; c++)
                    {
                        blocks[c].p.level.Blockchange(blocks[c].p, blocks[c].x, blocks[c].y, blocks[c].z, blocks[c].type);
                    }
                    lock (blocks)
                    {
                        blocks.RemoveRange(0, count);
                    }
                }
                catch (Exception e)
                {
                    Server.s.ErrorCase("error:" + e);
                    Server.s.Log("Block cache failed. " + blocks.Count + "lost.");
                    blocks.Clear();
                }
            };
            blocktimer.Start();
        }

        public static void Addblock(Player P, ushort X, ushort Y, ushort Z, byte Type)
        {
            b.x = X; b.y = Y; b.z = Z; b.type = Type; b.p = P;
            blocks.Add(b);
        }

        struct block { public Player p; public ushort x; public ushort y; public ushort z; public byte type; }
    }
}
