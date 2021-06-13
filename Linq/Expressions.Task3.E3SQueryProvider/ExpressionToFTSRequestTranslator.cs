using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            switch (node.Method.Name)
            {
                case "Where" when node.Method.DeclaringType == typeof(Queryable):
                    var predicate = node.Arguments[1];
                    Visit(predicate);
                    return node;

                case "Contains" when node.Method.DeclaringType == typeof(string):
                    Visit(node.Object);

                    _resultStringBuilder.Append("(*");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append("*)");

                    return node;

                case "Equals" when node.Method.DeclaringType == typeof(string):
                    Visit(node.Object);

                    _resultStringBuilder.Append("(");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append(")");

                    return node;

                case "StartsWith" when node.Method.DeclaringType == typeof(string):
                    Visit(node.Object);

                    _resultStringBuilder.Append("(");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append("*)");

                    return node;

                case "EndsWith" when node.Method.DeclaringType == typeof(string):
                    Visit(node.Object);

                    _resultStringBuilder.Append("(*");
                    Visit(node.Arguments[0]);
                    _resultStringBuilder.Append(")");

                    return node;

                default:
                    return base.VisitMethodCall(node);
            }
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    var constant = node.Right.NodeType == ExpressionType.Constant ? node.Right : node.Left;
                    var prop = node.Left.NodeType == ExpressionType.MemberAccess ? node.Left : node.Right;

                    Visit(prop);
                    _resultStringBuilder.Append("(");
                    Visit(constant);
                    _resultStringBuilder.Append(")");
                    break;
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(" AND ");
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
