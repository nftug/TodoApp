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
            IQueryService<CommentModel> querySearch,
            IDomainService<CommentModel> domain
        ) : base(repository, querySearch, domain)
        {
        }

        protected override CommentResultDTO CreateDTO(CommentModel item)
            => new(item);
    }
}
