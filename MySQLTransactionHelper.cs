using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MCForge
{
    class MySQLTransactionHelper : IDisposable
    {
        private MySqlConnection connection = null;
        private MySqlTransaction transaction = null;

        private MySQLTransactionHelper(string connString)
        {

            connection = new MySqlConnection(connString);
            connection.Open();
            connection.ChangeDatabase(Server.MySQLDatabaseName);

            transaction = connection.BeginTransaction();
        }

        public static MySQLTransactionHelper Create(string connString)
        {
            try
            {
                return new MySQLTransactionHelper(connString);
            }
            catch (Exception ex)
            {
                Server.ErrorLog(ex);
                return null;
            }
        }

        public void Execute(string query)
        {
			using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
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