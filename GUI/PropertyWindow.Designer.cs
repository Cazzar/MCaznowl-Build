/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)

Dual-licensed under the Educational Community License, Version 2.0 and
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
namespace MCForge.Gui
{
    partial class PropertyWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyWindow));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDiscard = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.txtCmdRanks = new System.Windows.Forms.TextBox();
            this.btnCmdHelp = new System.Windows.Forms.Button();
            this.txtCmdAllow = new System.Windows.Forms.TextBox();
            this.txtCmdLowest = new System.Windows.Forms.TextBox();
            this.txtCmdDisallow = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.listCommands = new System.Windows.Forms.ListBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.CustCmdTxtBox2 = new System.Windows.Forms.RichTextBox();
            this.btnDiscardcmd = new System.Windows.Forms.Button();
            this.btnSavecmd = new System.Windows.Forms.Button();
            this.btnIntextbox = new System.Windows.Forms.Button();
            this.btnUnload = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.CustCmdtxtBox = new System.Windows.Forms.TextBox();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.txtcmdranks2 = new System.Windows.Forms.TextBox();
            this.label74 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.extracmdpermnumber = new System.Windows.Forms.NumericUpDown();
            this.label72 = new System.Windows.Forms.Label();
            this.extracmdpermdesc = new System.Windows.Forms.TextBox();
            this.extracmdpermperm = new System.Windows.Forms.TextBox();
            this.listCommandsExtraCmdPerms = new System.Windows.Forms.ListBox();
            this.label24 = new System.Windows.Forms.Label();
            this.chkHelp = new System.Windows.Forms.CheckBox();
            this.chkPhysicsRest = new System.Windows.Forms.CheckBox();
            this.chkDeath = new System.Windows.Forms.CheckBox();
            this.chkCheap = new System.Windows.Forms.CheckBox();
            this.chkrankSuper = new System.Windows.Forms.CheckBox();
            this.txtBackup = new System.Windows.Forms.TextBox();
            this.txtafk = new System.Windows.Forms.TextBox();
            this.txtAFKKick = new System.Windows.Forms.TextBox();
            this.chkForceCuboid = new System.Windows.Forms.CheckBox();
            this.hackrank_kick = new System.Windows.Forms.CheckBox();
            this.chkIRC = new System.Windows.Forms.CheckBox();
            this.cmbIRCColour = new System.Windows.Forms.ComboBox();
            this.txtNick = new System.Windows.Forms.TextBox();
            this.txtIRCServer = new System.Windows.Forms.TextBox();
            this.txtChannel = new System.Windows.Forms.TextBox();
            this.txtOpChannel = new System.Windows.Forms.TextBox();
            this.ChkTunnels = new System.Windows.Forms.CheckBox();
            this.chkVerify = new System.Windows.Forms.CheckBox();
            this.chkWorld = new System.Windows.Forms.CheckBox();
            this.chkAutoload = new System.Windows.Forms.CheckBox();
            this.chkPublic = new System.Windows.Forms.CheckBox();
            this.cmbDefaultColour = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtMOTD = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtMaps = new System.Windows.Forms.TextBox();
            this.txtDepth = new System.Windows.Forms.TextBox();
            this.cmbDefaultRank = new System.Windows.Forms.ComboBox();
            this.cmbOpChat = new System.Windows.Forms.ComboBox();
            this.chkLogBeat = new System.Windows.Forms.CheckBox();
            this.cmbAdminChat = new System.Windows.Forms.ComboBox();
            this.chkTpToHigherRanks = new System.Windows.Forms.CheckBox();
            this.chkUseSQL = new System.Windows.Forms.CheckBox();
            this.cmbVerificationRank = new System.Windows.Forms.ComboBox();
            this.chkEnableVerification = new System.Windows.Forms.CheckBox();
            this.chkSpamControl = new System.Windows.Forms.CheckBox();
            this.txtGlobalChatNick = new System.Windows.Forms.TextBox();
            this.cmbGlobalChatColor = new System.Windows.Forms.ComboBox();
            this.lsChkSendAFKMain = new System.Windows.Forms.CheckBox();
            this.lsChkStartOnStartup = new System.Windows.Forms.CheckBox();
            this.lsNudVoteCount = new System.Windows.Forms.NumericUpDown();
            this.lsNudVoteTime = new System.Windows.Forms.NumericUpDown();
            this.lsCmbSetupRank = new System.Windows.Forms.ComboBox();
            this.lsNudFastLava = new System.Windows.Forms.NumericUpDown();
            this.lsNudKiller = new System.Windows.Forms.NumericUpDown();
            this.lsNudDestroy = new System.Windows.Forms.NumericUpDown();
            this.lsNudWater = new System.Windows.Forms.NumericUpDown();
            this.lsNudLayer = new System.Windows.Forms.NumericUpDown();
            this.lsNudLayerHeight = new System.Windows.Forms.NumericUpDown();
            this.lsNudLayerCount = new System.Windows.Forms.NumericUpDown();
            this.lsNudLayerTime = new System.Windows.Forms.NumericUpDown();
            this.lsNudRoundTime = new System.Windows.Forms.NumericUpDown();
            this.lsNudFloodTime = new System.Windows.Forms.NumericUpDown();
            this.lsCmbControlRank = new System.Windows.Forms.ComboBox();
            this.cmbGrieferStoneType = new System.Windows.Forms.ComboBox();
            this.chkGrieferStoneBan = new System.Windows.Forms.CheckBox();
            this.txtGrieferStone = new System.Windows.Forms.TextBox();
            this.cmbGrieferStoneRank = new System.Windows.Forms.ComboBox();
            this.lsNudLives = new System.Windows.Forms.NumericUpDown();
            this.cmbAFKKickPerm = new System.Windows.Forms.ComboBox();
            this.chkGuestLimitNotify = new System.Windows.Forms.CheckBox();
            this.sndcmddataChk = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnBlHelp = new System.Windows.Forms.Button();
            this.txtBlRanks = new System.Windows.Forms.TextBox();
            this.txtBlAllow = new System.Windows.Forms.TextBox();
            this.txtBlLowest = new System.Windows.Forms.TextBox();
            this.txtBlDisallow = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.listBlocks = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtMaxUndo = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.cmbColor = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.txtPermission = new System.Windows.Forms.TextBox();
            this.txtRankName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddRank = new System.Windows.Forms.Button();
            this.listRanks = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.chkPrmOnly = new System.Windows.Forms.CheckBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.lblGlobalChatColor = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.chkGlobalChat = new System.Windows.Forms.CheckBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.chkShowEmptyRanks = new System.Windows.Forms.CheckBox();
            this.chkIgnoreGlobal = new System.Windows.Forms.CheckBox();
            this.chkNotifyOnJoinLeave = new System.Windows.Forms.CheckBox();
            this.chkRepeatMessages = new System.Windows.Forms.CheckBox();
            this.txtRestartTime = new System.Windows.Forms.TextBox();
            this.txtMoneys = new System.Windows.Forms.TextBox();
            this.chkRestartTime = new System.Windows.Forms.CheckBox();
            this.chk17Dollar = new System.Windows.Forms.CheckBox();
            this.chkSmile = new System.Windows.Forms.CheckBox();
            this.label34 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.chkGrieferStone = new System.Windows.Forms.CheckBox();
            this.chkShutdown = new System.Windows.Forms.CheckBox();
            this.txtShutdown = new System.Windows.Forms.TextBox();
            this.hackrank_kick_time = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.txtBanMessage = new System.Windows.Forms.TextBox();
            this.txtCheap = new System.Windows.Forms.TextBox();
            this.chkBanMessage = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.txtRP = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.txtNormRp = new System.Windows.Forms.TextBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label76 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label32 = new System.Windows.Forms.Label();
            this.txtBackupLocation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkProfanityFilter = new System.Windows.Forms.CheckBox();
            this.tabIRC = new System.Windows.Forms.TabPage();
            this.grpSQL = new System.Windows.Forms.GroupBox();
            this.txtSQLPort = new System.Windows.Forms.TextBox();
            this.label70 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.txtSQLHost = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.txtSQLDatabase = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.txtSQLPassword = new System.Windows.Forms.TextBox();
            this.txtSQLUsername = new System.Windows.Forms.TextBox();
            this.grpIRC = new System.Windows.Forms.GroupBox();
            this.txtIRCPort = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.txtIrcId = new System.Windows.Forms.TextBox();
            this.chkIrcId = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblIRC = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.updateTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.notifyInGameUpdate = new System.Windows.Forms.CheckBox();
            this.autoUpdate = new System.Windows.Forms.CheckBox();
            this.chkUpdates = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.txtServerOwner = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.forceUpdateBtn = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.numPlayers = new System.Windows.Forms.NumericUpDown();
            this.chkAgreeToRules = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.numGuests = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.lblDefault = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.lblOpChat = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.chkAdminsJoinSilent = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtMain = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.editTxtsBt = new System.Windows.Forms.Button();
            this.chkWomDirect = new System.Windows.Forms.CheckBox();
            this.chkRestart = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ChkPort = new System.Windows.Forms.Button();
            this.ChkPortResult = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.lsBtnEndVote = new System.Windows.Forms.Button();
            this.lsBtnEndRound = new System.Windows.Forms.Button();
            this.lsBtnStopGame = new System.Windows.Forms.Button();
            this.lsBtnStartGame = new System.Windows.Forms.Button();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.lsBtnSaveSettings = new System.Windows.Forms.Button();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.label75 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.lsAddMap = new System.Windows.Forms.Button();
            this.lsRemoveMap = new System.Windows.Forms.Button();
            this.lsMapNoUse = new System.Windows.Forms.ListBox();
            this.lsMapUse = new System.Windows.Forms.ListBox();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.levelList = new System.Windows.Forms.TextBox();
            this.label78 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.chkEnableChangingLevels = new System.Windows.Forms.CheckBox();
            this.chkZombieOnlyServer = new System.Windows.Forms.CheckBox();
            this.chkUseLevelList = new System.Windows.Forms.CheckBox();
            this.chkNoPillaringDuringZombie = new System.Windows.Forms.CheckBox();
            this.ZombieName = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.chkNoLevelSavingDuringZombie = new System.Windows.Forms.CheckBox();
            this.chkNoRespawnDuringZombie = new System.Windows.Forms.CheckBox();
            this.chkZombieOnServerStart = new System.Windows.Forms.CheckBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.numCountReset = new System.Windows.Forms.NumericUpDown();
            this.label69 = new System.Windows.Forms.Label();
            this.numSpamMute = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.numSpamMessages = new System.Windows.Forms.NumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.listPasswords = new System.Windows.Forms.ListBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label84 = new System.Windows.Forms.Label();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.label83 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label79 = new System.Windows.Forms.Label();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabPage3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extracmdpermnumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudVoteCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudVoteTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudFastLava)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudKiller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudDestroy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudWater)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayerHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayerCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayerTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudRoundTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudFloodTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLives)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.tabIRC.SuspendLayout();
            this.grpSQL.SuspendLayout();
            this.grpIRC.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateTimeNumeric)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPlayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGuests)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCountReset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpamMute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpamMessages)).BeginInit();
            this.groupBox14.SuspendLayout();
            this.tabPage13.SuspendLayout();
            this.groupBox28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox27.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(336, 553);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDiscard
            // 
            this.btnDiscard.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscard.Location = new System.Drawing.Point(4, 553);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.Size = new System.Drawing.Size(75, 23);
            this.btnDiscard.TabIndex = 1;
            this.btnDiscard.Text = "Discard";
            this.btnDiscard.UseVisualStyleBackColor = true;
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(417, 553);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 8000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Information";
            this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.tabControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(488, 509);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Commands";
            this.toolTip.SetToolTip(this.tabPage3, "Which ranks can use which commands.");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage12);
            this.tabControl1.Location = new System.Drawing.Point(9, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(476, 502);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage6.Controls.Add(this.txtCmdRanks);
            this.tabPage6.Controls.Add(this.btnCmdHelp);
            this.tabPage6.Controls.Add(this.txtCmdAllow);
            this.tabPage6.Controls.Add(this.txtCmdLowest);
            this.tabPage6.Controls.Add(this.txtCmdDisallow);
            this.tabPage6.Controls.Add(this.label17);
            this.tabPage6.Controls.Add(this.label15);
            this.tabPage6.Controls.Add(this.label8);
            this.tabPage6.Controls.Add(this.listCommands);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(468, 476);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "Commands";
            // 
            // txtCmdRanks
            // 
            this.txtCmdRanks.Location = new System.Drawing.Point(9, 109);
            this.txtCmdRanks.Multiline = true;
            this.txtCmdRanks.Name = "txtCmdRanks";
            this.txtCmdRanks.ReadOnly = true;
            this.txtCmdRanks.Size = new System.Drawing.Size(225, 339);
            this.txtCmdRanks.TabIndex = 33;
            // 
            // btnCmdHelp
            // 
            this.btnCmdHelp.Location = new System.Drawing.Point(253, 419);
            this.btnCmdHelp.Name = "btnCmdHelp";
            this.btnCmdHelp.Size = new System.Drawing.Size(141, 29);
            this.btnCmdHelp.TabIndex = 34;
            this.btnCmdHelp.Text = "Help information";
            this.btnCmdHelp.UseVisualStyleBackColor = true;
            this.btnCmdHelp.Click += new System.EventHandler(this.btnCmdHelp_Click);
            // 
            // txtCmdAllow
            // 
            this.txtCmdAllow.Location = new System.Drawing.Point(108, 82);
            this.txtCmdAllow.Name = "txtCmdAllow";
            this.txtCmdAllow.Size = new System.Drawing.Size(86, 21);
            this.txtCmdAllow.TabIndex = 31;
            this.txtCmdAllow.LostFocus += new System.EventHandler(this.txtCmdAllow_TextChanged);
            // 
            // txtCmdLowest
            // 
            this.txtCmdLowest.Location = new System.Drawing.Point(108, 28);
            this.txtCmdLowest.Name = "txtCmdLowest";
            this.txtCmdLowest.Size = new System.Drawing.Size(86, 21);
            this.txtCmdLowest.TabIndex = 32;
            this.txtCmdLowest.LostFocus += new System.EventHandler(this.txtCmdLowest_TextChanged);
            // 
            // txtCmdDisallow
            // 
            this.txtCmdDisallow.Location = new System.Drawing.Point(108, 55);
            this.txtCmdDisallow.Name = "txtCmdDisallow";
            this.txtCmdDisallow.Size = new System.Drawing.Size(86, 21);
            this.txtCmdDisallow.TabIndex = 30;
            this.txtCmdDisallow.LostFocus += new System.EventHandler(this.txtCmdDisallow_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(52, 85);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 13);
            this.label17.TabIndex = 29;
            this.label17.Text = "And allow:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(28, 58);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 13);
            this.label15.TabIndex = 28;
            this.label15.Text = "But don\'t allow:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Lowest rank needed:";
            // 
            // listCommands
            // 
            this.listCommands.FormattingEnabled = true;
            this.listCommands.Location = new System.Drawing.Point(253, 19);
            this.listCommands.Name = "listCommands";
            this.listCommands.Size = new System.Drawing.Size(141, 394);
            this.listCommands.TabIndex = 26;
            this.listCommands.SelectedIndexChanged += new System.EventHandler(this.listCommands_SelectedIndexChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage7.Controls.Add(this.panel1);
            this.tabPage7.Controls.Add(this.CustCmdTxtBox2);
            this.tabPage7.Controls.Add(this.btnDiscardcmd);
            this.tabPage7.Controls.Add(this.btnSavecmd);
            this.tabPage7.Controls.Add(this.btnIntextbox);
            this.tabPage7.Controls.Add(this.btnUnload);
            this.tabPage7.Controls.Add(this.btnLoad);
            this.tabPage7.Controls.Add(this.btnCompile);
            this.tabPage7.Controls.Add(this.btnCreate);
            this.tabPage7.Controls.Add(this.label33);
            this.tabPage7.Controls.Add(this.CustCmdtxtBox);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(468, 476);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "Custom Commands";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(7, 26);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(84, 29);
            this.panel1.TabIndex = 37;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(41, 6);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(36, 16);
            this.radioButton1.TabIndex = 27;
            this.radioButton1.Text = "VB";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(2, 6);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(35, 16);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "C#";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // CustCmdTxtBox2
            // 
            this.CustCmdTxtBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustCmdTxtBox2.Location = new System.Drawing.Point(7, 61);
            this.CustCmdTxtBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CustCmdTxtBox2.Name = "CustCmdTxtBox2";
            this.CustCmdTxtBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.CustCmdTxtBox2.Size = new System.Drawing.Size(458, 382);
            this.CustCmdTxtBox2.TabIndex = 36;
            this.CustCmdTxtBox2.Text = "";
            this.CustCmdTxtBox2.WordWrap = false;
            // 
            // btnDiscardcmd
            // 
            this.btnDiscardcmd.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscardcmd.Location = new System.Drawing.Point(367, 449);
            this.btnDiscardcmd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnDiscardcmd.Name = "btnDiscardcmd";
            this.btnDiscardcmd.Size = new System.Drawing.Size(98, 23);
            this.btnDiscardcmd.TabIndex = 35;
            this.btnDiscardcmd.Text = "Discard";
            this.btnDiscardcmd.UseVisualStyleBackColor = true;
            this.btnDiscardcmd.Click += new System.EventHandler(this.btnDiscardcmd_Click);
            // 
            // btnSavecmd
            // 
            this.btnSavecmd.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSavecmd.Location = new System.Drawing.Point(9, 449);
            this.btnSavecmd.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSavecmd.Name = "btnSavecmd";
            this.btnSavecmd.Size = new System.Drawing.Size(103, 23);
            this.btnSavecmd.TabIndex = 34;
            this.btnSavecmd.Text = "Save Command";
            this.btnSavecmd.UseVisualStyleBackColor = true;
            this.btnSavecmd.Click += new System.EventHandler(this.btnSavecmd_Click);
            // 
            // btnIntextbox
            // 
            this.btnIntextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIntextbox.Location = new System.Drawing.Point(207, 32);
            this.btnIntextbox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnIntextbox.Name = "btnIntextbox";
            this.btnIntextbox.Size = new System.Drawing.Size(105, 23);
            this.btnIntextbox.TabIndex = 33;
            this.btnIntextbox.Text = "Load into Textbox";
            this.btnIntextbox.UseVisualStyleBackColor = true;
            this.btnIntextbox.Click += new System.EventHandler(this.btnIntextbox_Click);
            // 
            // btnUnload
            // 
            this.btnUnload.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnload.Location = new System.Drawing.Point(382, 32);
            this.btnUnload.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnUnload.Name = "btnUnload";
            this.btnUnload.Size = new System.Drawing.Size(83, 23);
            this.btnUnload.TabIndex = 32;
            this.btnUnload.Text = "Unload";
            this.btnUnload.UseVisualStyleBackColor = true;
            this.btnUnload.Click += new System.EventHandler(this.btnUnload_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(316, 32);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(62, 23);
            this.btnLoad.TabIndex = 31;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnCompile
            // 
            this.btnCompile.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompile.Location = new System.Drawing.Point(95, 32);
            this.btnCompile.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(105, 23);
            this.btnCompile.TabIndex = 30;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.Location = new System.Drawing.Point(316, 5);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(149, 23);
            this.btnCreate.TabIndex = 29;
            this.btnCreate.Text = "Create Command";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(3, 11);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(78, 12);
            this.label33.TabIndex = 28;
            this.label33.Text = "Command Name:";
            // 
            // CustCmdtxtBox
            // 
            this.CustCmdtxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustCmdtxtBox.Location = new System.Drawing.Point(95, 8);
            this.CustCmdtxtBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CustCmdtxtBox.Name = "CustCmdtxtBox";
            this.CustCmdtxtBox.Size = new System.Drawing.Size(217, 18);
            this.CustCmdtxtBox.TabIndex = 27;
            // 
            // tabPage12
            // 
            this.tabPage12.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage12.Controls.Add(this.txtcmdranks2);
            this.tabPage12.Controls.Add(this.label74);
            this.tabPage12.Controls.Add(this.label73);
            this.tabPage12.Controls.Add(this.extracmdpermnumber);
            this.tabPage12.Controls.Add(this.label72);
            this.tabPage12.Controls.Add(this.extracmdpermdesc);
            this.tabPage12.Controls.Add(this.extracmdpermperm);
            this.tabPage12.Controls.Add(this.listCommandsExtraCmdPerms);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new System.Drawing.Size(468, 476);
            this.tabPage12.TabIndex = 2;
            this.tabPage12.Text = "Additional Command Permissions";
            // 
            // txtcmdranks2
            // 
            this.txtcmdranks2.Location = new System.Drawing.Point(72, 189);
            this.txtcmdranks2.Multiline = true;
            this.txtcmdranks2.Name = "txtcmdranks2";
            this.txtcmdranks2.ReadOnly = true;
            this.txtcmdranks2.Size = new System.Drawing.Size(241, 273);
            this.txtcmdranks2.TabIndex = 46;
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(6, 70);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(64, 13);
            this.label74.TabIndex = 45;
            this.label74.Text = "Description:";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(3, 18);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(179, 13);
            this.label73.TabIndex = 44;
            this.label73.Text = "Command Extra Permission Number:";
            // 
            // extracmdpermnumber
            // 
            this.extracmdpermnumber.Location = new System.Drawing.Point(188, 16);
            this.extracmdpermnumber.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.extracmdpermnumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.extracmdpermnumber.Name = "extracmdpermnumber";
            this.extracmdpermnumber.Size = new System.Drawing.Size(120, 21);
            this.extracmdpermnumber.TabIndex = 43;
            this.extracmdpermnumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.extracmdpermnumber.ValueChanged += new System.EventHandler(this.extracmdpermnumber_ValueChanged);
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(7, 46);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(63, 13);
            this.label72.TabIndex = 42;
            this.label72.Text = "Permission:";
            // 
            // extracmdpermdesc
            // 
            this.extracmdpermdesc.Location = new System.Drawing.Point(72, 70);
            this.extracmdpermdesc.Multiline = true;
            this.extracmdpermdesc.Name = "extracmdpermdesc";
            this.extracmdpermdesc.ReadOnly = true;
            this.extracmdpermdesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.extracmdpermdesc.Size = new System.Drawing.Size(241, 113);
            this.extracmdpermdesc.TabIndex = 41;
            // 
            // extracmdpermperm
            // 
            this.extracmdpermperm.Location = new System.Drawing.Point(72, 43);
            this.extracmdpermperm.Name = "extracmdpermperm";
            this.extracmdpermperm.Size = new System.Drawing.Size(116, 21);
            this.extracmdpermperm.TabIndex = 40;
            // 
            // listCommandsExtraCmdPerms
            // 
            this.listCommandsExtraCmdPerms.FormattingEnabled = true;
            this.listCommandsExtraCmdPerms.Location = new System.Drawing.Point(319, 16);
            this.listCommandsExtraCmdPerms.Name = "listCommandsExtraCmdPerms";
            this.listCommandsExtraCmdPerms.Size = new System.Drawing.Size(141, 446);
            this.listCommandsExtraCmdPerms.TabIndex = 27;
            this.listCommandsExtraCmdPerms.SelectedIndexChanged += new System.EventHandler(this.listCommandsExtraCmdPerms_SelectedIndexChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(19, 53);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(48, 13);
            this.label24.TabIndex = 15;
            this.label24.Text = "/rp limit:";
            this.toolTip.SetToolTip(this.label24, "Limit for custom physics set by /rp");
            // 
            // chkHelp
            // 
            this.chkHelp.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkHelp.AutoSize = true;
            this.chkHelp.Location = new System.Drawing.Point(18, 56);
            this.chkHelp.Name = "chkHelp";
            this.chkHelp.Size = new System.Drawing.Size(56, 23);
            this.chkHelp.TabIndex = 20;
            this.chkHelp.Text = "Old help";
            this.toolTip.SetToolTip(this.chkHelp, "Should the old, cluttered help menu be used?");
            this.chkHelp.UseVisualStyleBackColor = true;
            // 
            // chkPhysicsRest
            // 
            this.chkPhysicsRest.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPhysicsRest.AutoSize = true;
            this.chkPhysicsRest.Location = new System.Drawing.Point(17, 20);
            this.chkPhysicsRest.Name = "chkPhysicsRest";
            this.chkPhysicsRest.Size = new System.Drawing.Size(89, 23);
            this.chkPhysicsRest.TabIndex = 22;
            this.chkPhysicsRest.Text = "Restart physics";
            this.toolTip.SetToolTip(this.chkPhysicsRest, "Restart physics on shutdown, clearing all physics blocks.");
            this.chkPhysicsRest.UseVisualStyleBackColor = true;
            // 
            // chkDeath
            // 
            this.chkDeath.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDeath.AutoSize = true;
            this.chkDeath.Location = new System.Drawing.Point(169, 27);
            this.chkDeath.Name = "chkDeath";
            this.chkDeath.Size = new System.Drawing.Size(75, 23);
            this.chkDeath.TabIndex = 21;
            this.chkDeath.Text = "Death count";
            this.toolTip.SetToolTip(this.chkDeath, "\"Bob has died 10 times.\"");
            this.chkDeath.UseVisualStyleBackColor = true;
            // 
            // chkCheap
            // 
            this.chkCheap.AutoSize = true;
            this.chkCheap.Location = new System.Drawing.Point(12, 75);
            this.chkCheap.Name = "chkCheap";
            this.chkCheap.Size = new System.Drawing.Size(103, 17);
            this.chkCheap.TabIndex = 23;
            this.chkCheap.Text = "Cheap message:";
            this.toolTip.SetToolTip(this.chkCheap, "Is immortality cheap and unfair?");
            this.chkCheap.UseVisualStyleBackColor = true;
            // 
            // chkrankSuper
            // 
            this.chkrankSuper.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkrankSuper.AutoSize = true;
            this.chkrankSuper.Location = new System.Drawing.Point(18, 84);
            this.chkrankSuper.Name = "chkrankSuper";
            this.chkrankSuper.Size = new System.Drawing.Size(195, 23);
            this.chkrankSuper.TabIndex = 24;
            this.chkrankSuper.Text = "SuperOPs can appoint other SuperOPs";
            this.toolTip.SetToolTip(this.chkrankSuper, "Does what it says on the tin");
            this.chkrankSuper.UseVisualStyleBackColor = true;
            // 
            // txtBackup
            // 
            this.txtBackup.Location = new System.Drawing.Point(89, 58);
            this.txtBackup.Name = "txtBackup";
            this.txtBackup.Size = new System.Drawing.Size(41, 21);
            this.txtBackup.TabIndex = 5;
            this.toolTip.SetToolTip(this.txtBackup, "How often should backups be taken, in seconds.\nDefault = 300");
            // 
            // txtafk
            // 
            this.txtafk.Location = new System.Drawing.Point(65, 17);
            this.txtafk.Name = "txtafk";
            this.txtafk.Size = new System.Drawing.Size(62, 21);
            this.txtafk.TabIndex = 10;
            this.toolTip.SetToolTip(this.txtafk, "How long the server should wait before declaring someone ask afk. (0 = No timer a" +
                    "t all)");
            // 
            // txtAFKKick
            // 
            this.txtAFKKick.Location = new System.Drawing.Point(65, 43);
            this.txtAFKKick.Name = "txtAFKKick";
            this.txtAFKKick.Size = new System.Drawing.Size(62, 21);
            this.txtAFKKick.TabIndex = 9;
            this.toolTip.SetToolTip(this.txtAFKKick, "Kick the user after they have been afk for this many minutes (0 = No kick)");
            // 
            // chkForceCuboid
            // 
            this.chkForceCuboid.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkForceCuboid.AutoSize = true;
            this.chkForceCuboid.Location = new System.Drawing.Point(523, 266);
            this.chkForceCuboid.Name = "chkForceCuboid";
            this.chkForceCuboid.Size = new System.Drawing.Size(78, 23);
            this.chkForceCuboid.TabIndex = 29;
            this.chkForceCuboid.Text = "Force Cuboid";
            this.toolTip.SetToolTip(this.chkForceCuboid, "When true, runs an attempted cuboid despite cuboid limits, until it hits the grou" +
                    "p limit for that user.");
            this.chkForceCuboid.UseVisualStyleBackColor = true;
            // 
            // hackrank_kick
            // 
            this.hackrank_kick.AutoSize = true;
            this.hackrank_kick.Location = new System.Drawing.Point(12, 47);
            this.hackrank_kick.Name = "hackrank_kick";
            this.hackrank_kick.Size = new System.Drawing.Size(193, 17);
            this.hackrank_kick.TabIndex = 32;
            this.hackrank_kick.Text = "Kick people who use hackrank after ";
            this.toolTip.SetToolTip(this.hackrank_kick, "Hackrank kicks people? Or not?");
            this.hackrank_kick.UseVisualStyleBackColor = true;
            // 
            // chkIRC
            // 
            this.chkIRC.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkIRC.AutoSize = true;
            this.chkIRC.Location = new System.Drawing.Point(22, 14);
            this.chkIRC.Name = "chkIRC";
            this.chkIRC.Size = new System.Drawing.Size(57, 23);
            this.chkIRC.TabIndex = 22;
            this.chkIRC.Text = "Use IRC";
            this.toolTip.SetToolTip(this.chkIRC, "Whether to use the IRC bot or not.\nIRC stands for Internet Relay Chat and allows " +
                    "for communication with the server while outside Minecraft.");
            this.chkIRC.UseVisualStyleBackColor = true;
            this.chkIRC.CheckedChanged += new System.EventHandler(this.chkIRC_CheckedChanged);
            // 
            // cmbIRCColour
            // 
            this.cmbIRCColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIRCColour.FormattingEnabled = true;
            this.cmbIRCColour.Location = new System.Drawing.Point(69, 166);
            this.cmbIRCColour.Name = "cmbIRCColour";
            this.cmbIRCColour.Size = new System.Drawing.Size(74, 21);
            this.cmbIRCColour.TabIndex = 23;
            this.toolTip.SetToolTip(this.cmbIRCColour, "The colour of the IRC nicks used in the IRC.");
            this.cmbIRCColour.SelectedIndexChanged += new System.EventHandler(this.cmbIRCColour_SelectedIndexChanged);
            // 
            // txtNick
            // 
            this.txtNick.Location = new System.Drawing.Point(48, 60);
            this.txtNick.Name = "txtNick";
            this.txtNick.Size = new System.Drawing.Size(106, 21);
            this.txtNick.TabIndex = 16;
            this.toolTip.SetToolTip(this.txtNick, "The Nick that the IRC bot will try and use.");
            this.txtNick.TextChanged += new System.EventHandler(this.txtNick_TextChanged);
            // 
            // txtIRCServer
            // 
            this.txtIRCServer.Location = new System.Drawing.Point(58, 24);
            this.txtIRCServer.Name = "txtIRCServer";
            this.txtIRCServer.Size = new System.Drawing.Size(123, 21);
            this.txtIRCServer.TabIndex = 15;
            this.toolTip.SetToolTip(this.txtIRCServer, "The IRC server to be used.\nDefault = irc.esper.net\nBetter choice = irc.foonetic.n" +
                    "et");
            // 
            // txtChannel
            // 
            this.txtChannel.Location = new System.Drawing.Point(67, 96);
            this.txtChannel.Name = "txtChannel";
            this.txtChannel.Size = new System.Drawing.Size(106, 21);
            this.txtChannel.TabIndex = 17;
            this.toolTip.SetToolTip(this.txtChannel, "The IRC channel to be used.");
            // 
            // txtOpChannel
            // 
            this.txtOpChannel.Location = new System.Drawing.Point(82, 131);
            this.txtOpChannel.Name = "txtOpChannel";
            this.txtOpChannel.Size = new System.Drawing.Size(106, 21);
            this.txtOpChannel.TabIndex = 26;
            this.toolTip.SetToolTip(this.txtOpChannel, "The IRC channel to be used.");
            // 
            // ChkTunnels
            // 
            this.ChkTunnels.Appearance = System.Windows.Forms.Appearance.Button;
            this.ChkTunnels.AutoSize = true;
            this.ChkTunnels.Location = new System.Drawing.Point(18, 20);
            this.ChkTunnels.Name = "ChkTunnels";
            this.ChkTunnels.Size = new System.Drawing.Size(85, 23);
            this.ChkTunnels.TabIndex = 4;
            this.ChkTunnels.Text = "Anti-Tunneling";
            this.toolTip.SetToolTip(this.ChkTunnels, "Should guests be limited to digging a certain depth?");
            this.ChkTunnels.UseVisualStyleBackColor = true;
            // 
            // chkVerify
            // 
            this.chkVerify.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkVerify.AutoSize = true;
            this.chkVerify.Location = new System.Drawing.Point(68, 22);
            this.chkVerify.Name = "chkVerify";
            this.chkVerify.Size = new System.Drawing.Size(79, 23);
            this.chkVerify.TabIndex = 4;
            this.chkVerify.Text = "Verify Names";
            this.toolTip.SetToolTip(this.chkVerify, "Make sure the user is who they claim to be.");
            this.chkVerify.UseVisualStyleBackColor = true;
            // 
            // chkWorld
            // 
            this.chkWorld.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWorld.AutoSize = true;
            this.chkWorld.Location = new System.Drawing.Point(103, 80);
            this.chkWorld.Name = "chkWorld";
            this.chkWorld.Size = new System.Drawing.Size(69, 23);
            this.chkWorld.TabIndex = 4;
            this.chkWorld.Text = "World chat";
            this.toolTip.SetToolTip(this.chkWorld, "If disabled, every map has isolated chat.\nIf enabled, every map is able to commun" +
                    "icate without special letters.");
            this.chkWorld.UseVisualStyleBackColor = true;
            // 
            // chkAutoload
            // 
            this.chkAutoload.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkAutoload.AutoSize = true;
            this.chkAutoload.Location = new System.Drawing.Point(16, 80);
            this.chkAutoload.Name = "chkAutoload";
            this.chkAutoload.Size = new System.Drawing.Size(85, 23);
            this.chkAutoload.TabIndex = 4;
            this.chkAutoload.Text = "Load on /goto";
            this.toolTip.SetToolTip(this.chkAutoload, "Load a map when a user wishes to go to it, and unload empty maps");
            this.chkAutoload.UseVisualStyleBackColor = true;
            // 
            // chkPublic
            // 
            this.chkPublic.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPublic.AutoSize = true;
            this.chkPublic.Location = new System.Drawing.Point(16, 22);
            this.chkPublic.Name = "chkPublic";
            this.chkPublic.Size = new System.Drawing.Size(46, 23);
            this.chkPublic.TabIndex = 4;
            this.chkPublic.Text = "Public";
            this.toolTip.SetToolTip(this.chkPublic, "Whether or not the server will appear on the server list.");
            this.chkPublic.UseVisualStyleBackColor = true;
            // 
            // cmbDefaultColour
            // 
            this.cmbDefaultColour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultColour.FormattingEnabled = true;
            this.cmbDefaultColour.Location = new System.Drawing.Point(89, 100);
            this.cmbDefaultColour.Name = "cmbDefaultColour";
            this.cmbDefaultColour.Size = new System.Drawing.Size(57, 21);
            this.cmbDefaultColour.TabIndex = 9;
            this.toolTip.SetToolTip(this.cmbDefaultColour, "The colour of the default chat used in the server.\nFor example, when you are aske" +
                    "d to select two corners in a cuboid.");
            this.cmbDefaultColour.SelectedIndexChanged += new System.EventHandler(this.cmbDefaultColour_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(57, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(333, 21);
            this.txtName.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtName, "The name of the server.\nPick something good!");
            // 
            // txtMOTD
            // 
            this.txtMOTD.Location = new System.Drawing.Point(57, 56);
            this.txtMOTD.Name = "txtMOTD";
            this.txtMOTD.Size = new System.Drawing.Size(333, 21);
            this.txtMOTD.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtMOTD, "The MOTD of the server.\nUse \"+hax\" to allow any WoM hack, \"-hax\" to disallow any " +
                    "hacks at all and use \"-fly\" and whatnot to disallow other things.");
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(57, 85);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(46, 21);
            this.txtPort.TabIndex = 2;
            this.toolTip.SetToolTip(this.txtPort, "The port that the server will output on.\nDefault = 25565\n\nChanging will reset you" +
                    "r ExternalURL.");
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // txtMaps
            // 
            this.txtMaps.Location = new System.Drawing.Point(77, 53);
            this.txtMaps.Name = "txtMaps";
            this.txtMaps.Size = new System.Drawing.Size(60, 21);
            this.txtMaps.TabIndex = 2;
            this.toolTip.SetToolTip(this.txtMaps, "The total number of maps which can be loaded at once.\nDefault = 5");
            this.txtMaps.TextChanged += new System.EventHandler(this.txtMaps_TextChanged);
            // 
            // txtDepth
            // 
            this.txtDepth.Location = new System.Drawing.Point(60, 51);
            this.txtDepth.Name = "txtDepth";
            this.txtDepth.Size = new System.Drawing.Size(41, 21);
            this.txtDepth.TabIndex = 2;
            this.toolTip.SetToolTip(this.txtDepth, "Depth which guests can dig.\nDefault = 4");
            this.txtDepth.TextChanged += new System.EventHandler(this.txtDepth_TextChanged);
            // 
            // cmbDefaultRank
            // 
            this.cmbDefaultRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultRank.FormattingEnabled = true;
            this.cmbDefaultRank.Location = new System.Drawing.Point(81, 18);
            this.cmbDefaultRank.Name = "cmbDefaultRank";
            this.cmbDefaultRank.Size = new System.Drawing.Size(81, 21);
            this.cmbDefaultRank.TabIndex = 21;
            this.toolTip.SetToolTip(this.cmbDefaultRank, "Default rank assigned to new visitors to the server.");
            // 
            // cmbOpChat
            // 
            this.cmbOpChat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOpChat.FormattingEnabled = true;
            this.cmbOpChat.Location = new System.Drawing.Point(83, 48);
            this.cmbOpChat.Name = "cmbOpChat";
            this.cmbOpChat.Size = new System.Drawing.Size(81, 21);
            this.cmbOpChat.TabIndex = 23;
            this.toolTip.SetToolTip(this.cmbOpChat, "Default rank required to read op chat.");
            // 
            // chkLogBeat
            // 
            this.chkLogBeat.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLogBeat.AutoSize = true;
            this.chkLogBeat.Location = new System.Drawing.Point(16, 80);
            this.chkLogBeat.Name = "chkLogBeat";
            this.chkLogBeat.Size = new System.Drawing.Size(91, 23);
            this.chkLogBeat.TabIndex = 24;
            this.chkLogBeat.Text = "Log Heartbeat?";
            this.toolTip.SetToolTip(this.chkLogBeat, "Debugging feature -- Toggles whether to log heartbeat activity.\r\nUseful when your" +
                    " server gets a URL slowly or not at all.");
            this.chkLogBeat.UseVisualStyleBackColor = true;
            // 
            // cmbAdminChat
            // 
            this.cmbAdminChat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAdminChat.FormattingEnabled = true;
            this.cmbAdminChat.Location = new System.Drawing.Point(100, 75);
            this.cmbAdminChat.Name = "cmbAdminChat";
            this.cmbAdminChat.Size = new System.Drawing.Size(81, 21);
            this.cmbAdminChat.TabIndex = 34;
            this.toolTip.SetToolTip(this.cmbAdminChat, "Default rank required to read op chat.");
            // 
            // chkTpToHigherRanks
            // 
            this.chkTpToHigherRanks.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkTpToHigherRanks.AutoSize = true;
            this.chkTpToHigherRanks.Location = new System.Drawing.Point(170, 46);
            this.chkTpToHigherRanks.Name = "chkTpToHigherRanks";
            this.chkTpToHigherRanks.Size = new System.Drawing.Size(127, 23);
            this.chkTpToHigherRanks.TabIndex = 40;
            this.chkTpToHigherRanks.Text = "Allow tp to higher ranks";
            this.toolTip.SetToolTip(this.chkTpToHigherRanks, "Allows the use of /tp to players of higher rank");
            this.chkTpToHigherRanks.UseVisualStyleBackColor = true;
            // 
            // chkUseSQL
            // 
            this.chkUseSQL.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkUseSQL.AutoSize = true;
            this.chkUseSQL.Location = new System.Drawing.Point(22, 281);
            this.chkUseSQL.Name = "chkUseSQL";
            this.chkUseSQL.Size = new System.Drawing.Size(74, 23);
            this.chkUseSQL.TabIndex = 28;
            this.chkUseSQL.Tag = "Whether or not the use of MySQL is enabled. You will need to have installed it fo" +
                "r this to work. MySQL includes features such as block tracking, colors, titles a" +
                "nd player info.";
            this.chkUseSQL.Text = "Use MySQL";
            this.toolTip.SetToolTip(this.chkUseSQL, "Whether to use the IRC bot or not.\nIRC stands for Internet Relay Chat and allows " +
                    "for communication with the server while outside Minecraft.");
            this.chkUseSQL.UseVisualStyleBackColor = true;
            this.chkUseSQL.CheckedChanged += new System.EventHandler(this.chkUseSQL_CheckedChanged);
            // 
            // cmbVerificationRank
            // 
            this.cmbVerificationRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVerificationRank.FormattingEnabled = true;
            this.cmbVerificationRank.Location = new System.Drawing.Point(60, 64);
            this.cmbVerificationRank.Name = "cmbVerificationRank";
            this.cmbVerificationRank.Size = new System.Drawing.Size(103, 21);
            this.cmbVerificationRank.TabIndex = 22;
            this.toolTip.SetToolTip(this.cmbVerificationRank, "The rank that verification is required for admins to gain access to commands.");
            // 
            // chkEnableVerification
            // 
            this.chkEnableVerification.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEnableVerification.AutoSize = true;
            this.chkEnableVerification.Location = new System.Drawing.Point(24, 20);
            this.chkEnableVerification.Name = "chkEnableVerification";
            this.chkEnableVerification.Size = new System.Drawing.Size(49, 23);
            this.chkEnableVerification.TabIndex = 23;
            this.chkEnableVerification.Text = "Enable";
            this.toolTip.SetToolTip(this.chkEnableVerification, "Whether or not the server will ask for verification from admins before they can u" +
                    "se commands.");
            this.chkEnableVerification.UseVisualStyleBackColor = true;
            // 
            // chkSpamControl
            // 
            this.chkSpamControl.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSpamControl.AutoSize = true;
            this.chkSpamControl.Location = new System.Drawing.Point(15, 20);
            this.chkSpamControl.Name = "chkSpamControl";
            this.chkSpamControl.Size = new System.Drawing.Size(49, 23);
            this.chkSpamControl.TabIndex = 24;
            this.chkSpamControl.Text = "Enable";
            this.toolTip.SetToolTip(this.chkSpamControl, "If enabled it mutes a player for spamming. Default false.\r\n");
            this.chkSpamControl.UseVisualStyleBackColor = true;
            // 
            // txtGlobalChatNick
            // 
            this.txtGlobalChatNick.Location = new System.Drawing.Point(42, 49);
            this.txtGlobalChatNick.Name = "txtGlobalChatNick";
            this.txtGlobalChatNick.Size = new System.Drawing.Size(63, 21);
            this.txtGlobalChatNick.TabIndex = 1;
            this.toolTip.SetToolTip(this.txtGlobalChatNick, "The name of the bot in the Global Chat IRC channel.");
            // 
            // cmbGlobalChatColor
            // 
            this.cmbGlobalChatColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGlobalChatColor.FormattingEnabled = true;
            this.cmbGlobalChatColor.Location = new System.Drawing.Point(9, 76);
            this.cmbGlobalChatColor.Name = "cmbGlobalChatColor";
            this.cmbGlobalChatColor.Size = new System.Drawing.Size(72, 21);
            this.cmbGlobalChatColor.TabIndex = 11;
            this.toolTip.SetToolTip(this.cmbGlobalChatColor, "The color of Global Chat text in-game.");
            this.cmbGlobalChatColor.SelectedIndexChanged += new System.EventHandler(this.cmbGlobalChatColor_SelectedIndexChanged);
            // 
            // lsChkSendAFKMain
            // 
            this.lsChkSendAFKMain.Appearance = System.Windows.Forms.Appearance.Button;
            this.lsChkSendAFKMain.AutoSize = true;
            this.lsChkSendAFKMain.Location = new System.Drawing.Point(19, 49);
            this.lsChkSendAFKMain.Name = "lsChkSendAFKMain";
            this.lsChkSendAFKMain.Size = new System.Drawing.Size(134, 23);
            this.lsChkSendAFKMain.TabIndex = 1;
            this.lsChkSendAFKMain.Text = "Send AFK Players To Main";
            this.toolTip.SetToolTip(this.lsChkSendAFKMain, "Send AFK players to the main map on a map change?");
            this.lsChkSendAFKMain.UseVisualStyleBackColor = true;
            // 
            // lsChkStartOnStartup
            // 
            this.lsChkStartOnStartup.Appearance = System.Windows.Forms.Appearance.Button;
            this.lsChkStartOnStartup.AutoSize = true;
            this.lsChkStartOnStartup.Location = new System.Drawing.Point(8, 20);
            this.lsChkStartOnStartup.Name = "lsChkStartOnStartup";
            this.lsChkStartOnStartup.Size = new System.Drawing.Size(156, 23);
            this.lsChkStartOnStartup.TabIndex = 0;
            this.lsChkStartOnStartup.Text = "Start Round On Server Startup";
            this.toolTip.SetToolTip(this.lsChkStartOnStartup, "Start Lava Survival when the server starts up?");
            this.lsChkStartOnStartup.UseVisualStyleBackColor = true;
            // 
            // lsNudVoteCount
            // 
            this.lsNudVoteCount.Location = new System.Drawing.Point(71, 78);
            this.lsNudVoteCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.lsNudVoteCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.lsNudVoteCount.Name = "lsNudVoteCount";
            this.lsNudVoteCount.Size = new System.Drawing.Size(95, 21);
            this.lsNudVoteCount.TabIndex = 3;
            this.toolTip.SetToolTip(this.lsNudVoteCount, "How many maps to put in the next map vote.");
            this.lsNudVoteCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lsNudVoteTime
            // 
            this.lsNudVoteTime.Location = new System.Drawing.Point(71, 106);
            this.lsNudVoteTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.lsNudVoteTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lsNudVoteTime.Name = "lsNudVoteTime";
            this.lsNudVoteTime.Size = new System.Drawing.Size(95, 21);
            this.lsNudVoteTime.TabIndex = 5;
            this.toolTip.SetToolTip(this.lsNudVoteTime, "Time until the next map vote ends.");
            this.lsNudVoteTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lsNudVoteTime.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // lsCmbSetupRank
            // 
            this.lsCmbSetupRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lsCmbSetupRank.FormattingEnabled = true;
            this.lsCmbSetupRank.Location = new System.Drawing.Point(71, 160);
            this.lsCmbSetupRank.Name = "lsCmbSetupRank";
            this.lsCmbSetupRank.Size = new System.Drawing.Size(95, 21);
            this.lsCmbSetupRank.TabIndex = 7;
            this.toolTip.SetToolTip(this.lsCmbSetupRank, "Minimum rank required to configure Lava Survival.");
            // 
            // lsNudFastLava
            // 
            this.lsNudFastLava.Location = new System.Drawing.Point(67, 20);
            this.lsNudFastLava.Name = "lsNudFastLava";
            this.lsNudFastLava.Size = new System.Drawing.Size(61, 21);
            this.lsNudFastLava.TabIndex = 1;
            this.toolTip.SetToolTip(this.lsNudFastLava, "Percent chance for fast lava.");
            // 
            // lsNudKiller
            // 
            this.lsNudKiller.Location = new System.Drawing.Point(67, 47);
            this.lsNudKiller.Name = "lsNudKiller";
            this.lsNudKiller.Size = new System.Drawing.Size(61, 21);
            this.lsNudKiller.TabIndex = 3;
            this.toolTip.SetToolTip(this.lsNudKiller, "Percent chance for killer lava/water.");
            // 
            // lsNudDestroy
            // 
            this.lsNudDestroy.Location = new System.Drawing.Point(67, 74);
            this.lsNudDestroy.Name = "lsNudDestroy";
            this.lsNudDestroy.Size = new System.Drawing.Size(61, 21);
            this.lsNudDestroy.TabIndex = 5;
            this.toolTip.SetToolTip(this.lsNudDestroy, "Percent chance for lava/water to destroy blocks.");
            // 
            // lsNudWater
            // 
            this.lsNudWater.Location = new System.Drawing.Point(67, 101);
            this.lsNudWater.Name = "lsNudWater";
            this.lsNudWater.Size = new System.Drawing.Size(61, 21);
            this.lsNudWater.TabIndex = 7;
            this.toolTip.SetToolTip(this.lsNudWater, "Percent chance for water flood.");
            // 
            // lsNudLayer
            // 
            this.lsNudLayer.Location = new System.Drawing.Point(67, 128);
            this.lsNudLayer.Name = "lsNudLayer";
            this.lsNudLayer.Size = new System.Drawing.Size(61, 21);
            this.lsNudLayer.TabIndex = 9;
            this.toolTip.SetToolTip(this.lsNudLayer, "Percent chance for layer flood.");
            // 
            // lsNudLayerHeight
            // 
            this.lsNudLayerHeight.Location = new System.Drawing.Point(211, 20);
            this.lsNudLayerHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lsNudLayerHeight.Name = "lsNudLayerHeight";
            this.lsNudLayerHeight.Size = new System.Drawing.Size(61, 21);
            this.lsNudLayerHeight.TabIndex = 12;
            this.toolTip.SetToolTip(this.lsNudLayerHeight, "Height of each layer.");
            this.lsNudLayerHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lsNudLayerCount
            // 
            this.lsNudLayerCount.Location = new System.Drawing.Point(211, 47);
            this.lsNudLayerCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.lsNudLayerCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lsNudLayerCount.Name = "lsNudLayerCount";
            this.lsNudLayerCount.Size = new System.Drawing.Size(61, 21);
            this.lsNudLayerCount.TabIndex = 14;
            this.toolTip.SetToolTip(this.lsNudLayerCount, "Number of layers to flood.");
            this.lsNudLayerCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lsNudLayerTime
            // 
            this.lsNudLayerTime.Location = new System.Drawing.Point(211, 74);
            this.lsNudLayerTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.lsNudLayerTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lsNudLayerTime.Name = "lsNudLayerTime";
            this.lsNudLayerTime.Size = new System.Drawing.Size(61, 21);
            this.lsNudLayerTime.TabIndex = 16;
            this.toolTip.SetToolTip(this.lsNudLayerTime, "Time between each layer.");
            this.lsNudLayerTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lsNudRoundTime
            // 
            this.lsNudRoundTime.Location = new System.Drawing.Point(211, 102);
            this.lsNudRoundTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.lsNudRoundTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lsNudRoundTime.Name = "lsNudRoundTime";
            this.lsNudRoundTime.Size = new System.Drawing.Size(61, 21);
            this.lsNudRoundTime.TabIndex = 18;
            this.toolTip.SetToolTip(this.lsNudRoundTime, "Time until the round ends.");
            this.lsNudRoundTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lsNudFloodTime
            // 
            this.lsNudFloodTime.Location = new System.Drawing.Point(211, 128);
            this.lsNudFloodTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.lsNudFloodTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lsNudFloodTime.Name = "lsNudFloodTime";
            this.lsNudFloodTime.Size = new System.Drawing.Size(61, 21);
            this.lsNudFloodTime.TabIndex = 20;
            this.toolTip.SetToolTip(this.lsNudFloodTime, "Time until the map is flooded.");
            this.lsNudFloodTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lsCmbControlRank
            // 
            this.lsCmbControlRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lsCmbControlRank.FormattingEnabled = true;
            this.lsCmbControlRank.Location = new System.Drawing.Point(84, 187);
            this.lsCmbControlRank.Name = "lsCmbControlRank";
            this.lsCmbControlRank.Size = new System.Drawing.Size(82, 21);
            this.lsCmbControlRank.TabIndex = 9;
            this.toolTip.SetToolTip(this.lsCmbControlRank, "Minimum rank required to administrate Lava Survival.");
            // 
            // cmbGrieferStoneType
            // 
            this.cmbGrieferStoneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrieferStoneType.FormattingEnabled = true;
            this.cmbGrieferStoneType.Location = new System.Drawing.Point(110, 138);
            this.cmbGrieferStoneType.Name = "cmbGrieferStoneType";
            this.cmbGrieferStoneType.Size = new System.Drawing.Size(94, 21);
            this.cmbGrieferStoneType.TabIndex = 13;
            this.toolTip.SetToolTip(this.cmbGrieferStoneType, "The block type that griefer_stone will look like in-game.");
            // 
            // chkGrieferStoneBan
            // 
            this.chkGrieferStoneBan.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkGrieferStoneBan.AutoSize = true;
            this.chkGrieferStoneBan.Location = new System.Drawing.Point(210, 138);
            this.chkGrieferStoneBan.Name = "chkGrieferStoneBan";
            this.chkGrieferStoneBan.Size = new System.Drawing.Size(127, 23);
            this.chkGrieferStoneBan.TabIndex = 43;
            this.chkGrieferStoneBan.Text = "Griefer_stone Tempban";
            this.toolTip.SetToolTip(this.chkGrieferStoneBan, "Should griefer_stone tempban the player or just kick them?");
            this.chkGrieferStoneBan.UseVisualStyleBackColor = true;
            // 
            // txtGrieferStone
            // 
            this.txtGrieferStone.Location = new System.Drawing.Point(186, 129);
            this.txtGrieferStone.Name = "txtGrieferStone";
            this.txtGrieferStone.Size = new System.Drawing.Size(134, 21);
            this.txtGrieferStone.TabIndex = 36;
            this.toolTip.SetToolTip(this.txtGrieferStone, "Kick message for griefer_stone. Only works if tempban is off!");
            // 
            // cmbGrieferStoneRank
            // 
            this.cmbGrieferStoneRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrieferStoneRank.FormattingEnabled = true;
            this.cmbGrieferStoneRank.Location = new System.Drawing.Point(148, 112);
            this.cmbGrieferStoneRank.Name = "cmbGrieferStoneRank";
            this.cmbGrieferStoneRank.Size = new System.Drawing.Size(87, 21);
            this.cmbGrieferStoneRank.TabIndex = 45;
            this.toolTip.SetToolTip(this.cmbGrieferStoneRank, "The maximum rank that griefer_stone will kick or ban.");
            // 
            // lsNudLives
            // 
            this.lsNudLives.Location = new System.Drawing.Point(71, 133);
            this.lsNudLives.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.lsNudLives.Name = "lsNudLives";
            this.lsNudLives.Size = new System.Drawing.Size(95, 21);
            this.lsNudLives.TabIndex = 10;
            this.toolTip.SetToolTip(this.lsNudLives, "The number of times a player can die before being out of the round. Set to 0 for " +
                    "unlimited.");
            this.lsNudLives.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbAFKKickPerm
            // 
            this.cmbAFKKickPerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAFKKickPerm.FormattingEnabled = true;
            this.cmbAFKKickPerm.Location = new System.Drawing.Point(65, 70);
            this.cmbAFKKickPerm.Name = "cmbAFKKickPerm";
            this.cmbAFKKickPerm.Size = new System.Drawing.Size(62, 21);
            this.cmbAFKKickPerm.TabIndex = 46;
            this.toolTip.SetToolTip(this.cmbAFKKickPerm, "Maximum rank that will be kicked by AFK kick.");
            // 
            // chkGuestLimitNotify
            // 
            this.chkGuestLimitNotify.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkGuestLimitNotify.AutoSize = true;
            this.chkGuestLimitNotify.Location = new System.Drawing.Point(246, 111);
            this.chkGuestLimitNotify.Name = "chkGuestLimitNotify";
            this.chkGuestLimitNotify.Size = new System.Drawing.Size(100, 23);
            this.chkGuestLimitNotify.TabIndex = 46;
            this.chkGuestLimitNotify.Text = "Guest Limit Notify";
            this.toolTip.SetToolTip(this.chkGuestLimitNotify, "Notify in-game if a guest can\'t join due to the guest limit being reached.");
            this.chkGuestLimitNotify.UseVisualStyleBackColor = true;
            // 
            // sndcmddataChk
            // 
            this.sndcmddataChk.AutoSize = true;
            this.sndcmddataChk.Location = new System.Drawing.Point(10, 20);
            this.sndcmddataChk.Name = "sndcmddataChk";
            this.sndcmddataChk.Size = new System.Drawing.Size(104, 17);
            this.sndcmddataChk.TabIndex = 0;
            this.sndcmddataChk.Text = "Send to MCForge";
            this.toolTip.SetToolTip(this.sndcmddataChk, "When this is turned on, MCForge will send all commands\r\nthat are being used to a " +
                    "webpage to count how many\r\ntimes commands are being used. No more data is sent\r\n" +
                    "through this.");
            this.sndcmddataChk.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.Transparent;
            this.tabPage5.Controls.Add(this.btnBlHelp);
            this.tabPage5.Controls.Add(this.txtBlRanks);
            this.tabPage5.Controls.Add(this.txtBlAllow);
            this.tabPage5.Controls.Add(this.txtBlLowest);
            this.tabPage5.Controls.Add(this.txtBlDisallow);
            this.tabPage5.Controls.Add(this.label18);
            this.tabPage5.Controls.Add(this.label19);
            this.tabPage5.Controls.Add(this.label20);
            this.tabPage5.Controls.Add(this.listBlocks);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(488, 509);
            this.tabPage5.TabIndex = 5;
            this.tabPage5.Text = "Blocks";
            // 
            // btnBlHelp
            // 
            this.btnBlHelp.Location = new System.Drawing.Point(253, 419);
            this.btnBlHelp.Name = "btnBlHelp";
            this.btnBlHelp.Size = new System.Drawing.Size(141, 29);
            this.btnBlHelp.TabIndex = 23;
            this.btnBlHelp.Text = "Help information";
            this.btnBlHelp.UseVisualStyleBackColor = true;
            this.btnBlHelp.Click += new System.EventHandler(this.btnBlHelp_Click);
            // 
            // txtBlRanks
            // 
            this.txtBlRanks.Location = new System.Drawing.Point(11, 122);
            this.txtBlRanks.Multiline = true;
            this.txtBlRanks.Name = "txtBlRanks";
            this.txtBlRanks.ReadOnly = true;
            this.txtBlRanks.Size = new System.Drawing.Size(225, 321);
            this.txtBlRanks.TabIndex = 22;
            // 
            // txtBlAllow
            // 
            this.txtBlAllow.Location = new System.Drawing.Point(113, 95);
            this.txtBlAllow.Name = "txtBlAllow";
            this.txtBlAllow.Size = new System.Drawing.Size(92, 21);
            this.txtBlAllow.TabIndex = 20;
            this.txtBlAllow.LostFocus += new System.EventHandler(this.txtBlAllow_TextChanged);
            // 
            // txtBlLowest
            // 
            this.txtBlLowest.Location = new System.Drawing.Point(113, 41);
            this.txtBlLowest.Name = "txtBlLowest";
            this.txtBlLowest.Size = new System.Drawing.Size(92, 21);
            this.txtBlLowest.TabIndex = 21;
            this.txtBlLowest.LostFocus += new System.EventHandler(this.txtBlLowest_TextChanged);
            // 
            // txtBlDisallow
            // 
            this.txtBlDisallow.Location = new System.Drawing.Point(113, 68);
            this.txtBlDisallow.Name = "txtBlDisallow";
            this.txtBlDisallow.Size = new System.Drawing.Size(92, 21);
            this.txtBlDisallow.TabIndex = 21;
            this.txtBlDisallow.LostFocus += new System.EventHandler(this.txtBlDisallow_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(57, 99);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 18;
            this.label18.Text = "And allow:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(33, 72);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 17;
            this.label19.Text = "But don\'t allow:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 44);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(105, 13);
            this.label20.TabIndex = 16;
            this.label20.Text = "Lowest rank needed:";
            // 
            // listBlocks
            // 
            this.listBlocks.FormattingEnabled = true;
            this.listBlocks.Location = new System.Drawing.Point(253, 19);
            this.listBlocks.Name = "listBlocks";
            this.listBlocks.Size = new System.Drawing.Size(141, 394);
            this.listBlocks.TabIndex = 15;
            this.listBlocks.SelectedIndexChanged += new System.EventHandler(this.listBlocks_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.txtMaxUndo);
            this.tabPage2.Controls.Add(this.label52);
            this.tabPage2.Controls.Add(this.lblColor);
            this.tabPage2.Controls.Add(this.cmbColor);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.txtFileName);
            this.tabPage2.Controls.Add(this.txtLimit);
            this.tabPage2.Controls.Add(this.txtPermission);
            this.tabPage2.Controls.Add(this.txtRankName);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.btnAddRank);
            this.tabPage2.Controls.Add(this.listRanks);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(488, 509);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Ranks";
            // 
            // txtMaxUndo
            // 
            this.txtMaxUndo.Location = new System.Drawing.Point(81, 122);
            this.txtMaxUndo.Name = "txtMaxUndo";
            this.txtMaxUndo.Size = new System.Drawing.Size(100, 21);
            this.txtMaxUndo.TabIndex = 15;
            this.txtMaxUndo.TextChanged += new System.EventHandler(this.txtMaxUndo_TextChanged);
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(18, 125);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(57, 13);
            this.label52.TabIndex = 14;
            this.label52.Text = "Max Undo:";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblColor
            // 
            this.lblColor.Location = new System.Drawing.Point(179, 144);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(26, 23);
            this.lblColor.TabIndex = 13;
            // 
            // cmbColor
            // 
            this.cmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColor.FormattingEnabled = true;
            this.cmbColor.Location = new System.Drawing.Point(81, 149);
            this.cmbColor.Name = "cmbColor";
            this.cmbColor.Size = new System.Drawing.Size(92, 21);
            this.cmbColor.TabIndex = 12;
            this.cmbColor.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(40, 154);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 11;
            this.label16.Text = "Color:";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(81, 206);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(100, 21);
            this.txtFileName.TabIndex = 4;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // txtLimit
            // 
            this.txtLimit.Location = new System.Drawing.Point(81, 95);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(100, 21);
            this.txtLimit.TabIndex = 4;
            this.txtLimit.TextChanged += new System.EventHandler(this.txtLimit_TextChanged);
            // 
            // txtPermission
            // 
            this.txtPermission.Location = new System.Drawing.Point(81, 68);
            this.txtPermission.Name = "txtPermission";
            this.txtPermission.Size = new System.Drawing.Size(100, 21);
            this.txtPermission.TabIndex = 4;
            this.txtPermission.TextChanged += new System.EventHandler(this.txtPermission_TextChanged);
            // 
            // txtRankName
            // 
            this.txtRankName.Location = new System.Drawing.Point(81, 41);
            this.txtRankName.Name = "txtRankName";
            this.txtRankName.Size = new System.Drawing.Size(100, 21);
            this.txtRankName.TabIndex = 4;
            this.txtRankName.TextChanged += new System.EventHandler(this.txtRankName_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 209);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Filename:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(41, 98);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Limit:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Permission:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Name:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(315, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Del";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAddRank
            // 
            this.btnAddRank.Location = new System.Drawing.Point(243, 6);
            this.btnAddRank.Name = "btnAddRank";
            this.btnAddRank.Size = new System.Drawing.Size(57, 23);
            this.btnAddRank.TabIndex = 1;
            this.btnAddRank.Text = "Add";
            this.btnAddRank.UseVisualStyleBackColor = true;
            this.btnAddRank.Click += new System.EventHandler(this.btnAddRank_Click);
            // 
            // listRanks
            // 
            this.listRanks.FormattingEnabled = true;
            this.listRanks.Location = new System.Drawing.Point(230, 35);
            this.listRanks.Name = "listRanks";
            this.listRanks.Size = new System.Drawing.Size(161, 381);
            this.listRanks.TabIndex = 0;
            this.listRanks.SelectedIndexChanged += new System.EventHandler(this.listRanks_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.groupBox24);
            this.tabPage4.Controls.Add(this.groupBox18);
            this.tabPage4.Controls.Add(this.groupBox19);
            this.tabPage4.Controls.Add(this.groupBox13);
            this.tabPage4.Controls.Add(this.groupBox12);
            this.tabPage4.Controls.Add(this.groupBox11);
            this.tabPage4.Controls.Add(this.groupBox10);
            this.tabPage4.Controls.Add(this.groupBox9);
            this.tabPage4.Controls.Add(this.chkProfanityFilter);
            this.tabPage4.Controls.Add(this.chkForceCuboid);
            this.tabPage4.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(488, 509);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Misc";
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.sndcmddataChk);
            this.groupBox24.Location = new System.Drawing.Point(9, 232);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(128, 51);
            this.groupBox24.TabIndex = 43;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Commands";
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.chkPrmOnly);
            this.groupBox18.Location = new System.Drawing.Point(369, 394);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(116, 112);
            this.groupBox18.TabIndex = 42;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Access to server";
            // 
            // chkPrmOnly
            // 
            this.chkPrmOnly.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPrmOnly.Location = new System.Drawing.Point(6, 20);
            this.chkPrmOnly.Name = "chkPrmOnly";
            this.chkPrmOnly.Size = new System.Drawing.Size(104, 37);
            this.chkPrmOnly.TabIndex = 47;
            this.chkPrmOnly.Text = "Premium Players Only";
            this.chkPrmOnly.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPrmOnly.UseVisualStyleBackColor = true;
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.cmbGlobalChatColor);
            this.groupBox19.Controls.Add(this.lblGlobalChatColor);
            this.groupBox19.Controls.Add(this.label51);
            this.groupBox19.Controls.Add(this.txtGlobalChatNick);
            this.groupBox19.Controls.Add(this.chkGlobalChat);
            this.groupBox19.Location = new System.Drawing.Point(368, 283);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(117, 104);
            this.groupBox19.TabIndex = 41;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Global Chat";
            // 
            // lblGlobalChatColor
            // 
            this.lblGlobalChatColor.Location = new System.Drawing.Point(84, 76);
            this.lblGlobalChatColor.Name = "lblGlobalChatColor";
            this.lblGlobalChatColor.Size = new System.Drawing.Size(21, 21);
            this.lblGlobalChatColor.TabIndex = 12;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(6, 52);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(30, 13);
            this.label51.TabIndex = 2;
            this.label51.Text = "Nick:";
            // 
            // chkGlobalChat
            // 
            this.chkGlobalChat.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkGlobalChat.AutoSize = true;
            this.chkGlobalChat.Location = new System.Drawing.Point(13, 20);
            this.chkGlobalChat.Name = "chkGlobalChat";
            this.chkGlobalChat.Size = new System.Drawing.Size(92, 23);
            this.chkGlobalChat.TabIndex = 0;
            this.chkGlobalChat.Text = "Use Global Chat";
            this.chkGlobalChat.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.chkGuestLimitNotify);
            this.groupBox13.Controls.Add(this.cmbGrieferStoneRank);
            this.groupBox13.Controls.Add(this.label48);
            this.groupBox13.Controls.Add(this.chkGrieferStoneBan);
            this.groupBox13.Controls.Add(this.cmbGrieferStoneType);
            this.groupBox13.Controls.Add(this.label47);
            this.groupBox13.Controls.Add(this.chkShowEmptyRanks);
            this.groupBox13.Controls.Add(this.chkIgnoreGlobal);
            this.groupBox13.Controls.Add(this.chkNotifyOnJoinLeave);
            this.groupBox13.Controls.Add(this.chkRepeatMessages);
            this.groupBox13.Controls.Add(this.chkDeath);
            this.groupBox13.Controls.Add(this.chkHelp);
            this.groupBox13.Controls.Add(this.txtRestartTime);
            this.groupBox13.Controls.Add(this.txtMoneys);
            this.groupBox13.Controls.Add(this.chkrankSuper);
            this.groupBox13.Controls.Add(this.chkRestartTime);
            this.groupBox13.Controls.Add(this.chk17Dollar);
            this.groupBox13.Controls.Add(this.chkSmile);
            this.groupBox13.Controls.Add(this.label34);
            this.groupBox13.Location = new System.Drawing.Point(10, 283);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(352, 223);
            this.groupBox13.TabIndex = 40;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Extra";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(6, 115);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(140, 13);
            this.label48.TabIndex = 44;
            this.label48.Text = "Griefer_stone kick/ban rank:";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(6, 140);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(98, 13);
            this.label47.TabIndex = 42;
            this.label47.Text = "Griefer_stone type:";
            // 
            // chkShowEmptyRanks
            // 
            this.chkShowEmptyRanks.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkShowEmptyRanks.AutoSize = true;
            this.chkShowEmptyRanks.Location = new System.Drawing.Point(192, 56);
            this.chkShowEmptyRanks.Name = "chkShowEmptyRanks";
            this.chkShowEmptyRanks.Size = new System.Drawing.Size(154, 23);
            this.chkShowEmptyRanks.TabIndex = 41;
            this.chkShowEmptyRanks.Text = "Show empty ranks in /players";
            this.chkShowEmptyRanks.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreGlobal
            // 
            this.chkIgnoreGlobal.AutoSize = true;
            this.chkIgnoreGlobal.Location = new System.Drawing.Point(194, 168);
            this.chkIgnoreGlobal.Name = "chkIgnoreGlobal";
            this.chkIgnoreGlobal.Size = new System.Drawing.Size(112, 17);
            this.chkIgnoreGlobal.TabIndex = 36;
            this.chkIgnoreGlobal.Tag = "If enabled, it makes it so, you cannot ignore players of opchat perm +.";
            this.chkIgnoreGlobal.Text = "Allow Ignoring Ops";
            this.chkIgnoreGlobal.UseVisualStyleBackColor = true;
            // 
            // chkNotifyOnJoinLeave
            // 
            this.chkNotifyOnJoinLeave.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNotifyOnJoinLeave.AutoSize = true;
            this.chkNotifyOnJoinLeave.Location = new System.Drawing.Point(18, 27);
            this.chkNotifyOnJoinLeave.Name = "chkNotifyOnJoinLeave";
            this.chkNotifyOnJoinLeave.Size = new System.Drawing.Size(140, 23);
            this.chkNotifyOnJoinLeave.TabIndex = 31;
            this.chkNotifyOnJoinLeave.Text = "Notify popup on join/leave";
            this.chkNotifyOnJoinLeave.UseVisualStyleBackColor = true;
            // 
            // chkRepeatMessages
            // 
            this.chkRepeatMessages.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkRepeatMessages.AutoSize = true;
            this.chkRepeatMessages.Location = new System.Drawing.Point(219, 84);
            this.chkRepeatMessages.Name = "chkRepeatMessages";
            this.chkRepeatMessages.Size = new System.Drawing.Size(127, 23);
            this.chkRepeatMessages.TabIndex = 29;
            this.chkRepeatMessages.Text = "Repeat message blocks";
            this.chkRepeatMessages.UseVisualStyleBackColor = true;
            // 
            // txtRestartTime
            // 
            this.txtRestartTime.Location = new System.Drawing.Point(155, 193);
            this.txtRestartTime.Name = "txtRestartTime";
            this.txtRestartTime.Size = new System.Drawing.Size(172, 21);
            this.txtRestartTime.TabIndex = 1;
            this.txtRestartTime.Text = "HH: mm: ss";
            // 
            // txtMoneys
            // 
            this.txtMoneys.Location = new System.Drawing.Point(92, 166);
            this.txtMoneys.Name = "txtMoneys";
            this.txtMoneys.Size = new System.Drawing.Size(82, 21);
            this.txtMoneys.TabIndex = 1;
            // 
            // chkRestartTime
            // 
            this.chkRestartTime.AutoSize = true;
            this.chkRestartTime.Location = new System.Drawing.Point(18, 197);
            this.chkRestartTime.Name = "chkRestartTime";
            this.chkRestartTime.Size = new System.Drawing.Size(131, 17);
            this.chkRestartTime.TabIndex = 0;
            this.chkRestartTime.Text = "Restart server at time:";
            this.chkRestartTime.UseVisualStyleBackColor = true;
            // 
            // chk17Dollar
            // 
            this.chk17Dollar.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk17Dollar.AutoSize = true;
            this.chk17Dollar.Location = new System.Drawing.Point(255, 27);
            this.chk17Dollar.Name = "chk17Dollar";
            this.chk17Dollar.Size = new System.Drawing.Size(91, 23);
            this.chk17Dollar.TabIndex = 22;
            this.chk17Dollar.Text = "$ before $name";
            this.chk17Dollar.UseVisualStyleBackColor = true;
            // 
            // chkSmile
            // 
            this.chkSmile.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSmile.AutoSize = true;
            this.chkSmile.Location = new System.Drawing.Point(92, 56);
            this.chkSmile.Name = "chkSmile";
            this.chkSmile.Size = new System.Drawing.Size(82, 23);
            this.chkSmile.TabIndex = 19;
            this.chkSmile.Text = "Parse emotes";
            this.chkSmile.UseVisualStyleBackColor = true;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(15, 169);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(71, 13);
            this.label34.TabIndex = 11;
            this.label34.Text = "Money name:";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.txtGrieferStone);
            this.groupBox12.Controls.Add(this.chkGrieferStone);
            this.groupBox12.Controls.Add(this.chkShutdown);
            this.groupBox12.Controls.Add(this.txtShutdown);
            this.groupBox12.Controls.Add(this.hackrank_kick);
            this.groupBox12.Controls.Add(this.hackrank_kick_time);
            this.groupBox12.Controls.Add(this.label36);
            this.groupBox12.Controls.Add(this.txtBanMessage);
            this.groupBox12.Controls.Add(this.chkCheap);
            this.groupBox12.Controls.Add(this.txtCheap);
            this.groupBox12.Controls.Add(this.chkBanMessage);
            this.groupBox12.Location = new System.Drawing.Point(146, 119);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(339, 158);
            this.groupBox12.TabIndex = 39;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Messages";
            // 
            // chkGrieferStone
            // 
            this.chkGrieferStone.AutoSize = true;
            this.chkGrieferStone.Location = new System.Drawing.Point(12, 132);
            this.chkGrieferStone.Name = "chkGrieferStone";
            this.chkGrieferStone.Size = new System.Drawing.Size(174, 17);
            this.chkGrieferStone.TabIndex = 35;
            this.chkGrieferStone.Text = "Custom griefer_stone message:";
            this.chkGrieferStone.UseVisualStyleBackColor = true;
            // 
            // chkShutdown
            // 
            this.chkShutdown.AutoSize = true;
            this.chkShutdown.Location = new System.Drawing.Point(12, 20);
            this.chkShutdown.Name = "chkShutdown";
            this.chkShutdown.Size = new System.Drawing.Size(158, 17);
            this.chkShutdown.TabIndex = 26;
            this.chkShutdown.Text = "Custom shutdown message:";
            this.chkShutdown.UseVisualStyleBackColor = true;
            // 
            // txtShutdown
            // 
            this.txtShutdown.Location = new System.Drawing.Point(176, 18);
            this.txtShutdown.MaxLength = 128;
            this.txtShutdown.Name = "txtShutdown";
            this.txtShutdown.Size = new System.Drawing.Size(145, 21);
            this.txtShutdown.TabIndex = 28;
            // 
            // hackrank_kick_time
            // 
            this.hackrank_kick_time.Location = new System.Drawing.Point(211, 45);
            this.hackrank_kick_time.Name = "hackrank_kick_time";
            this.hackrank_kick_time.Size = new System.Drawing.Size(60, 21);
            this.hackrank_kick_time.TabIndex = 33;
            this.hackrank_kick_time.Text = "5";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(277, 48);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(46, 13);
            this.label36.TabIndex = 34;
            this.label36.Text = "Seconds";
            // 
            // txtBanMessage
            // 
            this.txtBanMessage.Location = new System.Drawing.Point(147, 101);
            this.txtBanMessage.MaxLength = 128;
            this.txtBanMessage.Name = "txtBanMessage";
            this.txtBanMessage.Size = new System.Drawing.Size(173, 21);
            this.txtBanMessage.TabIndex = 27;
            // 
            // txtCheap
            // 
            this.txtCheap.Location = new System.Drawing.Point(121, 73);
            this.txtCheap.Name = "txtCheap";
            this.txtCheap.Size = new System.Drawing.Size(200, 21);
            this.txtCheap.TabIndex = 1;
            // 
            // chkBanMessage
            // 
            this.chkBanMessage.AutoSize = true;
            this.chkBanMessage.Location = new System.Drawing.Point(12, 104);
            this.chkBanMessage.Name = "chkBanMessage";
            this.chkBanMessage.Size = new System.Drawing.Size(129, 17);
            this.chkBanMessage.TabIndex = 25;
            this.chkBanMessage.Text = "Custom ban message:";
            this.chkBanMessage.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label24);
            this.groupBox11.Controls.Add(this.txtRP);
            this.groupBox11.Controls.Add(this.label28);
            this.groupBox11.Controls.Add(this.txtNormRp);
            this.groupBox11.Controls.Add(this.chkPhysicsRest);
            this.groupBox11.Location = new System.Drawing.Point(8, 119);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(130, 107);
            this.groupBox11.TabIndex = 38;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Physics Restart";
            // 
            // txtRP
            // 
            this.txtRP.Location = new System.Drawing.Point(81, 49);
            this.txtRP.Name = "txtRP";
            this.txtRP.Size = new System.Drawing.Size(41, 21);
            this.txtRP.TabIndex = 14;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(14, 79);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(61, 13);
            this.label28.TabIndex = 16;
            this.label28.Text = "Normal /rp:";
            // 
            // txtNormRp
            // 
            this.txtNormRp.Location = new System.Drawing.Point(81, 76);
            this.txtNormRp.Name = "txtNormRp";
            this.txtNormRp.Size = new System.Drawing.Size(41, 21);
            this.txtNormRp.TabIndex = 13;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.cmbAFKKickPerm);
            this.groupBox10.Controls.Add(this.label76);
            this.groupBox10.Controls.Add(this.label25);
            this.groupBox10.Controls.Add(this.txtafk);
            this.groupBox10.Controls.Add(this.label26);
            this.groupBox10.Controls.Add(this.txtAFKKick);
            this.groupBox10.Location = new System.Drawing.Point(352, 13);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(133, 100);
            this.groupBox10.TabIndex = 37;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "AFK";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(5, 74);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(54, 13);
            this.label76.TabIndex = 13;
            this.label76.Text = "Kick Rank:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(5, 21);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(54, 13);
            this.label25.TabIndex = 12;
            this.label25.Text = "AFK timer:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(5, 47);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(48, 13);
            this.label26.TabIndex = 11;
            this.label26.Text = "AFK Kick:";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label32);
            this.groupBox9.Controls.Add(this.txtBackupLocation);
            this.groupBox9.Controls.Add(this.label9);
            this.groupBox9.Controls.Add(this.txtBackup);
            this.groupBox9.Location = new System.Drawing.Point(8, 13);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(338, 100);
            this.groupBox9.TabIndex = 36;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Backups";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(14, 27);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(44, 13);
            this.label32.TabIndex = 3;
            this.label32.Text = "Backup:";
            // 
            // txtBackupLocation
            // 
            this.txtBackupLocation.Location = new System.Drawing.Point(64, 24);
            this.txtBackupLocation.Name = "txtBackupLocation";
            this.txtBackupLocation.Size = new System.Drawing.Size(262, 21);
            this.txtBackupLocation.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Backup time:";
            // 
            // chkProfanityFilter
            // 
            this.chkProfanityFilter.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkProfanityFilter.AutoSize = true;
            this.chkProfanityFilter.Location = new System.Drawing.Point(535, 325);
            this.chkProfanityFilter.Name = "chkProfanityFilter";
            this.chkProfanityFilter.Size = new System.Drawing.Size(87, 23);
            this.chkProfanityFilter.TabIndex = 30;
            this.chkProfanityFilter.Text = "Profanity Filter";
            this.chkProfanityFilter.UseVisualStyleBackColor = true;
            // 
            // tabIRC
            // 
            this.tabIRC.Controls.Add(this.grpSQL);
            this.tabIRC.Controls.Add(this.chkUseSQL);
            this.tabIRC.Controls.Add(this.grpIRC);
            this.tabIRC.Controls.Add(this.chkIRC);
            this.tabIRC.Location = new System.Drawing.Point(4, 22);
            this.tabIRC.Name = "tabIRC";
            this.tabIRC.Size = new System.Drawing.Size(488, 509);
            this.tabIRC.TabIndex = 6;
            this.tabIRC.Text = "IRC/SQL";
            // 
            // grpSQL
            // 
            this.grpSQL.Controls.Add(this.txtSQLPort);
            this.grpSQL.Controls.Add(this.label70);
            this.grpSQL.Controls.Add(this.linkLabel1);
            this.grpSQL.Controls.Add(this.txtSQLHost);
            this.grpSQL.Controls.Add(this.label43);
            this.grpSQL.Controls.Add(this.txtSQLDatabase);
            this.grpSQL.Controls.Add(this.label42);
            this.grpSQL.Controls.Add(this.label40);
            this.grpSQL.Controls.Add(this.label41);
            this.grpSQL.Controls.Add(this.txtSQLPassword);
            this.grpSQL.Controls.Add(this.txtSQLUsername);
            this.grpSQL.Location = new System.Drawing.Point(22, 310);
            this.grpSQL.Name = "grpSQL";
            this.grpSQL.Size = new System.Drawing.Size(284, 186);
            this.grpSQL.TabIndex = 29;
            this.grpSQL.TabStop = false;
            this.grpSQL.Text = "MySQL";
            // 
            // txtSQLPort
            // 
            this.txtSQLPort.Location = new System.Drawing.Point(192, 128);
            this.txtSQLPort.Name = "txtSQLPort";
            this.txtSQLPort.Size = new System.Drawing.Size(77, 21);
            this.txtSQLPort.TabIndex = 32;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(156, 131);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(30, 13);
            this.label70.TabIndex = 31;
            this.label70.Text = "Port:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 164);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(113, 13);
            this.linkLabel1.TabIndex = 30;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Tag = "Click here to go to the download page for MySQL.";
            this.linkLabel1.Text = "MySQL Download Page";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // txtSQLHost
            // 
            this.txtSQLHost.Location = new System.Drawing.Point(50, 128);
            this.txtSQLHost.Name = "txtSQLHost";
            this.txtSQLHost.Size = new System.Drawing.Size(100, 21);
            this.txtSQLHost.TabIndex = 8;
            this.txtSQLHost.Tag = "The host name for the database. Leave this unless problems occur.";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(12, 131);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(32, 13);
            this.label43.TabIndex = 7;
            this.label43.Text = "Host:";
            // 
            // txtSQLDatabase
            // 
            this.txtSQLDatabase.Location = new System.Drawing.Point(104, 94);
            this.txtSQLDatabase.Name = "txtSQLDatabase";
            this.txtSQLDatabase.Size = new System.Drawing.Size(100, 21);
            this.txtSQLDatabase.TabIndex = 6;
            this.txtSQLDatabase.Tag = "The name of the database stored (Default = MCZall)";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(12, 97);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(86, 13);
            this.label42.TabIndex = 5;
            this.label42.Text = "Database Name:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(12, 63);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(56, 13);
            this.label40.TabIndex = 4;
            this.label40.Text = "Password:";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(12, 28);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(59, 13);
            this.label41.TabIndex = 3;
            this.label41.Text = "Username:";
            // 
            // txtSQLPassword
            // 
            this.txtSQLPassword.Location = new System.Drawing.Point(74, 60);
            this.txtSQLPassword.Name = "txtSQLPassword";
            this.txtSQLPassword.PasswordChar = '*';
            this.txtSQLPassword.Size = new System.Drawing.Size(100, 21);
            this.txtSQLPassword.TabIndex = 2;
            this.txtSQLPassword.Tag = "The password set while installing MySQL";
            // 
            // txtSQLUsername
            // 
            this.txtSQLUsername.Location = new System.Drawing.Point(74, 25);
            this.txtSQLUsername.Name = "txtSQLUsername";
            this.txtSQLUsername.Size = new System.Drawing.Size(100, 21);
            this.txtSQLUsername.TabIndex = 1;
            this.txtSQLUsername.Tag = "The username set while installing MySQL";
            // 
            // grpIRC
            // 
            this.grpIRC.Controls.Add(this.txtIRCPort);
            this.grpIRC.Controls.Add(this.label50);
            this.grpIRC.Controls.Add(this.label49);
            this.grpIRC.Controls.Add(this.txtIrcId);
            this.grpIRC.Controls.Add(this.chkIrcId);
            this.grpIRC.Controls.Add(this.label6);
            this.grpIRC.Controls.Add(this.lblIRC);
            this.grpIRC.Controls.Add(this.txtOpChannel);
            this.grpIRC.Controls.Add(this.cmbIRCColour);
            this.grpIRC.Controls.Add(this.txtIRCServer);
            this.grpIRC.Controls.Add(this.label31);
            this.grpIRC.Controls.Add(this.label23);
            this.grpIRC.Controls.Add(this.txtChannel);
            this.grpIRC.Controls.Add(this.label4);
            this.grpIRC.Controls.Add(this.txtNick);
            this.grpIRC.Controls.Add(this.label5);
            this.grpIRC.Location = new System.Drawing.Point(22, 43);
            this.grpIRC.Name = "grpIRC";
            this.grpIRC.Size = new System.Drawing.Size(284, 232);
            this.grpIRC.TabIndex = 27;
            this.grpIRC.TabStop = false;
            this.grpIRC.Text = "IRC";
            // 
            // txtIRCPort
            // 
            this.txtIRCPort.Location = new System.Drawing.Point(223, 24);
            this.txtIRCPort.Name = "txtIRCPort";
            this.txtIRCPort.Size = new System.Drawing.Size(46, 21);
            this.txtIRCPort.TabIndex = 31;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(187, 27);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(30, 13);
            this.label50.TabIndex = 30;
            this.label50.Text = "Port:";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(138, 203);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(56, 13);
            this.label49.TabIndex = 29;
            this.label49.Text = "Password:";
            // 
            // txtIrcId
            // 
            this.txtIrcId.Location = new System.Drawing.Point(200, 200);
            this.txtIrcId.Name = "txtIrcId";
            this.txtIrcId.PasswordChar = '*';
            this.txtIrcId.Size = new System.Drawing.Size(69, 21);
            this.txtIrcId.TabIndex = 28;
            this.txtIrcId.Tag = "The password used for NickServ";
            // 
            // chkIrcId
            // 
            this.chkIrcId.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkIrcId.AutoSize = true;
            this.chkIrcId.Location = new System.Drawing.Point(15, 198);
            this.chkIrcId.Name = "chkIrcId";
            this.chkIrcId.Size = new System.Drawing.Size(120, 23);
            this.chkIrcId.TabIndex = 27;
            this.chkIrcId.Text = "Identify with NickServ";
            this.chkIrcId.UseVisualStyleBackColor = true;
            this.chkIrcId.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Server:";
            // 
            // lblIRC
            // 
            this.lblIRC.Location = new System.Drawing.Point(147, 164);
            this.lblIRC.Name = "lblIRC";
            this.lblIRC.Size = new System.Drawing.Size(26, 23);
            this.lblIRC.TabIndex = 24;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(12, 134);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(64, 13);
            this.label31.TabIndex = 25;
            this.label31.Text = "Op Channel:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(12, 169);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(51, 13);
            this.label23.TabIndex = 21;
            this.label23.Text = "IRC color:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Nick:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Channel:";
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.groupBox17);
            this.tabPage1.Controls.Add(this.groupBox8);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(488, 509);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Server";
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.updateTimeNumeric);
            this.groupBox17.Controls.Add(this.label71);
            this.groupBox17.Controls.Add(this.notifyInGameUpdate);
            this.groupBox17.Controls.Add(this.autoUpdate);
            this.groupBox17.Controls.Add(this.chkUpdates);
            this.groupBox17.Location = new System.Drawing.Point(370, 131);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(112, 130);
            this.groupBox17.TabIndex = 44;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Update Settings";
            // 
            // updateTimeNumeric
            // 
            this.updateTimeNumeric.Location = new System.Drawing.Point(55, 106);
            this.updateTimeNumeric.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.updateTimeNumeric.Name = "updateTimeNumeric";
            this.updateTimeNumeric.Size = new System.Drawing.Size(39, 21);
            this.updateTimeNumeric.TabIndex = 29;
            this.updateTimeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(6, 108);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(49, 13);
            this.label71.TabIndex = 5;
            this.label71.Text = "Seconds:";
            // 
            // notifyInGameUpdate
            // 
            this.notifyInGameUpdate.Appearance = System.Windows.Forms.Appearance.Button;
            this.notifyInGameUpdate.AutoSize = true;
            this.notifyInGameUpdate.Location = new System.Drawing.Point(5, 78);
            this.notifyInGameUpdate.Name = "notifyInGameUpdate";
            this.notifyInGameUpdate.Size = new System.Drawing.Size(87, 23);
            this.notifyInGameUpdate.TabIndex = 7;
            this.notifyInGameUpdate.Text = "Notify In-Game";
            this.notifyInGameUpdate.UseVisualStyleBackColor = true;
            // 
            // autoUpdate
            // 
            this.autoUpdate.Appearance = System.Windows.Forms.Appearance.Button;
            this.autoUpdate.AutoSize = true;
            this.autoUpdate.Location = new System.Drawing.Point(6, 49);
            this.autoUpdate.Name = "autoUpdate";
            this.autoUpdate.Size = new System.Drawing.Size(77, 23);
            this.autoUpdate.TabIndex = 6;
            this.autoUpdate.Text = "Auto Update";
            this.autoUpdate.UseVisualStyleBackColor = true;
            // 
            // chkUpdates
            // 
            this.chkUpdates.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkUpdates.AutoSize = true;
            this.chkUpdates.Location = new System.Drawing.Point(5, 20);
            this.chkUpdates.Name = "chkUpdates";
            this.chkUpdates.Size = new System.Drawing.Size(104, 23);
            this.chkUpdates.TabIndex = 4;
            this.chkUpdates.Text = "Check for updates";
            this.chkUpdates.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.txtServerOwner);
            this.groupBox8.Location = new System.Drawing.Point(230, 218);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(134, 43);
            this.groupBox8.TabIndex = 47;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Server Owner";
            // 
            // txtServerOwner
            // 
            this.txtServerOwner.Location = new System.Drawing.Point(6, 16);
            this.txtServerOwner.Name = "txtServerOwner";
            this.txtServerOwner.Size = new System.Drawing.Size(119, 21);
            this.txtServerOwner.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.forceUpdateBtn);
            this.groupBox7.Controls.Add(this.label30);
            this.groupBox7.Controls.Add(this.txtHost);
            this.groupBox7.Location = new System.Drawing.Point(329, 392);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(153, 104);
            this.groupBox7.TabIndex = 36;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Extra";
            // 
            // forceUpdateBtn
            // 
            this.forceUpdateBtn.AutoSize = true;
            this.forceUpdateBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.forceUpdateBtn.Location = new System.Drawing.Point(17, 70);
            this.forceUpdateBtn.Name = "forceUpdateBtn";
            this.forceUpdateBtn.Size = new System.Drawing.Size(80, 23);
            this.forceUpdateBtn.TabIndex = 6;
            this.forceUpdateBtn.Text = "Force update";
            this.forceUpdateBtn.UseVisualStyleBackColor = true;
            this.forceUpdateBtn.Click += new System.EventHandler(this.forceUpdateBtn_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(14, 21);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(75, 13);
            this.label30.TabIndex = 3;
            this.label30.Text = "Console State:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(17, 43);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(89, 21);
            this.txtHost.TabIndex = 2;
            this.txtHost.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label21);
            this.groupBox6.Controls.Add(this.numPlayers);
            this.groupBox6.Controls.Add(this.chkAgreeToRules);
            this.groupBox6.Controls.Add(this.label35);
            this.groupBox6.Controls.Add(this.numGuests);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.cmbDefaultColour);
            this.groupBox6.Controls.Add(this.lblDefault);
            this.groupBox6.Location = new System.Drawing.Point(6, 131);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(218, 130);
            this.groupBox6.TabIndex = 46;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Players";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(12, 22);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(67, 13);
            this.label21.TabIndex = 3;
            this.label21.Text = "Max Players:";
            // 
            // numPlayers
            // 
            this.numPlayers.Location = new System.Drawing.Point(85, 20);
            this.numPlayers.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numPlayers.Name = "numPlayers";
            this.numPlayers.Size = new System.Drawing.Size(60, 21);
            this.numPlayers.TabIndex = 29;
            this.numPlayers.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numPlayers.ValueChanged += new System.EventHandler(this.numPlayers_ValueChanged);
            // 
            // chkAgreeToRules
            // 
            this.chkAgreeToRules.AutoSize = true;
            this.chkAgreeToRules.Location = new System.Drawing.Point(15, 77);
            this.chkAgreeToRules.Name = "chkAgreeToRules";
            this.chkAgreeToRules.Size = new System.Drawing.Size(189, 17);
            this.chkAgreeToRules.TabIndex = 32;
            this.chkAgreeToRules.Tag = "Forces guests to use /agree on entry to the server";
            this.chkAgreeToRules.Text = "Force guests to read rules on entry\r\n";
            this.chkAgreeToRules.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(12, 48);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(65, 13);
            this.label35.TabIndex = 27;
            this.label35.Text = "Max Guests:";
            // 
            // numGuests
            // 
            this.numGuests.Location = new System.Drawing.Point(83, 46);
            this.numGuests.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numGuests.Name = "numGuests";
            this.numGuests.Size = new System.Drawing.Size(60, 21);
            this.numGuests.TabIndex = 28;
            this.numGuests.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 104);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Default color:";
            // 
            // lblDefault
            // 
            this.lblDefault.Location = new System.Drawing.Point(152, 100);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new System.Drawing.Size(21, 21);
            this.lblDefault.TabIndex = 10;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label29);
            this.groupBox5.Controls.Add(this.cmbDefaultRank);
            this.groupBox5.Controls.Add(this.lblOpChat);
            this.groupBox5.Controls.Add(this.cmbAdminChat);
            this.groupBox5.Controls.Add(this.cmbOpChat);
            this.groupBox5.Controls.Add(this.label37);
            this.groupBox5.Controls.Add(this.chkAdminsJoinSilent);
            this.groupBox5.Controls.Add(this.chkTpToHigherRanks);
            this.groupBox5.Location = new System.Drawing.Point(6, 392);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(317, 104);
            this.groupBox5.TabIndex = 45;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Ranks";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(7, 23);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(68, 13);
            this.label29.TabIndex = 20;
            this.label29.Text = "Default rank:";
            // 
            // lblOpChat
            // 
            this.lblOpChat.AutoSize = true;
            this.lblOpChat.Location = new System.Drawing.Point(7, 51);
            this.lblOpChat.Name = "lblOpChat";
            this.lblOpChat.Size = new System.Drawing.Size(70, 13);
            this.lblOpChat.TabIndex = 22;
            this.lblOpChat.Text = "Op Chat rank:";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(7, 80);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(87, 13);
            this.label37.TabIndex = 33;
            this.label37.Text = "Admin Chat rank:";
            // 
            // chkAdminsJoinSilent
            // 
            this.chkAdminsJoinSilent.AutoSize = true;
            this.chkAdminsJoinSilent.Location = new System.Drawing.Point(170, 20);
            this.chkAdminsJoinSilent.Name = "chkAdminsJoinSilent";
            this.chkAdminsJoinSilent.Size = new System.Drawing.Size(113, 17);
            this.chkAdminsJoinSilent.TabIndex = 39;
            this.chkAdminsJoinSilent.Tag = "Players who have the adminchat rank join the game silently.";
            this.chkAdminsJoinSilent.Text = "Admins join silently";
            this.chkAdminsJoinSilent.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.txtMain);
            this.groupBox4.Controls.Add(this.chkAutoload);
            this.groupBox4.Controls.Add(this.chkWorld);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.txtMaps);
            this.groupBox4.Location = new System.Drawing.Point(284, 271);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(198, 115);
            this.groupBox4.TabIndex = 44;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Level Settings";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(13, 26);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(63, 13);
            this.label27.TabIndex = 3;
            this.label27.Text = "Main name:";
            // 
            // txtMain
            // 
            this.txtMain.Location = new System.Drawing.Point(82, 20);
            this.txtMain.Name = "txtMain";
            this.txtMain.Size = new System.Drawing.Size(60, 21);
            this.txtMain.TabIndex = 2;
            this.txtMain.TextChanged += new System.EventHandler(this.txtMaps_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(13, 56);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(58, 13);
            this.label22.TabIndex = 3;
            this.label22.Text = "Max Maps:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.editTxtsBt);
            this.groupBox2.Controls.Add(this.chkWomDirect);
            this.groupBox2.Controls.Add(this.chkRestart);
            this.groupBox2.Controls.Add(this.chkPublic);
            this.groupBox2.Controls.Add(this.chkVerify);
            this.groupBox2.Controls.Add(this.chkLogBeat);
            this.groupBox2.Location = new System.Drawing.Point(6, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 115);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Advanced Configuration";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(114, 80);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(133, 23);
            this.button3.TabIndex = 43;
            this.button3.Text = "Edit WoM Direct Options";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // editTxtsBt
            // 
            this.editTxtsBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editTxtsBt.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editTxtsBt.Location = new System.Drawing.Point(152, 22);
            this.editTxtsBt.Name = "editTxtsBt";
            this.editTxtsBt.Size = new System.Drawing.Size(80, 23);
            this.editTxtsBt.TabIndex = 35;
            this.editTxtsBt.Text = "Edit Text Files";
            this.editTxtsBt.UseVisualStyleBackColor = true;
            this.editTxtsBt.Click += new System.EventHandler(this.editTxtsBt_Click_1);
            // 
            // chkWomDirect
            // 
            this.chkWomDirect.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWomDirect.AutoSize = true;
            this.chkWomDirect.Location = new System.Drawing.Point(114, 51);
            this.chkWomDirect.Name = "chkWomDirect";
            this.chkWomDirect.Size = new System.Drawing.Size(110, 23);
            this.chkWomDirect.TabIndex = 25;
            this.chkWomDirect.Text = "Enable WoM Direct";
            this.chkWomDirect.UseVisualStyleBackColor = true;
            this.chkWomDirect.CheckedChanged += new System.EventHandler(this.chkWomDirect_CheckedChanged);
            // 
            // chkRestart
            // 
            this.chkRestart.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkRestart.AutoSize = true;
            this.chkRestart.Location = new System.Drawing.Point(16, 51);
            this.chkRestart.Name = "chkRestart";
            this.chkRestart.Size = new System.Drawing.Size(90, 23);
            this.chkRestart.TabIndex = 4;
            this.chkRestart.Text = "Restart on error";
            this.chkRestart.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ChkTunnels);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtDepth);
            this.groupBox3.Location = new System.Drawing.Point(230, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(134, 82);
            this.groupBox3.TabIndex = 43;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tunnel Prevention";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Depth:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(417, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(68, 68);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMOTD);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.ChkPort);
            this.groupBox1.Controls.Add(this.ChkPortResult);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 124);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Configuration";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "MOTD:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Port:";
            // 
            // ChkPort
            // 
            this.ChkPort.Location = new System.Drawing.Point(120, 83);
            this.ChkPort.Name = "ChkPort";
            this.ChkPort.Size = new System.Drawing.Size(69, 23);
            this.ChkPort.TabIndex = 25;
            this.ChkPort.Text = "Check Port";
            this.ChkPort.UseVisualStyleBackColor = true;
            this.ChkPort.Click += new System.EventHandler(this.ChkPort_Click);
            // 
            // ChkPortResult
            // 
            this.ChkPortResult.BackColor = System.Drawing.SystemColors.Control;
            this.ChkPortResult.Location = new System.Drawing.Point(205, 84);
            this.ChkPortResult.Name = "ChkPortResult";
            this.ChkPortResult.ReadOnly = true;
            this.ChkPortResult.Size = new System.Drawing.Size(149, 21);
            this.ChkPortResult.TabIndex = 26;
            this.ChkPortResult.Text = "Port Check Not Started!";
            this.ChkPortResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabIRC);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Controls.Add(this.tabPage9);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage5);
            this.tabControl.Controls.Add(this.tabPage8);
            this.tabControl.Controls.Add(this.tabPage13);
            this.tabControl.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(0, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(496, 535);
            this.tabControl.TabIndex = 0;
            this.tabControl.Click += new System.EventHandler(this.tabControl_Click);
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage9.Controls.Add(this.tabControl2);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(488, 509);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = "Games";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage10);
            this.tabControl2.Controls.Add(this.tabPage11);
            this.tabControl2.Location = new System.Drawing.Point(9, 7);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(476, 499);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage10
            // 
            this.tabPage10.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage10.Controls.Add(this.groupBox23);
            this.tabPage10.Controls.Add(this.groupBox22);
            this.tabPage10.Controls.Add(this.groupBox21);
            this.tabPage10.Controls.Add(this.groupBox20);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(468, 473);
            this.tabPage10.TabIndex = 0;
            this.tabPage10.Text = "Lava Survival";
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.lsBtnEndVote);
            this.groupBox23.Controls.Add(this.lsBtnEndRound);
            this.groupBox23.Controls.Add(this.lsBtnStopGame);
            this.groupBox23.Controls.Add(this.lsBtnStartGame);
            this.groupBox23.Location = new System.Drawing.Point(291, 226);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(171, 165);
            this.groupBox23.TabIndex = 4;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Controls";
            // 
            // lsBtnEndVote
            // 
            this.lsBtnEndVote.Location = new System.Drawing.Point(6, 129);
            this.lsBtnEndVote.Name = "lsBtnEndVote";
            this.lsBtnEndVote.Size = new System.Drawing.Size(159, 30);
            this.lsBtnEndVote.TabIndex = 3;
            this.lsBtnEndVote.Text = "End Vote";
            this.lsBtnEndVote.UseVisualStyleBackColor = true;
            this.lsBtnEndVote.Click += new System.EventHandler(this.lsBtnEndVote_Click);
            // 
            // lsBtnEndRound
            // 
            this.lsBtnEndRound.Location = new System.Drawing.Point(6, 93);
            this.lsBtnEndRound.Name = "lsBtnEndRound";
            this.lsBtnEndRound.Size = new System.Drawing.Size(159, 30);
            this.lsBtnEndRound.TabIndex = 2;
            this.lsBtnEndRound.Text = "End Round";
            this.lsBtnEndRound.UseVisualStyleBackColor = true;
            this.lsBtnEndRound.Click += new System.EventHandler(this.lsBtnEndRound_Click);
            // 
            // lsBtnStopGame
            // 
            this.lsBtnStopGame.Location = new System.Drawing.Point(6, 57);
            this.lsBtnStopGame.Name = "lsBtnStopGame";
            this.lsBtnStopGame.Size = new System.Drawing.Size(159, 30);
            this.lsBtnStopGame.TabIndex = 1;
            this.lsBtnStopGame.Text = "Stop Game";
            this.lsBtnStopGame.UseVisualStyleBackColor = true;
            this.lsBtnStopGame.Click += new System.EventHandler(this.lsBtnStopGame_Click);
            // 
            // lsBtnStartGame
            // 
            this.lsBtnStartGame.Location = new System.Drawing.Point(6, 21);
            this.lsBtnStartGame.Name = "lsBtnStartGame";
            this.lsBtnStartGame.Size = new System.Drawing.Size(159, 30);
            this.lsBtnStartGame.TabIndex = 0;
            this.lsBtnStartGame.Text = "Start Game";
            this.lsBtnStartGame.UseVisualStyleBackColor = true;
            this.lsBtnStartGame.Click += new System.EventHandler(this.lsBtnStartGame_Click);
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.lsBtnSaveSettings);
            this.groupBox22.Controls.Add(this.lsNudFloodTime);
            this.groupBox22.Controls.Add(this.label67);
            this.groupBox22.Controls.Add(this.lsNudRoundTime);
            this.groupBox22.Controls.Add(this.label66);
            this.groupBox22.Controls.Add(this.lsNudLayerTime);
            this.groupBox22.Controls.Add(this.label65);
            this.groupBox22.Controls.Add(this.lsNudLayerCount);
            this.groupBox22.Controls.Add(this.label64);
            this.groupBox22.Controls.Add(this.lsNudLayerHeight);
            this.groupBox22.Controls.Add(this.label63);
            this.groupBox22.Controls.Add(this.lsNudLayer);
            this.groupBox22.Controls.Add(this.label62);
            this.groupBox22.Controls.Add(this.lsNudWater);
            this.groupBox22.Controls.Add(this.label61);
            this.groupBox22.Controls.Add(this.lsNudDestroy);
            this.groupBox22.Controls.Add(this.label60);
            this.groupBox22.Controls.Add(this.lsNudKiller);
            this.groupBox22.Controls.Add(this.label59);
            this.groupBox22.Controls.Add(this.lsNudFastLava);
            this.groupBox22.Controls.Add(this.label58);
            this.groupBox22.Location = new System.Drawing.Point(7, 199);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(278, 195);
            this.groupBox22.TabIndex = 3;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Map Settings";
            // 
            // lsBtnSaveSettings
            // 
            this.lsBtnSaveSettings.Location = new System.Drawing.Point(6, 155);
            this.lsBtnSaveSettings.Name = "lsBtnSaveSettings";
            this.lsBtnSaveSettings.Size = new System.Drawing.Size(266, 34);
            this.lsBtnSaveSettings.TabIndex = 21;
            this.lsBtnSaveSettings.Text = "Save Map Settings";
            this.lsBtnSaveSettings.UseVisualStyleBackColor = true;
            this.lsBtnSaveSettings.Click += new System.EventHandler(this.lsBtnSaveSettings_Click);
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(139, 131);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(61, 13);
            this.label67.TabIndex = 19;
            this.label67.Text = "Flood Time:";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(139, 105);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(65, 13);
            this.label66.TabIndex = 17;
            this.label66.Text = "Round Time:";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(139, 77);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(61, 13);
            this.label65.TabIndex = 15;
            this.label65.Text = "Layer Time:";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(139, 50);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(66, 13);
            this.label64.TabIndex = 13;
            this.label64.Text = "Layer Count:";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(139, 23);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(69, 13);
            this.label63.TabIndex = 11;
            this.label63.Text = "Layer Height:";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(7, 131);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(36, 13);
            this.label62.TabIndex = 8;
            this.label62.Text = "Layer:";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(7, 104);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(40, 13);
            this.label61.TabIndex = 6;
            this.label61.Text = "Water:";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(7, 77);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(47, 13);
            this.label60.TabIndex = 4;
            this.label60.Text = "Destroy:";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(7, 50);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(35, 13);
            this.label59.TabIndex = 2;
            this.label59.Text = "Killer:";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(7, 23);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(54, 13);
            this.label58.TabIndex = 0;
            this.label58.Text = "Fast Lava:";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.label75);
            this.groupBox21.Controls.Add(this.lsNudLives);
            this.groupBox21.Controls.Add(this.lsCmbControlRank);
            this.groupBox21.Controls.Add(this.label68);
            this.groupBox21.Controls.Add(this.lsCmbSetupRank);
            this.groupBox21.Controls.Add(this.label57);
            this.groupBox21.Controls.Add(this.lsNudVoteTime);
            this.groupBox21.Controls.Add(this.label56);
            this.groupBox21.Controls.Add(this.lsNudVoteCount);
            this.groupBox21.Controls.Add(this.label55);
            this.groupBox21.Controls.Add(this.lsChkSendAFKMain);
            this.groupBox21.Controls.Add(this.lsChkStartOnStartup);
            this.groupBox21.Location = new System.Drawing.Point(291, 6);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(172, 214);
            this.groupBox21.TabIndex = 2;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Settings";
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(8, 136);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(34, 13);
            this.label75.TabIndex = 11;
            this.label75.Text = "Lives:";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(8, 191);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(70, 13);
            this.label68.TabIndex = 8;
            this.label68.Text = "Control Rank:";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(8, 164);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(62, 13);
            this.label57.TabIndex = 6;
            this.label57.Text = "Setup Rank:";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(8, 109);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(57, 13);
            this.label56.TabIndex = 4;
            this.label56.Text = "Vote Time:";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(8, 82);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(62, 13);
            this.label55.TabIndex = 2;
            this.label55.Text = "Vote Count:";
            this.label55.Click += new System.EventHandler(this.label55_Click);
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.button2);
            this.groupBox20.Controls.Add(this.label54);
            this.groupBox20.Controls.Add(this.label53);
            this.groupBox20.Controls.Add(this.lsAddMap);
            this.groupBox20.Controls.Add(this.lsRemoveMap);
            this.groupBox20.Controls.Add(this.lsMapNoUse);
            this.groupBox20.Controls.Add(this.lsMapUse);
            this.groupBox20.Location = new System.Drawing.Point(6, 6);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(279, 186);
            this.groupBox20.TabIndex = 1;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Maps";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(102, 157);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Get Maps";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(187, 17);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(83, 13);
            this.label54.TabIndex = 6;
            this.label54.Text = "Maps Not In Use";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(19, 17);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(64, 13);
            this.label53.TabIndex = 5;
            this.label53.Text = "Maps In Use";
            // 
            // lsAddMap
            // 
            this.lsAddMap.Location = new System.Drawing.Point(102, 81);
            this.lsAddMap.Name = "lsAddMap";
            this.lsAddMap.Size = new System.Drawing.Size(75, 23);
            this.lsAddMap.TabIndex = 4;
            this.lsAddMap.Text = "<< Add";
            this.lsAddMap.UseVisualStyleBackColor = true;
            this.lsAddMap.Click += new System.EventHandler(this.lsAddMap_Click);
            // 
            // lsRemoveMap
            // 
            this.lsRemoveMap.Location = new System.Drawing.Point(102, 110);
            this.lsRemoveMap.Name = "lsRemoveMap";
            this.lsRemoveMap.Size = new System.Drawing.Size(75, 23);
            this.lsRemoveMap.TabIndex = 3;
            this.lsRemoveMap.Text = "Remove >>";
            this.lsRemoveMap.UseVisualStyleBackColor = true;
            this.lsRemoveMap.Click += new System.EventHandler(this.lsRemoveMap_Click);
            // 
            // lsMapNoUse
            // 
            this.lsMapNoUse.FormattingEnabled = true;
            this.lsMapNoUse.Location = new System.Drawing.Point(183, 33);
            this.lsMapNoUse.Name = "lsMapNoUse";
            this.lsMapNoUse.Size = new System.Drawing.Size(90, 147);
            this.lsMapNoUse.TabIndex = 2;
            // 
            // lsMapUse
            // 
            this.lsMapUse.FormattingEnabled = true;
            this.lsMapUse.Location = new System.Drawing.Point(6, 33);
            this.lsMapUse.Name = "lsMapUse";
            this.lsMapUse.Size = new System.Drawing.Size(90, 147);
            this.lsMapUse.TabIndex = 0;
            this.lsMapUse.SelectedIndexChanged += new System.EventHandler(this.lsMapUse_SelectedIndexChanged);
            // 
            // tabPage11
            // 
            this.tabPage11.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage11.Controls.Add(this.groupBox16);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(468, 473);
            this.tabPage11.TabIndex = 1;
            this.tabPage11.Text = "Misc";
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.levelList);
            this.groupBox16.Controls.Add(this.label78);
            this.groupBox16.Controls.Add(this.label77);
            this.groupBox16.Controls.Add(this.chkEnableChangingLevels);
            this.groupBox16.Controls.Add(this.chkZombieOnlyServer);
            this.groupBox16.Controls.Add(this.chkUseLevelList);
            this.groupBox16.Controls.Add(this.chkNoPillaringDuringZombie);
            this.groupBox16.Controls.Add(this.ZombieName);
            this.groupBox16.Controls.Add(this.label46);
            this.groupBox16.Controls.Add(this.chkNoLevelSavingDuringZombie);
            this.groupBox16.Controls.Add(this.chkNoRespawnDuringZombie);
            this.groupBox16.Controls.Add(this.chkZombieOnServerStart);
            this.groupBox16.Location = new System.Drawing.Point(6, 6);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(134, 378);
            this.groupBox16.TabIndex = 41;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Zombie Survival";
            // 
            // levelList
            // 
            this.levelList.Location = new System.Drawing.Point(8, 351);
            this.levelList.Name = "levelList";
            this.levelList.Size = new System.Drawing.Size(120, 21);
            this.levelList.TabIndex = 12;
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(0, 335);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(133, 13);
            this.label78.TabIndex = 11;
            this.label78.Text = "Must be comma seperated";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(38, 320);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(51, 13);
            this.label77.TabIndex = 10;
            this.label77.Text = "Level List";
            // 
            // chkEnableChangingLevels
            // 
            this.chkEnableChangingLevels.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEnableChangingLevels.AutoSize = true;
            this.chkEnableChangingLevels.Location = new System.Drawing.Point(5, 223);
            this.chkEnableChangingLevels.Name = "chkEnableChangingLevels";
            this.chkEnableChangingLevels.Size = new System.Drawing.Size(126, 23);
            this.chkEnableChangingLevels.TabIndex = 9;
            this.chkEnableChangingLevels.Text = "Enable Changing Levels";
            this.chkEnableChangingLevels.UseVisualStyleBackColor = true;
            // 
            // chkZombieOnlyServer
            // 
            this.chkZombieOnlyServer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZombieOnlyServer.AutoSize = true;
            this.chkZombieOnlyServer.Location = new System.Drawing.Point(6, 253);
            this.chkZombieOnlyServer.Name = "chkZombieOnlyServer";
            this.chkZombieOnlyServer.Size = new System.Drawing.Size(122, 36);
            this.chkZombieOnlyServer.TabIndex = 8;
            this.chkZombieOnlyServer.Text = "Zombie Only \r\nServer (Experimental!)";
            this.chkZombieOnlyServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZombieOnlyServer.UseVisualStyleBackColor = true;
            // 
            // chkUseLevelList
            // 
            this.chkUseLevelList.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkUseLevelList.AutoSize = true;
            this.chkUseLevelList.Location = new System.Drawing.Point(25, 293);
            this.chkUseLevelList.Name = "chkUseLevelList";
            this.chkUseLevelList.Size = new System.Drawing.Size(81, 23);
            this.chkUseLevelList.TabIndex = 7;
            this.chkUseLevelList.Text = "Use Level List";
            this.chkUseLevelList.UseVisualStyleBackColor = true;
            // 
            // chkNoPillaringDuringZombie
            // 
            this.chkNoPillaringDuringZombie.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNoPillaringDuringZombie.AutoSize = true;
            this.chkNoPillaringDuringZombie.Location = new System.Drawing.Point(21, 129);
            this.chkNoPillaringDuringZombie.Name = "chkNoPillaringDuringZombie";
            this.chkNoPillaringDuringZombie.Size = new System.Drawing.Size(83, 36);
            this.chkNoPillaringDuringZombie.TabIndex = 6;
            this.chkNoPillaringDuringZombie.Text = "No pillaring \r\nduring zombie";
            this.chkNoPillaringDuringZombie.UseVisualStyleBackColor = true;
            // 
            // ZombieName
            // 
            this.ZombieName.Location = new System.Drawing.Point(16, 196);
            this.ZombieName.Name = "ZombieName";
            this.ZombieName.Size = new System.Drawing.Size(100, 21);
            this.ZombieName.TabIndex = 4;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(8, 167);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(119, 26);
            this.label46.TabIndex = 3;
            this.label46.Text = "Name while Infected \r\nleave blank for no name\r\n";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkNoLevelSavingDuringZombie
            // 
            this.chkNoLevelSavingDuringZombie.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNoLevelSavingDuringZombie.AutoSize = true;
            this.chkNoLevelSavingDuringZombie.Location = new System.Drawing.Point(8, 89);
            this.chkNoLevelSavingDuringZombie.Name = "chkNoLevelSavingDuringZombie";
            this.chkNoLevelSavingDuringZombie.Size = new System.Drawing.Size(112, 36);
            this.chkNoLevelSavingDuringZombie.TabIndex = 2;
            this.chkNoLevelSavingDuringZombie.Text = "Disable level saving \r\n      during Zombie";
            this.chkNoLevelSavingDuringZombie.UseVisualStyleBackColor = true;
            // 
            // chkNoRespawnDuringZombie
            // 
            this.chkNoRespawnDuringZombie.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNoRespawnDuringZombie.AutoSize = true;
            this.chkNoRespawnDuringZombie.Location = new System.Drawing.Point(9, 47);
            this.chkNoRespawnDuringZombie.Name = "chkNoRespawnDuringZombie";
            this.chkNoRespawnDuringZombie.Size = new System.Drawing.Size(110, 36);
            this.chkNoRespawnDuringZombie.TabIndex = 1;
            this.chkNoRespawnDuringZombie.Text = "Disable respawning\r\n      during Zombie";
            this.chkNoRespawnDuringZombie.UseVisualStyleBackColor = true;
            // 
            // chkZombieOnServerStart
            // 
            this.chkZombieOnServerStart.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZombieOnServerStart.AutoSize = true;
            this.chkZombieOnServerStart.Location = new System.Drawing.Point(8, 19);
            this.chkZombieOnServerStart.Name = "chkZombieOnServerStart";
            this.chkZombieOnServerStart.Size = new System.Drawing.Size(111, 23);
            this.chkZombieOnServerStart.TabIndex = 0;
            this.chkZombieOnServerStart.Text = "Start on server start";
            this.chkZombieOnServerStart.UseVisualStyleBackColor = true;
            this.chkZombieOnServerStart.CheckedChanged += new System.EventHandler(this.chkZombieOnServerStart_CheckedChanged);
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.Transparent;
            this.tabPage8.Controls.Add(this.groupBox15);
            this.tabPage8.Controls.Add(this.groupBox14);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(488, 509);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "Verification/AntiSpam";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.numCountReset);
            this.groupBox15.Controls.Add(this.label69);
            this.groupBox15.Controls.Add(this.numSpamMute);
            this.groupBox15.Controls.Add(this.label45);
            this.groupBox15.Controls.Add(this.numSpamMessages);
            this.groupBox15.Controls.Add(this.label44);
            this.groupBox15.Controls.Add(this.chkSpamControl);
            this.groupBox15.Location = new System.Drawing.Point(225, 18);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(248, 189);
            this.groupBox15.TabIndex = 1;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Spam Control";
            // 
            // numCountReset
            // 
            this.numCountReset.Location = new System.Drawing.Point(170, 143);
            this.numCountReset.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numCountReset.Name = "numCountReset";
            this.numCountReset.Size = new System.Drawing.Size(60, 21);
            this.numCountReset.TabIndex = 34;
            this.numCountReset.Tag = "The number of seconds before the message counter is reset.";
            this.numCountReset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numCountReset.ValueChanged += new System.EventHandler(this.numCountReset_ValueChanged);
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(15, 145);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(149, 13);
            this.label69.TabIndex = 33;
            this.label69.Text = "Counter Reset Time (seconds):";
            // 
            // numSpamMute
            // 
            this.numSpamMute.Location = new System.Drawing.Point(158, 104);
            this.numSpamMute.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numSpamMute.Name = "numSpamMute";
            this.numSpamMute.Size = new System.Drawing.Size(60, 21);
            this.numSpamMute.TabIndex = 32;
            this.numSpamMute.Tag = "The number of seconds a player is muted for, for spamming.";
            this.numSpamMute.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(15, 106);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(137, 13);
            this.label45.TabIndex = 31;
            this.label45.Text = "Spam Mute Time (seconds) :";
            // 
            // numSpamMessages
            // 
            this.numSpamMessages.Location = new System.Drawing.Point(106, 67);
            this.numSpamMessages.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numSpamMessages.Name = "numSpamMessages";
            this.numSpamMessages.Size = new System.Drawing.Size(60, 21);
            this.numSpamMessages.TabIndex = 30;
            this.numSpamMessages.Tag = "The amount of messages that have to be sent before a player is muted.";
            this.numSpamMessages.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(15, 71);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(85, 13);
            this.label44.TabIndex = 25;
            this.label44.Text = "Spam Messages:";
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.btnReset);
            this.groupBox14.Controls.Add(this.listPasswords);
            this.groupBox14.Controls.Add(this.label39);
            this.groupBox14.Controls.Add(this.chkEnableVerification);
            this.groupBox14.Controls.Add(this.cmbVerificationRank);
            this.groupBox14.Controls.Add(this.label38);
            this.groupBox14.Location = new System.Drawing.Point(19, 18);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(191, 322);
            this.groupBox14.TabIndex = 0;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Admin Verification";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(45, 276);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(91, 27);
            this.btnReset.TabIndex = 25;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // listPasswords
            // 
            this.listPasswords.FormattingEnabled = true;
            this.listPasswords.Location = new System.Drawing.Point(24, 132);
            this.listPasswords.Name = "listPasswords";
            this.listPasswords.Size = new System.Drawing.Size(139, 134);
            this.listPasswords.TabIndex = 1;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(21, 106);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(98, 13);
            this.label39.TabIndex = 24;
            this.label39.Text = "Remove Passwords";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(21, 67);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(33, 39);
            this.label38.TabIndex = 1;
            this.label38.Text = "Rank:\r\n\r\n\r\n";
            // 
            // tabPage13
            // 
            this.tabPage13.BackColor = System.Drawing.Color.Transparent;
            this.tabPage13.Controls.Add(this.groupBox28);
            this.tabPage13.Controls.Add(this.groupBox27);
            this.tabPage13.Controls.Add(this.groupBox26);
            this.tabPage13.Controls.Add(this.groupBox25);
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage13.Size = new System.Drawing.Size(488, 509);
            this.tabPage13.TabIndex = 9;
            this.tabPage13.Text = "Review";
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.numericUpDown1);
            this.groupBox28.Controls.Add(this.label84);
            this.groupBox28.Location = new System.Drawing.Point(154, 235);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(328, 51);
            this.groupBox28.TabIndex = 4;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "Options";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(202, 19);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown1.TabIndex = 1;
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(7, 21);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(77, 13);
            this.label84.TabIndex = 0;
            this.label84.Text = "Cooldown time";
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.button4);
            this.groupBox27.Location = new System.Drawing.Point(154, 175);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(328, 53);
            this.groupBox27.TabIndex = 3;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Actions";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(7, 21);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(315, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "Clear queue";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.label83);
            this.groupBox26.Controls.Add(this.label82);
            this.groupBox26.Controls.Add(this.label81);
            this.groupBox26.Controls.Add(this.label80);
            this.groupBox26.Controls.Add(this.comboBox5);
            this.groupBox26.Controls.Add(this.comboBox4);
            this.groupBox26.Controls.Add(this.comboBox3);
            this.groupBox26.Controls.Add(this.comboBox2);
            this.groupBox26.Controls.Add(this.comboBox1);
            this.groupBox26.Controls.Add(this.label79);
            this.groupBox26.Location = new System.Drawing.Point(154, 7);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(328, 161);
            this.groupBox26.TabIndex = 2;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Permissions";
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(6, 131);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(53, 13);
            this.label83.TabIndex = 9;
            this.label83.Text = "Goto next";
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(6, 103);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(64, 13);
            this.label82.TabIndex = 8;
            this.label82.Text = "Clear queue";
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Location = new System.Drawing.Point(6, 75);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(67, 13);
            this.label81.TabIndex = 7;
            this.label81.Text = "Leave queue";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(6, 47);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(64, 13);
            this.label80.TabIndex = 6;
            this.label80.Text = "Enter queue";
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(178, 131);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(144, 21);
            this.comboBox5.TabIndex = 5;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(178, 103);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(144, 21);
            this.comboBox4.TabIndex = 4;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(178, 75);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(144, 21);
            this.comboBox3.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(178, 47);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(144, 21);
            this.comboBox2.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(178, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(144, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(6, 22);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(62, 13);
            this.label79.TabIndex = 0;
            this.label79.Text = "View queue";
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.listBox1);
            this.groupBox25.Location = new System.Drawing.Point(6, 6);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(141, 497);
            this.groupBox25.TabIndex = 1;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Review Queue";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 20);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(129, 472);
            this.listBox1.TabIndex = 0;
            // 
            // PropertyWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 585);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnDiscard);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PropertyWindow";
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.PropertyWindow_Load);
            this.Disposed += new System.EventHandler(this.PropertyWindow_Unload);
            this.tabPage3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage12.ResumeLayout(false);
            this.tabPage12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extracmdpermnumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudVoteCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudVoteTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudFastLava)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudKiller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudDestroy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudWater)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayerHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayerCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLayerTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudRoundTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudFloodTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsNudLives)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.tabIRC.ResumeLayout(false);
            this.tabIRC.PerformLayout();
            this.grpSQL.ResumeLayout(false);
            this.grpSQL.PerformLayout();
            this.grpIRC.ResumeLayout(false);
            this.grpIRC.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateTimeNumeric)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPlayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGuests)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.groupBox23.ResumeLayout(false);
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCountReset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpamMute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpamMessages)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.tabPage13.ResumeLayout(false);
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox27.ResumeLayout(false);
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.groupBox25.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDiscard;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btnBlHelp;
        private System.Windows.Forms.TextBox txtBlRanks;
        private System.Windows.Forms.TextBox txtBlAllow;
        private System.Windows.Forms.TextBox txtBlLowest;
        private System.Windows.Forms.TextBox txtBlDisallow;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ListBox listBlocks;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button btnCmdHelp;
        private System.Windows.Forms.TextBox txtCmdRanks;
        private System.Windows.Forms.TextBox txtCmdAllow;
        private System.Windows.Forms.TextBox txtCmdLowest;
        private System.Windows.Forms.TextBox txtCmdDisallow;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox listCommands;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.ComboBox cmbColor;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtLimit;
        private System.Windows.Forms.TextBox txtPermission;
        private System.Windows.Forms.TextBox txtRankName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAddRank;
        private System.Windows.Forms.ListBox listRanks;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox hackrank_kick_time;
        private System.Windows.Forms.TextBox txtShutdown;
        private System.Windows.Forms.TextBox txtBanMessage;
        private System.Windows.Forms.TextBox txtNormRp;
        private System.Windows.Forms.TextBox txtRP;
        private System.Windows.Forms.TextBox txtAFKKick;
        private System.Windows.Forms.TextBox txtafk;
        private System.Windows.Forms.TextBox txtBackup;
        private System.Windows.Forms.TextBox txtBackupLocation;
        private System.Windows.Forms.TextBox txtMoneys;
        private System.Windows.Forms.TextBox txtCheap;
        private System.Windows.Forms.TextBox txtRestartTime;
        private System.Windows.Forms.CheckBox hackrank_kick;
        private System.Windows.Forms.CheckBox chkNotifyOnJoinLeave;
        private System.Windows.Forms.CheckBox chkProfanityFilter;
        private System.Windows.Forms.CheckBox chkRepeatMessages;
        private System.Windows.Forms.CheckBox chkForceCuboid;
        private System.Windows.Forms.CheckBox chkShutdown;
        private System.Windows.Forms.CheckBox chkBanMessage;
        private System.Windows.Forms.CheckBox chkrankSuper;
        private System.Windows.Forms.CheckBox chkCheap;
        private System.Windows.Forms.CheckBox chkDeath;
        private System.Windows.Forms.CheckBox chk17Dollar;
        private System.Windows.Forms.CheckBox chkPhysicsRest;
        private System.Windows.Forms.CheckBox chkSmile;
        private System.Windows.Forms.CheckBox chkHelp;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.CheckBox chkRestartTime;
        private System.Windows.Forms.TabPage tabIRC;
        private System.Windows.Forms.TextBox txtOpChannel;
        private System.Windows.Forms.TextBox txtChannel;
        private System.Windows.Forms.TextBox txtIRCServer;
        private System.Windows.Forms.TextBox txtNick;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label lblIRC;
        private System.Windows.Forms.ComboBox cmbIRCColour;
        private System.Windows.Forms.CheckBox chkIRC;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.NumericUpDown numPlayers;
        private System.Windows.Forms.NumericUpDown numGuests;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox ChkPortResult;
        private System.Windows.Forms.TextBox txtDepth;
        private System.Windows.Forms.TextBox txtMain;
        private System.Windows.Forms.TextBox txtMaps;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtMOTD;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button ChkPort;
        private System.Windows.Forms.CheckBox chkLogBeat;
        private System.Windows.Forms.ComboBox cmbOpChat;
        private System.Windows.Forms.Label lblOpChat;
        private System.Windows.Forms.ComboBox cmbDefaultRank;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label lblDefault;
        private System.Windows.Forms.ComboBox cmbDefaultColour;
        private System.Windows.Forms.CheckBox chkRestart;
        private System.Windows.Forms.CheckBox chkPublic;
        private System.Windows.Forms.CheckBox chkAutoload;
        private System.Windows.Forms.CheckBox chkWorld;
        private System.Windows.Forms.CheckBox chkUpdates;
        private System.Windows.Forms.CheckBox chkVerify;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ChkTunnels;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.CheckBox chkAgreeToRules;
        private System.Windows.Forms.ComboBox cmbAdminChat;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.CheckBox chkAdminsJoinSilent;
        private System.Windows.Forms.Button editTxtsBt;
        private System.Windows.Forms.CheckBox chkTpToHigherRanks;
        private System.Windows.Forms.GroupBox grpIRC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox txtServerOwner;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chkIgnoreGlobal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        public System.Windows.Forms.RichTextBox CustCmdTxtBox2;
        public System.Windows.Forms.Button btnDiscardcmd;
        public System.Windows.Forms.Button btnSavecmd;
        public System.Windows.Forms.Button btnIntextbox;
        public System.Windows.Forms.Button btnUnload;
        public System.Windows.Forms.Button btnLoad;
        public System.Windows.Forms.Button btnCompile;
        public System.Windows.Forms.Button btnCreate;
        public System.Windows.Forms.Label label33;
        public System.Windows.Forms.TextBox CustCmdtxtBox;
        private System.Windows.Forms.GroupBox grpSQL;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox txtSQLHost;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox txtSQLDatabase;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox txtSQLPassword;
        private System.Windows.Forms.TextBox txtSQLUsername;
        private System.Windows.Forms.CheckBox chkUseSQL;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.NumericUpDown numSpamMute;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.NumericUpDown numSpamMessages;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.CheckBox chkSpamControl;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ListBox listPasswords;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.CheckBox chkEnableVerification;
        private System.Windows.Forms.ComboBox cmbVerificationRank;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.CheckBox chkNoPillaringDuringZombie;
        private System.Windows.Forms.TextBox ZombieName;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.CheckBox chkNoLevelSavingDuringZombie;
        private System.Windows.Forms.CheckBox chkNoRespawnDuringZombie;
        private System.Windows.Forms.CheckBox chkZombieOnServerStart;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox txtIrcId;
        private System.Windows.Forms.CheckBox chkIrcId;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox txtIRCPort;
        private System.Windows.Forms.CheckBox chkShowEmptyRanks;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox txtGlobalChatNick;
        private System.Windows.Forms.CheckBox chkGlobalChat;
        private System.Windows.Forms.ComboBox cmbGlobalChatColor;
        private System.Windows.Forms.Label lblGlobalChatColor;
        private System.Windows.Forms.TextBox txtMaxUndo;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.TextBox txtGrieferStone;
        private System.Windows.Forms.CheckBox chkGrieferStone;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Button lsAddMap;
        private System.Windows.Forms.Button lsRemoveMap;
        private System.Windows.Forms.ListBox lsMapNoUse;
        private System.Windows.Forms.ListBox lsMapUse;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.CheckBox lsChkSendAFKMain;
        private System.Windows.Forms.CheckBox lsChkStartOnStartup;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.NumericUpDown lsNudVoteTime;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.NumericUpDown lsNudVoteCount;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.ComboBox lsCmbSetupRank;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.NumericUpDown lsNudKiller;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.NumericUpDown lsNudFastLava;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.NumericUpDown lsNudLayer;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.NumericUpDown lsNudWater;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.NumericUpDown lsNudDestroy;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.NumericUpDown lsNudFloodTime;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.NumericUpDown lsNudRoundTime;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.NumericUpDown lsNudLayerTime;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.NumericUpDown lsNudLayerCount;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.NumericUpDown lsNudLayerHeight;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Button lsBtnSaveSettings;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.Button lsBtnEndRound;
        private System.Windows.Forms.Button lsBtnStopGame;
        private System.Windows.Forms.Button lsBtnStartGame;
        private System.Windows.Forms.ComboBox lsCmbControlRank;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Button lsBtnEndVote;
        private System.Windows.Forms.ComboBox cmbGrieferStoneType;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.CheckBox chkGrieferStoneBan;
        private System.Windows.Forms.ComboBox cmbGrieferStoneRank;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.NumericUpDown numCountReset;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.CheckBox chkWomDirect;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown lsNudLives;
        private System.Windows.Forms.TextBox txtSQLPort;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.CheckBox autoUpdate;
        private System.Windows.Forms.NumericUpDown updateTimeNumeric;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.CheckBox notifyInGameUpdate;
        private System.Windows.Forms.Button forceUpdateBtn;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.ListBox listCommandsExtraCmdPerms;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Label label72;
        public System.Windows.Forms.NumericUpDown extracmdpermnumber;
        public System.Windows.Forms.TextBox extracmdpermdesc;
        public System.Windows.Forms.TextBox extracmdpermperm;
        private System.Windows.Forms.TextBox txtcmdranks2;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.ComboBox cmbAFKKickPerm;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox chkGuestLimitNotify;
        private System.Windows.Forms.TextBox levelList;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.CheckBox chkEnableChangingLevels;
        private System.Windows.Forms.CheckBox chkZombieOnlyServer;
        private System.Windows.Forms.CheckBox chkUseLevelList;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.CheckBox chkPrmOnly;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.CheckBox sndcmddataChk;
        private System.Windows.Forms.TabPage tabPage13;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label84;
    }
}