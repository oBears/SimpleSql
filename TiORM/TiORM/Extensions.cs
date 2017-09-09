using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using TiORM.Query;
using TiORM.FluentMap.Resolvers;
using System.Globalization;
using System.Linq;


namespace TiORM
{
    public static class Extensions
    {
        private static DynamicParameters GetParamter(object param)
        {
            return new DynamicParameters();
        }

        public static int Execute(this IDbConnection conn, string sql, object param = null)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 20;
         
            //cmd.Parameters = 
            return cmd.ExecuteNonQuery();
        }
        public static T ExecuteScalar<T>(this IDbConnection conn, string sql, object param = null)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 20;
            //cmd.Parameters = param;
            return (T)cmd.ExecuteScalar();
        }

        public static IEnumerable<T> Query<T>(this IDbConnection conn, string sql, object param = null)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 20;
            //cmd.Parameters = param;
            var reader = cmd.ExecuteReader();
            var type = typeof(T);
            while (reader.Read() && reader.FieldCount != 0)
            {
                //TODO 判断T 是否是Class,  用反射创建对象并赋值（Emit优化）
                if (type.IsClass)
                {
                    //var props = DefaultResolver.ResolveProperties(type);
                    var props = type.GetProperties();
                    var obj = Activator.CreateInstance<T>();
                    foreach (var p in props)
                    {
                        var colName = p.Name;
                        var val = reader[colName];
                        //var index = reader.GetOrdinal(colName);
                        //var val = reader.GetInt32(index);
                        //TODO  这里获取的值
                        p.SetValue(obj, val);
                    }
                    yield return obj;
                }
                //TODO 基础类型直接转换
                yield return (T)Convert.ChangeType(reader[0], type, CultureInfo.InvariantCulture);
            }
        }

        public static UpdateBuilder<T> Update<T>(this IDbConnection conn)
        {
            return new UpdateBuilder<T>(conn);
        }
        public static DeleteBuilder<T> Delete<T>(this IDbConnection conn)
        {
            return new DeleteBuilder<T>(conn);
        }
        public static InsertBuilder<T> Insert<T>(this IDbConnection conn, T entity)
        {
            return new InsertBuilder<T>(conn, entity);
        }
        public static QueryOver<T> CreateQuery<T>(this IDbConnection conn)
        {
            return new QueryOver<T>(conn);
        }
        public static SQLQuery<T> CreateSQL<T>(this IDbConnection conn, string sql)
        {
            return new SQLQuery<T>(conn, sql);
        }

    }
}
