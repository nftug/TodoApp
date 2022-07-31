using Domain.Comments.Entities;
using Domain.Todos.Entities;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.DataSource;
using Infrastructure.Todos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Comments;

internal class CommentDataSourceSpecification : DataSourceSpecificationBase<Comment>
{
    public CommentDataSourceSpecification(DataContext context) : base(context)
    {
    }

    protected IDataSourceSpecification<Todo> TodoDataSource
        => new TodoDataSourceSpecification(_context);

    public override IQueryable<IDataModel<Comment>> Source
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

    public override Comment ToDomain(IDataModel<Comment> origin, bool recursive = false)
    {
        var _origin = (CommentDataModel)origin;

        return new Comment(
            id: _origin.Id,
            createdOn: _origin.CreatedOn,
            updatedOn: _origin.UpdatedOn,
            ownerUserId: _origin.OwnerUserId,
            content: new(_origin.Content),
            todoId: _origin.TodoId,
            todo: recursive ? TodoDataSource.ToDomain(_origin.Todo, recursive: false) : null!
        );
    }
}
