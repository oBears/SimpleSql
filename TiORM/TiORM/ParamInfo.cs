using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace TiORM
{
    public class ParamInfo : DbParameter
    {
        public override DbType DbType { set; get; }
        public override ParameterDirection Direction { set; get; }
        public override bool IsNullable { set; get; }
        public override string ParameterName { set; get; }
        public override int Size { set; get; }
        public override string SourceColumn { set; get; }
        public override bool SourceColumnNullMapping { set; get; }
        public override object Value { set; get; }
        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }
     
    }
}
