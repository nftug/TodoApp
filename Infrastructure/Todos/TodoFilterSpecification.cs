using Infrastructure.DataModels;
using System.Text.RegularExpressions;
using Domain.Todos.Entities;
using Domain.Todos.Queries;
using Domain.Shared.Queries;
using Infrastructure.Shared.Specifications.Filter.Extensions;
using Infrastructure.Shared.Specifications.Filter.Models;
using Infrastructure.Shared.Specifications.Filter;

namespace Infrastructure.Todos;

internal class TodoFilterSpecification : FilterSpecificationBase<Todo>
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

        var expressionGroup = new List<ExpressionGroup<TodoDataModel>>();

        // ユーザーIDで絞り込み
        expressionGroup.AddSimpleSearch(_param.UserId, x => x.OwnerUserId == _param.UserId);

        // 状態で絞り込み
        expressionGroup.AddSimpleSearch(_param.State, x => x.State == _param.State);

        // qで絞り込み
        expressionGroup.AddSearch(
            _param.Q,
            (n, k) => n.AddExpression(k, x =>
                (k.InQuotes ? x.Title : x.Title.ToLower()).Contains(k.Value) ||
                (k.InQuotes ? x.Description! : x.Description!.ToLower()).Contains(k.Value) ||
                x.Comments.Any(x => (k.InQuotes ? x.Content : x.Content!.ToLower()).Contains(k.Value)))
            );

        // タイトルで絞り込み
        expressionGroup.AddSearch(
            _param.Title,
            (n, k) => n.AddExpression(k, x =>
                (k.InQuotes ? x.Title : x.Title.ToLower()).Contains(k.Value))
            );

        // 説明文で絞り込み
        expressionGroup.AddSearch(
            _param.Description,
            (n, k) => n.AddExpression(k, x =>
                (k.InQuotes ? x.Description! : x.Description!.ToLower()).Contains(k.Value))
            );

        // コメントで絞り込み
        expressionGroup.AddSearch(
            _param.Comment,
            (n, k) => n.AddExpression(k, x =>
                x.Comments.Any(x => (k.InQuotes ? x.Content : x.Content!.ToLower()).Contains(k.Value)))
        );

        // ユーザー名で絞り込み
        expressionGroup.AddSearch(
            _param.UserName,
            (n, k) => n.AddExpression(k, x =>
                (k.InQuotes ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower()).Contains(k.Value))
            );

        // クエリ式を作成する
        return source.OfType<TodoDataModel>().ApplyExpressionGroup(expressionGroup);
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
