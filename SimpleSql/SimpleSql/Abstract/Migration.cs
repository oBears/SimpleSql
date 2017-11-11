using SimpleSql.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SimpleSql.Abstract
{
    public class Migration
    {
        private readonly IDbConnection conn;
        public List<DbMetaData> DbMetaDatas { private set; get; }
        public bool HasSystemTablePermissions { private set; get; }
        private string database;
        private string systemDataBase = "mysql";
        private string connectionStr;
        public Migration(IDbConnection dbConnection)
        {
            conn = dbConnection;
            database = conn.Database;
            connectionStr = conn.ConnectionString;
            try
            {
                conn.ConnectionString = conn.ConnectionString.Replace(database, systemDataBase);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                HasSystemTablePermissions = true;
            }
            catch (Exception ex)
            {
                HasSystemTablePermissions = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public bool CreateTable(string tableName, List<DbColumn> columns)
        {
            var cols = new List<string>();
            var keyCol = string.Empty;
            foreach (var col in columns)
            {
                var tmpSql = $"`{col.ColumnName}` {col.DataType}";
                if (col.Length > 0)
                    tmpSql += $"({col.Length})";
                if (!col.Nullable)
                    tmpSql += " NOT NULL";
                if (col.Increment)
                    tmpSql += " AUTO_INCREMENT";
                if (!string.IsNullOrEmpty(col.Default))
                    tmpSql += $" DEFAULT {col.Default}";
                if (col.Key)
                    keyCol = $" PRIMARY KEY ( `{col.ColumnName}` )";
                cols.Add(tmpSql);
            }
            if (!string.IsNullOrEmpty(keyCol))
                cols.Add(keyCol);
            var sql = $"CREATE TABLE {database}.`{tableName}`({string.Join(",", cols)})ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
            return conn.Execute(sql) > 0;
        }
        public bool DropColumn(string tableName, string columnName)
        {
            var sql = $"alter table {database}.`{tableName}` drop column {columnName}";
            return conn.Execute(sql) > 0;
        }
        public bool DropTable(string tableName)
        {
            var sql = $"drop table {database}.{tableName}";
            return conn.Execute(sql) > 0;
        }
        public bool ExistsColumn(string tableName, string columnName)
        {
            return DbMetaDatas.Any(x => x.Database == database && x.TableName == tableName && x.ColumnName == columnName);
        }
        public bool ExistsTable(string tableName)
        {
            return DbMetaDatas.Any(x => x.Database == database && x.TableName == tableName);
        }
        public bool ExistsDatabase()
        {
            return DbMetaDatas.Any(x => x.Database == database);
        }
        public bool CreateDatabase()
        {
            var sql = $"CREATE DATABASE {database}";
            return conn.Execute(sql) > 0;
        }

        public bool UpdateColumn(string tableName, DbColumn column)
        {
            var sql = $"ALTER TABLE {database}.{tableName} MODIFY {column.ColumnName} {column.DataType}";
            if (column.Length > 0)
                sql += $"({column.Length})";
            if (!column.Nullable)
                sql += " NOT NULL";
            if (!string.IsNullOrEmpty(column.Default))
                sql += $" DEFAULT '{column.Default}'";
            return conn.Execute(sql) > 0;

        }
        public bool AddPrimaryKey(string tableName, string columnName)
        {
            var sql = $"alter table {database}.`{tableName}` add primary key({columnName});";
            return conn.Execute(sql) > 0;
        }

        public bool RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            throw new NotImplementedException();
        }

        public void InitDatabase()
        {
            var sql = $"SELECT TABLE_SCHEMA `Database`,TABLE_NAME TableName,COLUMN_NAME ColumnName,IS_NULLABLE IsNullable,DATA_TYPE DataType,CHARACTER_MAXIMUM_LENGTH Length, COLUMN_KEY ColumnKey FROM information_schema.`COLUMNS` WHERE TABLE_SCHEMA='{database}'";
            DbMetaDatas = conn.Query<DbMetaData>(sql).ToList();
            if (!ExistsDatabase())
                CreateDatabase();
            var tableTypes = Assembly.GetEntryAssembly().GetExportedTypes().Where(t => t.GetCustomAttribute<SqlTableAttribute>() != null);
            foreach (var tableType in tableTypes)
            {
                var dbTable = DefaultResolver.GetDbTable(tableType);
                if (!ExistsTable(dbTable.TableName))
                    CreateTable(dbTable.TableName, dbTable.DbColumns);
            }
            if (HasSystemTablePermissions)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.ConnectionString = connectionStr;
            }
        }
    }
}
