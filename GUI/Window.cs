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
using System.ComponentModel;
using System.Data;
using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using MCForge;

namespace MCForge.Gui
{
    public partial class Window : Form
    {
        Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\." +
                                "([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

        // for cross thread use
        delegate void StringCallback(string s);
        delegate void PlayerListCallback(List<Player> players);
        delegate void ReportCallback(Report r);
        delegate void VoidDelegate();

        PlayerCollection pc = new PlayerCollection(new PlayerListView());
        LevelCollection lc = new LevelCollection(new LevelListView());

        public static event EventHandler Minimize;
        public NotifyIcon notifyIcon1 = new NotifyIcon();
        //  public static bool Minimized = false;

        internal static Server s;

        public Window()
        {
            InitializeComponent();
        }

        private void Window_Minimize(object sender, EventArgs e)
        {
            /*     if (!Minimized)
                  {
                      Minimized = true;
                      ntf.Text = "MCZall";
                      ntf.Icon = this.Icon;
                      ntf.Click += delegate
                      {
                          try
                          {
                              Minimized = false;
                              this.ShowInTaskbar = true;
                              this.Show();
                              WindowState = FormWindowState.Normal;
                          }
                          catch (Exception ex) { MessageBox.Show(ex.Message); }
                      };
                      ntf.Visible = true;
                      this.ShowInTaskbar = false;
                  } */
        }

        public static Window thisWindow;

        private void Window_Load(object sender, EventArgs e)
        {
            thisWindow = this;
            MaximizeBox = false;
            this.Text = "<server name here>";
            //this.Icon = new Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MCLawl.Lawl.ico"));

            this.Show();
            this.BringToFront();
            WindowState = FormWindowState.Normal;

            s = new Server();
            s.OnLog += WriteLine;
            s.OnCommand += newCommand;
            s.OnError += newError;
            s.OnSystem += newSystem;

            foreach (TabPage tP in tabControl1.TabPages)
                tabControl1.SelectTab(tP);
            tabControl1.SelectTab(tabControl1.TabPages[0]);

            s.HeartBeatFail += HeartBeatFail;
            s.OnURLChange += UpdateUrl;
            s.OnPlayerListChange += UpdateClientList;
            s.OnSettingsUpdate += SettingsUpdate;
            s.Start();
            notifyIcon1.Text = ("MCForge Server: " + Server.name).Truncate(64);

            this.notifyIcon1.ContextMenuStrip = this.iconContext;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);

            //if (File.Exists(Logger.ErrorLogPath))
            //txtErrors.Lines = File.ReadAllLines(Logger.ErrorLogPath);
            if (File.Exists("Changelog.txt"))
            {
                txtChangelog.Text = "Changelog for " + Server.Version + ":";
                foreach (string line in File.ReadAllLines(("Changelog.txt")))
                {
                    txtChangelog.AppendText("\r\n           " + line);
                }
            }

            // Bind player list
            dgvPlayers.DataSource = pc;
            dgvPlayers.Font = new Font("Calibri", 8.25f);

            dgvMaps.DataSource = new LevelCollection(new LevelListView());
            dgvMaps.Font = new Font("Calibri", 8.25f);

            System.Timers.Timer UpdateListTimer = new System.Timers.Timer(10000);
            UpdateListTimer.Elapsed += delegate
            {
                UpdateClientList(Player.players);
                UpdateMapList("'");
            }; UpdateListTimer.Start();

        }

        void SettingsUpdate()
        {
            if (Server.shuttingDown) return;
            if (txtLog.InvokeRequired)
            {
                VoidDelegate d = new VoidDelegate(SettingsUpdate);
                this.Invoke(d);
            }
            else
            {
                this.Text = Server.name + " MCForge Version: " + Server.Version;
            }
        }

        void HeartBeatFail()
        {
            WriteLine("Recent Heartbeat Failed");
        }

        void newError(string message)
        {
            try
            {
                if (txtErrors.InvokeRequired)
                {
                    LogDelegate d = new LogDelegate(newError);
                    this.Invoke(d, new object[] { message });
                }
                else
                {
                    txtErrors.AppendText(Environment.NewLine + message);
                }
            }
            catch { }
        }
        void newSystem(string message)
        {
            try
            {
                if (txtSystem.InvokeRequired)
                {
                    LogDelegate d = new LogDelegate(newSystem);
                    this.Invoke(d, new object[] { message });
                }
                else
                {
                    txtSystem.AppendText(Environment.NewLine + message);
                }
            }
            catch { }
        }

        delegate void LogDelegate(string message);

