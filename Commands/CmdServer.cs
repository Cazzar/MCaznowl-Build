﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using MCForge.SQL;

namespace MCForge {
    class CmdServer : Command {
        public override string name { get { return "server"; } }
        public override string shortcut { get { return "serv"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public CmdServer() { }

        public override void Use(Player p, string message) {
            switch (message) {
                case "": // To prevent '/server' from causing an error message
                    Help(p);
                    break;
                case "restart":
                case "update":
                case "shutdown":
                    Command.all.Find(message).Use(p, ""); //Will use other options later.
                    break;
                case "public":
                    Server.pub = true;
                    Player.SendMessage(p, "Server is now public!");
                    Server.s.Log("Server is now public!");
                    break;
                case "private":
                    Server.pub = false;
                    Player.SendMessage(p, "Server is now private!");
                    Server.s.Log("Server is now private!");
                    break;
                case "reset":  //made so ONLY the owner or console can use this command.
                    if (p != null && !Server.server_owner.ToLower().Equals(p.name.ToLower()) || Server.server_owner.Equals("Notch")) {
                        p.SendMessage("Sorry.  You must be the Server Owner or Console to reset the server.");
                        return;
                    }
                    //restting to default properties is dangerous... but recoverable.
                    //We save the old files to <name>.bkp, then delete them.
                    //Files needed:
                    //  Property files
                    //    Group
                    //    Server
                    //    Rank
                    //    Command
                    Player.SendMessage(p, "Backing up and deleting current property files.");
                    foreach (string name in Directory.GetFiles("properties")) {
                        File.Copy(name, name + ".bkp"); // create backup first.
                        File.Delete(name);
                    }
                    Player.SendMessage(p, "Done!  Restoring defaults...");
                    //We set he defaults here, then go to reload the settings.
                    setToDefault();
                    goto case "reload";
                case "reload":  // For security, only the owner and Console can use this.
                    if (p != null && !Server.server_owner.ToLower().Equals(p.name.ToLower()) || Server.server_owner.Equals("Notch")) {
                        p.SendMessage("Sorry.  You must be the Server Owner or Console to reload the server settings.");
                        return;
                    }
                    Player.SendMessage(p, "Reloading settings...");
                    Server.LoadAllSettings();
                    Player.SendMessage(p, "Settings reloaded!  You may need to restart the server, however.");
                    break;
                case "backup":
                    goto case "backup all";
                case "backup all":
                    // Backup Everything.
                    //   Create SQL statements for this.  The SQL will assume the settings for the current configuration are correct.
                    //   This means we use the currently defined port, database, user, password, and pooling.
                    // Also important to save everything to a .zip file (Though we can rename the extention.)
                    // When backing up, one option is to save all non-main program files.
                    //    This means all folders, and files in these folders.
                    Save(true);
                    break;
                case "backup db":
                    // Backup database only.
                    //   Create SQL statements for this.  The SQL will assume the settings for the current configuration are correct.
                    //   This means we use the currently defined port, database, user, password, and pooling.
                    // Also important to save everything to a .zip file (Though we can rename the extention.)
                    // When backing up, one option is to save all non-main program files.
                    //    This means all folders, and files in these folders.
                    Save(false, true);
                    break;
                case "backup allbutdb":
                    // Important to save everything to a .zip file (Though we can rename the extention.)
                    // When backing up, one option is to save all non-main program files.
                    //    This means all folders, and files in these folders.
                    Save(false);
                    break;
                case "restore":
                    ExtractPackage();
                    Player.SendMessage(p, "Server restored.  Restart is required to see all changes.");
                    break;
                default:
                    Player.SendMessage(p, "/server " + message + " is not currently implemented.");
                    goto case "";
                //case "help":
                //    Help(p);
                //    break;
            }
        }

        private void Save(bool withDB) {
            Save(true, withDB);
        }

        private void Save(bool withFiles, bool withDB) {
            CreatePackage("MCForge.zip", withFiles, withDB);
        }

        private void setToDefault() { // could do this elsewhere, but will worry about that later.
            //Other
            Server.higherranktp = true;
            Server.agreetorulesonentry = false;

            Server.tempBans = new List<Server.TempBan>();

            Server.afkset = new List<string>();
            Server.ircafkset = new List<string>();
            Server.afkmessages = new List<string>();
            Server.messages = new List<string>();

            Server.restartcountdown = "";
            Server.selectedrevision = "";

            Server.chatmod = false;

            //Global VoteKick In Progress Flag
            Server.voteKickInProgress = false;
            Server.voteKickVotesNeeded = 0;

            //Zombie
            Server.queLevel = false;
            Server.queZombie = false;
            Server.PlayedRounds = 0;
            Server.infection = false;
            Server.nextZombie = "";
            Server.nextLevel = "";
            Server.YesVotes = 0;
            Server.NoVotes = 0;
            Server.voting = false;
            Server.votingforlevel = false;
            Server.YesLevelVotes = 0;
            Server.NoLevelVotes = 0;
            Server.lastPlayerToInfect = "";
            Server.lastLevelVote1 = "";
            Server.lastLevelVote2 = "";
            Server.infectCombo = 0;
            Server.count = 0;
            Server.ZombieModeOn = false;
            Server.startZombieModeOnStartup = false;
            Server.noRespawn = true;
            Server.noLevelSaving = true;
            Server.noPillaring = true;
            Server.ZombieName = "";

            //Settings
            #region Server Settings
            Server.salt = "";

            Server.name = "[MCForge] Default";
            Server.motd = "Welcome!";
            Server.players = 12;
            //for the limiting no. of guests:
            Server.maxGuests = 10;

            Server.maps = 5;
            Server.port = 25565;
            Server.pub = true;
            Server.verify = true;
            Server.worldChat = true;
//            Server.guestGoto = false; //Never used

            Server.autorestart = false;
            Server.restarttime = DateTime.Now;

            //Spam Prevention
            Server.checkspam = false;
            Server.spamcounter = 8;
            Server.mutespamtime = 60;
            Server.spamcountreset = 5;

            Server.ZallState = "Alive";

            Server.level = "main";
            Server.errlog = "error.log";

            //Server.console = false;
            Server.reportBack = true;

            Server.irc = false;
//            Server.safemode = false;
            Server.ircPort = 6667;
            Server.ircNick = "ForgeBot";
            Server.ircServer = "irc.esper.net";
            Server.ircChannel = "#changethis";
            Server.ircOpChannel = "#changethistoo";
            Server.ircIdentify = false;
            Server.ircPassword = "";
            Server.verifyadmins = true;
            Server.verifyadminsrank = LevelPermission.Operator;

            Server.restartOnError = true;

            Server.antiTunnel = true;
            Server.maxDepth = 4;
            Server.Overload = 1500;
            Server.rpLimit = 500;
            Server.rpNormLimit = 10000;

            Server.backupLocation = System.Windows.Forms.Application.StartupPath + "/levels/backups";
            Server.backupInterval = 300;
            Server.blockInterval = 60;

            Server.physicsRestart = true;
            Server.deathcount = true;
            Server.AutoLoad = false;
            Server.physUndo = 20000;
            Server.totalUndo = 200;
            Server.rankSuper = true;
            Server.oldHelp = false;
            Server.parseSmiley = true;
            Server.useWhitelist = false;
            Server.forceCuboid = false;
            Server.profanityFilter = false;
            Server.notifyOnJoinLeave = false;
            Server.repeatMessage = false;
            Server.globalignoreops = false;

            Server.checkUpdates = true;

            Server.useMySQL = false;
            Server.MySQLHost = "127.0.0.1";
            Server.MySQLPort = "3306";
            Server.MySQLUsername = "root";
            Server.MySQLPassword = "password";
            Server.MySQLDatabaseName = "MCZallDB";
            Server.DatabasePooling = true;

            Server.DefaultColor = "&e";
            Server.IRCColour = "&5";

            Server.UseGlobalChat = true;
            Server.GlobalChatNick = "MCF" + new Random().Next();
            Server.GlobalChatColor = "&6";


            Server.afkminutes = 10;
            Server.afkkick = 45;
            //Server.RemotePort = 1337;

            Server.defaultRank = "guest";

            Server.dollardollardollar = true;
            Server.unsafe_plugin = true;
            Server.cheapMessage = true;
            Server.cheapMessageGiven = " is now being cheap and being immortal";
            Server.customBan = false;
            Server.customBanMessage = "You're banned!";
            Server.customShutdown = false;
            Server.customShutdownMessage = "Server shutdown. Rejoin in 10 seconds.";
            Server.customGrieferStone = false;
            Server.customGrieferStoneMessage = "Oh noes! You were caught griefing!";
            Server.moneys = "moneys";
            Server.opchatperm = LevelPermission.Operator;
            Server.adminchatperm = LevelPermission.Admin;
            Server.logbeat = false;
            Server.adminsjoinsilent = false;
            Server.mono = false;
            Server.server_owner = "Notch";
            Server.WomDirect = false;

            Server.flipHead = false;

            Server.shuttingDown = false;
            Server.restarting = false;

            //hackrank stuff
            Server.hackrank_kick = true;
            Server.hackrank_kick_time = 5; //seconds, it converts it to milliseconds in the command.

            // lol useless junk here lolololasdf poop
            Server.showEmptyRanks = false;
            Server.grieferStoneType = 1;
            Server.grieferStoneBan = true;
            Server.grieferStoneRank = LevelPermission.Guest;

            //WOM Direct
            Server.WomDirect = false;

            #endregion
        }

        public override void Help(Player p) {
            Player.SendMessage(p, "/server <reset|restart|reload|update|shutdown|public|private|backup|restore> - All server commands.");
            Player.SendMessage(p, "/server <reset>    - Reset everything to defaults. (Owner only)  WARNING: This will erase ALL properties.  Use with caution. (Likely requires restart)");
            Player.SendMessage(p, "/server <restart>  - Restart the server.");
            Player.SendMessage(p, "/server <reload>   - Reload the server files. (May require restart) (Owner only)");
            Player.SendMessage(p, "/server <update>   - Update the server");
            Player.SendMessage(p, "/server <shutdown> - Shutdown the server");
            Player.SendMessage(p, "/server <public>   - Make the server public. (Start listening for new connections.)");
            Player.SendMessage(p, "/server <private>  - Make the server private. (Stop listening for new connections.)");
            Player.SendMessage(p, "/server <restore>  - Restore the server from a backup.");
            Player.SendMessage(p, "/server <backup> [all/db/allbutdb] - Make a backup. (Default is all)");
            Player.SendMessage(p, "Backup options:");
            Player.SendMessage(p, "all      - Make a backup of the server and all SQL data. (Default)");
            Player.SendMessage(p, "db       - Just backup the database.");
            Player.SendMessage(p, "allbutdb - Backup everything BUT the database.");
        }

        //  -------------------------- CreatePackage --------------------------
        /// <summary>
        ///   Creates a package zip file containing specified
        ///   content and resource files.</summary>
        private static void CreatePackage(string packagePath, bool withFiles, bool withDB) {

            // Create the Package
            if (withDB) {
                SaveDatabase("SQL.sql");
            }

            Server.s.Log("Creating package...");
            using (ZipPackage package = (ZipPackage)ZipPackage.Open(packagePath, FileMode.Create)) {

                if (withFiles) {
                    Server.s.Log("Collecting Directory structure...");
                    string currDir = Directory.GetCurrentDirectory() + "\\";
                    List<Uri> partURIs = GetAllFiles(new DirectoryInfo("./"), new Uri(currDir));
                    Server.s.Log("Done!");

                    foreach (Uri loc in partURIs) {
                        if (!Uri.UnescapeDataString(loc.ToString()).Contains(packagePath)) {

                            // Add the part to the Package

                            ZipPackagePart packagePart =
                                (ZipPackagePart)package.CreatePart(loc, "");

                            // Copy the data to the Document Part
                            using (FileStream fileStream = new FileStream(
                                    "./" + Uri.UnescapeDataString(loc.ToString()), FileMode.Open, FileAccess.Read)) {
                                CopyStream(fileStream, packagePart.GetStream());
                            }// end:using(fileStream) - Close and dispose fileStream.
                        }
                    }// end:foreach(Uri loc)
                } else if (withDB) { // If we don't want to back up database, we don't do this part.
                    ZipPackagePart packagePart =
                                (ZipPackagePart)package.CreatePart(new Uri("/SQL.sql", UriKind.Relative), "", CompressionOption.Normal);
                    CopyStream(File.OpenRead("SQL.sql"), packagePart.GetStream());
                }// end:if(withFiles)
            }// end:using (Package package) - Close and dispose package.
            Server.s.Log("Server backed up!");
        }// end:CreatePackage()

        private static void SaveDatabase(string filename) {
            using (StreamWriter sql = new StreamWriter(File.Create(filename))) {
                Database.CopyDatabase(sql);
            }
        }

        private static List<Uri> GetAllFiles(DirectoryInfo currDir, Uri baseUri) {
            List<Uri> uriList = new List<Uri>();
            foreach (FileSystemInfo entry in currDir.GetFileSystemInfos()) {
                if (entry is FileInfo) {
                    // Make a relative URI
                    Uri fullURI = new Uri(((FileInfo)entry).FullName);
                    Uri relURI = baseUri.MakeRelativeUri(fullURI);
                    if (relURI.ToString().IndexOfAny("/\\".ToCharArray()) > 0) {
                        uriList.Add(PackUriHelper.CreatePartUri(relURI));
                    }
                } else {
                    uriList.AddRange(GetAllFiles((DirectoryInfo)entry, baseUri));
                }
            }
            return uriList;
        }


        //  --------------------------- CopyStream ---------------------------
        /// <summary>
        ///   Copies data from a source stream to a target stream.</summary>
        /// <param name="source">
        ///   The source stream to copy from.</param>
        /// <param name="target">
        ///   The destination stream to copy to.</param>
        private static void CopyStream(Stream source, Stream target) {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }// end:CopyStream()

        private static void ExtractPackage() {
            int errors = 0;
            using (ZipPackage zip = (ZipPackage)ZipPackage.Open(File.OpenRead("MCForge.zip"))) {
                PackagePartCollection pc = zip.GetParts();
                foreach (ZipPackagePart item in pc) {
                    try {
                        CopyStream(item.GetStream(), File.Create("./" + Uri.UnescapeDataString(item.Uri.ToString())));
                    } catch {
                        try {
                            Directory.CreateDirectory("./" + item.Uri.ToString().Substring(0, item.Uri.ToString().LastIndexOfAny("\\/".ToCharArray())));
                            CopyStream(item.GetStream(), File.Create("./" + Uri.UnescapeDataString(item.Uri.ToString())));
                        } catch (IOException e) {
                            Server.ErrorLog(e);
                            Server.s.Log("Caught Error.  See log for more details.  Will continue with rest of files.");
                            errors++;
                        }
                    }
                    if (item.Uri.ToString().ToLower().Contains("sql.sql")) { // If it's in there, they backed it up, meaning they want it restored
                        Database.fillDatabase(item.GetStream());
                    }
                }
            }
        }
    }
}
