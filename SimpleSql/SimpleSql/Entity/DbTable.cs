using SimpleSql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace SimpleSql.Entity
{
    public class DbTable
    {
        public Type Type { set; private get; }
        public string TableName { set; get; }
        public List<DbColumn> DbColumns { set; get; }
        public DbTable(Type type)
        {
            Type = type;
            var attr = type.GetCustomAttribute<SqlTableAttribute>();
            if (attr != null)
                TableName = attr.TableName;
            if (string.IsNullOrEmpty(TableName))
                TableName = type.Name;
            DbColumns = new List<DbColumn>();
            foreach (var p in type.GetProperties())
                if (p.GetCustomAttribute<SqlIgnoreAttribute>() == null)
                    DbColumns.Add(new DbColumn(p));

        }
    }
}
