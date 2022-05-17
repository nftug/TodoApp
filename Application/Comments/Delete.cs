using MediatR;
using Domain.Comments;
using Domain.Shared;

namespace Application.Comments;

public class Delete
{
    public class Command : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ICommentRepository _commentRepository;

        public Handler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var CommentItem = await _commentRepository.FindAsync(request.Id);

            if (CommentItem == null)
                throw new NotFoundException();
            if (CommentItem.OwnerUserId != request.UserId)
                throw new BadRequestException();

            await _commentRepository.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
