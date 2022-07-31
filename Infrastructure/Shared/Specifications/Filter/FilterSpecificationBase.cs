using System.Text.RegularExpressions;
using Domain.Shared.Entities;
using Domain.Shared.Queries;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.Filter.Extensions;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Shared.Specifications.Filter;

internal abstract class FilterSpecificationBase<TDomain> : IFilterSpecification<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;

    public FilterSpecificationBase(DataContext context)
    {
        _context = context;
    }

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
}
