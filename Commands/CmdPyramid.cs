/*
   Thanks to aaron1tasker
*/
/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)

	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MCForge
{
    public class CmdPyramid : Command
    {
        public override string name { get { return "pyramid"; } }
        public override string shortcut { get { return "pd"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Builder; } }
        public CmdPyramid() { }

        public override void Use(Player p, string message)
        {
            if (!Directory.Exists("pyramid"))
            {
                Directory.CreateDirectory("pyramid");
            }
            if (!File.Exists("pyramid/" + p.name + "block.txt"))
            {
                int number = message.Split(' ').Length;
                if (number > 2) { Help(p); return; }
                if (number == 2)
                {
                    int pos = message.IndexOf(' ');
                    string t = message.Substring(0, pos).ToLower();
                    string s = message.Substring(pos + 1).ToLower();
                    byte type = Block.Byte(t);
                    if (type == 255) { Player.SendMessage(p, "There is no block \"" + t + "\"."); return; }
                    if (!Block.canPlace(p, type)) { Player.SendMessage(p, "Cannot place that."); return; }
                    SolidType solid;
                    if (s == "solid") { solid = SolidType.solid; }
                    else if (s == "hollow") { solid = SolidType.hollow; }
                    else if (s == "reverse") { solid = SolidType.reverse; }
                    else { Help(p); return; }
                    CatchPos cpos; cpos.solid = solid; cpos.type = type;
                    cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
                    cpos.type = Block.Byte(message);
                    File.WriteAllText("pyramid/" + p.name + "block.txt", t);
                }
                else if (message != "")
                {
                    SolidType solid = SolidType.hollow;
                    message = message.ToLower();
                    byte type; unchecked { type = (byte)-1; }
                    File.WriteAllText("pyramid/" + p.name + "block.txt", "stone");
                    if (message == "solid") { solid = SolidType.solid; }
                    else if (message == "hollow") { solid = SolidType.hollow; }
                    else if (message == "reverse") { solid = SolidType.reverse; }
                    else
                    {
                        byte t = Block.Byte(message);
                        if (t == 255) { Player.SendMessage(p, "There is no block \"" + message + "\"."); return; }
                        if (!Block.canPlace(p, t)) { Player.SendMessage(p, "Cannot place that."); return; }
                        File.WriteAllText("pyramid/" + p.name + "block.txt", message);

                    } CatchPos cpos; cpos.solid = solid; cpos.type = type;
                    cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
                }
                else
                {
                    CatchPos cpos; cpos.solid = SolidType.solid; unchecked { cpos.type = (byte)-1; }
                    cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
                }
                Player.SendMessage(p, "Place two blocks to determine the edges.");
                p.ClearBlockchange();
                if (!File.Exists("pyramid/" + p.name + "block.txt"))
                {
                    File.WriteAllText("pyramid/" + p.name + "block.txt", "stone");
                }
                p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
            }
            else
            {
                Player.SendMessage(p, "The pyramid you are already creating needs time to finish. If its not finished or there has been an error, in thirty seconds a new one will be started for you");
                Thread.Sleep(30000);
                Player.SendMessage(p, "30 seconds are up, restarting pyramid process");
                Thread.Sleep(2000);
                if (File.Exists("pyramid/" + p.name + "x1.txt"))
                {
                    File.Delete("pyramid/" + p.name + "x1.txt");
                }
                if (File.Exists("pyramid/" + p.name + "x2.txt"))
                {
                    File.Delete("pyramid/" + p.name + "x2.txt");
                }
                if (File.Exists("pyramid/" + p.name + "y1.txt"))
                {
                    File.Delete("pyramid/" + p.name + "y1.txt");
                }
                if (File.Exists("pyramid/" + p.name + "y2.txt"))
                {
                    File.Delete("pyramid/" + p.name + "y2.txt");
                }
                if (File.Exists("pyramid/" + p.name + "z1.txt"))
                {
                    File.Delete("pyramid/" + p.name + "z1.txt");
                }
                if (File.Exists("pyramid/" + p.name + "z2.txt"))
                {
                    File.Delete("pyramid/" + p.name + "z2.txt");
                }
                if (File.Exists("pyramid/" + p.name + "block.txt"))
                {
                    File.Delete("pyramid/" + p.name + "block.txt");
                }
                if (File.Exists("pyramid/" + p.name + "total.txt"))
                {
                    File.Delete("pyramid/" + p.name + "total.txt");
                }
                if (File.Exists("pyramid/" + p.name + "total2.txt"))
                {
                    File.Delete("pyramid/" + p.name + "total2.txt");
                }
                Use(p, message);
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/pyramid [type] <solid/hollow/reverse> - create a pyramid of blocks.");
        }
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos bp = (CatchPos)p.blockchangeObject;
            bp.x = x; bp.y = y; bp.z = z; p.blockchangeObject = bp;

            //<writes in text file the x coords>
            using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "x1.txt", true))
            {
                writer.WriteLine(bp.x);
            }
            //</writes in text file the x coords>

            //<writes in text file the y coords>
            using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "z1.txt", true))
            {
                writer.WriteLine(bp.y);
            }
            //</writes in text file the y coords>

            //<writes in text file the z coords>
            using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "y1.txt", true))
            {
                writer.WriteLine(bp.z);
            }
            //</writes in text file the z coords>

            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }
        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos cpos = (CatchPos)p.blockchangeObject;
            cpos.x = x; cpos.y = y; cpos.z = z; p.blockchangeObject = cpos;
            List<Pos> buffer = new List<Pos>();

            switch (cpos.solid)
            {
                case SolidType.solid:
                    buffer.Capacity = Math.Abs(cpos.x - x) * Math.Abs(cpos.y - y) * Math.Abs(cpos.z - z);
                    //<writes in text file new coordinates for x>
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "x2.txt", true))
                    {
                        writer.WriteLine(cpos.x);
                    }
                    //</writes in text file new coordinates for x>

                    //<writes in text file new coordinates for y>
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "z2.txt", true))
                    {
                        writer.WriteLine(cpos.y);
                    }
                    //</writes in text file new coordinates for y>

                    //<writes in text file new coordinates for z>
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "y2.txt", true))
                    {
                        writer.WriteLine(cpos.z);
                    }
                    //</writes in text file new coordinates for z>

                    String x1read = File.ReadAllText("pyramid/" + p.name + "x1.txt");
                    String x2read = File.ReadAllText("pyramid/" + p.name + "x2.txt");
                    String y1read = File.ReadAllText("pyramid/" + p.name + "y1.txt");
                    String y2read = File.ReadAllText("pyramid/" + p.name + "y2.txt");
                    String z1read = File.ReadAllText("pyramid/" + p.name + "z1.txt");
                    String z2read = File.ReadAllText("pyramid/" + p.name + "z2.txt");
                    int x1parse = int.Parse(x1read);
                    int x2parse = int.Parse(x2read);
                    int y1parse = int.Parse(y1read);
                    int y2parse = int.Parse(y2read);
                    int z1parse = int.Parse(z1read);
                    int z2parse = int.Parse(z2read);


                    if (z1parse == z2parse) //checks if pyramid is on a level surface
                    {
                        Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt"));
                        click(p, x1parse + " " + z2parse + " " + y1parse);
                        click(p, x2parse + " " + z2parse + " " + y2parse);

                        for (int i = 1; i > 0; ) //looper to create pyramid
                        {
                            if (x1parse == x2parse)
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x1parse + " " + z2parse + " " + y1parse); //clicks on coords from text files
                                click(p, x2parse + " " + z2parse + " " + y2parse); //clicks on coords from text files
                                i--;
                            }
                            else if (y1parse == y2parse) { i--; }
                            if (x1parse > x2parse) //checks if 2 coords are the same for x and provides escape sequence if they are
                            {
                                if ((x1parse - x2parse) == 1)
                                {
                                    Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                    click(p, x1parse + " " + z2parse + " " + y1parse); //clicks on coords from text files
                                    click(p, x2parse + " " + z2parse + " " + y2parse); //clicks on coords from text files
                                    i--;
                                }
                            }
                            else if ((x2parse - x1parse) == 1)  //checks if 2 coords are the same for y and provides escape sequence if they are
                            {
                                i--;
                            }
                            if (y1parse > y2parse) //checks if 2 coords are the same for x and provides escape sequence if they are
                            {
                                if ((y1parse - y2parse) == 1)
                                {
                                    Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                    click(p, x1parse + " " + z2parse + " " + y1parse); //clicks on coords from text files
                                    click(p, x2parse + " " + z2parse + " " + y2parse); //clicks on coords from text files
                                    i--;
                                }
                            }
                            else if ((y2parse - y1parse) == 1)  //checks if 2 coords are the same for y and provides escape sequence if they are
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x1parse + " " + z2parse + " " + y1parse); //clicks on coords from text files
                                click(p, x2parse + " " + z2parse + " " + y2parse); //clicks on coords from text files
                                i--;
                            }
                            Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                            click(p, x1parse + " " + z2parse + " " + y1parse); //clicks on coords from text files
                            click(p, x2parse + " " + z2parse + " " + y2parse); //clicks on coords from text files
                            
                            if (x1parse > x2parse) //checks if one is greater than the other. This way it knows which one to add one two and which one to minus one from so that it reaches the middle.
                            {
                                x1parse--;
                                File.WriteAllText("pyramid/" + p.name + "x1.txt", Convert.ToString(x1parse)); //adds toone
                                x2parse++;
                                File.WriteAllText("pyramid/" + p.name + "x2.txt", Convert.ToString(x2parse)); //adds to the other
                            }
                            else
                            {
                                x1parse++;
                                File.WriteAllText("pyramid/" + p.name + "x1.txt", Convert.ToString(x1parse)); //adds to the other one
                                x2parse--;
                                File.WriteAllText("pyramid/" + p.name + "x2.txt", Convert.ToString(x2parse)); //takes from the other one
                            }

                            if (y1parse > y2parse) //does the same for the y coords
                            {
                                y1parse--;
                                File.WriteAllText("pyramid/" + p.name + "y1.txt", Convert.ToString(y1parse));
                                y2parse++;
                                File.WriteAllText("pyramid/" + p.name + "y2.txt", Convert.ToString(y2parse));
                            }
                            else
                            {
                                y1parse++;
                                File.WriteAllText("pyramid/" + p.name + "y1.txt", Convert.ToString(y1parse));
                                y2parse--;
                                File.WriteAllText("pyramid/" + p.name + "y2.txt", Convert.ToString(y2parse));
                            }
                            z2parse++;
                            File.WriteAllText("pyramid/" + p.name + "z2.txt", Convert.ToString(z2parse)); //adds to the z coords                                     
                        }

                        //<deletes all the text files>
                        File.Delete("pyramid/" + p.name + "x1.txt");
                        File.Delete("pyramid/" + p.name + "x2.txt");
                        File.Delete("pyramid/" + p.name + "y1.txt");
                        File.Delete("pyramid/" + p.name + "y2.txt");
                        File.Delete("pyramid/" + p.name + "z1.txt");
                        File.Delete("pyramid/" + p.name + "z2.txt");
                        if (File.Exists("pyramid/" + p.name + "block.txt"))
                        {
                            File.Delete("pyramid/" + p.name + "block.txt");
                        }
                        Player.SendMessage(p, "Pyramid Completed");
                        //</deletes all the text files>
                    }
                    else
                    {
                        //<what happens if pyramid is not level>
                        File.Delete("pyramid/" + p.name + "x1.txt");
                        File.Delete("pyramid/" + p.name + "x2.txt");
                        File.Delete("pyramid/" + p.name + "y1.txt");
                        File.Delete("pyramid/" + p.name + "y2.txt");
                        File.Delete("pyramid/" + p.name + "z1.txt");
                        File.Delete("pyramid/" + p.name + "z2.txt");
                        if (File.Exists("pyramid/" + p.name + "block.txt"))
                        {
                            File.Delete("pyramid/" + p.name + "block.txt");
                        }
                        Player.SendMessage(p, "The two edges of the pyramid must be on the same level");
                        //</what happens if pyramid is not level>
                    }

                    break;

                case SolidType.reverse:
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "x2.txt", true))
                    {
                        writer.WriteLine(cpos.x);
                    }
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "z2.txt", true))
                    {
                        writer.WriteLine(cpos.y);
                    }
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "y2.txt", true))
                    {
                        writer.WriteLine(cpos.z);
                    }

                    String x111read = File.ReadAllText("pyramid/" + p.name + "x1.txt");
                    String x211read = File.ReadAllText("pyramid/" + p.name + "x2.txt");
                    String y111read = File.ReadAllText("pyramid/" + p.name + "y1.txt");
                    String y211read = File.ReadAllText("pyramid/" + p.name + "y2.txt");
                    String z111read = File.ReadAllText("pyramid/" + p.name + "z1.txt");
                    String z211read = File.ReadAllText("pyramid/" + p.name + "z2.txt");
                    int x111parse = int.Parse(x111read);
                    int x211parse = int.Parse(x211read);
                    int y111parse = int.Parse(y111read);
                    int y211parse = int.Parse(y211read);
                    int z111parse = int.Parse(z111read);
                    int z211parse = int.Parse(z211read);

                    if (z111parse == z211parse)
                    {
                        for (int i = 1; i > 0; )
                        {
                            if (x111parse == x211parse)
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x111parse + " " + z211parse + " " + y111parse); //clicks on coords from text files
                                click(p, x211parse + " " + z211parse + " " + y211parse); //clicks on coords from text files
                                i--;
                            }
                            else if (y111parse == y211parse) { i--; }
                            if (x111parse > x211parse) //checks if 2 coords are the same for x and provides escape sequence if they are
                            {
                                if ((x111parse - x211parse) == 1)
                                {
                                    Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                    click(p, x111parse + " " + z211parse + " " + y111parse); //clicks on coords from text files
                                    click(p, x211parse + " " + z211parse + " " + y211parse); //clicks on coords from text files
                                    i--;
                                }
                            }
                            else if ((x211parse - x111parse) == 1)  //checks if 2 coords are the same for y and provides escape sequence if they are
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x111parse + " " + z211parse + " " + y111parse); //clicks on coords from text files
                                click(p, x211parse + " " + z211parse + " " + y211parse); //clicks on coords from text files
                                i--;
                            }
                            if (y111parse > y211parse) //checks if 2 coords are the same for x and provides escape sequence if they are
                            {
                                if ((y111parse - y211parse) == 1)
                                {
                                    Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                    click(p, x111parse + " " + z211parse + " " + y111parse); //clicks on coords from text files
                                    click(p, x211parse + " " + z211parse + " " + y211parse);
                                    i--;
                                }
                            }
                            else if ((y211parse - y111parse) == 1)  //checks if 2 coords are the same for y and provides escape sequence if they are
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x111parse + " " + z211parse + " " + y111parse); //clicks on coords from text files
                                click(p, x211parse + " " + z211parse + " " + y211parse);
                                i--;
                            }
                            Command.all.Find("silentcuboid").Use(p, "air");
                            click(p, x111parse + " " + z211parse + " " + y111parse);
                            click(p, x211parse + " " + z211parse + " " + y211parse);
                            Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt") + " " + "walls");
                            click(p, x111parse + " " + z211parse + " " + y111parse);
                            click(p, x211parse + " " + z211parse + " " + y211parse);

                            if (x111parse > x211parse)
                            {
                                x111parse--;
                                File.WriteAllText("pyramid/" + p.name + "x1.txt", Convert.ToString(x111parse));
                                x211parse++;
                                File.WriteAllText("pyramid/" + p.name + "x2.txt", Convert.ToString(x211parse));
                            }
                            else
                            {
                                x111parse++;
                                File.WriteAllText("pyramid/" + p.name + "x1.txt", Convert.ToString(x111parse));
                                x211parse--;
                                File.WriteAllText("pyramid/" + p.name + "x2.txt", Convert.ToString(x211parse));
                            }

                            if (y111parse > y211parse)
                            {
                                y111parse--;
                                File.WriteAllText("pyramid/" + p.name + "y1.txt", Convert.ToString(x111parse));
                                y211parse++;
                                File.WriteAllText("pyramid/" + p.name + "y2.txt", Convert.ToString(x211parse));
                            }
                            else
                            {
                                y111parse++;
                                File.WriteAllText("pyramid/" + p.name + "y1.txt", Convert.ToString(y111parse));
                                y211parse--;
                                File.WriteAllText("pyramid/" + p.name + "y2.txt", Convert.ToString(y211parse));
                            }
                            z211parse--;
                            File.WriteAllText("pyramid/" + p.name + "z2.txt", Convert.ToString(z211parse));
                        }

                        File.Delete("pyramid/" + p.name + "x1.txt");
                        File.Delete("pyramid/" + p.name + "x2.txt");
                        File.Delete("pyramid/" + p.name + "y1.txt");
                        File.Delete("pyramid/" + p.name + "y2.txt");
                        File.Delete("pyramid/" + p.name + "z1.txt");
                        File.Delete("pyramid/" + p.name + "z2.txt");
                        if (File.Exists("pyramid/" + p.name + "block.txt"))
                        {
                            File.Delete("pyramid/" + p.name + "block.txt");
                        }
                        Player.SendMessage(p, "Pyramid Completed");
                    }
                    else
                    {
                        File.Delete("pyramid/" + p.name + "x1.txt");
                        File.Delete("pyramid/" + p.name + "x2.txt");
                        File.Delete("pyramid/" + p.name + "y1.txt");
                        File.Delete("pyramid/" + p.name + "y2.txt");
                        File.Delete("pyramid/" + p.name + "z1.txt");
                        File.Delete("pyramid/" + p.name + "z2.txt");
                        if (File.Exists("pyramid/" + p.name + "block.txt"))
                        {
                            File.Delete("pyramid/" + p.name + "block.txt");
                        }
                        Player.SendMessage(p, "The two edges of the pyramid must be on the same level");
                    }
                    break;

                case SolidType.hollow:
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "x2.txt", true))
                    {
                        writer.WriteLine(cpos.x);
                    }
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "z2.txt", true))
                    {
                        writer.WriteLine(cpos.y);
                    }
                    using (StreamWriter writer = new StreamWriter("pyramid/" + p.name + "y2.txt", true))
                    {
                        writer.WriteLine(cpos.z);
                    }

                    String x11read = File.ReadAllText("pyramid/" + p.name + "x1.txt");
                    String x21read = File.ReadAllText("pyramid/" + p.name + "x2.txt");
                    String y11read = File.ReadAllText("pyramid/" + p.name + "y1.txt");
                    String y21read = File.ReadAllText("pyramid/" + p.name + "y2.txt");
                    String z11read = File.ReadAllText("pyramid/" + p.name + "z1.txt");
                    String z21read = File.ReadAllText("pyramid/" + p.name + "z2.txt");
                    int x11parse = int.Parse(x11read);
                    int x21parse = int.Parse(x21read);
                    int y11parse = int.Parse(y11read);
                    int y21parse = int.Parse(y21read);
                    int z11parse = int.Parse(z11read);
                    int z21parse = int.Parse(z21read);

                    if (z11parse == z21parse)
                    {
                        Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt"));
                        click(p, x11parse + " " + z21parse + " " + y11parse);
                        click(p, x21parse + " " + z21parse + " " + y21parse);

                        for (int i = 1; i > 0; )
                        {
                            if (x11parse == x21parse)
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x11parse + " " + z21parse + " " + y11parse); //clicks on coords from text files
                                click(p, x21parse + " " + z21parse + " " + y21parse);
                                i--;
                            }
                            else if (y11parse == y21parse) { i--; }
                            if (x11parse > x21parse) //checks if 2 coords are the same for x and provides escape sequence if they are
                            {
                                if ((x11parse - x21parse) == 1)
                                {
                                    Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                    click(p, x11parse + " " + z21parse + " " + y11parse); //clicks on coords from text files
                                    click(p, x21parse + " " + z21parse + " " + y21parse);
                                    i--;
                                }
                            }
                            else if ((x21parse - x11parse) == 1)  //checks if 2 coords are the same for y and provides escape sequence if they are
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x11parse + " " + z21parse + " " + y11parse); //clicks on coords from text files
                                click(p, x21parse + " " + z21parse + " " + y21parse);
                                i--;
                            }
                            if (y11parse > y21parse) //checks if 2 coords are the same for x and provides escape sequence if they are
                            {
                                if ((y11parse - y21parse) == 1)
                                {
                                    Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                    click(p, x11parse + " " + z21parse + " " + y11parse); //clicks on coords from text files
                                    click(p, x21parse + " " + z21parse + " " + y21parse);
                                    i--;
                                }
                            }
                            else if ((y21parse - y11parse) == 1)  //checks if 2 coords are the same for y and provides escape sequence if they are
                            {
                                Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt")); //cuboid
                                click(p, x11parse + " " + z21parse + " " + y11parse); //clicks on coords from text files
                                click(p, x21parse + " " + z21parse + " " + y21parse);
                                i--;
                            }
                            Command.all.Find("silentcuboid").Use(p, File.ReadAllText("pyramid/" + p.name + "block.txt") + " " + "walls");
                            click(p, x11parse + " " + z21parse + " " + y11parse);
                            click(p, x21parse + " " + z21parse + " " + y21parse);

                            if (x11parse > x21parse)
                            {
                                x11parse--;
                                File.WriteAllText("pyramid/" + p.name + "x1.txt", Convert.ToString(x11parse));
                                x21parse++;
                                File.WriteAllText("pyramid/" + p.name + "x2.txt", Convert.ToString(x21parse));
                            }
                            else
                            {
                                x11parse++;
                                File.WriteAllText("pyramid/" + p.name + "x1.txt", Convert.ToString(x11parse));
                                x21parse--;
                                File.WriteAllText("pyramid/" + p.name + "x2.txt", Convert.ToString(x21parse));
                            }

                            if (y11parse > y21parse)
                            {
                                y11parse--;
                                File.WriteAllText("pyramid/" + p.name + "y1.txt", Convert.ToString(x11parse));
                                y21parse++;
                                File.WriteAllText("pyramid/" + p.name + "y2.txt", Convert.ToString(x21parse));
                            }
                            else
                            {
                                y11parse++;
                                File.WriteAllText("pyramid/" + p.name + "y1.txt", Convert.ToString(y11parse));
                                y21parse--;
                                File.WriteAllText("pyramid/" + p.name + "y2.txt", Convert.ToString(y21parse));
                            }
                            z21parse++;
                            File.WriteAllText("pyramid/" + p.name + "z2.txt", Convert.ToString(z21parse));
                        }
                        File.Delete("pyramid/" + p.name + "x1.txt");
                        File.Delete("pyramid/" + p.name + "x2.txt");
                        File.Delete("pyramid/" + p.name + "y1.txt");
                        File.Delete("pyramid/" + p.name + "y2.txt");
                        File.Delete("pyramid/" + p.name + "z1.txt");
                        File.Delete("pyramid/" + p.name + "z2.txt");
                        if (File.Exists("pyramid/" + p.name + "block.txt"))
                        {
                            File.Delete("pyramid/" + p.name + "block.txt");
                        }
                        Player.SendMessage(p, "Pyramid Completed");
                    }
                    else
                    {
                        File.Delete("pyramid/" + p.name + "x1.txt");
                        File.Delete("pyramid/" + p.name + "x2.txt");
                        File.Delete("pyramid/" + p.name + "y1.txt");
                        File.Delete("pyramid/" + p.name + "y2.txt");
                        File.Delete("pyramid/" + p.name + "z1.txt");
                        File.Delete("pyramid/" + p.name + "z2.txt");
                        if (File.Exists("pyramid/" + p.name + "block.txt"))
                        {
                            File.Delete("pyramid/" + p.name + "block.txt");
                        }
                        Player.SendMessage(p, "The two edges of the pyramid must be on the same level");
                    }
                    break;
            }

            if (Server.forceCuboid)
            {
                int counter = 1;
                buffer.ForEach(delegate(Pos pos)
                {
                    if (counter <= p.group.maxBlocks)
                    {
                        counter++;
                        p.level.Blockchange(p, pos.x, pos.y, pos.z, type);
                    }
                });
                if (counter >= p.group.maxBlocks)
                {
                    Player.SendMessage(p, "Tried to pyramid " + buffer.Count + " blocks, but your limit is " + p.group.maxBlocks + ".");
                    Player.SendMessage(p, "Executed pyramid up to limit.");
                }
                else
                {
                    Player.SendMessage(p, buffer.Count.ToString() + " blocks.");
                }
                if (p.staticCommands) p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
                return;
            }

            if (buffer.Count > p.group.maxBlocks)
            {
                Player.SendMessage(p, "You tried to pyramid " + buffer.Count + " blocks.");
                Player.SendMessage(p, "You cannot pyramid more than " + p.group.maxBlocks + ".");
                return;
            }

            Player.SendMessage(p, buffer.Count.ToString() + " blocks.");

            buffer.ForEach(delegate(Pos pos)
            {
                p.level.Blockchange(p, pos.x, pos.y, pos.z, type);
            });

            if (p.staticCommands) p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        void BufferAdd(List<Pos> list, ushort x, ushort y, ushort z)
        {
            Pos pos; pos.x = x; pos.y = y; pos.z = z; list.Add(pos);
        }
        struct Pos
        {
            public ushort x, y, z;
        }
        struct CatchPos
        {
            public SolidType solid;
            public byte type;
            public ushort x, y, z;
        }
        enum SolidType { solid, hollow, reverse };
        //<click command to stop text>
        public void click(Player p, string message)
        {
            string[] parameters = message.Split(' ');
            ushort[] click = p.lastClick;
            if (message.IndexOf(' ') != -1)
            {
                if (parameters.Length != 3)
                {
                    Help(p);
                    return;
                }
                else
                {
                    for (int value = 0; value < 3; value++)
                    {
                        if (parameters[value].ToLower() == "x" || parameters[value].ToLower() == "y" || parameters[value].ToLower() == "z")
                            click[value] = p.lastClick[value];
                        else if (isValid(parameters[value], value, p))
                            click[value] = ushort.Parse(parameters[value]);
                        else
                        {
                            Player.SendMessage(p, "\"" + parameters[value] + "\" was not valid");
                            return;
                        }
                    }
                }
            }
            p.manualChange(click[0], click[1], click[2], 0, Block.rock);
        }
        //</click command to stop text>

        //<something to do with click command>
        private bool isValid(string message, int dimension, Player p)
        {
            ushort testValue;
            try
            {
                testValue = ushort.Parse(message);
            }
            catch { return false; }
            if (testValue < 0)
                return false;
            if (testValue >= p.level.width && dimension == 0) return false;
            else if (testValue >= p.level.depth && dimension == 1) return false;
            else if (testValue >= p.level.height && dimension == 2) return false;
            return true;
        }
        //</something to do with click command> 
    }
}