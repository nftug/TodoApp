using Domain.Comment;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comment;

public class Edit
    : EditBase<CommentModel, CommentResultDTO, CommentCommandDTO>
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

        protected override void Put(CommentModel item, Command request)
        {
            item.Edit(new(request.Item.Content!));
        }
    }
}
