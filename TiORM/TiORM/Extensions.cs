using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using SimpleSql.Query;
using SimpleSql.FluentMap.Resolvers;
using System.Globalization;
using System.Linq;
using SimpleSql.Common;


namespace SimpleSql
{
    public static class Extensions
    {
        private static void SetupCommand(IDbCommand cmd, string sql, object parm = null)
        {
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (parm != null)
            {
                var paramters = GetParamters(parm).parameters;
                foreach (var item in paramters.Values)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = item.Name;
                    p.Value = item.Value;
                    var dbType = item.DbType;
                    //查找DbType
                    if (dbType == null && item.Value != null)
                        dbType = TypeMap.Lookup(item.Value.GetType());
                    if (dbType.HasValue)
                        p.DbType = dbType.Value;
                    if (item.Size.HasValue)
                        p.Size = item.Size.Value;
                    if (item.Direction.HasValue)
                        p.Direction = item.Direction.Value;
                    cmd.Parameters.Add(p);
                }
            }
        }
        private static DynamicParameters GetParamters(object param)
        {
            if (param is DynamicParameters) return param as DynamicParameters;
            var type = param.GetType();
            if (!type.IsClass)
            {
                throw new Exception($"{type.Name}不是Class");
            }
            //TODO 缓存属性信息
            var props = type.GetProperties();
            var dynamicParamters = new DynamicParameters();
            foreach (var p in props)
            {
                dynamicParamters.Add(p.Name, p.GetValue(param));
            }
            return dynamicParamters;
        }
        public static int Execute(this IDbConnection conn, string sql, object param = null)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = conn.CreateCommand();
            SetupCommand(cmd, sql, param); ;
            return cmd.ExecuteNonQuery();
        }
        public static T ExecuteScalar<T>(this IDbConnection conn, string sql, object param = null)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = conn.CreateCommand();
            SetupCommand(cmd, sql, param);
            return (T)Convert.ChangeType(cmd.ExecuteScalar(), typeof(T), CultureInfo.InvariantCulture);
        }
        public static IEnumerable<T> Query<T>(this IDbConnection conn, string sql, object param = null)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var cmd = conn.CreateCommand();
            SetupCommand(cmd, sql, param); ;
            var reader = cmd.ExecuteReader();
            var type = typeof(T);
            while (reader.Read() && reader.FieldCount != 0)
            {
                //TODO Emit优化反射部分
                if (type.IsClass)
                {
                    var props = type.GetProperties();
                    var obj = Activator.CreateInstance<T>();
                    foreach (var p in props)
                    {
                        var colName = p.Name;
                        var val = reader[colName];
                        p.SetValue(obj, val);
                    }
                    yield return obj;
                }
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

    }
}
