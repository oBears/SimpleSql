using System;
using System.Collections.Generic;
using System.Text;
using SimpleSql.FluentMap.Mapping;
using SimpleSql.FluentMap.Resolvers;

namespace SimpleSql.Migration
{
    public class DbMigrations
    {
        public void InitTable()
        {
            var keys = MapConfig.EntityMaps.Keys;
            foreach (var enttiyType in keys)
            {
                var tableName = DefaultResolver.ResolveTableName(enttiyType);
                var columns = DefaultResolver.ResolveColumns(enttiyType);
            }
        }


    }
}
