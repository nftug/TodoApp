using Domain.Todos;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Application.Shared.UseCase;

namespace Application.Todos;

public class Delete : DeleteBase<Todo, TodoDataModel>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo, TodoDataModel> repository)
            : base(repository)
        {
        }
    }
}
