using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleSql.FluentMap.Mapping
{
    public class PropertyMap
    {
        public PropertyMap(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            var columnType = propertyInfo.PropertyType;
            if (columnType.IsGenericTypeDefinition && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Nullable = true;
                columnType = columnType.GetGenericArguments()[0];
            }
            if (columnType== typeof(Int32))
            {
                Type = "int";
            }
            if (columnType == typeof(String))
            {
                Type = "varchar";
            }
            if (columnType == typeof(String))
            {
                Type = "varchar";
            }
            //TODO 根据属性类型 

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

        public PropertyMap Column(string name)
        {
            ColumnName = name;
            return this;
        }
        public PropertyMap Ignore()
        {
            Ignored = true;
            return this;
        }
        public PropertyMap IsKey()
        {
            Key = true;
            return this;
        }
        public PropertyMap IsIdentity()
        {
            Identity = true;
            return this;
        }

        public PropertyMap DataType(string type)
        {
            Type = type;
            return this;
        }
        public PropertyMap IsNullable()
        {
            Nullable = true;
            return this;
        }
        public PropertyMap Length(int size)
        {
            Size = size;
            return this;
        }
    }
}
