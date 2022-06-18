using MediatR;
using Domain.Comments;
using Domain.Shared;
using Domain.Interfaces;
using Infrastructure.DataModels;

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
        private readonly IRepository<Comment, CommentDataModel> _commentRepository;

        public Handler(IRepository<Comment, CommentDataModel> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Unit> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.FindAsync(request.Id);

            if (comment == null)
                throw new NotFoundException();
            if (comment.OwnerUserId != request.UserId)
                throw new BadRequestException();

            await _commentRepository.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
