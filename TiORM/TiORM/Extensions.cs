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
using TiORM.Common;


namespace TiORM
{
    public static class Extensions
    {

        public static void SetupCommand(IDbCommand cmd, string sql, object parm = null)
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
                    p.DbType = dbType.Value;
                    if (item.Size.HasValue)
                        p.Size = item.Size.Value;
                    if (item.Direction.HasValue)
                        p.Direction = item.Direction.Value;
                    cmd.Parameters.Add(p);
                }
            }
        }
        public static DynamicParameters GetParamters(object param)
        {
            if (param is DynamicParameters) return param as DynamicParameters;
            var type = param.GetType();
            if (!type.IsClass)
            {
                throw new Exception($"{type.Name}不是Class");
            }
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
            SetupCommand(cmd, sql, param); ;
            return (T)cmd.ExecuteScalar();
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
