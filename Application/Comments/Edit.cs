using Domain.Comments;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Application.Shared.UseCase;

namespace Application.Comments;

public class Edit
    : EditBase<Comment, CommentDataModel, CommentResultDTO, CommentCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment, CommentDataModel> repository)
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
