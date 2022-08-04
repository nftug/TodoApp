using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Domain.Shared.Entities;
using Domain.Shared.Queries;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.Filter.Extensions;
using Infrastructure.Shared.Specifications.Filter.Managers;
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

    public virtual IQueryable<IDataModel<TDomain>> GetFilteredQuery
        (IQueryable<IDataModel<TDomain>> source, IQueryParameter<TDomain> param)
    {
        var query = GetQueryByParameter(source, param);
        return OrderQuery(query, param);
    }

    protected abstract IQueryable<IDataModel<TDomain>> GetQueryByParameter
        (IQueryable<IDataModel<TDomain>> source, IQueryParameter<TDomain> param);

    protected virtual IQueryable<IDataModel<TDomain>> OrderQuery(
        IQueryable<IDataModel<TDomain>> query,
        IQueryParameter<TDomain> param
    )
    {
        bool isDescending = Regex.IsMatch(param.Sort, "^-");
        string orderBy = Regex.Replace(param.Sort, "^-", "");

        return orderBy switch
        {
            "createdOn" => query.OrderByAscDesc(x => x.CreatedOn, isDescending),
            "updatedOn" => query.OrderByAscDesc(x => x.UpdatedOn, isDescending),
            _ => query
        };
    }

    protected static Expression<Func<TDataModel, bool>> Contains(
        Keyword keyword,
        string propertyName
    )
        => KeywordConvertManager.Contains<TDataModel>(keyword, propertyName);

    protected static Expression<Func<TDataModel, bool>> ContainsInChild(
        Keyword keyword,
        string propertyName,
        string childPropertyName
    )
        => KeywordConvertManager.ContainsInChild<TDataModel>(keyword, propertyName, childPropertyName);

    protected static Expression<Func<TDataModel, bool>> ContainsInChildren<T>(
        Keyword keyword,
        string propertyName,
        string childPropertyName
    )
        => KeywordConvertManager.ContainsInChildren<TDataModel, T>(keyword, propertyName, childPropertyName);
}
