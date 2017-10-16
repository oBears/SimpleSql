using SimpleSql.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Abstract
{
    public interface IDbMaintenance
    {
        List<string> GetTableList();
        List<DbColumn> GetColumnsByTableName(string tableName);
        bool ExistsTable(string tableName);
        bool ExistsColumn(string tableName, string columnName);
        bool HasSystemTablePermissions();
        bool AddPrimaryKey(string tableName, string columnName);
        bool AddColumn(string tableName, DbColumn columnInfo);
        bool UpdateColumn(string tableName, DbColumn column);
        bool CreateTable(string tableName, List<DbColumn> columns);
        bool DropTable(string tableName);
        bool DropColumn(string tableName, string columnName);
        bool DropConstraint(string tableName, string constraintName);
        bool TruncateTable(string tableName);
        bool RenameColumn(string tableName, string oldColumnName, string newColumnName);
    }
}
