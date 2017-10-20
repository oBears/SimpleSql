using SimpleSql.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace SimpleSql.Abstract
{
    public class Migration
    {
        private readonly IDbConnection conn;
        public Migration(IDbConnection dbConnection)
        {
            conn = dbConnection;
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
            var sql = $"CREATE TABLE {tableName}({string.Join(",", cols)})ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            conn.Execute(sql);
            return true;
        }
        public bool DropColumn(string tableName, string columnName)
        {
            var sql = $"alter table `{tableName}` drop column {columnName}";
            conn.Execute(sql);
            return true;
        }
        public bool DropTable(string tableName)
        {
            var sql = $"drop table {tableName}";
            throw new NotImplementedException();
        }
        public bool ExistsColumn(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }
        public bool ExistsTable(string tableName)
        {
            throw new NotImplementedException();
        }
        public bool UpdateColumn(string tableName, DbColumn column)
        {
            var sql = $"ALTER TABLE {tableName} MODIFY {column.ColumnName} {column.DataType}";
            if (column.Length > 0)
                sql += $"({column.Length})";
            if (!column.Nullable)
                sql += " NOT NULL";
            if (!string.IsNullOrEmpty(column.Default))
                sql += $" DEFAULT '{column.Default}'";

            throw new NotImplementedException();
        }
        public bool AddPrimaryKey(string tableName, string columnName)
        {
            var sql = $"alter table `{tableName}` add primary key({columnName});";
            throw new NotImplementedException();
        }
        public bool HasSystemTablePermissions()
        {
            throw new NotImplementedException();
        }
        public bool RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            throw new NotImplementedException();
        }

        public void Check()
        {
            var sql = "SELECT TABLE_NAME,COLUMN_NAME,IS_NULLABLE,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH, COLUMN_KEY FROM information_schema.`COLUMNS` WHERE TABLE_SCHEMA='test'";

        }
    }
    public class Col
    {
        public string TableName { set; get; }
        public string ColumnName { set; get; }
        public string IsNullable { set; get; }
        public string DataType { set; get; }
        public int Length { set; get; }
        public bool PrimaryKey { set; get; }
    }
}
