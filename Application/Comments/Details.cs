using Domain.Comments;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Application.Shared.UseCase;

namespace Application.Comments;

public class Details : DetailsBase<Comment, CommentDataModel, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment, CommentDataModel> repository)
            : base(repository)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}
