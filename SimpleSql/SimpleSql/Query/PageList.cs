using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql.Query
{
    public class PageInfo<T>
    {
        public int Count { set; get; }
        public IEnumerable<T> Items { set; get; }

    }
}
