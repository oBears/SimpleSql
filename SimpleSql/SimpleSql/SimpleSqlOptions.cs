using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SimpleSql.Abstract;

namespace SimpleSql
{
    public class SimpleSqlOptions
    {
        public IDbConnection DbConnection { set; get; }
        public SimpleSqlOptions UseMysql(string connectionStr)
        {
            DbConnection = new MySqlConnection(connectionStr);
            return this;
        }
        public SimpleSqlOptions UseSqlite(string connectionStr)
        {
            throw new NotImplementedException();
        }

        public SimpleSqlOptions UseNpsql(string connectionStr)
        {
            throw new NotImplementedException();
        }

        public SimpleSqlOptions UseMirgation()
        {
            var m = new Migration(DbConnection);
            m.InitDatabase();
            return this;
        }

    }
}
