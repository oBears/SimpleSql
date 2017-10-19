using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Entity
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SqlColumnAttribute : Attribute
    {
        public string ColumnName { get;  set; }
        /// <summary>
        /// 主键
        /// </summary>
        public bool Key { get;  set; }
        /// <summary>
        /// 自增
        /// </summary>
        public bool Increment { get;  set; }
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
    }
}
