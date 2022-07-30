using Domain.Comments.Entities;
using Domain.Interfaces;
using Domain.Shared.Exceptions;
using Domain.Shared.Services;
using Domain.Todos.Entities;

namespace Domain.Comments.Services;

public class CommentService : DomainServiceBase<Comment>
{
    private readonly IRepository<Todo> _todoRepository;

    public CommentService(
        IRepository<Comment> repository,
        IRepository<Todo> todoRepository
    )
        : base(repository)
    {
        _todoRepository = todoRepository;
    }

    public override async Task<bool> CanCreate(Comment item, Guid? userId)
    {
        var existsParent = await _todoRepository.FindAsync(item.TodoId);
        if (existsParent == null)
            throw new DomainException(nameof(item.TodoId), "指定されたTodoは存在しません");

        return true;
    }

    public override Task<bool> CanDelete(Comment item, Guid? userId)
    {
        var result = item.OwnerUserId == userId || item.Todo.OwnerUserId == userId;
        return Task.FromResult(result);
    }
}
