using System;
using System.Collections.Generic;
using System.Text;
using SimpleSql.Entity;


namespace SimpleSql.Test.Model
{
    public class ClassRoom
    {
        [SqlColumn(Increment = true, Key = true)]
        public int Id { set; get; }
        public string ClassName { set; get; }
        public int Member { set; get; }
    }


}
