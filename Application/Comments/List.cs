using Domain.Comments;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Application.Shared.UseCase;

namespace Application.Comments;

public class List : ListBase<Comment, CommentDataModel, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Comment, CommentDataModel> repository,
            IQuerySearch<CommentDataModel> querySearch
        )
            : base(repository, querySearch)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}
