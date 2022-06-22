using Domain.Comments;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comments;

public class List : ListBase<Comment, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Comment> repository,
            IQuerySearch<Comment> querySearch
        )
            : base(repository, querySearch)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}
