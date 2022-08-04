using Domain.Todos.Entities;
using Infrastructure.DataModels;
using Infrastructure.Shared.Services.FilterQuery;
using Infrastructure.Shared.Specifications.DataSource;
using Infrastructure.Shared.Specifications.Filter;

namespace Infrastructure.Todos;

public class TodoFilterQueryService : FilterQueryServiceBase<Todo, TodoDataModel>
{
    public TodoFilterQueryService(DataContext context) : base(context)
    {
    }

    protected override IDataSourceSpecification<Todo> DataSource
        => new TodoDataSourceSpecification(_context);

    protected override IFilterSpecification<Todo, TodoDataModel> FilterSpecification
        => new TodoFilterSpecification(_context);
}
