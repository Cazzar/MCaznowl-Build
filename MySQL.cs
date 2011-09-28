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
    public static class MySQL
    {
		
        public static string connString = "Data Source=" + Server.MySQLHost + ";Port=" + Server.MySQLPort + ";User ID=" + Server.MySQLUsername + ";Password=" + Server.MySQLPassword + ";Pooling=" + Server.MySQLPooling;
        public static void executeQuery(string queryString, bool createDB = false)
        {
			int totalCount = 0;
            if (!Server.useMySQL)
                return;
    retry:  try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    if (!createDB)
                    {
                        conn.ChangeDatabase(Server.MySQLDatabaseName);
                    }
					using (MySqlCommand cmd = new MySqlCommand(queryString, conn))
					{
						cmd.ExecuteNonQuery();
						conn.Close();
					}
                }
            }
            catch (Exception e)
            {
                if (!createDB)
                {
                    totalCount++;
                    if (totalCount > 10)
                    {
                        File.AppendAllText("MySQL_error.log", DateTime.Now + " " + queryString + "\r\n");
                        Server.ErrorLog(e);
                    }
                    else
                    {
                        goto retry;
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        public static DataTable fillData(string queryString, bool skipError = false)
        {
			int totalCount = 0;
			using (DataTable toReturn = new DataTable("toReturn"))
			{
				if (!Server.useMySQL)
					return toReturn;
			retry: try
				{
					using (var conn = new MySqlConnection(connString))
					{
						conn.Open();
						conn.ChangeDatabase(Server.MySQLDatabaseName);
						using (MySqlDataAdapter da = new MySqlDataAdapter(queryString, conn))
						{
							da.Fill(toReturn);
						}
						conn.Close();
					}
				}
				catch (Exception e)
				{
					totalCount++;
					if (totalCount > 10)
					{
						if (!skipError)
						{
							File.AppendAllText("MySQL_error.log", DateTime.Now + " " + queryString + "\r\n");
							Server.ErrorLog(e);
						}
					}
					else
						goto retry;
				}

				return toReturn;
			}
        }

        public static void CopyDatabase(StreamWriter sql) {
            //We technically know all tables in the DB...  But since this is MySQL, we can also get them all with a MySQL command
            //So we show the tables, and store the result.
            //Also output information data (Same format as phpMyAdmin's dump)

            sql.WriteLine("-- MCForge SQL Database Dump");
            sql.WriteLine("-- version 0.0.0");
            sql.WriteLine("-- http://www.mcforge.net");
            sql.WriteLine("--");
            sql.WriteLine("-- Host: {0}", Server.MySQLHost);
            sql.WriteLine("-- Generation Time: {0} at {1}", DateTime.Now.Date, DateTime.Now.TimeOfDay);
            sql.WriteLine("-- MCForge Version: {0}",  Server.Version);
            sql.WriteLine();
            //Extra stuff goes here
            sql.WriteLine();
            //database here
            using (DataTable sqlTables = fillData("SHOW TABLES")) {

                foreach (DataRow sqlTablesRow in sqlTables.Rows) {
                    string tableName = sqlTablesRow.Field<string>(0);
                    //For each table, we iterate through all rows, (and save them)
                    sql.WriteLine("-- --------------------------------------------------------");
                    sql.WriteLine();
                    sql.WriteLine("--");
                    sql.WriteLine("-- Table structure for table");
                    sql.WriteLine("--");
                    sql.WriteLine();
                    sql.WriteLine("CREATE TABLE if not exists `{0}` (",tableName);
                    List<string[]> tableSchema = new List<string[]>();
                    string[] rowParams;
                    using (DataTable tableRowSchema = fillData("DESCRIBE " + tableName)) {
                        rowParams = new string[tableRowSchema.Columns.Count];
                        string pri = "";
                        foreach (DataRow row in tableRowSchema.Rows) {
                            //Save the info contained to file
                            List<string> tmp = new List<string>(); 
                            for (int col = 0; col < tableRowSchema.Columns.Count; col++) {
                                tmp.Add(row.Field<string>(col));
                            }// end:for(col)
                            rowParams = tmp.ToArray<string>();
                            rowParams[2] = (rowParams[2].ToLower().Equals("no") ? "NOT " : "" ) + "NULL";
                            sql.WriteLine("`{0}` {1} {2}" + (rowParams[5].Equals("") ? "" : " {5}") + ",", rowParams);
                            pri += (rowParams[3].ToLower().Equals("pri") ? rowParams[0] + ";" : "");
                            tableSchema.Add(rowParams);
                        }// end:foreach(DataRow row)
                        if (!pri.Equals("")) {
                            string[] tmp = pri.Substring(0, pri.Length-1).Split(';');
                            sql.Write("PRIMARY KEY (`");
                            foreach (string prim in tmp) {
                                sql.Write(prim);
                                sql.Write("`" + (tmp.ElementAt<string>(tmp.Count() - 1).Equals(prim) ? ")" : ", `"));
                            }
                        }
                    }
                    sql.WriteLine();
                    sql.WriteLine(");");
                    sql.WriteLine();
                    sql.WriteLine("--");
                    sql.WriteLine("-- Dumping data for table `{0}`", tableName);
                    sql.WriteLine("--");
                    sql.WriteLine();
                    using (DataTable tableRowData = fillData("SELECT * FROM  " + tableName)) {
                        if (tableRowData.Rows.Count > 0) {
                            sql.Write("INSERT INTO `{0}` (`", tableName);
                            foreach (string[] rParams in tableSchema) {
                                sql.Write(rParams[0]);
                                sql.Write((tableSchema.ElementAt<string[]>(tableSchema.Count() - 1).Equals(rParams) ? "`) VALUES" : "`, `"));
                            }
                            List<DataColumn> allCols = new List<DataColumn>();
                            foreach (DataColumn col in tableRowData.Columns) {
                                allCols.Add(col);
                            }
                            foreach (DataRow row in tableRowData.Rows) {
                                //Save the info contained to file
                                sql.WriteLine();
                                sql.Write("(");
                                for (int col = 0; col < row.ItemArray.Length; col++) {
                                    //The values themselves can be integers or strings, or null
                                    Type eleType = allCols[col].DataType;
                                    if (row.IsNull(col)) {
                                        sql.Write("NULL");

                                    } else if (eleType.Name.Equals("DateTime")) {
                                        sql.Write(row.Field<DateTime>(col).ToShortDateString());

                                        ////} else if (eleType.Name.Equals("UInt16")) {
                                        ////    sql.Write(row.Field<UInt16>(col).ToString());

                                        ////} else if (eleType.Name.Equals("Int16")) {
                                        ////    sql.Write(row.Field<Int16>(col).ToString());

                                        ////} else if (eleType.Name.Equals("UInt32")) {
                                        ////    sql.Write(row.Field<UInt32>(col).ToString());

                                        ////} else if (eleType.Name.Equals("Int32")) {
                                        ////    sql.Write(row.Field<Int32>(col).ToString());

                                        ////} else if (eleType.Name.Equals("UInt64")) {
                                        ////    sql.Write(row.Field<UInt64>(col).ToString());

                                        ////} else if (eleType.Name.Equals("Int64")) {
                                        ////    sql.Write(row.Field<Int64>(col).ToString());

                                        ////} else if (eleType.Name.Equals("Byte")) {
                                        ////    sql.Write(row.Field<Byte>(col).ToString());

                                        //} else if (eleType.Name.Equals("SByte")) {
                                        //    sql.Write(row.Field<SByte>(col).ToString());

                                        //} else if (eleType.Name.Equals("Boolean")) {
                                        //    sql.Write(row.Field<Boolean>(col).ToString());

                                    } else if (eleType.Name.Equals("String")) {
                                        sql.Write("'{0}'", row.Field<string>(col));

                                    } else {
                                        sql.Write(row.Field<Object>(col));

                                    }
                                    sql.Write((col < row.ItemArray.Length - 1 ? ", " : "),"));
                                }// end:for(col)

                            }// end:foreach(DataRow row)
                            sql.Flush();

                            sql.BaseStream.Seek(-1, SeekOrigin.Current);
                            sql.WriteLine(";");
                            sql.WriteLine();
                        }
                    }
                }// end:foreach(DataRow sqlTablesRow)
            }
        }// end:CopyDatabase()
    }
}

