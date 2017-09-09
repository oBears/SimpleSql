using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace TiORM
{
    public class ParamInfo
    {
        public  string Name { set; get; }
        public  object Value { set; get; }
        public DbType? DbType { set; get; }
        public ParameterDirection? Direction { set; get; }
        public  int? Size { set; get; }
     
    }
}