        /// <summary>
        /// Does the same as Console.WriteLine() only in the form
        /// </summary>
        /// <param name="s">The line to write</param>
        public void WriteLine(string s)
        {
            if (Server.shuttingDown) return;
            if (this.InvokeRequired)
            {
                LogDelegate d = new LogDelegate(WriteLine);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                //txtLog.AppendText(Environment.NewLine + s);
                txtLog.AppendTextAndScroll(s);
            }
        }

        /// <summary>
        /// Updates the list of client names in the window
        /// </summary>
        /// <param name="players">The list of players to add</param>
        public void UpdateClientList(List<Player> players)
        {

            if (this.InvokeRequired)
            {
                PlayerListCallback d = new PlayerListCallback(UpdateClientList);
                this.Invoke(d, new object[] { players });
            }
            else
            {

                if (dgvPlayers.DataSource == null)
                    dgvPlayers.DataSource = pc;

                // Try to keep the same selection on update
                string selected = null;
                if (pc.Count > 0 && dgvPlayers.SelectedRows.Count > 0)
                {
                    selected = (from DataGridViewRow row in dgvPlayers.Rows where row.Selected select pc[row.Index]).First().name;
                }

                // Update the data source and control
                //dgvPlayers.SuspendLayout();

                pc = new PlayerCollection(new PlayerListView());
                Player.players.ForEach(delegate(Player p) { pc.Add(p); });

                //dgvPlayers.Invalidate();
                dgvPlayers.DataSource = pc;
                // Reselect player
                if (selected != null)
                {
                    foreach (Player p in Player.players)
                        foreach (DataGridViewRow row in dgvPlayers.Rows)
                            if (String.Equals(row.Cells[0].Value, selected))
                                row.Selected = true;
                }

                dgvPlayers.Refresh();
                //dgvPlayers.ResumeLayout();
            }

        }

        public void UpdateMapList(string blah)
        {
            /*
            if (this.InvokeRequired) {
                LogDelegate d = new LogDelegate(UpdateMapList);
                this.Invoke(d, new object[] { blah });
            } else {
                LevelCollection lc = new LevelCollection(new LevelListView());
                Server.levels.ForEach(delegate(Level l) { lc.Add(l); });
                dgvMaps.SuspendLayout();
                dgvMaps.DataSource = lc;
                //dgvMaps.Invalidate();
                dgvMaps.ResumeLayout();
            }
            */
            if (this.InvokeRequired)
            {
                LogDelegate d = new LogDelegate(UpdateMapList);
                this.Invoke(d, new object[] { blah });
            }
            else
            {

                if (dgvMaps.DataSource == null)
                    dgvMaps.DataSource = lc;

                // Try to keep the same selection on update
                string selected = null;
                if (lc.Count > 0 && dgvMaps.SelectedRows.Count > 0)
                {
                    selected = (from DataGridViewRow row in dgvMaps.Rows where row.Selected select lc[row.Index]).First().name;
                }

                // Update the data source and control
                //dgvPlayers.SuspendLayout();

                lc = new LevelCollection(new LevelListView());
                Server.levels.ForEach(delegate(Level l) { lc.Add(l); });

                //dgvPlayers.Invalidate();
                dgvMaps.DataSource = lc;
                // Reselect map
                if (selected != null)
                {
                    foreach (Level l in Server.levels)
                        foreach (DataGridViewRow row in dgvMaps.Rows)
                            if (String.Equals(row.Cells[0].Value, selected))
                                row.Selected = true;
                }

                dgvMaps.Refresh();
                //dgvPlayers.ResumeLayout();
            }
        }

        /// <summary>
        /// Places the server's URL at the top of the window
        /// </summary>
        /// <param name="s">The URL to display</param>
        public void UpdateUrl(string s)
        {
            if (this.InvokeRequired)
            {
                StringCallback d = new StringCallback(UpdateUrl);
                this.Invoke(d, new object[] { s });
            }
            else
                txtUrl.Text = s;
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (Server.restarting == true || MessageBox.Show("Really Shutdown the Server? All Connections will break!", "Exit", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                RemoveNotifyIcon();
                MCForge_.Gui.Program.ExitProgram(false);
            }
            else
            {
                // Prevents form from closing when user clicks the X and then hits 'cancel'
                e.Cancel = true;
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtInput.Text == null || txtInput.Text.Trim() == "") { return; }
                string text = txtInput.Text.Trim();
                string newtext = text;
                if (txtInput.Text[0] == '#')
                {
                    newtext = text.Remove(0, 1).Trim();
                    Player.GlobalMessageOps("To Ops &f-" + Server.DefaultColor + "Console [&a" + Server.ZallState + Server.DefaultColor + "]&f- " + newtext);
                    Server.s.Log("(OPs): Console: " + newtext);
                    IRCBot.Say("Console: " + newtext, true);
                    //   WriteLine("(OPs):<CONSOLE> " + txtInput.Text);
                    txtInput.Clear();
                }
                else
                {
                    Player.GlobalMessage("Console [&a" + Server.ZallState + Server.DefaultColor + "]: &f" + txtInput.Text);
                    IRCBot.Say("Console [" + Server.ZallState + "]: " + txtInput.Text);
                    WriteLine("<CONSOLE> " + txtInput.Text);
                    txtInput.Clear();
                }
            }
        }

