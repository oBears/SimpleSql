﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SimpleSql.Query;
using System.Reflection;
using SimpleSql.Infrastructure;
using SimpleSql.Entity;

namespace SimpleSql.Abstract
{
    public class SqlTranslator<T>
    {
        public readonly Type Type = typeof(T);
        public DynamicParameters Params = new DynamicParameters();
        private int _index = 0;
        public string ParamIndex
        {
            get { return $"@p{_index++}"; }
        }
        public string VisitExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    return VisitAndOr((BinaryExpression)expression);
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    return VisitBinary((BinaryExpression)expression);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression)expression);
                case ExpressionType.New:
                    return VisitNew((NewExpression)expression);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression)expression);
                case ExpressionType.Lambda:
                    return VisitExpression(((LambdaExpression)expression).Body);
                case ExpressionType.Convert:
                    return VisitUnaryExpression((UnaryExpression)expression);
                default:
                    throw new NotImplementedException($"不支持类型{expression.NodeType.ToString()}");
            }
        }
        public string VisitBinary(BinaryExpression expression)
        {
            //节点类型是字段或属性
            if (expression.Left.NodeType == ExpressionType.MemberAccess||expression.Left.NodeType== ExpressionType.Convert)
            {
                var op = string.Empty;
                switch (expression.NodeType)
                {
                    case ExpressionType.Equal:
                        op = "=";
                        break;
                    case ExpressionType.GreaterThan:
                        op = ">";
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        op = ">=";
                        break;
                    case ExpressionType.LessThan:
                        op = "<";
                        break;
                    case ExpressionType.LessThanOrEqual:
                        op = "<=";
                        break;
                    case ExpressionType.NotEqual:
                        op = "<>";
                        break;
                    default:
                        throw new NotImplementedException($"不支持类型{expression.NodeType.ToString()}");
                }
                var fKey = VisitExpression(expression.Left);
                var fVal = ExpressionHelper.GetExpressValue(expression.Right);
                var p = ParamIndex;
                Params.Add(p, fVal);
                return $" {fKey}{op}{p}";
            }
            return string.Empty;
        }
        public string VisitMemberAccess(MemberExpression expression)
        {
            return DefaultResolver.ResolveColumnName((PropertyInfo)expression.Member);
        }
        public string VisitUnaryExpression(UnaryExpression expression)
        {
            return VisitExpression(expression.Operand);
        }
        public string VisitMethodCall(MethodCallExpression expression)
        {
            var methodName = expression.Method.Name;
            var args = expression.Arguments;
            switch (methodName)
            {
                case "Like":
                    {
                        var fKey = VisitMemberAccess((MemberExpression)args[0]);
                        var fval = ExpressionHelper.GetExpressValue(args[1]);
                        var mode = (MatchMode)ExpressionHelper.GetExpressValue(args[2]);
                        var p = ParamIndex;
                        switch (mode)
                        {
                            case MatchMode.Any:
                                Params.Add(p, $"%{fval}%");
                                break;
                            case MatchMode.Start:
                                Params.Add(p, $"{fval}%");
                                break;
                            case MatchMode.End:
                                Params.Add(p, $"%{fval}");
                                break;
                            default:
                                break;
                        }
                        return $" {fKey} like @{p}";
                    }
                case "In":
                    {
                        var fKey = VisitMemberAccess((MemberExpression)args[0]);
                        var fval = VisitNewArrayInit((NewArrayExpression)args[1]);
                        if (fval.Length > 0)
                        {
                            var inParams = new StringBuilder();
                            foreach (var item in fval)
                            {
                                var p = ParamIndex;
                                Params.Add(p, fval);
                                inParams.Append($"@{p},");
                            }
                            return $" {fKey} in ({inParams.ToString().TrimEnd(',')})";
                        }
                        return string.Empty;
                    }
                case "NotIn":
                    {
                        var fKey = VisitMemberAccess((MemberExpression)args[0]);
                        var fval = VisitNewArrayInit((NewArrayExpression)args[1]);
                        if (fval.Length > 0)
                        {
                            var inParams = new StringBuilder();
                            foreach (var item in fval)
                            {
                                var p = ParamIndex;
                                Params.Add(p, fval);
                                inParams.Append($"@{p},");
                            }
                            return $" {fKey} not in ({inParams.ToString().TrimEnd(',')})";
                        }
                        return string.Empty;
                    }
                default:
                    throw new NotImplementedException($"不支持方法{methodName}");
            }
        }
        public string VisitAndOr(BinaryExpression andOr)
        {
            var op = andOr.NodeType == ExpressionType.AndAlso ? " AND " : " OR ";
            //处理左边表达式
            var leftSql = VisitExpression(andOr.Left);
            //处理右边表达式
            var rightSql = VisitExpression(andOr.Right);
            return $"({leftSql} {op} {rightSql})";
        }
        public string VisitNew(NewExpression expression)
        {
            StringBuilder builder = new StringBuilder();
            if (expression.Members.Count > 0)
            {
                foreach (var member in expression.Members)
                {
                    builder.Append(member.Name + ",");
                }
            }
            return builder.ToString().TrimEnd(',');
        }
        public object[] VisitNewArrayInit(NewArrayExpression expression)
        {
            var val = Expression.Lambda<Func<object[]>>(expression).Compile()();
            return val;
        }
    }
}
