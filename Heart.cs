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
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Collections;

namespace MCForge
{

    public static class Heart
    {
        //static int _timeout = 60 * 1000;

        private static int max_retries = 3;

        static string hash = null;
        public static string serverURL;
        static string DefaultParameters;
        //static string players = "";
        //static string worlds = "";

        //static BackgroundWorker worker;
        static Random MCForgeBeatSeed = new Random(Process.GetCurrentProcess().Id);
        static StreamWriter beatlogger;

        static System.Timers.Timer MinecraftBeatTimer = new System.Timers.Timer(500);
        static System.Timers.Timer MCForgeBeatTimer;

        static object Lock = new object();

        public static void Init()
        {
            if(Server.logbeat)
            {
                if(!File.Exists("heartbeat.log"))
                {
                    File.Create("heartbeat.log").Close();
                }
            }
            MCForgeBeatTimer = new System.Timers.Timer(1000 + MCForgeBeatSeed.Next(0, 2500));
            DefaultParameters = "port=" + Server.port +
                            "&max=" + Server.players +
                            "&name=" + UrlEncode(Server.name) +
                            "&public=" + Server.pub +
                            "&version=" + Server.version;

            Thread backupThread = new Thread(new ThreadStart(delegate
            {
                MinecraftBeatTimer.Elapsed += delegate
                {
                    MinecraftBeatTimer.Interval = 55000;
                    try
                    {
                        Pump(new MinecraftBeat());
                    }
                    catch (Exception e) { Server.ErrorLog(e); }
                };
                MinecraftBeatTimer.Start();

                Thread.Sleep(5000);

                MCForgeBeatTimer.Elapsed += delegate
                {
                    MCForgeBeatTimer.Interval = 10*60*1000; // 10 minutes
                    try
                    {
                        Pump(new MCForgeBeat());
                    }
                    catch (Exception e)
                    {
                        Server.ErrorLog(e);
                    }
                };
                MCForgeBeatTimer.Start();

                Thread.Sleep(5000);
                System.Timers.Timer WomBeat = new System.Timers.Timer(55000);
                WomBeat.Elapsed += delegate
                {
                    if (Server.WomDirect)
                    {
                        try
                        {
                            Pump(new WOMBeat());
                        }
                        catch (Exception e)
                        {
                            Server.ErrorLog(e);
                        }
                    }
                };
                WomBeat.Start();
            }));
            backupThread.Start();
        }

        public static bool Pump(Beat beat)
        {
            //lock (Lock)
            //{
                String beattype = beat.GetType().Name;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(beat.URL));

                beat.Parameters = DefaultParameters;

                if (beat.Log)
                {
                    beatlogger = new StreamWriter("heartbeat.log", true);
                }

                int totalTries = 0;
                int totalTriesStream = 0;

