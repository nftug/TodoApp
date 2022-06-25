using Domain.Comment;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comment;

public class Create
    : CreateBase<CommentModel, CommentResultDTO, CommentCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<CommentModel> repository,
            IDomainService<CommentModel> domain
        ) : base(repository, domain)
        {
        }

        protected override CommentModel CreateDomain(Command request)
            => CommentModel.CreateNew(
                content: new(request.Item.Content!),
                todoId: request.Item.TodoId,
                ownerUserId: request.UserId
            );

        protected override CommentResultDTO CreateDTO(CommentModel item)
            => new(item);
    }
}