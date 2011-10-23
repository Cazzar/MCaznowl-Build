using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
//using MySql.Data.MySqlClient;

namespace MCForge
{
    namespace SQL 
    {
        class SQLiteTransactionHelper : DatabaseTransactionHelper
        {
            private SQLiteConnection connection = null;
            private SQLiteTransaction transaction = null;

            //in SQLiteTransactionHelper() {
            //    this(SQLite.connString);
            //}

            private SQLiteTransactionHelper() {
                init(SQLite.connString);
            }

            private SQLiteTransactionHelper(string connString)
            {
                init(connString);
            }

            private void init(string connString) {

                connection = new SQLiteConnection(connString);
                connection.Open();
                //connection.ChangeDatabase(Server.MySQLDatabaseName);

                transaction = connection.BeginTransaction();
            }

            public static DatabaseTransactionHelper Create() {
                return Create(SQLite.connString);
            }

            public static DatabaseTransactionHelper Create(string connString) {
                try
                {
                    return new SQLiteTransactionHelper(connString);
                }
                catch (Exception ex)
                {
                    Server.ErrorLog(ex);
                    return null;
                }
            }

            public override void Execute(string query)
            {
                try {
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection, transaction)) {
                        cmd.ExecuteNonQuery();
                    }
                } catch (Exception e) {
                    System.IO.File.AppendAllText("MySQL_error.log", DateTime.Now + " " + query + "\r\n");
                    Server.ErrorLog(e);
                    throw e; // Ensures that only one error is thrown (though two will be caught.)
                }
            }

            public override void Commit()
            {
                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Server.ErrorLog(ex);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Server.ErrorLog(ex2);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            public override void Dispose()
            {
                transaction.Dispose();
                connection.Dispose();
                transaction = null;
                connection = null;
            }
        }
    }
}