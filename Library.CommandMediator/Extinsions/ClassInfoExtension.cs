using System.Linq.Expressions;
using System.Reflection;

namespace Library.CommandMediator.Extinsions
{
    internal static class ClassInfoExtension
    {
        public static PropertyInfo GetPropertyInfo<TType, TReturn>(this Expression<Func<TType, TReturn>> property)
        {
            LambdaExpression lambda = property;
            var memberExpression = lambda.Body is UnaryExpression expression
                ? (MemberExpression)expression.Operand
                : (MemberExpression)lambda.Body;

            return (PropertyInfo)memberExpression.Member;
        }
    }
}
