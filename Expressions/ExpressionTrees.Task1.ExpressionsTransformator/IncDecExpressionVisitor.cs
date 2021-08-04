using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private readonly Dictionary<string, int> _parametersMap;

        public IncDecExpressionVisitor(Dictionary<string, int> parametersMap)
        {
            _parametersMap = parametersMap;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parametersMap.ContainsKey(node.Name))
            {
                return Expression.Constant(_parametersMap[node.Name]);
            }
            return base.VisitParameter(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
            => Expression.Lambda(Visit(node.Body));

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Right is ConstantExpression c && c.Value is int value && value == 1)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        return Expression.Increment(base.Visit(node.Left));
                    case ExpressionType.Subtract:
                        return Expression.Decrement(base.Visit(node.Left));
                }
            }
            return base.VisitBinary(node);
        }
    }
}
