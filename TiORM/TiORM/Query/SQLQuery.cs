using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TiORM.Query
{
    public class SQLQuery<T>
    {
        private readonly SqlBuilder _sqlBuilder;
        private readonly IDbConnection _conn;

        public SQLQuery(IDbConnection conn, string selectSQL)
        {
            _sqlBuilder = new SqlBuilder();
            _sqlBuilder.AppendSelect(selectSQL);
            _conn = conn;
        }
        public SQLQuery<T> Where(string whereSQL)
        {
            return And(whereSQL);
        }
        public SQLQuery<T> And(string whereSQL)
        {
            _sqlBuilder.AppendAnd(whereSQL);
            return this;
        }
        public SQLQuery<T> Or(string whereSQL)
        {
            _sqlBuilder.AppendOr(whereSQL);
            return this;
        }
        public SQLQuery<T> OrderBy(string orderSql)
        {
            _sqlBuilder.AppendOrder(orderSql);
            return this;
        }
        public SQLQuery<T> Skip(int num)
        {
            _sqlBuilder.SkipNum = num;
            return this;
        }
        public SQLQuery<T> Take(int num)
        {
            _sqlBuilder.TakeNum = num;
            return this;
        }
        public List<T> ToList(object param = null)
        {
            return _conn.Query<T>(_sqlBuilder.CustomSQL, param).ToList();
        }
    }
}
