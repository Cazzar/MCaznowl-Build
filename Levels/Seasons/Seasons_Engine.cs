﻿using System;
using System.Threading;
using System.Collections.Generic;
namespace MCForge
{
    public class SeasonsCore
    {
        public Thread t;
        public Random random = new Random();
        public bool started = false;
        public Seasons GetSeason()
        {
            if (DateTime.Now.Month == 12 || DateTime.Now.Month == 1)
                return Season.winter;
            else if (DateTime.Now.Month == 9 || DateTime.Now.Month == 10 || DateTime.Now.Month == 11)
                return Season.fall;
            else
                return null;
        }
        public int GetStorm()
        {
            if (DateTime.Now.Month == 12)
                return 2;
            else if (DateTime.Now.Month == 1)
                return 3;
            else if (DateTime.Now.Month == 9)
                return 6;
            else if (DateTime.Now.Month == 10)
                return 6;
            else if (DateTime.Now.Month == 11)
                return 6;
            else
                return 0;
        }
        public int GetSpawn()
        {
            if (DateTime.Now.Month == 12)
                return 20;
            else if (DateTime.Now.Month == 1)
                return 10;
            else if (DateTime.Now.Month == 9)
                return 5;
            else if (DateTime.Now.Month == 10)
                return 5;
            else if (DateTime.Now.Month == 11)
                return 5;
            else
                return 0;
        }
        public SeasonsCore(Level l)
        {
            if (Server.UseSeasons)
            {
                started = true;
                if (GetSeason() != null)
                {
                    Seasons s = GetSeason();
                    s.spawn = GetStorm();
                    if (s == Season.winter)
                    {
                        Replace("grass snow", l);
                        Winter(s, l);
                    }
                }
                else
                    started = false;
            }
            else
                started = false;
        }
        public void Start(Level l)
        {
            if (Server.UseSeasons)
            {
                started = true;
                if (GetSeason() != null)
                {
                    Seasons s = GetSeason();
                    s.spawn = GetStorm();
                    if (s == Season.winter)
                    {
                        Replace("grass snow", l);
                        Winter(s, l);
                    }
                }
                else
                    started = false;
            }
            else
                started = false;
        }
        public void Replace(string message, Level mainlevel)
        {
            if (message.IndexOf(' ') == -1 || message.Split(' ').Length > 2) { return; }

            byte b1, b2;

            b1 = Block.Byte(message.Split(' ')[0]);
            b2 = Block.Byte(message.Split(' ')[1]);

            if (b1 == Block.Zero || b2 == Block.Zero) { return; }
            ushort x, y, z; int currentBlock = 0;
            List<Pos> stored = new List<Pos>(); Pos pos;

            foreach (byte b in mainlevel.blocks)
            {
                if (b == b1)
                {
                    mainlevel.IntToPos(currentBlock, out x, out y, out z);
                    pos.x = x; pos.y = y; pos.z = z;
                    stored.Add(pos);
                }
                currentBlock++;
            }

            foreach (Pos Pos in stored)
            {
                mainlevel.SetTile(Pos.x, Pos.y, Pos.z, b2);
                Player.GlobalBlockchange(mainlevel, Pos.x, Pos.y, Pos.z, b2);
            }
        }
        public struct Pos { public ushort x, y, z; }
        public void Winter(Seasons s, Level l)
        {
            t = new Thread(new ThreadStart(delegate
            {
                ushort x = l.width;
                ushort y = l.depth;
                ushort z = l.height;
                List<BlockID> list = new List<BlockID>();
                while (true)
                {
                    int spawn;
                    if (random.Next(s.spawn) == 1 || random.Next(s.spawn) == 0)
                        spawn = random.Next(1, GetSpawn());
                    else
                        spawn = 0;
                    int i = 0;
                    while (i != spawn)
                    {
                        BlockID temp = new BlockID((ushort)random.Next(x), y, (ushort)random.Next(z), s.Block, Block.fallsnow);
                        list.Add(temp);
                        l.Blockchange(temp.x, temp.y, temp.z, temp.id);
                        i++;
                    }
                    list.ForEach(delegate(BlockID current)
                    {
                        //if (current.C == random.Next(4))
                        //    current.C++;
                        int x1 = random.Next(-1, 2);
                        int z1 = random.Next(-1, 2);
                        if (l.GetTile(current.x, current.y, current.z) == Block.air)
                            list.Remove(current);
                        else if (current.C < 3)
                            current.C++;
                        else if (l.GetTile((ushort)(current.x + x1), (ushort)(current.y - 1), current.z) == Block.air && random.Next(3) == 1)
                        {
                            l.Blockchange(current.x, current.y, current.z, Block.air);
                            l.Blockchange((ushort)(current.x + x1), (ushort)(current.y - 1), current.z, current.id1);
                            current.y = (ushort)(current.y - 1);
                            current.x = (ushort)(current.x + x1);
                        }
                        else if (l.GetTile(current.x, (ushort)(current.y - 1), (ushort)(current.z + z1)) == Block.air && random.Next(3) == 1)
                        {
                            l.Blockchange(current.x, current.y, current.z, Block.air);
                            l.Blockchange(current.x, (ushort)(current.y - 1), (ushort)(current.z + z1), current.id1);
                            current.y = (ushort)(current.y - 1);
                            current.z = (ushort)(current.z + z1);
                        }
                        else if (l.GetTile(current.x, (ushort)(current.y - 1), current.z) == Block.air)
                        {
                            l.Blockchange(current.x, current.y, current.z, Block.air);
                            l.Blockchange(current.x, (ushort)(current.y - 1), current.z, current.id1);
                            current.y = (ushort)(current.y - 1);
                        }
                        else if (current.C > 30 && random.Next(4) == 2)
                        {
                            l.Blockchange(current.x, current.y, current.z, Block.air);
                            list.Remove(current);
                        }
                        else
                        {
                            if (l.GetTile(current.x, (ushort)(current.y - 1), current.z) == Block.grass)
                                l.Blockchange(current.x, (ushort)(current.y - 1), current.z, Block.snow);
                            current.C++;
                        }
                    });
                    Thread.Sleep(250);
                }
            }));
            t.Start();
        }
        public void Stop(Level l)
        {
            started = false;
            if (t != null)
            {
                t.Abort();
                t.Join();
            }
            Replace("fallsnow air", l);
            Replace("snow grass", l);
        }
    }
    public class BlockID
    {
        public ushort x;
        public ushort y;
        public ushort z;
        public byte id;
        public byte id1;
        public int C;
        public BlockID(ushort x1, ushort y1, ushort z1, byte b, byte fallb, int time = 0, int spawner = 0)
        {
            C = time;
            x = x1;
            y = y1;
            z = z1;
            id = b;
            id1 = fallb;
        }
    }
}
