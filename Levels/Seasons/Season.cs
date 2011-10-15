﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public class Season
    {
        public static Seasons winter = new Seasons(true, false, Block.white);
        public static Seasons fall = new Seasons(false, true, Block.leaf);
        public Seasons s;
        public Season(byte b)
        {
            if (b == Block.white)
                s = new Seasons(true, false, Block.snow);
            else if (b == Block.leaf)
                s = new Seasons(false, true, Block.leaf);
            else
                s = new Seasons(false, false, b);
        }
        public Season(byte b, bool winter, bool fall = false)
        {
            s = new Seasons(winter, fall, b);
        }
    }
    public class Seasons
    {
        public bool Winter;
        public bool Fall;
        public byte Block;
        public int spawn;
        public Seasons(bool winter, bool fall, byte block, int spawner = 0)
        {
            Block = block;
            Fall = fall;
            Winter = winter;
            spawn = spawner;
        }
    }
}
