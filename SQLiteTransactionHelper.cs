using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
//using MySql.Data.MySqlClient;

namespace MCForge
{
    class SQLiteTransactionHelper : IDisposable
    {
        private SQLiteConnection connection = null;
        private SQLiteTransaction transaction = null;

        private SQLiteTransactionHelper(string connString)
        {

            connection = new SQLiteConnection(connString);
            connection.Open();
            //connection.ChangeDatabase(Server.MySQLDatabaseName);

            transaction = connection.BeginTransaction();
        }

        public static SQLiteTransactionHelper Create(string connString)
        {
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

        public void Execute(string query)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(query, connection, transaction))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Commit()
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

        public void Dispose()
        {
            transaction.Dispose();
            connection.Dispose();
            transaction = null;
            connection = null;
        }
    }
}