using SimpleSql.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SimpleSql
{
    public static class Cache
    {
        public static readonly ConcurrentDictionary<Type, DbTable> TableMaps = new ConcurrentDictionary<Type, DbTable>();
    }
}
