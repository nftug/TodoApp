using MediatR;
using Domain.Comments;
using Domain.Shared;

namespace Application.Comments;

public class Edit
{
    public class Command : IRequest<CommentResultDTO>
    {
        public CommentCommandDTO CommentCommandDTO { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, CommentCommandDTO CommentItemDTO, string userId)
        {
            CommentCommandDTO = CommentItemDTO;
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Command, CommentResultDTO>
    {
        private readonly ICommentRepository _commentRepository;

        public Handler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentResultDTO> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Id != request.CommentCommandDTO.Id)
                throw new DomainException("id", "IDが正しくありません");

            var Comment = await _commentRepository.FindAsync(request.Id);
            if (Comment == null)
                throw new NotFoundException();
            if (Comment.OwnerUserId != request.UserId)
                throw new BadRequestException();

            Comment.Edit(
                content: new CommentContent(request.CommentCommandDTO.Content)
            );

            var result = await _commentRepository.UpdateAsync(Comment);

            return CommentResultDTO.CreateResultDTO(result);
        }
    }
}
