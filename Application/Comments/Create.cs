using Domain.Comments;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comments;

public class Create
    : CreateBase<Comment, CommentResultDTO, CommentCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment> repository)
            : base(repository)
        {
        }

        protected override Comment CreateDomain(Command request)
            => Comment.CreateNew(
                    content: new(request.Item.Content!),
                    todoId: request.Item.TodoId,
                    ownerUserId: request.UserId
                );

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}