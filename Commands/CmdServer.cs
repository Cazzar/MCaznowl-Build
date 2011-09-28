using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;

namespace MCForge {
    class CmdServer : Command {
        public override string name { get { return "server"; } }
        public override string shortcut { get { return "serv"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdServer() { }

        public override void Use(Player p, string message) {
            switch (message) {
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
                case "reset":  //made so only the owner or console can use this command.
                    if (p != null && !Server.server_owner.ToLower().Equals(p.name.ToLower())) {
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
                        File.Copy(name, name + ".bkp");
                        File.Delete(name);
                    }
                    Player.SendMessage(p, "Done!  Restoring defaults...");
                    //We set he defaults here, then go to reload the settings.
                    setToDefault();
                    goto case "reload";
                case "reload":
                    Player.SendMessage(p, "Reloading settings...");
                    Server.LoadAllSettings();
                    //Plugin.Unload();
                    //Plugin.Load();
                    Player.SendMessage(p, "Settings reloaded!");
                    break;
                case "backup":
                    goto case "backup all";
                case "backup all":
                    // Important to backup database as well, if MySQL is enabled.
                    //   Create SQL statements for this.  The SQL will assume the settings for the current configuration are correct.
                    //   This means we use the currently defined port, database, user, password, and pooling.
                    // Also important to save everything to a .zip file (Though we can rename the extention.)
                    // When backing up, one option is to save all non-main program files.
                    //    This means all folders, and files in these folders.
                    Save(Server.useMySQL);
                    break;
                case "backup db":
                    // Important to backup database as well, if MySQL is enabled.
                    //   Create SQL statements for this.  The SQL will assume the settings for the current configuration are correct.
                    //   This means we use the currently defined port, database, user, password, and pooling.
                    // Also important to save everything to a .zip file (Though we can rename the extention.)
                    // When backing up, one option is to save all non-main program files.
                    //    This means all folders, and files in these folders.
                    Save(false, true);
                    break;
                case "backup allbutdb":
                    // Important to backup database as well, if MySQL is enabled.
                    //   Create SQL statements for this.  The SQL will assume the settings for the current configuration are correct.
                    //   This means we use the currently defined port, database, user, password, and pooling.
                    // Also important to save everything to a .zip file (Though we can rename the extention.)
                    // When backing up, one option is to save all non-main program files.
                    //    This means all folders, and files in these folders.
                    Save(false);
                    break;
                case "restore":
                    ExtractPackage();
                    break;
                default:
                    Help(p);
                    break;
            }
        }

        private void Save(bool withDB) {
            Save(true, withDB);
        }

        private void Save(bool withFiles, bool withDB) {
            CreatePackage("MCForge.zip", withFiles, withDB);
        }

        //private void saveAllFolders(bool withDB) {
        //    //All folders and files will be saved.  So, first, get their names in a linked list.
        //    //DeflateStream zipFile = new DeflateStream(File.Create("serverBackup.zip"), CompressionMode.Compress);
        //    //byte[] bytes = BitConverter.GetBytes('h');
        //    //zipFile.Write(bytes, 0, bytes.Length);
        //    //zipFile.Flush();
        //    //zipFile.Close();
        //    CreatePackage("MCForge.zip", withDB);

        //}

        //private void saveDatabase() {
        //    throw new NotImplementedException();
        //}

        private void setToDefault() {
            //Other
            Server.higherranktp = true;
            Server.agreetorulesonentry = false;

            Server.devs = new List<string>(new string[] { "dmitchell94", "jordanneil23", "501st_commander", "fenderrock87", "edh649", "philipdenseje", "hypereddie10", "erickilla", "the_legacy", "fredlllll", "soccer101nic", "headdetect", "merlin33069", "bizarrecake", "jasonbay13", "cazzar", "snowl", "techjar", "herocane", "copyboy", "nerketur"});

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
            Server.guestGoto = false;

            //Spam Prevention
            Server.checkspam = false;
            Server.spamcounter = 8;
            Server.mutespamtime = 60;
            Server.spamcountreset = 5;

            Server.ZallState = "Alive";

            Server.level = "main";
            Server.errlog = "error.log";

            Server.console = false;
            Server.reportBack = true;

            Server.irc = false;
            Server.safemode = false;
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

            Server.useMySQL = true;
            Server.MySQLHost = "127.0.0.1";
            Server.MySQLPort = "3306";
            Server.MySQLUsername = "root";
            Server.MySQLPassword = "password";
            Server.MySQLDatabaseName = "MCZallDB";
            Server.MySQLPooling = true;

            Server.DefaultColor = "&e";
            Server.IRCColour = "&5";

            Server.UseGlobalChat = true;
            Server.GlobalChatNick = "MCF" + new Random().Next(Int32.MaxValue);
            Server.GlobalChatColor = "&6";


            Server.afkminutes = 10;
            Server.afkkick = 45;
            Server.RemotePort = 1337;

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

            #endregion
        }

        public override void Help(Player p) {
            Player.SendMessage(p, "/server <reset|restart|reload|update|shutdown|public|private|backup|restore> - All server commands.");
            Player.SendMessage(p, "/server <reset>   - Reset everything to defaults.  WARNING: This will erase ALL properties.  Use with caution.");
            Player.SendMessage(p, "/server <restart> - Restart the sserver.");
            Player.SendMessage(p, "/server <reload>  - Reload the server files.");
            Player.SendMessage(p, "/server <update>  - Update the server");
            Player.SendMessage(p, "/server <shutdown>  - Shutdown the server");
            Player.SendMessage(p, "/server <public>  - Make the server public. (Start listening for new connections.)");
            Player.SendMessage(p, "/server <private> - Make the server private. (Stop listening for new connections.)");
            Player.SendMessage(p, "Not yet implmented:");
            Player.SendMessage(p, "/server <backup>  - Make a complete backup of the server and all SQL data.");
            Player.SendMessage(p, "/server <restore> - Restore the server from a backup.");
        }

        //  -------------------------- CreatePackage --------------------------
        /// <summary>
        ///   Creates a package zip file containing specified
        ///   content and resource files.</summary>
        private static void CreatePackage(string packagePath, bool withFiles, bool withDB) {

            // Create the Package
            if (withDB) {
                SaveDataBase("SQL.sql");
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
                }// end:if(withFiles)
            }// end:using (Package package) - Close and dispose package.
            Server.s.Log("Server backed up!");
        }// end:CreatePackage()

        private static void SaveDataBase(string filename) {
            using (StreamWriter sql = new StreamWriter(File.Create(filename))) {
                MySQL.CopyDatabase(sql);
            }
        }

        //private static void CopyDatabase(StreamWriter sql) {
        //    string database = Server.MySQLDatabaseName;
        //    MySQL.executeQuery("SELECT * FROM " + database + ";");
        //}


        //private static void SaveDataBase() {
        //    throw new NotImplementedException();
        //}

        private static List<Uri> GetAllFiles(DirectoryInfo currDir, Uri baseUri) {
            List<Uri> uriList = new List<Uri>();
            foreach (FileSystemInfo entry in currDir.GetFileSystemInfos()) {
                if (entry is FileInfo) {
                    // Make a relative URI
                    Uri fullURI = new Uri(((FileInfo)entry).FullName);
                    Uri relURI = baseUri.MakeRelativeUri(fullURI);
                    uriList.Add(PackUriHelper.CreatePartUri(relURI));
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
            using (ZipPackage zip = (ZipPackage)ZipPackage.Open(File.OpenRead("MCForge.zip"))) {
                PackagePartCollection pc = zip.GetParts();
                foreach (ZipPackagePart item in pc) {
                    CopyStream(item.GetStream(), File.Create("./" + Uri.UnescapeDataString(item.Uri.ToString())));
                }
            }
        }
    }
}
