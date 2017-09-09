using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using TiORM.FluentMap.Resolvers;

namespace TiORM
{
    public class BaseBuilder<T>
    {
        public IDbConnection _conn;
        public Type type = typeof(T);
        public SqlTranslator<T> _sqlTranslator;
        public string TableName { set; get; }

        public BaseBuilder(IDbConnection conn)
        {
            _conn = conn;
            _sqlTranslator = new SqlTranslator<T>();
            TableName = DefaultResolver.ResolveTableName(type);
        }
        public void SetParamter(string key,object val)
        {
             _sqlTranslator.Params.Add(key, val);
        }
        public string GetNewParamter()
        {
            return _sqlTranslator.PIndex;
        }
        public Dictionary<string,object> GetParamters()
        {
            return _sqlTranslator.Params;
        }
        public virtual int Execute()
        {
            throw new NotImplementedException();
        }

     

    }

}
