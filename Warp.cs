using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace MCForge
{
    public static class Warp
    {
        public static List<Wrp> Warps = new List<Wrp>();
        private static List<Wrp> TempDeletedWarpsList = new List<Wrp>();
        private static List<Wrp> FailedLoadingWarpsList = new List<Wrp>();



        public static Wrp GetWarp(string name)
        {
            foreach (Wrp w in Warps)
            {
                if (w.name.ToLower(CultureInfo.CurrentCulture).Trim() == name.ToLower(CultureInfo.CurrentCulture).Trim())
                {
                    return w;
                }
            }
            return null;
        }

        public static void AddWarp(string name, Player p)
        {
            Wrp w = new Wrp();
            try
            {
                w.name = name;
                w.lvlname = p.level.name;
                w.x = p.pos[0];
                w.y = p.pos[1];
                w.z = p.pos[2];
                w.rotx = p.rot[0];
                w.roty = p.rot[1];
                Warps.Add(w);
                SAVE();
            }
            catch { }
        }

        public static void DeleteWarp(string name)
        {
            Wrp wa = new Wrp();
            foreach (Wrp w in Warps)
            {
                if (w.name.ToLower(CultureInfo.CurrentCulture).Trim() == name.ToLower(CultureInfo.CurrentCulture).Trim())
                {
                    wa = w;
                    break;
                }

            }
            TempDeletedWarpsList.Add(wa);
            Warps.Remove(wa);
            SAVE();
        }

        public static void MoveWarp(string Wrp, Player p)
        {
            Wrp w = new Wrp();
            w = GetWarp(Wrp);
            Wrp wa = new Wrp();
            try
            {
                Warps.Remove(w);
                wa.name = w.name;
                wa.lvlname = p.level.name;
                wa.x = p.pos[0];
                wa.y = p.pos[1];
                wa.z = p.pos[2];
                wa.rotx = p.rot[0];
                wa.roty = p.rot[1];
                Warps.Add(wa);
                SAVE();
            }
            catch { }
        }

        public static bool WarpExists(string name)
        {
            foreach (Wrp w in Warps)
            {
                if (w.name.ToLower(CultureInfo.CurrentCulture).Trim() == name.ToLower(CultureInfo.CurrentCulture).Trim())
                {
                    return true;
                }
            }
            return false;
        }

        public static void SAVE()
        {
            using (StreamWriter SW = new StreamWriter("extra/warps.save"))
            {
                foreach (Wrp warp in Warps)
                {
                    SW.WriteLine(warp.name + ":" + warp.lvlname + ":" + warp.x.ToString(CultureInfo.CurrentCulture) + ":" + warp.y.ToString(CultureInfo.CurrentCulture) + ":" + warp.z.ToString(CultureInfo.CurrentCulture) + ":" + warp.rotx.ToString(CultureInfo.CurrentCulture) + ":" + warp.roty.ToString(CultureInfo.CurrentCulture));
                }

                try
                {
                    if (TempDeletedWarpsList.Count >= 1)
                    {
                        SW.WriteLine("");
                        SW.WriteLine("#Deleted Warps:");
                        foreach (Wrp BAKwarp in TempDeletedWarpsList)
                        {
                            SW.WriteLine("#" + BAKwarp.name + ":" + BAKwarp.lvlname + ":" + BAKwarp.x.ToString(CultureInfo.CurrentCulture) + ":" + BAKwarp.y.ToString(CultureInfo.CurrentCulture) + ":" + BAKwarp.z.ToString(CultureInfo.CurrentCulture) + ":" + BAKwarp.rotx.ToString(CultureInfo.CurrentCulture) + ":" + BAKwarp.roty.ToString(CultureInfo.CurrentCulture));
                        }
                    }
                    TempDeletedWarpsList.Clear();
                }
                catch { Server.s.Log("Saving backups of deleted warps failed!"); }

                try
                {
                    if (FailedLoadingWarpsList.Count >= 1)
                    {
                        SW.WriteLine("#");
                        SW.WriteLine("#FAILED LOADING:");
                        foreach (Wrp FAILwarp in FailedLoadingWarpsList)
                        {
                            SW.WriteLine("#" + FAILwarp.name + ":" + FAILwarp.lvlname + ":" + FAILwarp.x.ToString(CultureInfo.CurrentCulture) + ":" + FAILwarp.y.ToString(CultureInfo.CurrentCulture) + ":" + FAILwarp.z.ToString(CultureInfo.CurrentCulture) + ":" + FAILwarp.rotx.ToString(CultureInfo.CurrentCulture) + ":" + FAILwarp.roty.ToString(CultureInfo.CurrentCulture));
                        }
                    }
                    FailedLoadingWarpsList.Clear();
                }
                catch { Server.s.Log("Saving failed loading warps failed!"); }
                SW.Dispose();
            }
        }

        public static void LOAD()
        {
            if (File.Exists("extra/warps.save"))
            {
                using (StreamReader SR = new StreamReader("extra/warps.save"))
                {
                    bool failed = false;
                    bool anyfailed = false;
                    string line;
                    while (SR.EndOfStream == false)
                    {
                        line = SR.ReadLine().ToLower(CultureInfo.CurrentCulture).Trim();
                        if (!line.StartsWith("#", StringComparison.CurrentCulture) && line.Contains(":"))
                        {
                            string[] LINE = line.ToLower(CultureInfo.CurrentCulture).Split(':');
                            Wrp warp = new Wrp();
                            failed = false;
                            try
                            {
                                warp.name = LINE[0];
                                warp.lvlname = LINE[1];
                                warp.x = ushort.Parse(LINE[2], CultureInfo.CurrentCulture);
                                warp.y = ushort.Parse(LINE[3], CultureInfo.CurrentCulture);
                                warp.z = ushort.Parse(LINE[4], CultureInfo.CurrentCulture);
                                warp.rotx = byte.Parse(LINE[5], CultureInfo.CurrentCulture);
                                warp.roty = byte.Parse(LINE[6], CultureInfo.CurrentCulture);
                            }
                            catch
                            {
                                Server.s.Log("Couldn't load a Warp! Look in the 'extra/warps.save' file to see the unloaded warp");
                                FailedLoadingWarpsList.Add(warp);
                                failed = true;
                                anyfailed = true;
                            }
                            if (failed == false)
                            {
                                Warps.Add(warp);
                            }
                        }
                    }
                    if (anyfailed == true)
                    {
                        SAVE();
                    }
                    SR.Dispose();
                }
            }
        }
    }

    public class Wrp
    {
        public string name;
        public string lvlname;
        public ushort x;
        public ushort y;
        public ushort z;
        public byte rotx;
        public byte roty;
    }
}
