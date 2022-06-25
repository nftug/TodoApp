using Domain.Comment;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comment;

public class Details : DetailsBase<CommentModel, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<CommentModel> repository,
            IDomainService<CommentModel> domain
        ) : base(repository, domain)
        {
        }

        protected override CommentResultDTO CreateDTO(CommentModel item)
            => new(item);
    }
}
