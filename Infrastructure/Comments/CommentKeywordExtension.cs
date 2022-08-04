using System.Linq.Expressions;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.Filter.Managers;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Comments;

internal static class CommentKeywordExtension
{
    internal static Expression<Func<CommentDataModel, bool>> Contains(
        this Keyword keyword,
        string propertyName
    )
        => KeywordConvertManager.Contains<CommentDataModel>(keyword, propertyName);

    internal static Expression<Func<CommentDataModel, bool>> ContainsInChild(
        this Keyword keyword,
        string propertyName,
        string childPropertyName
    )
        => KeywordConvertManager.ContainsInChild<CommentDataModel>(keyword, propertyName, childPropertyName);

    internal static Expression<Func<CommentDataModel, bool>> ContainsInChildren<T>(
        this Keyword keyword,
        string propertyName,
        string childPropertyName
    )
        => KeywordConvertManager.ContainsInChildren<CommentDataModel, T>(keyword, propertyName, childPropertyName);
}