            retry: try
                {
                    totalTries++;
                    totalTriesStream = 0;

                    beat.Prepare();

                    // Set all the request settings
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                    byte[] formData = Encoding.ASCII.GetBytes(beat.Parameters);
                    request.ContentLength = formData.Length;
                    request.Timeout = 15000; // 15 seconds

          retryStream: try
                    {
                        totalTriesStream++;
                        using (Stream requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(formData, 0, formData.Length);
                            if (Server.logbeat && beat.Log)
                            {
                                BeatLog(beat, beattype + " request sent at " + DateTime.Now.ToString());
                            }
                            requestStream.Flush();
                            requestStream.Close();
                        }
                    }
                    catch (WebException e)
                    {
                        //Server.ErrorLog(e);
                        if (e.Status == WebExceptionStatus.Timeout)
                        {
                            if (Server.logbeat && beat.Log)
                            {
#if DEBUG
                                Server.s.Log(beattype + " timeout detected at " + DateTime.Now.ToString());
#endif
                                BeatLog(beat, beattype + " timeout detected at " + DateTime.Now.ToString());
                            }
                            if (totalTriesStream < max_retries)
                            {
                                goto retryStream;
                            }
                            else
                            {
                                if (Server.logbeat && beat.Log)
                                    BeatLog(beat, beattype + " timed out " + max_retries + " times. Aborting this request. " + DateTime.Now.ToString());
                                Server.s.Log(beattype + " timed out " + max_retries + " times. Aborting this request.");
                                //throw new WebException("Failed during request.GetRequestStream()", e.InnerException, e.Status, e.Response);
                                beatlogger.Close();
                                return false;
                            }
                        }
                        else if (Server.logbeat && beat.Log)
                        {
#if DEBUG
                            Server.s.Log(beattype + " non-timeout exception detected: " + e.Message);
#endif
                            BeatLog(beat, beattype + " non-timeout exception detected: " + e.Message);
                            BeatLog(beat, "Stack Trace: " + e.StackTrace);
                        }
                    }

                    //if (hash == null)
                    //{
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                        {
                            if (Server.logbeat && beat.Log)
                            {
#if DEBUG
                                Server.s.Log(beattype + " response received at " + DateTime.Now.ToString());
#endif
                                BeatLog(beat, beattype + " response received at " + DateTime.Now.ToString());
                            }

                            if (String.IsNullOrEmpty(hash) && response.ContentLength > 0)
                            {
                                // Instead of getting a single line, get the whole damn thing and we'll strip stuff out
                                string line = responseReader.ReadToEnd().Trim();
                                if (Server.logbeat && beat.Log)
                                {
                                    BeatLog(beat, "Received: " + line);
                                }

                                beat.OnPump(line);
                            }
                            else
                            {
                                beat.OnPump(String.Empty);
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.Timeout)
                    {
                        if (Server.logbeat && beat.Log)
                        {
#if DEBUG
                            Server.s.Log(beattype + " timeout detected at " + DateTime.Now.ToString());
#endif
                            BeatLog(beat, "Timeout detected at " + DateTime.Now.ToString());
                        }
                        Pump(beat);
                    }
                }
                catch (Exception e)
                {
                    if (Server.logbeat && beat.Log)
                    {
                        BeatLog(beat, beattype + " failure #" + totalTries + " at " + DateTime.Now.ToString());
                    }
                    if (totalTries < max_retries) goto retry;
                    if (Server.logbeat && beat.Log)
                    {
#if DEBUG
                        Server.s.Log(beattype + " failed " + max_retries + " times.  Stopping.");
#endif
                        BeatLog(beat, "Failed " + max_retries + " times.  Stopping.");
                        beatlogger.Close();
                    }
                    return false;
                }
                finally
                {
                    request.Abort();
                }
                if (beatlogger != null)
                {
                    beatlogger.Close();
                }
            //}
            return true;
        }

        public static string UrlEncode(string input)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if ((input[i] >= '0' && input[i] <= '9') ||
                    (input[i] >= 'a' && input[i] <= 'z') ||
                    (input[i] >= 'A' && input[i] <= 'Z') ||
                    input[i] == '-' || input[i] == '_' || input[i] == '.' || input[i] == '~')
                {
                    output.Append(input[i]);
                }
                else if (Array.IndexOf<char>(reservedChars, input[i]) != -1)
                {
                    output.Append('%').Append(((int)input[i]).ToString("X"));
                }
            }
            return output.ToString();
        }

        private static void BeatLog(Beat beat, string text)
        {
            if (Server.logbeat && beat.Log && beatlogger != null)
            {
                try
                {
                    beatlogger.WriteLine(text);
                }
                catch { }
            }
        }

        /* public static Int32 MACToInt()
        {
            var nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            if (Server.mono)
            {
                foreach (var nic in nics)
                {
                    if (nic.Name != "l0")
                    {
                        nics[0] = nic;
                        break;
                    }
                }
            }
            string MAC = nics[0].GetPhysicalAddress().ToString();
            Regex regex = new Regex(@"[^0-9a-f]");
            MAC = regex.Replace(MAC, "");
            Int32 seed = 0;
     retry: try
            {
                seed = Convert.ToInt32(MAC);
            }
            catch (OverflowException)
            {
                MAC = MAC.Remove(MAC.Length - 1);
                goto retry;
            }
            catch(FormatException)
            {
                seed = new Random().Next();
            }
            return seed;
        } */

        public static char[] reservedChars = { ' ', '!', '*', '\'', '(', ')', ';', ':', '@', '&',
                                                 '=', '+', '$', ',', '/', '?', '%', '#', '[', ']' };
    }

    public enum BeatType { Minecraft, TChalo, MCForge }
}
