using Domain.Comments.Entities;
using Domain.Todos.Entities;
using Infrastructure.Comments;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.DataSource;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Todos;

internal class TodoDataSourceSpecification : DataSourceSpecificationBase<Todo>
{
    public TodoDataSourceSpecification(DataContext context) : base(context)
    {
    }

    protected IDataSourceSpecification<Comment> CommentDataSource
        => new CommentDataSourceSpecification(_context);

    public override IQueryable<IDataModel<Todo>> Source
        => _context.Todo
            .Include(x => x.Comments)
            .Include(x => x.OwnerUser)
            .Select(x => new TodoDataModel
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                OwnerUserId = x.OwnerUserId,
                OwnerUser = x.OwnerUser != null
                    ? new UserDataModel { UserName = x.OwnerUser.UserName }
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

    public override Todo ToDomain(IDataModel<Todo> origin, bool recursive = true)
    {
        var _origin = (TodoDataModel)origin;

        return new Todo(
            id: _origin.Id,
            createdOn: _origin.CreatedOn,
            updatedOn: _origin.UpdatedOn,
            ownerUserId: _origin.OwnerUserId,
            title: new(_origin.Title),
            description: new(_origin.Description),
            period: new(_origin.StartDate, _origin.EndDate),
            state: new(_origin.State),
            comments: recursive
                ? _origin.Comments
                    .Select(x => CommentDataSource.ToDomain(x, recursive: false))
                    .ToList()
                : new List<Comment>()
        );
    }
}
