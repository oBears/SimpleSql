using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using SimpleSql.Entity;
using SimpleSql;
using System.Linq;
using SimpleSql.Abstract;

namespace SimpleSql.Query
{
    public class QueryOver<T>
    {
        private readonly SqlTranslator<T> _sqlTranslator;
        private readonly SqlBuilder _sqlBuilder;
        private readonly IDbConnection _conn;
        public Type Type { private set; get; }
        public QueryOver(IDbConnection conn)
        {
            Type = typeof(T);
            _sqlTranslator = new SqlTranslator<T>();
            _sqlBuilder = new SqlBuilder()
            {
                TableName = DefaultResolver.ResolveTableName(Type),
                DefaultColumns = DefaultResolver.ResolveColumnNames(Type)
            };
            _conn = conn;
        }
        public QueryOver<T> Select(Expression<Func<T, object>> expression)
        {
            var sql = _sqlTranslator.VisitExpression(expression);
            _sqlBuilder.AppendSelect(sql);
            return this;
        }
        public QueryOver<T> Where(Expression<Func<T, bool>> expression)
        {
            return And(expression);
        }
        public QueryOver<T> WhereIF(bool flag, Expression<Func<T, bool>> expression)
        {
            if (flag)
                return And(expression);
            return this;
        }
        public QueryOver<T> And(Expression<Func<T, bool>> expression)
        {
            var sql = _sqlTranslator.VisitExpression(expression);
            _sqlBuilder.AppendAnd(sql);
            return this;
        }
        public QueryOver<T> Or(Expression<Func<T, bool>> expression)
        {
            var sql = _sqlTranslator.VisitExpression(expression);
            _sqlBuilder.AppendOr(sql);
            return this;
        }

        public QueryOver<T> OrderBy(Expression<Func<T, object>> expression)
        {
            var sql = _sqlTranslator.VisitExpression(expression);
            _sqlBuilder.AppendOrder($"{sql} ASC");
            return this;
        }
        public QueryOver<T> DescOrderBy(Expression<Func<T, object>> expression)
        {

            var sql = _sqlTranslator.VisitExpression(expression);
            _sqlBuilder.AppendOrder($"{sql} DESC");
            return this;
        }
        public QueryOver<T> OrderBy(string name)
        {
            _sqlBuilder.AppendOrder($"{name} ASC");
            return this;
        }

        public QueryOver<T> DescOrderBy(string name)
        {
            _sqlBuilder.AppendOrder($"{name} DESC");
            return this;
        }
        public QueryOver<T> Skip(int num)
        {
            _sqlBuilder.SkipNum = num;
            return this;
        }
        public QueryOver<T> Take(int num)
        {
            _sqlBuilder.TakeNum = num;
            return this;
        }
        public List<T> ToList()
        {
            return _conn.Query<T>(_sqlBuilder.BasicSQL, GetParameters()).ToList();
        }
        public List<TResult> ToList<TResult>()
        {
            return _conn.Query<TResult>(_sqlBuilder.BasicSQL, GetParameters()).ToList();
        }

        public PageInfo<T> ToPageList()
        {
            var pageInfo =new PageInfo<T>();
            pageInfo.Items = _conn.Query<T>(_sqlBuilder.BasicSQL, GetParameters()).ToList();
            pageInfo.Count = _conn.ExecuteScalar<int>(_sqlBuilder.CountSQL, GetParameters());
            return pageInfo;
        }
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return this.Where(expression).Take(1).ToList().FirstOrDefault();
        }
        public TResult FirstOrDefault<TResult>()
        {
            return this.Take(1).ToList<TResult>().FirstOrDefault();

        }
        public T Get(object id)
        {
            var keyCol = DefaultResolver.ResolveKey(Type);
            var paramName = _sqlTranslator.ParamIndex;
            _sqlBuilder.AppendAnd($"{keyCol.ColumnName}={paramName}");
            _sqlTranslator.Params.Add(paramName, id);
            return this.Take(1).ToList().FirstOrDefault();
        }

        private DynamicParameters GetParameters()
        {
            return _sqlTranslator.Params;
        }
    }
}
