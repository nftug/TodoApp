using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Queries;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.Filter.Extensions;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Shared.Specifications.Filter;

internal abstract class FilterSpecificationBase<TDomain, TDataModel> : IFilterSpecification<TDomain, TDataModel>
    where TDomain : ModelBase
    where TDataModel : IDataModel<TDomain>
{
    protected readonly DataContext _context;

    public FilterSpecificationBase(DataContext context)
    {
        _context = context;
    }

    protected List<ExpressionGroup<TDataModel>> ExpressionGroups { get; } = new();

    protected abstract void AddQueryByParameter
        (IQueryable<IDataModel<TDomain>> source, IQueryParameter<TDomain> param);

    public IQueryable<IDataModel<TDomain>> GetFilteredQuery
        (IQueryable<IDataModel<TDomain>> source, IQueryParameter<TDomain> param)
    {
        AddQueryByParameter(source, param);
        var query = source.OfType<TDataModel>().ApplyExpressionGroup(ExpressionGroups);
        return OrderQuery(query, param);
    }

    protected static IQueryable<IDataModel<TDomain>> OrderQuery(
        IQueryable<TDataModel> query,
        IQueryParameter<TDomain> param
    )
    {
        bool isDescending = Regex.IsMatch(param.Sort, "^-");
        string key = Regex.Replace(param.Sort, "^-", "");

        try
        {
            return query.OrderByKey(key, isDescending).OfType<IDataModel<TDomain>>();
        }
        catch (InvalidOperationException)
        {
            throw new DomainException("Sort", $"並び替えのキー \"{key}\" が見つかりません。");
        }
    }


    /// <summary>
    /// キーワードに変換せずに検索クエリを追加
    /// </summary>
    protected void AddQuery(
        object? fieldValue,
        Expression<Func<TDataModel, bool>> predicate
    )
        => ExpressionGroups.AddSearch(fieldValue, predicate);

    /// <summary>
    /// フィールドにキーワードを含む検索を追加
    /// x => x.Foo.Contains(keyword)
    /// </summary>
    protected void AddContains<T>(
        string? fieldValue,
        Expression<Func<TDataModel, T>> expression
    )
        => ExpressionGroups.AddSearch(fieldValue, k => k.Contains(expression));

    /// <summary>
    /// 子フィールドにキーワードを含む検索を追加
    /// x => x.Foo.Any(y => x.Bar.Contains(keyword))
    /// </summary>
    protected void AddContainsInChildren<T, U>(
        string? fieldValue,
        Expression<Func<TDataModel, IList<T>>> parentExpression,
        Expression<Func<T, U>> childExpression
    )
        => ExpressionGroups.AddSearch(fieldValue, k => k.ContainsInChildren(parentExpression, childExpression));

    /// <summary>
    /// 複数フィールドに対する横断検索を追加
    /// </summary>
    protected void AddSearch(
        string? fieldValue,
        Func<Keyword, Expression<Func<TDataModel, bool>>> expressionFunc
    )
        => ExpressionGroups.AddSearch(fieldValue, expressionFunc);

    protected static Expression<Func<TDataModel, bool>> Contains<T>(
        Keyword keyword,
        Expression<Func<TDataModel, T>> expression
    )
        => keyword.Contains(expression);

    protected static Expression<Func<TDataModel, bool>> ContainsInChildren<T, U>(
        Keyword keyword,
        Expression<Func<TDataModel, IList<T>>> parentExpression,
        Expression<Func<T, U>> childExpression
    )
        => keyword.ContainsInChildren(parentExpression, childExpression);
}
