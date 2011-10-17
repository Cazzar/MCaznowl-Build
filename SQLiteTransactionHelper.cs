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