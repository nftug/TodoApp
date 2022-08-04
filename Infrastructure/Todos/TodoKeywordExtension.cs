using System.Linq.Expressions;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.Filter.Managers;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Todos;

internal static class TodoKeywordExtension
{
    internal static Expression<Func<TodoDataModel, bool>> Contains(
        this Keyword keyword,
        string propertyName
    )
        => KeywordConvertManager.Contains<TodoDataModel>(keyword, propertyName);

    internal static Expression<Func<TodoDataModel, bool>> ContainsInChild(
        this Keyword keyword,
        string propertyName,
        string childPropertyName
    )
        => KeywordConvertManager.ContainsInChild<TodoDataModel>(keyword, propertyName, childPropertyName);

    internal static Expression<Func<TodoDataModel, bool>> ContainsInChildren<T>(
        this Keyword keyword,
        string propertyName,
        string childPropertyName
    )
        => KeywordConvertManager.ContainsInChildren<TodoDataModel, T>(keyword, propertyName, childPropertyName);
}
