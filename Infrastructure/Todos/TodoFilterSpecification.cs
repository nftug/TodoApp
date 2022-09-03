using Infrastructure.DataModels;
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

    protected override void AddQueryByParameter(IQueryParameter<Todo> param)
    {
        var _param = (TodoQueryParameter)param;

        AddQuery(_param.UserId, x => x.OwnerUserId == _param.UserId);

        var stateValue = new TodoState(_param.State).Value;
        AddQuery(_param.State, x => x.State == stateValue);

        AddSearch(_param.Q, k => ExpressionCombiner.OrElse(
            Contains(k, x => x.Title),
            Contains(k, x => x.Description),
            ContainsInChildren(k, x => x.Comments, y => y.Content)));

        AddContains(_param.Title, x => x.Title);

        AddContains(_param.Description, x => x.Description);

        AddContainsInChildren(_param.Comment, x => x.Comments, x => x.Content);

        AddContains(_param.UserName, x => x.OwnerUser!.UserName);
    }
}
