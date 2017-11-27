using System;
using System.Collections.Generic;
using System.Data;
using SimpleSql.Query;
using System.Globalization;
using SimpleSql.Infrastructure;
using SimpleSql.Abstract;
using System.Linq;


namespace SimpleSql
{
    public static class Extensions
    {
        public static UpdateBuilder<T> Update<T>(this IDbConnection conn)
        {
            return new UpdateBuilder<T>(conn);
        }
        public static DeleteBuilder<T> Delete<T>(this IDbConnection conn)
        {
            return new DeleteBuilder<T>(conn);
        }
        public static QueryOver<T> CreateQuery<T>(this IDbConnection conn)
        {
            return new QueryOver<T>(conn);
        }
        public static T Get<T>(this IDbConnection conn, object id)
        {
            return new QueryOver<T>(conn).Get(id);
        }
        public static int Insert<T>(this IDbConnection conn, T entity)
        {
            return new InsertBuilder<T>(conn, entity).Execute();
        }
        public static int Delete<T>(this IDbConnection conn, object id)
        {
            return new DeleteBuilder<T>(conn, id).Execute();
        }
        public static int Update<T>(this IDbConnection conn, T entity)
        {
            return new UpdateBuilder<T>(conn, entity).Execute();
        }
        public static object InsertAndGetId<T>(this IDbConnection conn, T entity)
        {
            return new InsertBuilder<T>(conn, entity).ExecuteAndGetId();
        }
    }
}
