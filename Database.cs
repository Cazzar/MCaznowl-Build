using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
//using MySql.Data.MySqlClient;
//using System.Data.SQLite;


namespace MCForge {
    namespace SQL {
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
                List<String> sqlTables = (Database.getTables());
                foreach (string tableName in sqlTables) {
                    //For each table, we iterate through all rows, (and save them)
                    sql.WriteLine("-- --------------------------------------------------------");
                    sql.WriteLine();
                    sql.WriteLine("--");
                    sql.WriteLine("-- Table structure for table `{0}`", tableName);
                    sql.WriteLine("--");
                    sql.WriteLine();
                    List<string[]> tableSchema = new List<string[]>();
                    if (Server.useMySQL) {
                        string[] rowParams;
                        string pri;
                        sql.WriteLine("CREATE TABLE IF NOT EXISTS `{0}` (", tableName);
                        using (DataTable tableRowSchema = Database.fillData("DESCRIBE " + tableName)) {
                            rowParams = new string[tableRowSchema.Columns.Count];
                            pri = "";
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
                        }
                        if (!pri.Equals("")) {
                            string[] tmp = pri.Substring(0, pri.Length - 1).Split(';');
                            sql.Write("PRIMARY KEY (`");
                            foreach (string prim in tmp) {
                                sql.Write(prim);
                                sql.Write("`" + (tmp.ElementAt<string>(tmp.Count() - 1).Equals(prim) ? ")" : ", `"));
                            }
                        }
                        sql.WriteLine(");");
                    } else {
                        using (DataTable tableSQL = Database.fillData("SELECT sql FROM" +
                                                                            "   (SELECT * FROM sqlite_master UNION ALL" +
                                                                            "    SELECT * FROM sqlite_temp_master)" +
                                                                            "WHERE tbl_name LIKE '" + tableName + "'" +
                                                                            "  AND type!='meta' AND sql NOT NULL AND name NOT LIKE 'sqlite_%'" +
                                                                            "ORDER BY substr(type,2,1), name")) {
                            //just print out the data in the table.
                            foreach (DataRow row in tableSQL.Rows) {
                                string tableSQLString = row.Field<string>(0);
                                sql.WriteLine(tableSQLString.Replace(" " + tableName, " `" + tableName + "`").Replace("CREATE TABLE `" + tableName + "`", "CREATE TABLE IF NOT EXISTS `" + tableName + "`"));
                                //We parse this ourselves to find the actual types.
                                tableSchema = getSchema(tableSQLString);

                            }
                        }
                        using (DataTable tableRowSchema = Database.fillData("")) {

                        }
                    }
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
                            foreach (DataRow row in tableRowData.Rows) { //We rely on the correct datatype being given here.
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

            private static List<string[]> getSchema(string tableSQLString) {
                // All SQL for creating tables looks like "CREATE TABLE [IF NOT EXISTS] <TableName> (<ColumnDef>[, ... [, PRIMARY KEY (<ColumnName>[, ...])]])
                // <ColumnDef> = <name> <type> [[NOT|DEFAULT] NULL] [PRIMARY KEY] [AUTO_INCREMENT]
                List<string[]> schema = new List<string[]>();
                int foundStart = tableSQLString.IndexOf("(") + 1;
                int foundLength = tableSQLString.LastIndexOf(")") - foundStart;
                tableSQLString = tableSQLString.Substring(foundStart, foundLength);
                // Now we have everything inside the parenthisies.
                string[] column = tableSQLString.Split(',');
                foreach (string col in column) {
                    if (!col.ToUpper().StartsWith("PRIMARY KEY")) {
                        string[] split = col.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //Just to make it the same as the MySQL schema.
                        schema.Add(new string[] { split[0], split[1],
                                              ( split.Count() > 2 ? (split[2].ToUpper() == "NOT" ? "NOT NULL" : "DEFAULT NULL") : ""),
                                              ( split.Count() > 2 ? (split[split.Count() - 2].ToUpper() == "PRIMARY" && split[split.Count() - 1].ToUpper() == "KEY" ? "PRI" : "") : ""),
                                              "NULL",
                                              (split.Contains("AUTO_INCREMENT") || split.Contains("AUTOINCREMENT") ? "AUTO_INCREMENT" : "")});
                    }
                }
                return schema;
            }

            private static List<string> getTables() {
                List<string> tableNames = new List<string>();
                using (DataTable tables = fillData((Server.useMySQL ? "SHOW TABLES" : "SELECT * FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'"))) {
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
                        MySQL.execute(queryString, createDB);
                    } else {
                        if (!createDB) // Databases do not need to be created in SQLite.
                            SQLite.execute(queryString);
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
                            MySQL.fill(queryString, toReturn);
                        } else {
                            SQLite.fill(queryString, toReturn);
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

            internal static void fillDatabase(Stream stream) {
                //Backup
                using (FileStream backup = File.Create("backup.sql")) {
                    CopyDatabase(new StreamWriter(backup));
                }
                //Delete old
                List<string> tables = getTables();
                foreach (string name in tables) {
                    executeQuery(String.Format("DROP TABLE {0}", name));
                }
                //Make new
                string script = new StreamReader(stream).ReadToEnd();
                executeQuery(script);
            }
        }
    }
}