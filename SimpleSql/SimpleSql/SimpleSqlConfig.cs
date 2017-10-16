using SimpleSql.FluentMap.Mapping;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace SimpleSql
{
    public static class SimpleSqlConfig
    {
        public static readonly ConcurrentDictionary<Type, IEntityMap> EntityMaps = new ConcurrentDictionary<Type, IEntityMap>();

        public static void AddMap<TEntity>(EntityMap<TEntity> entityMap)
        {
            EntityMaps.TryAdd(typeof(TEntity), entityMap);
        }
        //TODO 全局配置大小写
        /// <summary>
        /// 区分大小写
        /// </summary>
        public static bool CaseSensitive { get; set; }
        /// <summary>
        /// 自动迁移
        /// </summary>
        public static bool AutoMigration { set; get; }
    }
}
