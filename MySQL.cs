/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
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
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
//using System.Data.SQLite;

namespace MCForge
{
    namespace SQL
    {
        public static class MySQL //: Database //Extending for future improvement (Making it object oriented later)
        {
            private static string connStringFormat = "Data Source={0};Port={1};User ID={2};Password={3};Pooling={4}";

            public static string connString { get { return String.Format(connStringFormat, Server.MySQLHost, Server.MySQLPort, Server.MySQLUsername, Server.MySQLPassword, Server.DatabasePooling); } }
            public static void executeQuery(string queryString, bool createDB = false)
            {
                Database.executeQuery(queryString, createDB);
            }

            public static DataTable fillData(string queryString, bool skipError = false)
            {
                return Database.fillData(queryString, skipError);
            }

            internal static void execute(string queryString, bool createDB = false) {
                using (var conn = new MySqlConnection(connString)) {
                    conn.Open();
                    if (!createDB) {
                        conn.ChangeDatabase(Server.MySQLDatabaseName);
                    }
                    using (MySqlCommand cmd = new MySqlCommand(queryString, conn)) {
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

            internal static void fill(string queryString, DataTable toReturn) {
                using (var conn = new MySqlConnection(connString)) {
                    conn.Open();
                    conn.ChangeDatabase(Server.MySQLDatabaseName);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(queryString, conn)) {
                        da.Fill(toReturn);
                    }
                    conn.Close();
                }
            }
        }
    }
}