        private void txtCommands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sentCmd = "", sentMsg = "";

                if (txtCommands.Text == null || txtCommands.Text.Trim() == "")
                {
                    newCommand("CONSOLE: Whitespace commands are not allowed.");
                    txtCommands.Clear();
                    return;
                }

                if (txtCommands.Text[0] == '/')
                    if (txtCommands.Text.Length > 1)
                        txtCommands.Text = txtCommands.Text.Substring(1);

                if (txtCommands.Text.IndexOf(' ') != -1)
                {
                    sentCmd = txtCommands.Text.Split(' ')[0];
                    sentMsg = txtCommands.Text.Substring(txtCommands.Text.IndexOf(' ') + 1);
                }
                else if (txtCommands.Text != "")
                {
                    sentCmd = txtCommands.Text;
                }
                else
                {
                    return;
                }

                try
                {
                    Command.all.Find(sentCmd).Use(null, sentMsg);
                    newCommand("CONSOLE: USED /" + sentCmd + " " + sentMsg);
                }
                catch (Exception ex)
                {
                    Server.ErrorLog(ex);
                    newCommand("CONSOLE: Failed command.");
                }

                txtCommands.Clear();
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Really Shutdown the Server? All Connections will break!", "Exit", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                MCForge_.Gui.Program.ExitProgram(false);
            }
        }

        public void newCommand(string p)
        {
            if (txtCommandsUsed.InvokeRequired)
            {
                LogDelegate d = new LogDelegate(newCommand);
                this.Invoke(d, new object[] { p });
            }
            else
            {
                txtCommandsUsed.AppendTextAndScroll(p);
            }
        }

        void ChangeCheck(string newCheck) { Server.ZallState = newCheck; }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            if (txtHost.Text != "") ChangeCheck(txtHost.Text);
        }

        private void btnProperties_Click_1(object sender, EventArgs e)
        {
            if (!prevLoaded) { PropertyForm = new PropertyWindow(); prevLoaded = true; }
            PropertyForm.Show();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (!MCForge_.Gui.Program.CurrentUpdate)
                MCForge_.Gui.Program.UpdateCheck();
            else
            {
                Thread messageThread = new Thread(new ThreadStart(delegate
                {
                    MessageBox.Show("Already checking for updates.");
                })); messageThread.Start();
            }
        }

        public static bool prevLoaded = false;
        Form PropertyForm;
        Form UpdateForm;

        private void gBChat_Enter(object sender, EventArgs e)
        {

        }

        private void btnExtra_Click_1(object sender, EventArgs e)
        {
            if (!prevLoaded) { PropertyForm = new PropertyWindow(); prevLoaded = true; }
            PropertyForm.Show();
            PropertyForm.Top = this.Top + this.Height - txtCommandsUsed.Height;
            PropertyForm.Left = this.Left;
        }

        private void Window_Resize(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.BringToFront();
            WindowState = FormWindowState.Normal;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            UpdateForm = new UpdateWindow();
            UpdateForm.Show();
        }

        private void tmrRestart_Tick(object sender, EventArgs e)
        {
            if (Server.autorestart)
            {
                if (DateTime.Now.TimeOfDay.CompareTo(Server.restarttime.TimeOfDay) > 0 && (DateTime.Now.TimeOfDay.CompareTo(Server.restarttime.AddSeconds(1).TimeOfDay)) < 0)
                {
                    Player.GlobalMessage("The time is now " + DateTime.Now.TimeOfDay);
                    Player.GlobalMessage("The server will now begin auto restart procedures.");
                    Server.s.Log("The time is now " + DateTime.Now.TimeOfDay);
                    Server.s.Log("The server will now begin auto restart procedures.");

                    RemoveNotifyIcon();
                    Process Restarter = new Process();

                    Restarter.StartInfo.FileName = "Restarter.exe";
                    Restarter.StartInfo.Arguments = "Program.cs";

                    Restarter.Start();
                }
            }
        }

        private void openConsole_Click(object sender, EventArgs e)
        {
            // Yes, it's a hacky fix.  Don't ask :v
            this.Show();
            this.BringToFront();
            WindowState = FormWindowState.Normal;
            this.Show();
            this.BringToFront();
            WindowState = FormWindowState.Normal;
        }

        private void shutdownServer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Really Quit?", "Exit", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                RemoveNotifyIcon();
                MCForge_.Gui.Program.ExitProgram(false);
            }

        }

        private Player GetSelectedPlayer()
        {
            if (this.dgvPlayers.SelectedRows == null)
                return null;

            if (this.dgvPlayers.SelectedRows.Count == 0)
                return null;

            return (Player)(this.dgvPlayers.SelectedRows[0].DataBoundItem);
        }

        private Level GetSelectedLevel()
        {
            if (this.dgvMaps.SelectedRows == null)
                return null;

            if (this.dgvMaps.SelectedRows.Count == 0)
                return null;

            return (Level)(this.dgvMaps.SelectedRows[0].DataBoundItem);
        }

        private void clonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player p = GetSelectedPlayer();
            if (p != null)
            {
                Command.all.Find("clones").Use(null, p.name);
            }
        }

        private void voiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player p = GetSelectedPlayer();
            if (p != null)
            {
                Command.all.Find("voice").Use(null, p.name);
            }
        }

        private void whoisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player p = GetSelectedPlayer();
            if (p != null)
            {
                Command.all.Find("whois").Use(null, p.name);
            }
        }

        private void kickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player p = GetSelectedPlayer();
            if (p != null)
            {
                Command.all.Find("kick").Use(null, p.name + " You have been kicked by the console.");
            }
        }


        private void banToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player p = GetSelectedPlayer();
            if (p != null)
            {
                Command.all.Find("ban").Use(null, p.name);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("physics").Use(null, l.name + " 0");
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("physics").Use(null, l.name + " 1");
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("physics").Use(null, l.name + " 2");
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("physics").Use(null, l.name + " 3");
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("physics").Use(null, l.name + " 4");
            }
        }

        private void unloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("unload").Use(null, l.name);
            }
        }

        private void finiteModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("map").Use(null, l.name + " finite");
            }
        }

        private void animalAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("map").Use(null, l.name + " ai");
            }
        }

        private void edgeWaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("map").Use(null, l.name + " edge");
            }
        }

        private void growingGrassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("map").Use(null, l.name + " grass");
            }
        }

        private void survivalDeathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("map").Use(null, l.name + " death");
            }
        }

        private void killerBlocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("map").Use(null, l.name + " killer");
            }
        }

        private void rPChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("map").Use(null, l.name + " chat");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Level l = GetSelectedLevel();
            if (l != null)
            {
                Command.all.Find("save").Use(null, l.name);
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            foreach (TabPage tP in tabControl1.TabPages)
            {
                foreach (Control ctrl in tP.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        TextBox txtBox = (TextBox)ctrl;
                        txtBox.Update();

                    }
                }
            }
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restart?", "Restart", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                RemoveNotifyIcon();
                Process Restarter = new Process();

                Restarter.StartInfo.FileName = "Restarter.exe";
                Restarter.StartInfo.Arguments = "Program.cs";

                Restarter.Start();
            }

        }

        private void restartServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Restart_Click(sender, e);
        }

        private void DatePicker1_ValueChanged(object sender, EventArgs e)
        {
            string dayofmonth = dateTimePicker1.Value.Day.ToString().PadLeft(2, '0');
            string year = dateTimePicker1.Value.Year.ToString();
            string month = dateTimePicker1.Value.Month.ToString().PadLeft(2, '0');

            string ymd = year + "-" + month + "-" + dayofmonth;
            string filename = ymd + ".txt";

            if (!File.Exists(Path.Combine("logs/", filename)))
            {
                //MessageBox.Show("Sorry, the log for " + ymd + " doesn't exist, please select another one");
                LogsTxtBox.Text = "No logs found for: " + ymd;
            }
            else
            {
                LogsTxtBox.Text = null;
                LogsTxtBox.Text = File.ReadAllText(Path.Combine("logs/", filename));
            }

        }

        private void txtUrl_DoubleClick(object sender, EventArgs e)
        {
            txtUrl.SelectAll();
        }

        private void RemoveNotifyIcon()
        {
            if (notifyIcon1 != null)
            {
                notifyIcon1.Visible = false;
                notifyIcon1.Dispose();
            }
        }

        private void dgvPlayers_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }
    }
}
