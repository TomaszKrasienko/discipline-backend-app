using System.Linq.Expressions;

namespace discipline.infrastructure.DAL;

public static class TypeReplacer {
    public static Expression<Func<TTo, bool>> ReplaceParameter<TFrom, TTo>(this Expression<Func<TFrom, bool>> target) 
    {
        return (Expression<Func<TTo, bool>>) new WhereReplacerVisitor<TFrom, TTo>().Visit(target);
    }
    private class WhereReplacerVisitor<TFrom, TTo> : ExpressionVisitor 
    {
        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(TTo), "c");

        protected override Expression VisitLambda<T>(Expression<T> node) {
            // replace parameter here
            return Expression.Lambda(Visit(node.Body), _parameter);
        }            

        protected override Expression VisitMember(MemberExpression node) {
            // replace parameter member access with new type
            if (node.Member.DeclaringType == typeof(TFrom) && node.Expression is ParameterExpression) {
                return Expression.PropertyOrField(_parameter, node.Member.Name);
            }
            return base.VisitMember(node);
        }
    }
}