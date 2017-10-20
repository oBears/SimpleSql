using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SimpleSql.Infrastructure;

namespace SimpleSql.Entity
{
    public class DbColumn
    {
        public DbColumn(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            var colAttr = propertyInfo.GetCustomAttribute<SqlColumnAttribute>();
            if (colAttr != null)
            {
                ColumnName = colAttr.ColumnName;
                Key = colAttr.Key;
                Increment = colAttr.Increment;
                Length = colAttr.Length;
                Nullable = colAttr.Nullable;
                DataType = colAttr.DataType;
            }
            if (string.IsNullOrEmpty(ColumnName))
                ColumnName = propertyInfo.Name;
            if (string.IsNullOrEmpty(DataType))
            {
                //GET DataType
                //var columnType = propertyInfo.PropertyType;
                //if (columnType.IsGenericTypeDefinition && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                //{
                //    Nullable = true;
                //    columnType = columnType.GetGenericArguments()[0];
                //}
                //var dbConvert = ServiceLooker.GetService<DbTypeConverter>();
                //Type = dbConvert.Convert(columnType);
            }
        }
        public string ColumnName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public bool Key { get; private set; }
        /// <summary>
        /// 自增
        /// </summary>
        public bool Increment { get; private set; }
        /// <summary>
        /// 大小
        /// </summary>
        public int Length { set; get; }
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool Nullable { set; get; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { set; get; }
        public string Default { set; get; }
        public PropertyInfo PropertyInfo { get; private set; }
    }
}
