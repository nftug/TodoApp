using Domain.Comments;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Comments;

public class Edit
    : EditBase<Comment, CommentResultDTO, CommentCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment> repository)
            : base(repository)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);

        protected override void Put(Comment item, Command request)
        {
            item.Edit(new(request.Item.Content!));
        }
    }
}
