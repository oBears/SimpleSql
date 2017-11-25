using SimpleSql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSql.Entity
{
    public static class DefaultResolver
    {
        public static string ResolveTableName(Type type) => GetDbTable(type).TableName;
        public static IEnumerable<PropertyInfo> ResolveProperties(Type type, bool filterIdentity = false)
        {
            foreach (var dbColumn in GetDbTable(type).DbColumns)
            {
                if (!filterIdentity || !dbColumn.Increment)
                {
                    yield return dbColumn.PropertyInfo;
                }
            }
        }
        public static PropertyInfo ResolveKeyProperty(Type type, out bool isIncrement)
        {
            var dbColumn = GetDbTable(type).DbColumns.FirstOrDefault(p => p.Key);
            isIncrement = dbColumn.Increment;
            return dbColumn.PropertyInfo;

        }
        public static string ResolveColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType != null)
            {
                var dbColumn = GetDbTable(propertyInfo.DeclaringType).DbColumns.FirstOrDefault(m => m.PropertyInfo.Name == propertyInfo.Name);
                return dbColumn.ColumnName;
            }
            throw new Exception($"属性{propertyInfo.Name}没有基类");

        }
        public static IEnumerable<string> ResolveColumnNames(Type type, IEnumerable<PropertyInfo> propertyInfos)
        {
            foreach (var property in propertyInfos)
            {
                var dbColumn = GetDbTable(type).DbColumns.FirstOrDefault(p => p.PropertyInfo.Name == property.Name);
                if (dbColumn != null)
                    yield return dbColumn.ColumnName;
            }

        }
        public static IEnumerable<string> ResolveColumnNames(Type type, bool filterIdentity = false)
        {
            foreach (var dbColumn in GetDbTable(type).DbColumns)
            {
                if (!filterIdentity || !dbColumn.Increment)
                    yield return dbColumn.ColumnName;
            }
        }

        public static DbTable GetDbTable(Type type)
        {
            if (!Cache.TableMaps.TryGetValue(type, out DbTable dbtable))
            {
                dbtable = new DbTable(type);
                Cache.TableMaps.TryAdd(type, dbtable);
            }
            return dbtable;
        }

        public static IEnumerable<DbColumn> ResolveColumns(Type type)
        {
            return GetDbTable(type).DbColumns;
        }
        public static DbColumn ResolveKey(Type type)
        {
            var keyCol= GetDbTable(type).DbColumns.FirstOrDefault(x => x.Key);
            if (keyCol == null)
                throw new Exception($"类{type.Name}没有设置主键");
            return keyCol;
        }
    }
}
