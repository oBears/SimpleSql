using SimpleSql.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSql.FluentMap.Resolvers
{
    public static class DefaultResolver
    {
        public static string ResolveTableName(Type type) => GetEntityMap(type).TableName;
        public static IEnumerable<PropertyInfo> ResolveProperties(Type type, bool filterIdentity = false)
        {
            foreach (var dbColumn in GetEntityMap(type).DbColumns)
            {
                if (!dbColumn.Ignored && (!filterIdentity || !dbColumn.Identity))
                {
                    yield return dbColumn.PropertyInfo;
                }
            }
        }
        public static PropertyInfo ResolveKeyProperty(Type type, out bool isIdentity)
        {
            var dbColumn = GetEntityMap(type).DbColumns.FirstOrDefault(p => p.Key);
            isIdentity = dbColumn.Identity;
            return dbColumn.PropertyInfo;

        }
        public static string ResolveColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType != null)
            {
                var dbColumn = GetEntityMap(propertyInfo.DeclaringType).DbColumns.FirstOrDefault(m => m.PropertyInfo.Name == propertyInfo.Name);
                return dbColumn.ColumnName;
            }
            throw new Exception($"属性{propertyInfo.Name}没有基类");

        }
        public static IEnumerable<string> ResolveColumnNames(Type type, IEnumerable<PropertyInfo> propertyInfos)
        {
            foreach (var property in propertyInfos)
            {
                var dbColumn = GetEntityMap(type).DbColumns.FirstOrDefault(p => p.PropertyInfo.Name == property.Name);
                if (dbColumn != null && !dbColumn.Ignored)
                {
                    yield return dbColumn.ColumnName;
                }
            }

        }
        public static IEnumerable<string> ResolveColumnNames(Type type, bool filterIdentity = false)
        {
            foreach (var dbColumn in GetEntityMap(type).DbColumns)
            {
                if (!dbColumn.Ignored && (!filterIdentity || !dbColumn.Identity))
                {
                    yield return dbColumn.ColumnName;
                }
            }
        }

        public static IEntityMap GetEntityMap(Type type)
        {
            if (!SimpleSqlConfig.EntityMaps.TryGetValue(type, out IEntityMap entityMap))
            {
                throw new Exception($"没有注册配置类{type.Name}");
            }
            return entityMap;
        }

        public static IEnumerable<DbColumn> ResolveColumns(Type type)
        {
            return GetEntityMap(type).DbColumns;
        }
    }
}
