﻿using System;
using System.Collections.Generic;

namespace MCForge
{
    public class CmdSPlace : Command
    {
        public override string name { get { return "splace"; } }
        public override string shortcut { get { return "set"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        private ushort distance, interval;
        private byte blocktype = (byte)1;
        public CmdSPlace() { }

        public override void Use(Player p, string message)
        {
            distance = 0; interval = 0;
            if (message == "") { Help(p); return; }
            if (message.Split(' ').Length > 1)
            {
                try { ushort.TryParse(message.Split(' ')[0], out distance); }
                catch { Player.SendMessage(p, "Distance must be a number."); return; }
                try { ushort.TryParse(message.Split(' ')[1], out interval); }
                catch { Player.SendMessage(p, "Interval must be a number."); return; }
            }
            else
            {
                try { ushort.TryParse(message, out distance); }
                catch { Player.SendMessage(p, "Distance must be a number."); return; }
            }
            if (distance < 1)
            {
                Player.SendMessage(p, "Enter a distance greater than 0."); return;
            }
            if (interval >= distance)
            {
                Player.SendMessage(p, "The Interval cannot be greater than the distance."); return;
            }

            CatchPos cpos;
            cpos.givenMessage = message;
            cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
            Player.SendMessage(p, "Place two blocks to determine direction.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/splace [distance] [interval] - Measures a set [distance] and places a stone block at each end.");
            Player.SendMessage(p, "Optionally place a block at set [interval] between them.");
        }
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos bp = (CatchPos)p.blockchangeObject;
            bp.x = x; bp.y = y; bp.z = z; p.blockchangeObject = bp;
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }
        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            type = p.bindings[type];
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos cpos = (CatchPos)p.blockchangeObject;
            if (x == cpos.x && z == cpos.z) { Player.SendMessage(p, "No direction was selected"); return; }
            if (Math.Abs(cpos.x - x) > Math.Abs(cpos.z - z))
            {
                if (x > cpos.x)
                {
                    p.level.Blockchange(p, (ushort)(cpos.x + (distance - 1)), cpos.y, cpos.z, blocktype);
                    p.level.Blockchange(p, cpos.x, cpos.y, cpos.z, blocktype);
                    if (interval > 0)
                    {
                        for (ushort offset = interval; cpos.x + offset < cpos.x + (distance - 1); offset += interval)
                        {
                            p.level.Blockchange(p, (ushort)(cpos.x + offset), cpos.y, cpos.z, blocktype);
                        }
                    }
                }
                else
                {
                    p.level.Blockchange(p, (ushort)(cpos.x - (distance - 1)), cpos.y, cpos.z, blocktype);
                    p.level.Blockchange(p, cpos.x, cpos.y, cpos.z, b = blocktype);
                    if (interval > 0)
                    {
                        for (ushort offset = interval; cpos.x - (distance - 1) < cpos.x - offset; offset += interval)
                        {
                            p.level.Blockchange(p, (ushort)(cpos.x - offset), cpos.y, cpos.z, blocktype);
                        }
                    }
                }
            }
            else
            {
                if (z > cpos.z)
                {
                    p.level.Blockchange(p, cpos.x, cpos.y, (ushort)(cpos.z + (distance - 1)), blocktype);
                    p.level.Blockchange(p, cpos.x, cpos.y, cpos.z, blocktype);
                    if (interval > 0)
                    {
                        for (ushort offset = interval; cpos.z + offset < cpos.z + (distance - 1); offset += interval)
                        {
                            p.level.Blockchange(p, cpos.x, cpos.y, (ushort)(cpos.z + offset), blocktype);
                        }
                    }
                }
                else
                {
                    p.level.Blockchange(p, cpos.x, cpos.y, (ushort)(cpos.z - (distance - 1)), blocktype);
                    p.level.Blockchange(p, cpos.x, cpos.y, cpos.z, blocktype);
                    if (interval > 0)
                    {
                        for (ushort offset = interval; cpos.z - (distance - 1) < cpos.z - offset; offset += interval)
                        {
                            p.level.Blockchange(p, cpos.x, cpos.y, (ushort)(cpos.z - offset), blocktype);
                        }
                    }
                }
            }
            if (interval > 0)
            {
                Player.SendMessage(p, "Placed stone blocks " + interval + " apart");
            }
            else
            {
                Player.SendMessage(p, "Placed stone blocks " + distance + " apart");
            }
            if (p.staticCommands) p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        struct CatchPos
        {
            public ushort x, y, z; public string givenMessage;
        }

    }
}