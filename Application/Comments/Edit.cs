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
        public bool IsPartial { get; set; }

        public Command(Guid id, CommentCommandDTO CommentItemDTO, string userId, bool isPartial)
        {
            CommentCommandDTO = CommentItemDTO;
            Id = id;
            UserId = userId;
            IsPartial = isPartial;
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
            var inputItem = request.CommentCommandDTO;

            if (request.Id != inputItem.Id)
                throw new DomainException("id", "IDが正しくありません");

            var comment = await _commentRepository.FindAsync(request.Id);
            if (comment == null)
                throw new NotFoundException();
            if (comment.OwnerUserId != request.UserId)
                throw new BadRequestException();

            comment.Edit(
                content: new CommentContent(inputItem.Content)
            );

            var result = await _commentRepository.UpdateAsync(comment);

            return CommentResultDTO.CreateResultDTO(result);
        }
    }
}
