using System.Collections.Generic;

namespace SimpleSql.FluentMap.Mapping
{
    public interface IEntityMap
    {
        string TableName { get; }
        List<DbColumn> DbColumns { get; }
    }
}
