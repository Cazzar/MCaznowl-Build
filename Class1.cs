﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SQLite;


namespace MCForge {
    class Database {
        public static void CopyDatabase(StreamWriter sql) {
            //We technically know all tables in the DB...  But since this is MySQL, we can also get them all with a MySQL command
            //So we show the tables, and store the result.
            //Also output information data (Same format as phpMyAdmin's dump)

            //Important note:  This does NOT account for foreign keys, BLOB's etc.  It only works for what we actually put in the db.

            sql.WriteLine("-- MCForge SQL Database Dump");
            sql.WriteLine("-- version 1.0");
            sql.WriteLine("-- http://www.mcforge.net");
            sql.WriteLine("--");
            sql.WriteLine("-- Host: {0}", Server.MySQLHost);
            sql.WriteLine("-- Generation Time: {0} at {1}", DateTime.Now.Date, DateTime.Now.TimeOfDay);
            sql.WriteLine("-- MCForge Version: {0}", Server.Version);
            sql.WriteLine();
            //Extra stuff goes here
            sql.WriteLine();
            //database here
            using (List<String> sqlTables = (Database.getTables())) {

                foreach (DataRow sqlTablesRow in sqlTables.Rows) {
                    string tableName = sqlTablesRow.Field<string>(0);
                    //For each table, we iterate through all rows, (and save them)
                    sql.WriteLine("-- --------------------------------------------------------");
                    sql.WriteLine();
                    sql.WriteLine("--");
                    sql.WriteLine("-- Table structure for table `{0}`", tableName);
                    sql.WriteLine("--");
                    sql.WriteLine();
                    sql.WriteLine("CREATE TABLE IF NOT EXISTS `{0}` (", tableName);
                    List<string[]> tableSchema = new List<string[]>();
                    string[] rowParams;
                    using (DataTable tableRowSchema = Database.fillData("DESCRIBE " + tableName)) {
                        rowParams = new string[tableRowSchema.Columns.Count];
                        string pri = "";
                        foreach (DataRow row in tableRowSchema.Rows) {
                            //Save the info contained to file
                            List<string> tmp = new List<string>();
                            for (int col = 0; col < tableRowSchema.Columns.Count; col++) {
                                tmp.Add(row.Field<string>(col));
                            }// end:for(col)
                            rowParams = tmp.ToArray<string>();
                            rowParams[2] = (rowParams[2].ToLower().Equals("no") ? "NOT " : "DEFAULT ") + "NULL";
                            sql.WriteLine("`{0}` {1} {2}" + (rowParams[5].Equals("") ? "" : " {5}") + ",", rowParams);
                            pri += (rowParams[3].ToLower().Equals("pri") ? rowParams[0] + ";" : "");
                            tableSchema.Add(rowParams);
                        }// end:foreach(DataRow row)
                        if (!pri.Equals("")) {
                            string[] tmp = pri.Substring(0, pri.Length - 1).Split(';');
                            sql.Write("PRIMARY KEY (`");
                            foreach (string prim in tmp) {
                                sql.Write(prim);
                                sql.Write("`" + (tmp.ElementAt<string>(tmp.Count() - 1).Equals(prim) ? ")" : ", `"));
                            }
                        }
                    }
                    sql.WriteLine(");");
                    sql.WriteLine();
                    using (DataTable tableRowData = Database.fillData("SELECT * FROM  " + tableName)) {
                        if (tableRowData.Rows.Count > 0) {
                            sql.WriteLine("--");
                            sql.WriteLine("-- Dumping data for table `{0}`", tableName);
                            sql.WriteLine("--");
                            sql.WriteLine();
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

                                    } else if (eleType.Name.Equals("DateTime")) { // special format
                                        DateTime dt = row.Field<DateTime>(col);
                                        sql.Write("'{0}-{1}-{2} {3}:{4}:{5}'", new object[] { dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second });

                                        //} else if (eleType.Name.Equals("Boolean")) {
                                        //    sql.Write(row.Field<Boolean>(col).ToString());

                                    } else if (eleType.Name.Equals("String")) { // Requires ''
                                        sql.Write("'{0}'", row.Field<string>(col));

                                    } else {
                                        sql.Write(row.Field<Object>(col)); // We assume all other data is left as-is
                                        //This includes numbers, and booleans.  (As well as objects, but we don't save them into the database)

                                    }
                                    sql.Write((col < row.ItemArray.Length - 1 ? ", " : "),"));
                                }// end:for(col)

                            }// end:foreach(DataRow row)
                            sql.Flush();

                            sql.BaseStream.Seek(-1, SeekOrigin.Current);
                            sql.WriteLine(";");
                        } else {
                            sql.WriteLine("-- No data in table `{0}`!", tableName);
                        }
                        sql.WriteLine();
                    }
                }// end:foreach(DataRow sqlTablesRow)
            }
        }

        private static List<string> getTables() {
            List<string> tableNames = new List<string>();
            using (DataTable tables = fillData((Server.useMySQL ? "SHOW TABLES" : "SELECT * FROM sqlite_master"))) {
                foreach (DataRow row in tables.Rows) {
                    string tableName = row.Field<string>((Server.useMySQL ? 0 : 1));
                    tableNames.Add(tableName);
                }
            }
            return tableNames;
        }// end:CopyDatabase()

        public static void executeQuery(string queryString, bool createDB = false) {
            int totalCount = 0;
            retry: try {
                if (Server.useMySQL) {
                    using (var conn = new MySqlConnection(MySQL.connString)) {
                        conn.Open();
                        if (!createDB) {
                            conn.ChangeDatabase(Server.MySQLDatabaseName);
                        }
                        using (MySqlCommand cmd = new MySqlCommand(queryString, conn)) {
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                } else {
                    using (var conn = new SQLiteConnection(SQLite.connString)) {
                        conn.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn)) {
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            } catch (Exception e) {
                if (!createDB || !Server.useMySQL) {
                    totalCount++;
                    if (totalCount > 10) {
                        File.AppendAllText("MySQL_error.log", DateTime.Now + " " + queryString + "\r\n");
                        Server.ErrorLog(e);
                    } else {
                        goto retry;
                    }
                } else {
                    throw e;
                }
            }
        }

        public static DataTable fillData(string queryString, bool skipError = false) {
            int totalCount = 0;
            using (DataTable toReturn = new DataTable("toReturn")) {
            retry: try {
                    if (Server.useMySQL) {
                        using (var conn = new MySqlConnection(MySQL.connString)) {
                            conn.Open();
                            conn.ChangeDatabase(Server.MySQLDatabaseName);
                            using (MySqlDataAdapter da = new MySqlDataAdapter(queryString, conn)) {
                                da.Fill(toReturn);
                            }
                            conn.Close();
                        }
                    } else {
                        using (var conn = new SQLiteConnection(SQLite.connString)) {
                            conn.Open();
                            using (SQLiteDataAdapter da = new SQLiteDataAdapter(queryString, conn)) {
                                da.Fill(toReturn);
                            }
                            conn.Close();
                        }
                    }
                } catch (Exception e) {
                    totalCount++;
                    if (totalCount > 10) {
                        if (!skipError) {
                            File.AppendAllText("MySQL_error.log", DateTime.Now + " " + queryString + "\r\n");
                            Server.ErrorLog(e);
                        }
                    } else
                        goto retry;
                }

                return toReturn;
            }
        }

    }
}
