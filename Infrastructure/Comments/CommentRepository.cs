using Domain.Comments.Entities;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Infrastructure.Services.Repository;
using Infrastructure.Todos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Comments;

public class CommentRepository : RepositoryBase<Comment>
{
    public CommentRepository(
        DataContext context,
        IQueryService<Comment> queryService
    )
        : base(context, queryService)
    {
    }

    internal CommentRepository() { }

    private static TodoRepository TodoRepository => new();

    protected override IQueryable<CommentDataModel> Source
        => _context.Comment
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
                    Description = x.Todo.Description,
                    StartDate = x.Todo.StartDate,
                    EndDate = x.Todo.EndDate,
                    State = x.Todo.State,
                    Id = x.Todo.Id,
                    CreatedOn = x.Todo.CreatedOn,
                    UpdatedOn = x.Todo.UpdatedOn,
                    OwnerUserId = x.Todo.OwnerUserId
                }
            });

    protected override async Task AddEntityAsync(IDataModel<Comment> entity)
        => await _context.Comment.AddAsync((CommentDataModel)entity);

    protected override void UpdateEntity(IDataModel<Comment> entity)
        => _context.Comment.Update((CommentDataModel)entity);

    protected override void RemoveEntity(IDataModel<Comment> entity)
        => _context.Comment.Remove((CommentDataModel)entity);

    protected override IDataModel<Comment> ToDataModel(Comment origin)
        => new CommentDataModel(origin);

    internal override Comment ToDomain(IDataModel<Comment> origin, bool recursive = true)
    {
        var _origin = (CommentDataModel)origin;

        return new Comment(
            id: _origin.Id,
            createdOn: _origin.CreatedOn,
            updatedOn: _origin.UpdatedOn,
            ownerUserId: _origin.OwnerUserId,
            content: new(_origin.Content),
            todoId: _origin.TodoId,
            todo: recursive ? TodoRepository.ToDomain(_origin.Todo, recursive: false) : null!
        );
    }

    protected override void Transfer(Comment origin, IDataModel<Comment> dataModel)
        => ((CommentDataModel)dataModel).Transfer(origin);
}
