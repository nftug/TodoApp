using Domain.Comments;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comments;

public class Details : DetailsBase<Comment, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment> repository)
            : base(repository)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}
