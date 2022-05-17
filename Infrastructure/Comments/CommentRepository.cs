using Domain.Comments;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;

namespace Infrastructure.Comments;

public class CommentRepository : ICommentRepository
{
    private readonly DataContext _context;

    public CommentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment Comment)
    {
        var CommentDataModel = ToDataModel(Comment);
        await _context.Comments.AddAsync(CommentDataModel);
        await _context.SaveChangesAsync();

        return ToModel(CommentDataModel);
    }

    public async Task<Comment> UpdateAsync(Comment Comment)
    {
        var foundCommentDataModel = await _context.Comments
                                               .FirstOrDefaultAsync(
                                                   x => x.Id == Comment.Id
                                               );
        if (foundCommentDataModel == null)
            throw new NotFoundException();

        var CommentDataModel = Transfer(Comment, foundCommentDataModel);

        _context.Comments.Update(CommentDataModel);
        await _context.SaveChangesAsync();

        return ToModel(CommentDataModel);
    }

    private CommentDataModel ToDataModel(Comment Comment)
    {
        return new CommentDataModel
        {
            Content = Comment.Content.Value,
            TodoId = Comment.TodoId,
            CreatedDateTime = Comment.CreatedDateTime,
            UpdatedDateTime = Comment.UpdatedDateTime,
            OwnerUserId = Comment.OwnerUserId
        };
    }

    private CommentDataModel Transfer(Comment Comment, CommentDataModel CommentDataModel)
    {
        CommentDataModel.Content = Comment.Content.Value;
        CommentDataModel.TodoId = Comment.TodoId;
        CommentDataModel.CreatedDateTime = Comment.CreatedDateTime;
        CommentDataModel.UpdatedDateTime = Comment.UpdatedDateTime;
        CommentDataModel.OwnerUserId = Comment.OwnerUserId;

        return CommentDataModel;
    }

    public async Task<Comment?> FindAsync(Guid id)
    {
        var CommentDataModel = await _context.Comments
                                          .FirstOrDefaultAsync(
                                              x => x.Id == id
                                          );
        return CommentDataModel != null ? ToModel(CommentDataModel) : null;
    }

    private Comment ToModel(CommentDataModel CommentDataModel)
    {
        return Comment.CreateFromRepository(
            id: CommentDataModel.Id,
            content: new CommentContent(CommentDataModel.Content),
            todoId: CommentDataModel.TodoId,
            createdDateTime: CommentDataModel.CreatedDateTime,
            updatedDateTime: CommentDataModel.UpdatedDateTime,
            ownerUserId: CommentDataModel?.OwnerUserId
        );
    }

    public async Task RemoveAsync(Guid id)
    {
        var CommentDataModel = await _context.Comments.FindAsync(id);

        if (CommentDataModel == null)
            throw new NotFoundException();

        _context.Comments.Remove(CommentDataModel);
        await _context.SaveChangesAsync();
    }
}
