using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SimpleSql.Abstract;
using System.Data.Common;

namespace SimpleSql
{
    public class SimpleSqlOptions
    {
        public DbProviderFactory dbProvider { set; get; }
        public string ConnnectString { set; get; }
        public SimpleSqlOptions UseMysql(string connectionStr)
        {
            dbProvider = MySqlClientFactory.Instance;
            ConnnectString = connectionStr;
            return this;
        }
        public SimpleSqlOptions UseSqlite(string connectionStr)
        {
            ConnnectString = connectionStr;
            throw new NotImplementedException();
        }
        public SimpleSqlOptions UseNpsql(string connectionStr)
        {
            ConnnectString = connectionStr;
            throw new NotImplementedException();
        }
        public IDbConnection CreateConnection()
        {
            var conn = dbProvider.CreateConnection();
            conn.ConnectionString = ConnnectString;
            return conn;
        }
        public SimpleSqlOptions UseMirgation()
        {
            var m = new Migration(CreateConnection());
            m.InitDatabase();
            return this;
        }

    }
}
