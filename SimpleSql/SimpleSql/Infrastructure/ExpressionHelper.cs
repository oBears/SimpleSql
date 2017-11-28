using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using System.Collections;

namespace SimpleSql.Infrastructure
{
    public static class ExpressionHelper
    {
        public static MemberInfo GetMemberInfo(LambdaExpression expression)
        {
            var expr = ((LambdaExpression)expression).Body;
            if (expr.NodeType == ExpressionType.MemberAccess)
                return ((MemberExpression)expr).Member;
            throw new ArgumentException("expression 不是 MemberExpression 类型");
        }
        public static object GetExpressValue(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
                return VisitConstant((ConstantExpression)expression);
            if (expression.Type == typeof(int))
                return Expression.Lambda<Func<int>>(expression).Compile()();
            else if (expression.Type == typeof(string))
                return Expression.Lambda<Func<string>>(expression).Compile()();
            else if (expression.Type == typeof(double))
                return Expression.Lambda<Func<double>>(expression).Compile()();
            else if (expression.Type == typeof(decimal))
                return Expression.Lambda<Func<decimal>>(expression).Compile()();
            else if (expression.Type == typeof(DateTime))
                return Expression.Lambda<Func<DateTime>>(expression).Compile()();
            else
                return Expression.Lambda<Func<object>>(expression).Compile()();
        }
        private static object VisitConstant(ConstantExpression expression) => expression.Value;

        public static IEnumerable GetEnumerableValue(Expression expression)
        {
            return Expression.Lambda<Func<IEnumerable>>(expression).Compile()();
        }
    }
}
