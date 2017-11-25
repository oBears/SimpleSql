using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using SimpleSql.Entity;

namespace SimpleSql.Abstract
{
    public class UpdateBuilder<T> : BaseBuilder<T>
    {
        private StringBuilder WhereBuilder = new StringBuilder();
        private StringBuilder SetFieldBuilder = new StringBuilder();
        public UpdateBuilder(IDbConnection conn) : base(conn)
        {

        }
        public UpdateBuilder(IDbConnection conn,T entity) : base(conn)
        {
            var properties = DefaultResolver.ResolveProperties(type, false);
            foreach (var p in properties)
            {
                var fieldName = DefaultResolver.ResolveColumnName(p);
                var paramterName = GetNewParamter();
                var sql = $"{fieldName}={paramterName}";
                SetFieldBuilder.Append(SetFieldBuilder.Length > 0 ? $",{sql}" : $" SET {sql}");
                SetParamter(paramterName, p.GetValue(entity, null));
            }
        }
        public UpdateBuilder<T> Where(Expression<Func<T, bool>> expression)
        {
            var sql = _sqlTranslator.VisitExpression(expression);
            WhereBuilder.Append(WhereBuilder.Length > 0 ? $" AND {sql}" : $" WHERE {sql}");
            return this;
        }
        public UpdateBuilder<T> SetField(Expression<Func<T, bool>> expression)
        {
            var sql = _sqlTranslator.VisitExpression(expression);
            SetFieldBuilder.Append(SetFieldBuilder.Length > 0 ? $" ,{sql}" : $" SET {sql}");
            return this;
        }
        public override int Execute()
        {
            var sql = $"UPDATE {TableName}  {SetFieldBuilder.ToString()} {WhereBuilder.ToString()}";
            return _conn.Execute(sql, GetParamters());
        }
    }

}
