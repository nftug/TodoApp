using Infrastructure.DataModels;
using System.Text.RegularExpressions;
using Domain.Todos.Entities;
using Domain.Todos.Queries;
using Domain.Shared.Queries;
using Infrastructure.Shared.Specifications.Filter.Extensions;
using Infrastructure.Shared.Specifications.Filter;
using Domain.Todos.ValueObjects;

namespace Infrastructure.Todos;

internal class TodoFilterSpecification : FilterSpecificationBase<Todo, TodoDataModel>
{
    public TodoFilterSpecification(DataContext context)
        : base(context)
    {
    }

    protected override IQueryable<IDataModel<Todo>> GetQueryByParameter(
        IQueryable<IDataModel<Todo>> source,
        IQueryParameter<Todo> param
    )
    {
        var _param = (TodoQueryParameter)param;

        ExpressionGroups.AddSimpleSearch(_param.UserId, x => x.OwnerUserId == _param.UserId);

        var state = TodoState.CreateFromString(_param.State)?.Value;
        ExpressionGroups.AddSimpleSearch(state, x => x.State == state);

        ExpressionGroups.AddSearch(_param.Q, k =>
            ExpressionCombiner.OrElse(
                Contains(k, "Title"),
                Contains(k, "Description"),
                ContainsInChildren<CommentDataModel>(k, "Comments", "Content")));

        ExpressionGroups.AddSearch(_param.Title, k => Contains(k, "Title"));

        ExpressionGroups.AddSearch(_param.Description, k => Contains(k, "Description"));

        ExpressionGroups.AddSearch(_param.Comment, k =>
            ContainsInChildren<CommentDataModel>(k, "Comments", "Content"));

        ExpressionGroups.AddSearch(_param.UserName, k => ContainsInChild(k, "OwnerUser", "UserName"));

        return source.OfType<TodoDataModel>().ApplyExpressionGroup(ExpressionGroups);
    }

    protected override IQueryable<TodoDataModel> OrderQuery(
        IQueryable<IDataModel<Todo>> query,
        IQueryParameter<Todo> param
    )
    {
        var _query = query.Cast<TodoDataModel>();
        bool isDescending = Regex.IsMatch(param.Sort, "^-");
        string orderBy = Regex.Replace(param.Sort, "^-", "");

        return orderBy switch
        {
            "createdOn" => _query.OrderByAscDesc(x => x.CreatedOn, isDescending),
            "updatedOn" => _query.OrderByAscDesc(x => x.UpdatedOn, isDescending),
            "title" => _query.OrderByAscDesc(x => x.Title, isDescending),
            _ => query.Cast<TodoDataModel>()
        };
    }
}
