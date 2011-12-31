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
using System.Globalization;

namespace MCForge
{
    namespace SQL
    {
        public static class SQLite //: Database //Extending for future improvement (Making it object oriented later).
        {
            private static string connStringFormat = "Data Source =" + Server.apppath + "/MCForge.db; Version =3; Pooling ={0}; Max Pool Size =1000;";

            public static string connString { get { return String.Format(CultureInfo.CurrentCulture, connStringFormat, Server.DatabasePooling); } }

            public static void executeQuery(string query)
            {
                Database.executeQuery(query);
            }

            public static DataTable fillData(string query, bool skipError = false)
            {
                return Database.fillData(query, skipError);
            }

            internal static void execute(string query) {
                using (var conn = new SQLiteConnection(SQLite.connString)) {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn)) {
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

            internal static void fill(string query, DataTable toReturn) {
                using (var conn = new SQLiteConnection(SQLite.connString)) {
                    conn.Open();
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn)) {
                        da.Fill(toReturn);
                    }
                    conn.Close();
                }
            }
        }
    }
}
