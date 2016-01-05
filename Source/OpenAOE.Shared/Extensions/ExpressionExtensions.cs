using System.Linq.Expressions;
using System.Reflection;

namespace OpenAOE.Extensions
{
    /// <summary>
    /// Extension methods for handling expression trees
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Converts an expression into a <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns>The member info.</returns>
        /// <remarks>Originates from Caliburn Micro, under MIT License. Copyright (c) 2010 Blue Spire Consulting, Inc.</remarks>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression) expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression) lambda.Body;
                memberExpression = (MemberExpression) unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression) lambda.Body;
            }

            return memberExpression.Member;
        }
    }
}
