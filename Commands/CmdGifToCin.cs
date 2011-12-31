/*
	Copyright 2011 MCForge
	
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
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Globalization;

namespace MCForge
{
    public class CmdGifToCin : Command
    {
        public override string name { get { return "giftocin"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }

        String msg = "";
        int picHeight/* = 0*/;
        int picWidth/* = 0*/;
        CmdImagePrint2 imgprnt = new CmdImagePrint2();
        CmdSCinema scin = new CmdSCinema();
        CmdCuboid cuboid = new CmdCuboid();
        block[] blox = new block[2];
        String pllvl;
        block ppos;
        public override void Use(Player p, string message)
        {
            //first check if file exists
            if (File.Exists("extra/images/" + message + ".gif"))
            {
                p.SendMessage("Place 2 Blocks to Determine the Direction");
                p.ClearBlockchange();
                //happens when block is changed. then call blockchange1
                msg = message;
                p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);

            }
            else
            {
                Player.SendMessage(p, "File does not exist");
                return;
            }
        }

        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte Type)
        {
            //get the pos of first block
            p.ClearBlockchange();
            byte t = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, t);
            //undone change
            blox[0].x = x;
            blox[0].y = y;
            blox[0].z = z;
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }

        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte Type)
        {
            ppos.x = p.pos[0];
            ppos.y = p.pos[1];
            ppos.z = p.pos[2];
            pllvl = p.level.name;
            blox[1].x = x;
            blox[1].y = y;
            blox[1].z = z;// jetz direction rausfinden und dann unten verwenden. 
            int direction; //1 = 0,0,0 nach 1,0,0  2= 0,0,0 nach 0,1,0  3= 1,0,0 nach 0 und 4 = 010 nach 0
            if (blox[1].x > blox[0].x)
            {
                direction = 1;
                p.SendMessage("dir1");
            }
            else if (blox[1].x < blox[0].x)
            {
                direction = 3;
                p.SendMessage("dir3");
            }
            else if (blox[1].z > blox[0].z)
            {
                direction = 2;
                p.SendMessage("dir2");
            }
            else if (blox[1].z < blox[0].z)
            {
                direction = 4;
                p.SendMessage("dir4");
            }
            else
            {
                direction = 1;
                p.SendMessage("else");
            }

            String cinName = "";
            cinName = msg;
            using (Stream imageStreamSource = new FileStream("extra/images/" + msg + ".gif", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                GifBitmapDecoder decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                for (int i = 0; i < decoder.Frames.Count; i++)
                {
                    //saving all frames as pngs.
                    BitmapSource bitmapSource = decoder.Frames[i];
                    using (FileStream fs = new FileStream("extra/images/" + i.ToString(CultureInfo.CurrentCulture) + ".bmp", FileMode.Create))
                    {
                        BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(fs);
                    }
                }
                using (System.Drawing.Bitmap tbmp = new System.Drawing.Bitmap("extra/images/0.bmp"))
                {
                    picHeight = tbmp.Height;
                    picWidth = tbmp.Width;
                    //create the new map... 
                    Command.all.Find("newlvl").Use(p, "gtctempmap " + picWidth.ToString(CultureInfo.CurrentCulture) + " " + picHeight.ToString(CultureInfo.CurrentCulture) + " " + picWidth.ToString(CultureInfo.CurrentCulture) + " space");
                    Command.all.Find("load").Use(p, "gtctempmap");
                    //moving the player to this map
                    Command.all.Find("move").Use(p, p.name + " gtctempmap");
                    System.Threading.Thread.Sleep(2000);
                    for (int i = 0; i < decoder.Frames.Count; i++)
                    {
                        p.SendMessage("Start processing Frame " + i);
                        workFrame(i, p, cinName, direction);
                        p.SendMessage("Done");
                    }
                    p.SendMessage("YAY! everything should be done");
                    Command.all.Find("move").Use(p, p.name + " " + pllvl);
                    unchecked { p.SendPos((byte)-1, ppos.x, ppos.y, ppos.z, 0, 0); }
                    Command.all.Find("deletelvl").Use(p, "gtctempmap");//deleting templvl
                    for (int i = 0; i < decoder.Frames.Count; i++)
                    {
                        File.Delete("extra/images/" + i.ToString(CultureInfo.CurrentCulture) + ".bmp");
                    }
                }
            }
        }

        public struct block
        {
            public ushort x;
            public ushort y;
            public ushort z;

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Object obj)
            {
                throw new NotImplementedException();
            }

            public static bool operator ==(block x, block y)
            {
                throw new NotImplementedException();
            }

            public static bool operator !=(block x, block y)
            {
                throw new NotImplementedException();
            }
        }

        public void workFrame(int number, Player p, String name, int dir)
        {
            block pos1 = new block();// startblock for imgprint and scinema
            block pos2 = new block();//endblock for imgprint
            block pos3 = new block();//endblock for savecinema
            switch (dir)
            {
                case 1: //1 = 0,0,0 nach 1,0,0 
                    pos1.x = 0;
                    pos1.y = 0;
                    pos1.z = 0;
                    pos2.x = 1;
                    pos2.y = 0;
                    pos2.z = 0;
                    pos3.x = (ushort)(picWidth - 1);
                    pos3.y = (ushort)(picHeight - 1);
                    pos3.z = 1;
                    break;
                case 2: //2= 0,0,0 nach 0,1,0
                    pos1.x = 1;
                    pos1.y = 0;
                    pos1.z = 0;
                    pos2.x = 1;
                    pos2.y = 0;
                    pos2.z = 1;
                    pos3.x = 0;
                    pos3.y = (ushort)(picHeight - 1);
                    pos3.z = (ushort)(picWidth - 1);
                    break;
                case 3: //3= 1,0,0 nach 0,0,0
                    pos1.x = (ushort)(picWidth - 1);
                    pos1.y = 0;
                    pos1.z = 1;
                    pos2.x = (ushort)(picWidth - 2);
                    pos2.y = 0;
                    pos2.z = 1;
                    pos3.x = 0;
                    pos3.y = (ushort)(picHeight - 1);
                    pos3.z = 0;
                    break;
                case 4: // 4 = 0,1,0 nach 0,0,0
                    pos1.x = 0;
                    pos1.y = 0;
                    pos1.z = (ushort)(picWidth - 1);
                    pos2.x = 0;
                    pos2.y = 0;
                    pos2.z = (ushort)(picWidth - 2);
                    pos3.x = 1;
                    pos3.y = (ushort)(picHeight - 1);
                    pos3.z = 0;
                    break;
                default:

                    break;
            }
            cuboid.Use(p, "air");
            cuboid.Blockchange1(p, 0, 0, 0, 1);
            cuboid.Blockchange2(p, (ushort)(picWidth - 1), (ushort)(picHeight - 1), (ushort)(picWidth - 1), 1);
            //System.Threading.Thread.Sleep(3000);
            imgprnt.Use(p, number.ToString(CultureInfo.CurrentCulture));
            imgprnt.Blockchange1(p, pos1.x, pos1.y, pos1.z, 1);
            imgprnt.Blockchange2(p, pos2.x, pos2.y, pos2.z, 1);
            //had to send the blockchanges manual. 
            //printed the image in the level...
            while (imgprnt.working)
            {
                System.Threading.Thread.Sleep(100);
            }
            scin.Use(p, name);
            scin.Blockchange1(p, pos1.x, pos1.y, pos1.z, 1);
            scin.Blockchange2(p, pos3.x, pos3.y, pos3.z, 1);
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/giftocin <Filename> - Converts a .gif file to a .cin file. u can play the .cin file with the pcinema command.");
        }
    }


}