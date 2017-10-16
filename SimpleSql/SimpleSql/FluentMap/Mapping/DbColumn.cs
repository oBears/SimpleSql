using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SimpleSql.Infrastructure;

namespace SimpleSql.FluentMap.Mapping
{
    public class DbColumn
    {
        public DbColumn(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            //var columnType = propertyInfo.PropertyType;
            //if (columnType.IsGenericTypeDefinition && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //{
            //    Nullable = true;
            //    columnType = columnType.GetGenericArguments()[0];
            //}
            //var dbConvert = ServiceLooker.GetService<DbTypeConverter>();
            //Type = dbConvert.Convert(columnType);
        }
        public string ColumnName { get; private set; }

        /// <summary>
        /// 忽略
        /// </summary>
        public bool Ignored { get; private set; }
        /// <summary>
        /// 主键
        /// </summary>
        public bool Key { get; private set; }
        /// <summary>
        /// 自增
        /// </summary>
        public bool Identity { get; private set; }
        /// <summary>
        /// 大小
        /// </summary>
        public int Size { set; get; }
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool Nullable { set; get; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type { set; get; }
        public PropertyInfo PropertyInfo { get; private set; }

        public DbColumn Column(string name)
        {
            ColumnName = name;
            return this;
        }
        public DbColumn Ignore()
        {
            Ignored = true;
            return this;
        }
        public DbColumn IsKey()
        {
            Key = true;
            return this;
        }
        public DbColumn IsIdentity()
        {
            Identity = true;
            return this;
        }

        public DbColumn DataType(string type)
        {
            Type = type;
            return this;
        }
        public DbColumn IsNullable()
        {
            Nullable = true;
            return this;
        }
        public DbColumn Length(int size)
        {
            Size = size;
            return this;
        }
    }
}
