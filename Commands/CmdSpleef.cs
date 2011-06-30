/* 
	Copyright 2011 MCDerp Team Based in Canada
	MCDerp has been Licensed with the below license:
	
	http://www.binpress.com/license/view/l/62e7c4034ccb45cd39d8dcbe9ed87bd8
	Or, you can read the below summary;

	Can be used on 1 site, unlimited servers
	Personal use only (cannot be resold or distributed)
	Non-commercial use only
	Cannot modify source-code for any purpose (cannot create derivative works)
	Software trademarks are included in the license
	Software patents are included in the license
*/
		
using System;
using System.Timers;
using System.Collections.Generic;

namespace MCForge
{
    public class CmdSpleef : Command
    {
        public override string name { get { return "spleef"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }

        List<Player> red = new List<Player>();
        List<Player> blue = new List<Player>();
        Level spleeflvl;
        area arena;
        mycuboid cuboid;
        Timer Checker;
        bool started = false;
        Player someone;

        public override void Use(Player p, string message)
        {
            String[] temp = message.Split(' ');
            if (temp.Length == 1)
            {
                if (temp[0].ToLower() == "zone" && !started)
                {//define the plate on which the players play
                    someone = p;
                    p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
                    Player.SendMessage(p, "Place two Blocks to determine the Edges");
                }
                else if (temp[0].ToLower() == "start" && red.Count > 0 && blue.Count >0)
                {//starts the round if players are set
                    if (started)
                    {
                        Player.SendMessage(p, "spleef is running");
                        return;
                    }
                    if (red.Count <1)
                    {
                        Player.SendMessage(p, "red Team is empty. Cant start");
                        return;
                    }
                    if (blue.Count < 1)
                    {
                        Player.SendMessage(p, "blue Team is empty. Cant start");
                        return;
                    }
                    if (red != null && blue != null)
                    {
                        Player.GlobalMessage("Spleef started");
                        //if players are on a different lvl, move them to spleeflvl
                        foreach (Player redp in red)
                        {
                            if (redp.level != spleeflvl)
                            {
                                Command.all.Find("goto").Use(Player.Find(redp.name), spleeflvl.name);
                                Player.GlobalMessage("Waiting for red Player " + redp.name);
                            }
                        }
                        foreach (Player bluep in blue)
                        {
                            if (bluep.level != spleeflvl)
                            {
                                Command.all.Find("goto").Use(Player.Find(bluep.name), spleeflvl.name);
                                Player.GlobalMessage("Waiting for blue Player " + bluep.name);
                            }
                        }
                        System.Threading.Thread.Sleep(3000);//wait 3 seconds for levelchange
                        start();
                    }
                    else
                    {
                        Player.SendMessage(p, "not every Player is set"); return;
                    }
                }
                else if (temp[0].ToLower() == "red" && !started)
                {//joins red team
                    if (!red.Contains(p)&& !blue.Contains(p))
                    {
                        red.Add(p);
                        Player.SendMessage(p, "You joined the red team");
                    }
                    else
                    {
                        Player.SendMessage(p, "You are already in the red or blue team");
                    }
                }
                else if (temp[0].ToLower() == "blue" && !started)
                {//joins blue team
                    if (!red.Contains(p) && !blue.Contains(p))
                    {
                        blue.Add(p);
                        Player.SendMessage(p, "You joined the blue team");
                    }
                    else
                    {
                        Player.SendMessage(p, "You are already in the blue or red Team");
                    }
                }
                else if (temp[0].ToLower() == "exit" && !started)
                {
                    if (red.Contains(p))
                    {
                        red.Remove(p);
                        Player.SendMessage(p, "you exited the red team");
                    }
                    else if (blue.Contains(p))
                    {
                        blue.Remove(p);
                        Player.SendMessage(p, "you exited the blue team");
                    }
                    else
                    {
                        Player.SendMessage(p, "You weren't participating.");
                    }
                }
                else if (temp[0].ToLower() == "reset" && started)
                {
                    reset(true);
                }
                else
                {
                    
                    foreach (Player redp in red)
                    {
                        try
                        {
                            if (Player.Find(redp.name) == null)
                            {
                                red.Remove(redp);
                                Player.SendMessage(p, "Empty red player removed");
                            }
                        }
                        catch (Exception e)
                        {
                            red.Remove(redp);
                            Player.SendMessage(p, "Empty red player removed");
                        }
                    }

                    foreach (Player bluep in blue)
                    {
                        try
                        {
                            if (Player.Find(bluep.name) == null)
                            {
                                blue.Remove(bluep);
                                Player.SendMessage(p, "Empty blue player removed");
                            }
                        }
                        catch (Exception e)
                        {
                            blue.Remove(bluep);
                            Player.SendMessage(p, "Empty blue player removed");
                        }
                    }
                    Help(p); return;
                }
            }
            else
            {
                Help(p); return;
            }
            
        }

        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte typ = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, typ);
            p.blockchangeObject = new block(x,y,z,type);
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }
        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte typ = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, typ);
            block firstblock = (block)p.blockchangeObject;
            spleeflvl = p.level;
            arena.originX = Math.Min(x, firstblock.x);
            arena.originY = Math.Min(y, firstblock.y);
            arena.originZ = Math.Min(z, firstblock.z);
            arena.dimX = (ushort)(Math.Max(x,firstblock.x) - Math.Min(x,firstblock.x));
            arena.dimZ = (ushort)(Math.Max(z, firstblock.z) - Math.Min(z, firstblock.z));
            arena.areatype = type;
            point p1 = new point(arena.originX, arena.originY, arena.originZ);
            point p2 = new point((ushort)(arena.originX+arena.dimX), arena.originY,(ushort)(arena.originZ+arena.dimZ));
            cuboid.Use(someone,p.level, p1,p2,type);
            p1.y--;
            p2.y -= 2;
            cuboid.Use(someone, p.level, p1, p2, Block.air);
            //cuboiding 2 blocks air for the players to fall down.
            p2.y--;
            p1.y -= 2;
            cuboid.Use(someone, p.level, p1, p2, Block.magma);
            //magma for let the player respawn after he fell down... have to find another way.. without killing him
        }

        void start()
        {
            started = true;
            foreach (Player redp in red)
            {
                Command.all.Find("move").Use(redp, (ushort)(arena.originX) + " " + (ushort)(arena.originY + 1) + " " + (ushort)(arena.originZ));
                redp.frozen = true;
            }
            foreach (Player bluep in blue)
            {
                Command.all.Find("move").Use(bluep, (ushort)(arena.originX + arena.dimX) + " " + (ushort)(arena.originY + 1) + " " + (ushort)(arena.originZ + arena.dimZ));
                bluep.frozen = true;
            }
            point p1 = new point(arena.originX, arena.originY, arena.originZ);
            point p2 = new point((ushort)(arena.originX + arena.dimX), arena.originY, (ushort)(arena.originZ + arena.dimZ));
            cuboid.Use(someone, spleeflvl, p1, p2, arena.areatype);
            //build glass container
            point OOO = new point((ushort)(arena.originX - 1), (ushort)(arena.originY + 1), (ushort)(arena.originZ - 1));
            point IIO = new point((ushort)(arena.originX + arena.dimX + 1), (ushort)(arena.originY + 7), (ushort)(arena.originZ - 1));
            point IOO = new point((ushort)(arena.originX + arena.dimX + 1), (ushort)(arena.originY + 1), (ushort)(arena.originZ - 1));
            point OII = new point((ushort)(arena.originX - 1), (ushort)(arena.originY + 7), (ushort)(arena.originZ +arena.dimZ +1));
            point OOI = new point((ushort)(arena.originX - 1), (ushort)(arena.originY + 1), (ushort)(arena.originZ + arena.dimZ + 1));
            point III = new point((ushort)(arena.originX + arena.dimX + 1), (ushort)(arena.originY + 7), (ushort)(arena.originZ + arena.dimZ + 1));
            point OIO = new point((ushort)(arena.originX -1), (ushort)(arena.originY + 7), (ushort)(arena.originZ - 1));
            cuboid.Use(someone,spleeflvl, OOO, IIO, Block.glass);
            cuboid.Use(someone,spleeflvl, OOO, OII, Block.glass);
            cuboid.Use(someone, spleeflvl, IOO, III, Block.glass);
            cuboid.Use(someone, spleeflvl, OOI, III, Block.glass);
            cuboid.Use(someone, spleeflvl, OIO, III, Block.glass);
            //countdown
            Player.GlobalMessage("5");
            System.Threading.Thread.Sleep(1000);
            Player.GlobalMessage("4");
            System.Threading.Thread.Sleep(1000);
            Player.GlobalMessage("3");
            System.Threading.Thread.Sleep(1000);
            Player.GlobalMessage("2");
            System.Threading.Thread.Sleep(1000);
            Player.GlobalMessage("1");
            System.Threading.Thread.Sleep(1000);
            Player.GlobalMessage("SPLEEEEEEEF");
            foreach (Player redp in red)
            {
                redp.frozen = false;
            }
            foreach (Player bluep in blue)
            {
                bluep.frozen = false;
            }
            Checker = new Timer(150);
            Checker.Enabled = true;
            Checker.Elapsed += new ElapsedEventHandler(Check);
        }


        void Check(object sender, ElapsedEventArgs e)
        {
            //check if one of the players fell down
            //or is outside cage
            foreach (Player redp in red)
            {
                
                if (redp.pos[1] / 32 < (arena.originY+1) || !inCourt(redp))
                {
                    //one red player fell down
                    if (!inCourt(redp))
                    {
                        Player.GlobalMessage(redp.name + " from the red team was outside the cage and was disqualified");
                    }
                    else
                    {
                        Player.GlobalMessage(redp.name + " from the red team fell down and was disqualified");
                    }
                    red.Remove(redp);
                    Command.all.Find("spawn").Use(redp,"");
                    if (red.Count < 1)
                    {
                        Player.GlobalMessage("Red team Lost the spleef match against the blue team");
                        Player.GlobalMessage("Blue Players remained in Court:");
                        String bps = "";
                        foreach (Player bluep in blue)
                        {
                            bps += bluep.name + ", ";
                        }
                        Player.GlobalMessage(bps);
                        reset(false);
                        return;
                    }
                }
            }
            foreach (Player bluep in blue)
            {
                if (bluep.pos[1] / 32 < (arena.originY + 1) || !inCourt(bluep))
                {
                    //blue lost
                    if (!inCourt(bluep))
                    {
                        Player.GlobalMessage(bluep.name + " from the blue team was outside the cage and was disqualified");
                    }
                    else
                    {
                        Player.GlobalMessage(bluep.name + " from the blue team fell down and was disqualified");
                    }
                    blue.Remove(bluep);
                    Command.all.Find("spawn").Use(bluep, "");
                    if (blue.Count < 1)
                    {
                        Player.GlobalMessage("Blue team Lost the spleef match against the red team");
                        Player.GlobalMessage("Red Players remained in Court:");
                        String rps = "";
                        foreach (Player redp in red)
                        {
                            rps += redp.name + ", ";
                        }
                        Player.GlobalMessage(rps);
                        reset(false);
                        return;
                    }
                }
            }
        }

        void reset(bool justcmd)
        {
            //reset field
            started = false;
            //delete players
            foreach (Player redp in red)
            {
                redp.ClearBlockchange();
            }
            foreach (Player bluep in blue)
            {
                bluep.ClearBlockchange();
            }
            //delete glass cage
            point OOO = new point((ushort)(arena.originX - 1), (ushort)(arena.originY + 1), (ushort)(arena.originZ - 1));
            point IIO = new point((ushort)(arena.originX + arena.dimX + 1), (ushort)(arena.originY + 7), (ushort)(arena.originZ - 1));
            point IOO = new point((ushort)(arena.originX + arena.dimX + 1), (ushort)(arena.originY + 1), (ushort)(arena.originZ - 1));
            point OII = new point((ushort)(arena.originX - 1), (ushort)(arena.originY + 7), (ushort)(arena.originZ + arena.dimZ + 1));
            point OOI = new point((ushort)(arena.originX - 1), (ushort)(arena.originY + 1), (ushort)(arena.originZ + arena.dimZ + 1));
            point III = new point((ushort)(arena.originX + arena.dimX + 1), (ushort)(arena.originY + 7), (ushort)(arena.originZ + arena.dimZ + 1));
            point OIO = new point((ushort)(arena.originX - 1), (ushort)(arena.originY + 7), (ushort)(arena.originZ - 1));
            cuboid.Use(someone, spleeflvl, OOO, IIO, Block.air);
            cuboid.Use(someone, spleeflvl, OOO, OII, Block.air);
            cuboid.Use(someone, spleeflvl, IOO, III, Block.air);
            cuboid.Use(someone, spleeflvl, OOI, III, Block.air);
            cuboid.Use(someone, spleeflvl, OIO, III, Block.air);
            Checker.Dispose();
            /*if (justcmd)
            {
                foreach (Player redp in red)
                {
                    redp.spawning = true;
                }
                foreach (Player bluep in blue)
                {
                    bluep.spawning = true;
                }
            }*/
            red.Clear();
            blue.Clear();
            point p1 = new point(arena.originX, arena.originY, arena.originZ);
            point p2 = new point((ushort)(arena.originX + arena.dimX), arena.originY, (ushort)(arena.originZ + arena.dimZ));
            cuboid.Use(someone, spleeflvl, p1, p2, arena.areatype);
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/spleef <blah> where blah can be:");
            Player.SendMessage(p, "zone - defines the Spleef zone. cuboids the arenafloor. be sure to have 2 blocks space under it");
            Player.SendMessage(p, "red - join the red Team");
            Player.SendMessage(p, "blue - join the blue Team");
            Player.SendMessage(p, "exit - exits your Team");
            Player.SendMessage(p, "start - starts the spleef match with the Players in the Teams. cant start when one Team is empty");
            Player.SendMessage(p, "reset - resets the arena. in case players are trapped, or something went wrong");
        }

        bool inCourt(Player p)
        {
            if (p.pos[0] < arena.originX || p.pos[0] > arena.originX + arena.dimX ||
                p.pos[2] < arena.originZ || p.pos[2] > arena.originZ + arena.dimZ)
            {
                return false;
            }else{
                return true;
            }
        }

        struct block
        {
            public ushort x;
            public ushort y;
            public ushort z;
            public byte type;

            public block(ushort x, ushort y, ushort z, byte type)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.type = type;
            }
        }

        struct area
        {
            public ushort originX;
            public ushort originY;
            public ushort originZ;
            public ushort dimX;
            public ushort dimZ;
            public byte areatype;
        }
        struct point{
            public ushort x;
            public ushort y;
            public ushort z;
            public point(ushort x, ushort y, ushort z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        struct mycuboid
        {
            public void Use(Player pl,Level lvl, ushort x, ushort y, ushort z, ushort x2, ushort y2, ushort z2,byte type)
            {
                for (ushort xx = Math.Min(x, x2); xx <= Math.Max(x, x2); xx++)
                {
                    for (ushort yy = Math.Min(y, y2); yy <= Math.Max(y, y2); yy++)
                    {
                        for (ushort zz = Math.Min(z, z2); zz <= Math.Max(z, z2); zz++)
                        {
                            lvl.Blockchange(pl, xx, yy, zz, type);
                        }
                    }
                }
                
            }
            public void Use(Player pl,Level lvl, point one, point two, byte type)
            {
                for (ushort xx = Math.Min(one.x, two.x); xx <= Math.Max(one.x, two.x); xx++)
                {
                    for (ushort yy = Math.Min(one.y, two.y); yy <= Math.Max(one.y, two.y); yy++)
                    {
                        for (ushort zz = Math.Min(one.z, one.z); zz <= Math.Max(one.z, two.z); zz++)
                        {
                            //Player.GlobalBlockchange(Level.Find(lvl.name), xx, yy, zz, type); doesnt work >=(
                            lvl.Blockchange(pl, xx, yy, zz, type);
                        }
                    }
                }

            }
        }
    }
}