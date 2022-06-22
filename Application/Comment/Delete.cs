using Domain.Comment;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comment;

public class Delete : DeleteBase<CommentModel>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<CommentModel> repository)
            : base(repository)
        {
        }
    }
}
