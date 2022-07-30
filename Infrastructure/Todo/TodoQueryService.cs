using Infrastructure.DataModels;
using Infrastructure.Services.QueryService;
using Infrastructure.Services.QueryService.Models;
using Infrastructure.Services.QueryService.Extensions;
using Domain.Interfaces;
using Domain.Todo;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Todo;

public class TodoQueryService : QueryServiceBase<TodoModel>
{
    public TodoQueryService(DataContext context)
        : base(context)
    {
    }

    protected override IQueryable<TodoDataModel> GetQueryByParameter
        (IQueryParameter<TodoModel> param)
    {
        var _param = (TodoQueryParameter)param;

        // OwnerUserの不要なカラムを取得させない
        // コメントの並び順も整える
        var query = _context.Todo
            .Include(x => x.Comments)
            .Include(x => x.OwnerUser)
            .Select(x => new TodoDataModel
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                OwnerUserId = x.OwnerUserId,
                OwnerUser = x.OwnerUser != null
                    ? new UserDataModel<Guid> { UserName = x.OwnerUser.UserName }
                    : null,
                Title = x.Title,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                State = x.State,
                Comments = x.Comments
                    .OrderByDescending(x => x.CreatedOn)
                    .ToList()
            });

        var expressionGroup = new List<ExpressionGroup<TodoDataModel>>();

        // ユーザーIDで絞り込み
        var userIdField = new SearchField<TodoDataModel>();
        if (_param.UserId != null)
            userIdField.Node.AddExpression(
                Keyword.CreateDummy(),
                x => x.OwnerUserId == _param.UserId
            );
        expressionGroup.AddExpressionNode(userIdField);

        // 状態で絞り込み
        var stateField = new SearchField<TodoDataModel>();
        if (_param.State != null)
            stateField.Node.AddExpression(
                Keyword.CreateDummy(),
                x => x.State == _param.State
            );
        expressionGroup.AddExpressionNode(stateField);

        // qで絞り込み
        var qField = new SearchField<TodoDataModel>(_param.Q);
        foreach (var k in GetKeyword(_param.Q))
            qField.Node.AddExpression(k, x =>
                (k.InQuotes ? x.Title : x.Title.ToLower()).Contains(k.Value) ||
                (k.InQuotes ? x.Description! : x.Description!.ToLower()).Contains(k.Value) ||
                x.Comments.Any(x =>
                    (k.InQuotes ? x.Content : x.Content!.ToLower()).Contains(k.Value))
            );
        expressionGroup.AddExpressionNode(qField);

        // タイトルで絞り込み
        var titleField = new SearchField<TodoDataModel>(_param.Title);
        foreach (var k in GetKeyword(_param.Title))
            titleField.Node.AddExpression(k, x =>
                (k.InQuotes ? x.Title : x.Title.ToLower()).Contains(k.Value)
            );
        expressionGroup.AddExpressionNode(titleField);

        // 説明文で絞り込み
        var descriptionField = new SearchField<TodoDataModel>(_param.Description);
        foreach (var k in GetKeyword(_param.Description))
            descriptionField.Node.AddExpression(k, x =>
                (k.InQuotes ? x.Description! : x.Description!.ToLower()).Contains(k.Value)
            );
        expressionGroup.AddExpressionNode(descriptionField);

        // コメントで絞り込み
        var commentField = new SearchField<TodoDataModel>(_param.Comment);
        foreach (var k in GetKeyword(_param.Comment))
            commentField.Node.AddExpression(k, x =>
                x.Comments.Any(x =>
                    (k.InQuotes ? x.Content : x.Content!.ToLower()).Contains(k.Value))
            );
        expressionGroup.AddExpressionNode(commentField);

        // ユーザー名で絞り込み
        var userNameField = new SearchField<TodoDataModel>(_param.UserName);
        foreach (var k in GetKeyword(_param.UserName))
            userNameField.Node.AddExpression(k, x =>
                (k.InQuotes
                    ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower())
                .Contains(k.Value)
            );
        expressionGroup.AddExpressionNode(userNameField);

        // クエリ式を作成する
        return query.ApplyExpressionGroup(expressionGroup);
    }

    protected override IQueryable<TodoDataModel> OrderQuery(
        IQueryable<IEntity<TodoModel>> query,
        IQueryParameter<TodoModel> param
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
