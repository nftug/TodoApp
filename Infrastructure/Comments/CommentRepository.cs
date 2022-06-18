using Domain.Comments;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;

namespace Infrastructure.Comments;

public class CommentRepository : IRepository<Comment, CommentDataModel>
{
    private readonly DataContext _context;

    public CommentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        // 外部キーの存在チェック
        var todoDataModel = await _context.Todos.FindAsync(comment.TodoId);
        if (todoDataModel == null)
            throw new DomainException(nameof(comment.TodoId), "このIDのTodoは存在しません");

        var commentDataModel = ToDataModel(comment);
        await _context.Comments.AddAsync(commentDataModel);
        await _context.SaveChangesAsync();

        return ToModel(commentDataModel);
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        var foundCommentDataModel = await _context.Comments
            .FirstOrDefaultAsync(x => x.Id == comment.Id);

        if (foundCommentDataModel == null)
            throw new NotFoundException();

        var commentDataModel = Transfer(comment, foundCommentDataModel);

        _context.Comments.Update(commentDataModel);
        await _context.SaveChangesAsync();

        return ToModel(commentDataModel);
    }

    public async Task<List<Comment>> GetPaginatedListAsync
        (IQueryable<CommentDataModel> query, IQueryParameter param)
    {
        var (page, limit) = (param.Page, param.Limit);

        return await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(x => ToModel(x))
            .ToListAsync();
    }

    public async Task<Comment?> FindAsync(Guid id)
    {
        var commentDataModel = await _context.Comments
            .FirstOrDefaultAsync(x => x.Id == id);

        return commentDataModel != null
            ? ToModel(commentDataModel) : null;
    }

    public async Task RemoveAsync(Guid id)
    {
        var commentDataModel = await _context.Comments.FindAsync(id);

        if (commentDataModel == null)
            throw new NotFoundException();

        _context.Comments.Remove(commentDataModel);
        await _context.SaveChangesAsync();
    }

    private static CommentDataModel ToDataModel(Comment comment)
    {
        return new()
        {
            Content = comment.Content.Value,
            TodoId = comment.TodoId,
            CreatedDateTime = comment.CreatedDateTime,
            UpdatedDateTime = comment.UpdatedDateTime,
            OwnerUserId = comment.OwnerUserId
        };
    }

    private static CommentDataModel Transfer
        (Comment comment, CommentDataModel CommentDataModel)
    {
        CommentDataModel.Content = comment.Content.Value;
        CommentDataModel.TodoId = comment.TodoId;
        CommentDataModel.CreatedDateTime = comment.CreatedDateTime;
        CommentDataModel.UpdatedDateTime = comment.UpdatedDateTime;
        CommentDataModel.OwnerUserId = comment.OwnerUserId;

        return CommentDataModel;
    }

    private static Comment ToModel
        (CommentDataModel commentDataModel)
    {
        return new(
            id: commentDataModel.Id,
            content: new(commentDataModel.Content),
            todoId: commentDataModel.TodoId,
            createdDateTime: commentDataModel.CreatedDateTime,
            updatedDateTime: commentDataModel.UpdatedDateTime,
            ownerUserId: commentDataModel?.OwnerUserId
        );
    }
}
