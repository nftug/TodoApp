using Domain.Comments;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comments;

public class Delete : DeleteBase<Comment>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment> repository)
            : base(repository)
        {
        }
    }
}
