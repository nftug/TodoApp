using MediatR;
using Domain.Comments;
using Domain.Shared;

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
        private readonly ICommentRepository _commentRepository;

        public Handler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentResultDTO> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _commentRepository.FindAsync(request.Id);

            if (result == null)
                throw new NotFoundException();

            return CommentResultDTO.CreateResultDTO(result);
        }
    }
}
