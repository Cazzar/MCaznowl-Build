using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MCForge
{
    namespace SQL
    {
        class MySQLTransactionHelper : DatabaseTransactionHelper
        {
            private MySqlConnection connection = null;
            private MySqlTransaction transaction = null;

            public MySQLTransactionHelper(){
                init(MySQL.connString);
            }

            public MySQLTransactionHelper(string conn)
            {
                init(conn);
            }

            private void init(string conn) {
                connection = new MySqlConnection(conn);
                connection.Open();
                connection.ChangeDatabase(Server.MySQLDatabaseName);

                transaction = connection.BeginTransaction();
            }

            public static DatabaseTransactionHelper Create() {
                return Create(MySQL.connString);
            }

            public static DatabaseTransactionHelper Create(string conn)
            {
                try
                {
                    return new MySQLTransactionHelper(conn);
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
                    using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction)) {
                        cmd.ExecuteNonQuery();
                    }
                } catch (Exception e) {
                    System.IO.File.AppendAllText("MySQL_error.log", DateTime.Now + " " + query + "\r\n");
                    Server.ErrorLog(e);
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