using SimpleSql.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Abstract
{
    public abstract class DbTypeConverter
    {
        public virtual string Convert(Type type)
        {
            var dbtype = string.Empty;
            if (type == typeof(int))
                dbtype = "int";
            if (type == typeof(string))
                dbtype = "varchar";
            if (type == typeof(DateTime))
                dbtype = "datetime";
            if (type == typeof(double))
                dbtype = "double";
            if (type == typeof(float))
                dbtype = "float";
            return dbtype;
        }
    }
}
