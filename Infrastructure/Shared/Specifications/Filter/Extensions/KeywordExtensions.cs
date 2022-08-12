using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Shared.Specifications.Filter.Extensions;

internal static class KeywordExtensions
{
    public static Expression<Func<T, bool>> Contains<T, U>(
        this Keyword keyword,
        Expression<Func<T, U>> expression
    )
    {
        var (parameter, property) = expression.GetParameterAndProperty();

        var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

        Expression instance = keyword.InQuotes ? property : Expression.Call(property, toLower!);
        var value = Expression.Constant(keyword.Value);
        var body = Expression.Call(instance, contains!, value);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> ContainsInChildren<T, U, V>(
        this Keyword keyword,
        Expression<Func<T, IList<U>>> parentExpression,
        Expression<Func<U, V>> childExpression
    )
    {
        var (parameter, property) = parentExpression.GetParameterAndProperty();

        var any = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(U));

        var anyLambda = keyword.Contains(childExpression);
        var body = Expression.Call(any!, property, anyLambda);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
