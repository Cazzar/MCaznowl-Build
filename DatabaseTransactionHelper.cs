using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MCForge {
    namespace SQL {
        public abstract class DatabaseTransactionHelper : IDisposable {

            public static DatabaseTransactionHelper Create() {
                if (Server.useMySQL) {
                    return MySQLTransactionHelper.Create();
                } else {
                    return SQLiteTransactionHelper.Create();
                }
            }

            public abstract void Execute(string query);

            public abstract void Commit();

            public abstract void Dispose();
        }
    }
}