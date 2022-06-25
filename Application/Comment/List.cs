using Domain.Comment;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comment;

public class List : ListBase<CommentModel, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<CommentModel> repository,
            IQueryService<CommentModel> querySearch
        )
            : base(repository, querySearch)
        {
        }

        protected override CommentResultDTO CreateDTO(CommentModel item)
            => new(item);
    }
}
