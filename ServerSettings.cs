/*
	Copyright 2011 MCForge
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.osedu.org/licenses/ECL-2.0
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
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
