using MediatR;
using Domain.Comments;
using Domain.Shared;
using Infrastructure.DataModels;

namespace Application.Comments;

public class Details
{
    public class Query : IRequest<CommentResultDTO>
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }

        public Query(Guid id, string? userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Query, CommentResultDTO>
    {
        private readonly IRepository<Comment, CommentDataModel> _commentRepository;

        public Handler(IRepository<Comment, CommentDataModel> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentResultDTO> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.FindAsync(request.Id);

            if (comment == null)
                throw new NotFoundException();

            return new CommentResultDTO(comment);
        }
    }
}
