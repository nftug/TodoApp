namespace Domain.Comments;

public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment> UpdateAsync(Comment comment);
    Task<Comment?> FindAsync(Guid id);
    Task RemoveAsync(Guid id);
}
