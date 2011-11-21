/*
    Copyright 2011 MCForge/ForgeCraft team
	
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
using System.Text;

namespace MCForge.Remote
{
    public partial class Remote
    {
        public bool HandleLogin(string message)
        {
            if (!String.IsNullOrEmpty(message))
                return message.Split(':')[0] == RemoteServer.username && message.Split(':')[1] == RemoteServer.password;
            return false;
        }
        public void RemoteChat(string m)
        {
            if (String.IsNullOrEmpty(m)) return;
            switch (m[0])
            {
                case '/':
                    {
                        m = m.Remove(0, 1);
                        string[] args = m.Split(' ');
                        string cmd = args[0];
                        Command command = Command.all.Find(cmd);

                        if (command == null)
                        {
                            Server.s.Log("Unrecognized command: " + cmd);
                            return;
                        }

                        StringBuilder lol = new StringBuilder();
                        for (int i = 1; i <= args.Length - 1; i++)
                        {
                            lol.Append(" " + args[i]);
                        }

                        command.Use(null, lol.ToString().Trim());
                        return;

                    }

                case '#':
                    {
                        m = m.Remove(0, 1);
                        Gui.Window.thisWindow.WriteOp("[Remote]: " + m);
                        if (this.RemoteType == 1)
                        {
                            string messaged = new StringBuilder().Append("Remote").Append("ĥ").Append(m).ToString();
                            messaged = EncryptMobile(messaged, _keyMobile);
                            byte[] buffed = new byte[(messaged.Length*2) + 3];
                            util.EndianBitConverter.Big.GetBytes((short) messaged.Length).CopyTo(buffed, 1);
                            buffed[0] = (byte) 2;
                            Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                            SendData(0x05, buffed);
                        }
                        Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + "To Ops -Remote- " + m + Environment.NewLine);
                            //Did it like this to avoid OnLog event
                        Player.GlobalMessageOps("%fTo Ops -%6Remote%f- " + m);
                        return;
                    }
                case '~':
                    {
                        m = m.Remove(0, 1);
                        Gui.Window.thisWindow.WriteAdmin("[Remote]: " + m);


                        string messaged = new StringBuilder().Append("Remote").Append("ĥ").Append(m).ToString();
                        messaged = EncryptMobile(messaged, _keyMobile);
                        byte[] buffed = new byte[(messaged.Length * 2) + 3];
                        util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                        buffed[0] = (byte)4;
                        Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                        SendData(0x05, buffed);

                        Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + "To Admins -Remote- " + m + Environment.NewLine);  //Did it like this to avoid OnLog event
                        Player.GlobalMessageAdmins("%fTo Admins -%6Remote%f- " + m);
                        return;

                    }
                default:
                    Gui.Window.thisWindow.WriteLine("[Remote]: " + m);
                    if (RemoteType == 1)
                    {
                        string messaged = new StringBuilder().Append("Remote").Append("ĥ").Append(m).ToString();
                        messaged = EncryptMobile(messaged, _keyMobile);
                        byte[] buffed = new byte[(messaged.Length * 2) + 3];
                        util.EndianBitConverter.Big.GetBytes((short)messaged.Length).CopyTo(buffed, 1);
                        buffed[0] = 4;
                        Encoding.BigEndianUnicode.GetBytes(messaged).CopyTo(buffed, 3);
                        SendData(0x05, buffed);
                        buffed[0] = 1;
                        SendData(0x05, buffed);
                    }
                    else
                    {
                        m = "[Remote]: " + m;
                        SendData(18, new byte[]{0xA});
                    }
                    Logger.Write(string.Format("{0}{1}{2}", DateTime.Now.ToString("(HH:mm:ss) "), m, Environment.NewLine));  //Did it like this to avoid OnLog event
                    Player.GlobalMessage(string.Format("{0}[Remote]: {1}{2}", c.navy, c.white, m));
                    return;
            }
        }

    }
}
