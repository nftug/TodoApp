using Infrastructure.Services.QueryService;
using Infrastructure.Services.QueryService.Models;
using Infrastructure.Services.QueryService.Extensions;
using Domain.Interfaces;
using Domain.Comment;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Comment;

public class CommentQueryService : QueryServiceBase<CommentModel>
{
    public CommentQueryService(DataContext context)
        : base(context)
    {
    }

    protected override IQueryable<CommentDataModel> GetQueryByParameter
        (IQueryParameter<CommentModel> param)
    {
        var _param = (CommentQueryParameter)param;
        var query = _context.Comment
            .Include(x => x.OwnerUser)
            .Include(x => x.Todo)
            .Select(x => new CommentDataModel
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                OwnerUserId = x.OwnerUserId,
                OwnerUser = x.OwnerUser != null
                    ? new UserDataModel<Guid> { UserName = x.OwnerUser.UserName }
                    : null,
                Content = x.Content,
                TodoId = x.TodoId,
                Todo = new TodoDataModel
                {
                    Title = x.Todo.Title,
                    Description = x.Todo.Description
                }
            });

        var expressionGroup = new List<ExpressionGroup<CommentDataModel>>();

        // ユーザーIDで絞り込み
        var userIdField = new SearchField<CommentDataModel>();
        if (_param.UserId != null)
        {
            userIdField.Node.AddExpression(
                x => x.OwnerUserId == _param.UserId,
                Keyword.CreateDummy()
            );
        }
        expressionGroup.AddExpressionNode(userIdField);

        // qで絞り込み
        var qField = new SearchField<CommentDataModel>(_param.Q);
        foreach (var keyword in GetKeyword(_param.Q))
            qField.Node.AddExpression(x =>
                (keyword.InQuotes ? x.Content : x.Content.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(qField);

        // 内容で絞り込み
        var contentField = new SearchField<CommentDataModel>(_param.Content);
        foreach (var keyword in GetKeyword(_param.Content))
            contentField.Node.AddExpression(x =>
                (keyword.InQuotes ? x.Content : x.Content.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(contentField);

        // ユーザー名で絞り込み
        var userNameField = new SearchField<CommentDataModel>(_param.UserName);
        foreach (var keyword in GetKeyword(_param.UserName))
            userNameField.Node.AddExpression(x =>
                (keyword.InQuotes
                    ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(userNameField);

        return query.ApplyExpressionGroup(expressionGroup);
    }
}
