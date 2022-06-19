using Domain.Comments;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Application.Shared.UseCase;

namespace Application.Comments;

public class Delete : DeleteBase<Comment, CommentDataModel>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment, CommentDataModel> repository)
            : base(repository)
        {
        }
    }
}
