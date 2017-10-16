using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using SimpleSql.Infrastructure;

namespace SimpleSql.FluentMap.Mapping
{
    public class EntityMap<T>: IEntityMap
    {
        public string TableName { get; private set; }
        public List<DbColumn> DbColumns { set; get; } = new List<DbColumn>();
        public DbColumn Map<TValue>(Expression<Func<T, TValue>> expression)
        {
            var p = (PropertyInfo)ExpressionHelper.GetMemberInfo(expression);
            var pMap = new DbColumn(p);
            //TODO 判断是否重复添加
            DbColumns.Add(pMap);
            return pMap;
        }

        public DbColumn Id<TValue>(Expression<Func<T, TValue>> expression)
        {
            return Map(expression).Column("Id").IsKey().IsIdentity();
        }
        public void Table(string name)
        {
            TableName = name;
        }
    }
}
