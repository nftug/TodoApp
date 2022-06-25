using Domain.Interfaces;
using Domain.Shared;
using Domain.Todo;

namespace Domain.Comment;

public class CommentService : DomainServiceBase<CommentModel>
{
    private readonly IRepository<CommentModel> _commentRepository;
    private readonly IRepository<TodoModel> _todoRepository;

    public CommentService(
        IRepository<CommentModel> commentRepository,
        IRepository<TodoModel> todoRepository
    )
    {
        _commentRepository = commentRepository;
        _todoRepository = todoRepository;
    }

    public override async Task<bool> CanCreate(CommentModel item, Guid? userId)
    {
        var existsParent = await _todoRepository.FindAsync(item.TodoId);
        if (existsParent == null)
            throw new DomainException(nameof(item.TodoId), "指定されたTodoは存在しません");

        return true;
    }

    public override Task<bool> CanDelete(CommentModel item, Guid? userId)
    {
        var result = item.OwnerUserId == userId || item.Todo.OwnerUserId == userId;
        return Task.FromResult(result);
    }
}
