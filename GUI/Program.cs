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
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using MCForge;

namespace MCForge_.Gui
{
    public static class Program
    {
        public static bool usingConsole = false;

        private static string CurrentVersionFile = "http://www.mcforge.net/curversion.txt";
        private static string DLLLocation = "http://www.mcforge.net/MCForge_.dll";
        private static string ChangelogLocation = "http://www.mcforge.net/changelog.txt";
        //private static string RevisionList = "http://www.mcforge.net/revs.txt";
        //private static string HeartbeatAnnounce = "http://www.mcforge.net/hbannounce.php";
        private static string ArchivePath = "http://www.mcforge.net/archives/exe/";

        [DllImport("kernel32")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static void GlobalExHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Server.ErrorLog(ex);
            Thread.Sleep(500);

            if (!Server.restartOnError)
                Program.restartMe();
            else
                Program.restartMe(false);
        }

        public static void ThreadExHandler(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Server.ErrorLog(ex);
            Thread.Sleep(500);

            if (!Server.restartOnError)
                Program.restartMe();
            else
                Program.restartMe(false);
        }

        public static void Main(string[] args)
        {
            if (Process.GetProcessesByName("MCForge").Length != 1)
            {
                foreach (Process pr in Process.GetProcessesByName("MCForge"))
                {
                    if (pr.MainModule.BaseAddress == Process.GetCurrentProcess().MainModule.BaseAddress)
                        if (pr.Id != Process.GetCurrentProcess().Id)
                            pr.Kill();
                }
            }

            PidgeonLogger.Init();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.GlobalExHandler);
            Application.ThreadException += new ThreadExceptionEventHandler(Program.ThreadExHandler);
            bool skip = false;
        remake:
            try
            {
                if (!File.Exists("Viewmode.cfg") || skip)
                {
                    StreamWriter SW = new StreamWriter(File.Create("Viewmode.cfg"));
                    SW.WriteLine("#This file controls how the console window is shown to the server host");
                    SW.WriteLine("#cli: True or False (Determines whether a CLI interface is used) (Set True if on Mono)");
                    SW.WriteLine("#high-quality: True or false (Determines whether the GUI interface uses higher quality objects)");
                    SW.WriteLine();
                    SW.WriteLine("cli = false");
                    SW.WriteLine("high-quality = true");
                    SW.Flush();
                    SW.Close();
                    SW.Dispose();
                }

                if (File.ReadAllText("Viewmode.cfg") == "") { skip = true; goto remake; }

                string[] foundView = File.ReadAllLines("Viewmode.cfg");
                if (foundView[0][0] != '#') { skip = true; goto remake; }

                if (foundView[4].Split(' ')[2].ToLower() == "true")
                {
                    Server s = new Server();
                    s.OnLog += Console.WriteLine;
                    s.OnCommand += Console.WriteLine;
                    s.OnSystem += Console.WriteLine;
                    s.Start();

                    Console.Title = Server.name + " - MCForge " + Server.Version;
                    usingConsole = true;
                    handleComm(Console.ReadLine());

                    //Application.Run();
                }
                else
                {

                    IntPtr hConsole = GetConsoleWindow();
                    if (IntPtr.Zero != hConsole)
                    {
                        ShowWindow(hConsole, 0);
                    }
                    UpdateCheck(true);
                    if (foundView[5].Split(' ')[2].ToLower() == "true")
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                    }

                    updateTimer.Elapsed += delegate { UpdateCheck(); }; updateTimer.Start();

                    Application.Run(new MCForge.Gui.Window());
                }
            }
            catch (Exception e) { Server.ErrorLog(e); return; }
        }

        public static void handleComm(string s)
        {
            string sentCmd = "", sentMsg = "";

            s = s.Trim(); // Make sure we have no whitespace!
            if (s.StartsWith("/")) s = s.Remove(0, 1);
            else goto talk;
            if (s.IndexOf(' ') != -1)
            {
                sentCmd = s.Split(' ')[0];
                sentMsg = s.Substring(s.IndexOf(' ') + 1);
            }
            else if (s != "")
            {
                sentCmd = s;
            }
            else
            {
                goto talk;
            }

            try
            {
                Command cmd = Command.all.Find(sentCmd);
                if (cmd != null)
                {
                    cmd.Use(null, sentMsg);
                    Console.WriteLine("CONSOLE: USED /" + sentCmd + " " + sentMsg);
                    handleComm(Console.ReadLine());
                    return;
                }
                else
                {
                    Console.WriteLine("CONSOLE: Unknown command.");
                    handleComm(Console.ReadLine());
                    return;
                }
            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
                Console.WriteLine("CONSOLE: Failed command.");
                handleComm(Console.ReadLine());
                return;
            }

        talk: handleComm("/say " + Server.DefaultColor + "Console [&a" + Server.ZallState + Server.DefaultColor + "]: &f" + s);
            handleComm(Console.ReadLine());
        }
        /*
        public static void handleComm(string s)
        {

            string sentCmd = "", sentMsg = "";
            List<string> cmdtest = new List<string>();
            cmdtest = Command.all.commandNames();
            //blank lines are considered accidental
            if (s == "")
            {
                handleComm(Console.ReadLine());
                return;
            }

            //commands all start with a slash

            if (s.IndexOf('/') == 0)
            {
                //remove the preceding slash
                s = s.Remove(0, 1);

                //continue parsing
                if (s.IndexOf(' ') != -1)
                {
                    sentCmd = s.Split(' ')[0];
                    sentMsg = s.Substring(s.IndexOf(' ') + 1);
                }
                else if (s != "")
                {
                    sentCmd = s;
                }
            }
            //anything else is treated as chat
            else
            {
                sentCmd = "say";
                sentMsg = Server.DefaultColor + "Console [&a" + Server.ZallState + Server.DefaultColor + "]: &f" + s;
            }



            try
            {

                Command cmd = Command.all.Find(sentCmd);
                if (cmd != null)
                {
                    cmd.Use(null, sentMsg);
                    Console.WriteLine("CONSOLE: USED /" + sentCmd + " " + sentMsg);
                    handleComm(Console.ReadLine());
                    return;
                }


            }
            catch (Exception e)
            {
                Server.ErrorLog(e);
                Console.WriteLine("CONSOLE: Failed command.");
                handleComm(Console.ReadLine());
                return;
            }
            //handleComm(Console.ReadLine());


        } */

        public static bool CurrentUpdate = false;
        static bool msgOpen = false;
        public static System.Timers.Timer updateTimer = new System.Timers.Timer(120 * 60 * 1000);

        public static void UpdateCheck(bool wait = false, Player p = null)
        {
            CurrentUpdate = true;
            Thread updateThread = new Thread(new ThreadStart(delegate
            {
                WebClient Client = new WebClient();

                if (wait) { if (!Server.checkUpdates) return; Thread.Sleep(10000); }
                try
                {
                    if (Client.DownloadString(Program.CurrentVersionFile) != Server.Version)
                    {
                        if (Server.autoupdate == true || p != null)
                        {
                            if (Server.autonotify == true || p != null)
                            {
                                if (p != null) Server.restartcountdown = "20";
                                Player.GlobalMessage("Update found. Prepare for restart in &f" + Server.restartcountdown + Server.DefaultColor + " seconds.");
                                Server.s.Log("Update found. Prepare for restart in " + Server.restartcountdown + " seconds.");
                                double nxtTime = Convert.ToDouble(Server.restartcountdown);
                                DateTime nextupdate = DateTime.Now.AddMinutes(nxtTime);
                                int timeLeft = Convert.ToInt32(Server.restartcountdown);
                                System.Timers.Timer countDown = new System.Timers.Timer();
                                countDown.Interval = 1000;
                                countDown.Start();
                                countDown.Elapsed += delegate
                                {
                                    if (Server.autoupdate == true || p != null)
                                    {
                                        Player.GlobalMessage("Updating in &f" + timeLeft + Server.DefaultColor + " seconds.");
                                        Server.s.Log("Updating in " + timeLeft + " seconds.");
                                        timeLeft = timeLeft - 1;
                                        if (timeLeft < 0)
                                        {
                                            Player.GlobalMessage("---UPDATING SERVER---");
                                            Server.s.Log("---UPDATING SERVER---");
                                            countDown.Stop();
                                            countDown.Dispose();
                                            PerformUpdate(false);
                                        }
                                    }
                                    else
                                    {
                                        Player.GlobalMessage("Stopping auto restart.");
                                        Server.s.Log("Stopping auto restart.");
                                        countDown.Stop();
                                        countDown.Dispose();
                                    }
                                };
                            }
                            else
                            {
                                PerformUpdate(false);
                            }

                        }
                        else
                        {
                            if (!msgOpen && !usingConsole)
                            {
                                if (Server.autonotify)
                                {
                                    msgOpen = true;
                                    if (MessageBox.Show("New version found. Would you like to update?", "Update?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        PerformUpdate(false);
                                    }
                                    msgOpen = false;
                                }
                                else { }
                            }
                            else
                            {
                                ConsoleColor prevColor = Console.ForegroundColor;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("An update was found!");
                                Console.WriteLine("Update using the file at " + DLLLocation + " and placing it over the top of your current MCForge_.dll!");
                                Console.ForegroundColor = prevColor;
                            }
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "No update found!");
                    }
                }
                catch { Server.s.Log("No web server found to update on."); }
                Client.Dispose();
                CurrentUpdate = false;
            })); updateThread.Start();
        }

        public static void PerformUpdate(bool oldrevision)
        {
            try
            {
                StreamWriter SW;
                if (!Server.mono)
                {
                    if (!File.Exists("Update.bat"))
                        SW = new StreamWriter(File.Create("Update.bat"));
                    else
                    {
                        if (File.ReadAllLines("Update.bat")[0] != "::Version 3")
                        {
                            SW = new StreamWriter(File.Create("Update.bat"));
                        }
                        else
                        {
                            SW = new StreamWriter(File.Create("Update_generated.bat"));
                        }
                    }
                    SW.WriteLine("::Version 3");
                    SW.WriteLine("TASKKILL /pid %2 /F");
                    SW.WriteLine("if exist MCForge_.dll.backup (erase MCForge_.dll.backup)");
                    SW.WriteLine("if exist MCForge_.dll (rename MCForge_.dll MCForge_.dll.backup)");
                    SW.WriteLine("if exist MCForge.new (rename MCForge.new MCForge_.dll)");
                    SW.WriteLine("start MCForge.exe");
                }
                else
                {
                    if (!File.Exists("Update.sh"))
                        SW = new StreamWriter(File.Create("Update.sh"));
                    else
                    {
                        if (File.ReadAllLines("Update.sh")[0] != "#Version 2")
                        {
                            SW = new StreamWriter(File.Create("Update.sh"));
                        }
                        else
                        {
                            SW = new StreamWriter(File.Create("Update_generated.sh"));
                        }
                    }
                    SW.WriteLine("#Version 2");
                    SW.WriteLine("#!/bin/bash");
                    SW.WriteLine("kill $2");
                    SW.WriteLine("rm MCForge_.dll.backup");
                    SW.WriteLine("mv MCForge_.dll MCForge.dll_.backup");
                    SW.WriteLine("wget " + DLLLocation);
                    SW.WriteLine("mono MCForge.exe");
                }

                SW.Flush(); SW.Close(); SW.Dispose();

                string filelocation = "";
                string verscheck = "";
                Process proc = Process.GetCurrentProcess();
                string assemblyname = proc.ProcessName + ".exe";
                if (!oldrevision)
                {
                    WebClient client = new WebClient();
                    Server.selectedrevision = client.DownloadString(Program.CurrentVersionFile);
                    client.Dispose();
                }
                verscheck = Server.selectedrevision.TrimStart('r');
                int vers = int.Parse(verscheck.Split('.')[0]);
                if (oldrevision) { filelocation = (Program.ArchivePath + Server.selectedrevision + ".exe"); }
                if (!oldrevision) { filelocation = (DLLLocation); }
                WebClient Client = new WebClient();
                Client.DownloadFile(filelocation, "MCLawl.new");
                Client.DownloadFile(Program.ChangelogLocation, "extra/Changelog.txt");

                // Its possible there are no levels or players loaded yet
                // Only save them if they exist, otherwise we fail-whale
                if (Server.levels != null && Server.levels.Any())
                    foreach (Level l in Server.levels)
                        if (Server.lava.active && Server.lava.HasMap(l.name)) l.saveChanges();
                        else l.Save();

                if (Player.players != null && Player.players.Any())
                    foreach (Player pl in Player.players) pl.save();

                string fileName;
                if (!Server.mono) fileName = "Update.bat";
                else fileName = "Update.sh";

                try
                {
                    if (MCForge.Gui.Window.thisWindow.notifyIcon1 != null)
                    {
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Icon = null;
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Visible = false;
                    }
                }
                catch { }

                Process p = Process.Start(fileName, "main " + System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
                p.WaitForExit();
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }

        static public void ExitProgram(Boolean AutoRestart)
        {
            Thread exitThread;
            Server.Exit();

            exitThread = new Thread(new ThreadStart(delegate
            {
                try
                {
                    if (MCForge.Gui.Window.thisWindow.notifyIcon1 != null)
                    {
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Icon = null;
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Visible = false;
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Dispose();
                    }
                }
                catch { }

                try
                {
                    saveAll();

                    if (AutoRestart == true) restartMe();
                    else Server.process.Kill();
                }
                catch
                {
                    Server.process.Kill();
                }
            })); exitThread.Start();
        }

        static public void restartMe(bool fullRestart = true)
        {
            Thread restartThread = new Thread(new ThreadStart(delegate
            {
                saveAll();

                Server.shuttingDown = true;
                Server.restarting = true;
                try
                {
                    if (MCForge.Gui.Window.thisWindow.notifyIcon1 != null)
                    {
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Icon = null;
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Visible = false;
                        MCForge.Gui.Window.thisWindow.notifyIcon1.Dispose();
                    }
                }
                catch { }

                if (Server.listen != null) Server.listen.Close();
                if (!Server.mono || fullRestart)
                {
                    Application.Restart();
                    Server.process.Kill();
                }
                else
                {
                    Server.s.Start();
                }
            }));
            restartThread.Start();
        }
        static public void saveAll()
        {
            try
            {
                List<Player> kickList = new List<Player>();
                kickList.AddRange(Player.players);
                foreach (Player p in kickList) { p.Kick("Server restarted! Rejoin!"); }
            }
            catch (Exception exc) { Server.ErrorLog(exc); }

            try
            {
                string level = null;
                foreach (Level l in Server.levels)
                {
                    if (!Server.lava.active || !Server.lava.HasMap(l.name))
                    {
                        level = level + l.name + "=" + l.physics + System.Environment.NewLine;
                        l.Save();
                    }
                    l.saveChanges();
                }

                if(!Server.AutoLoad) File.WriteAllText("text/autoload.txt", level);
            }
            catch (Exception exc) { Server.ErrorLog(exc); }
        }
    }
}

