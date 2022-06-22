using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared.QuerySearch;
using Infrastructure.Shared.QuerySearch.Models;
using Infrastructure.Shared.QuerySearch.Extensions;
using Domain.Interfaces;
using Domain.Todos;

namespace Infrastructure.Todos;

public class TodoQuerySearchService
    : QuerySearchServiceBase<Todo>
{
    public TodoQuerySearchService(DataContext context)
        : base(context)
    {
    }

    public override IQueryable<TodoDataModel> GetFilteredQuery
        (IQueryParameter<Todo> param)
    {
        var _param = (TodoQueryParameter)param;

        var query = _context.Todos
            .Include(x => x.Comments)
            .Include(x => x.OwnerUser)
            .AsQueryable();

        var expressionGroup = new List<ExpressionGroup<TodoDataModel>>();

        // ユーザーIDで絞り込み
        var userIdField = new SearchField<TodoDataModel>(_param.UserId);
        if (!string.IsNullOrEmpty(_param.UserId))
        {
            userIdField.Node.AddExpression(
                x => x.OwnerUserId == _param.UserId,
                Keyword.CreateDummy()
            );
        }
        expressionGroup.AddExpressionNode(userIdField);

        // 状態で絞り込み
        var stateField = new SearchField<TodoDataModel>();
        if (_param.State != null)
            stateField.Node.AddExpression(
                x => x.State == _param.State,
                Keyword.CreateDummy()
            );
        expressionGroup.AddExpressionNode(stateField);

        // qで絞り込み
        var qField = new SearchField<TodoDataModel>(_param.Q);
        foreach (var keyword in GetKeyword(_param.Q))
            qField.Node.AddExpression(x =>
                (keyword.InQuotes ? x.Title : x.Title.ToLower())
                    .Contains(keyword.Value) ||
                (keyword.InQuotes ? x.Description! : x.Description!.ToLower())
                    .Contains(keyword.Value) ||
                x.Comments.Any(x =>
                    (keyword.InQuotes ? x.Content : x.Content!.ToLower())
                        .Contains(keyword.Value)),
                keyword
            );
        expressionGroup.AddExpressionNode(qField);

        // タイトルで絞り込み
        var titleField = new SearchField<TodoDataModel>(_param.Title);
        foreach (var keyword in GetKeyword(_param.Title))
            titleField.Node.AddExpression(x =>
                (keyword.InQuotes ? x.Title : x.Title.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(titleField);

        // 説明文で絞り込み
        var descriptionField = new SearchField<TodoDataModel>(_param.Description);
        foreach (var keyword in GetKeyword(_param.Description))
            descriptionField.Node.AddExpression(x =>
                (keyword.InQuotes ? x.Description! : x.Description!.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(descriptionField);

        // コメントで絞り込み
        var commentField = new SearchField<TodoDataModel>(_param.Comment);
        foreach (var keyword in GetKeyword(_param.Comment))
            commentField.Node.AddExpression(x =>
                x.Comments.Any(x =>
                    (keyword.InQuotes ? x.Content : x.Content!.ToLower())
                        .Contains(keyword.Value)),
                keyword
            );
        expressionGroup.AddExpressionNode(commentField);

        // ユーザー名で絞り込み
        var userNameField = new SearchField<TodoDataModel>(_param.UserName);
        foreach (var keyword in GetKeyword(_param.UserName))
            userNameField.Node.AddExpression(x =>
                (keyword.InQuotes
                    ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(userNameField);

        // クエリ式を作成する
        query = query.ApplyExpressionGroup(expressionGroup);

        return query;
    }
}
