using System;

namespace SimpleSql.Abstract
{
    public class DbMetaData
    {
        public string Database { set; get; }
        public string TableName { set; get; }
        public string ColumnName { set; get; }
        public string IsNullable { set; get; }
        public string DataType { set; get; }
        public UInt64 Length { set; get; }
        public string ColumnKey { set; get; }
    }
}
