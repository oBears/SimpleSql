using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Entity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SqlTableAttribute : Attribute
    {
        public string TableName { set; get; }

        public SqlTableAttribute(string tableName)
        {
            TableName = tableName;
        }

    }
}
