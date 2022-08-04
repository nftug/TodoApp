using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Shared.Specifications.Filter.Managers;

internal class KeywordConvertManager
{
    private static Expression<Func<T, bool>> Contains<T>(
        Keyword keyword,
        ParameterExpression param,
        MemberExpression property
    )
    {
        var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

        Expression instance = keyword.InQuotes ? property : Expression.Call(property, toLower!);
        var value = Expression.Constant(keyword.Value);
        var body = Expression.Call(instance, contains!, value);

        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    public static Expression<Func<T, bool>> Contains<T>(
        Keyword keyword,
        string propertyName
    )
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);

        return Contains<T>(keyword, param, property);
    }

    public static Expression<Func<T, bool>> ContainsInChild<T>(
        Keyword keyword,
        string propertyName,
        string childPropertyName
    )
    {
        var param = Expression.Parameter(typeof(T), "x");
        var parentProperty = Expression.Property(param, propertyName);
        var property = Expression.Property(parentProperty, childPropertyName);

        return Contains<T>(keyword, param, property);
    }

    public static Expression<Func<T, bool>> ContainsInChildren<T, U>(
        Keyword keyword,
        string propertyName,
        string childPropertyName
    )
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);

        var any = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(U));

        var anyLambda = Contains<U>(keyword, childPropertyName);
        var body = Expression.Call(any!, property, anyLambda);

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
