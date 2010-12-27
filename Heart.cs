/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl) Licensed under the
	Educational Community License, Version 2.0 (the "License"); you may
	not use this file except in compliance with the License. You may
	obtain a copy of the License at
	
	http://www.osedu.org/licenses/ECL-2.0
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
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

        static string hash;
        public static string serverURL;
        static string DefaultParameters;
        static string players = "";
        static string worlds = "";

        //static BackgroundWorker worker;
        static Random MCForgeBeatSeed = new Random(Process.GetCurrentProcess().Id);
        static StreamWriter beatlogger;

        static System.Timers.Timer MinecraftBeatTimer = new System.Timers.Timer(500);
        static System.Timers.Timer MCForgeBeatTimer;

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

                MCForgeBeatTimer.Elapsed += delegate
                {
                    MCForgeBeatTimer.Interval = 300000;
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
            }));
            backupThread.Start();
        }

        public static bool Pump(Beat beat)
        {
            String beattype = beat.GetType().Name;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(beat.URL));

            beat.Parameters = DefaultParameters;

            if(beat.Log)
            {
                beatlogger = new StreamWriter("heartbeat.log", true);
            }

            int totalTries = 0;
            
    retry:  try
            {
                totalTries++;

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
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(formData, 0, formData.Length);
                        if (Server.logbeat && beat.Log)
                        {
                            beatlogger.WriteLine(beattype + " request sent at " + DateTime.Now.ToString());
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
                            beatlogger.WriteLine(beattype + " timeout detected at " + DateTime.Now.ToString());
                        }
                        goto retryStream;
                        //throw new WebException("Failed during request.GetRequestStream()", e.InnerException, e.Status, e.Response);
                    }
                    else if (Server.logbeat && beat.Log)
                    {
                        beatlogger.WriteLine(beattype + " non-timeout exception detected: " + e.Message);
                        beatlogger.Write("Stack Trace: " + e.StackTrace);
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
                            beatlogger.WriteLine(beattype + " response received at " + DateTime.Now.ToString());
                        }

                        if (hash == null && response.ContentLength > 0)
                        {
                            string line = responseReader.ReadLine();
                            if (Server.logbeat && beat.Log)
                            {        
                                beatlogger.WriteLine("Received: " + line);
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
                        beatlogger.WriteLine("Timeout detected at " + DateTime.Now.ToString());
                    }
                    Pump(beat);
                }
            }
            catch (Exception e)
            {
                if (Server.logbeat && beat.Log)
                {
                    beatlogger.WriteLine(beattype + " failure #" + totalTries + " at " + DateTime.Now.ToString());
                }
                if (totalTries < 3) goto retry;
                if (Server.logbeat && beat.Log)
                {
                    beatlogger.WriteLine("Failed three times.  Stopping.");
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
