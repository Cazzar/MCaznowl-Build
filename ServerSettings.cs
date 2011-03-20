using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    public static class ServerSettings
    {
        // The Update URL must be hardcoded in Program.cs!!
        // Don't even try referencing this from Program.cs it will fail miserably.
        public static string CurrentVersionFile = "http://www.mcforge.net/curversion.txt";
        public static string DLLLocation = "http://www.mcforge.net/MCForge_.dll";
        public static string ChangelogLocation = "http://www.mcforge.net/changelog.txt";
        public static string RevisionList = "http://www.mcforge.net/revs.txt";
        public static string HeartbeatAnnounce = "http://www.mcforge.net/hbannounce.php";
        public static string ArchivePath = "http://www.mcforge.net/archives/exe/";
    }
}
