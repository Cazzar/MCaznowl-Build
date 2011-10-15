/*
	Copyright 2011 MCForge
		
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
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace MCForge
{
    class SQLite
    {
        public static string connString = "Data Source =" + Server.apppath + "/MCForge.db; Version =3; Pooling =" + Server.DatabasePooling +"; Max Pool Size =1000;";


        public static void executeQuery(string queryString)
        {
            Database.executeQuery(queryString);
        }

        public static DataTable fillData(string queryString, bool skipError = false)
        {
            return Database.fillData(queryString, skipError);
        }
    }
}
