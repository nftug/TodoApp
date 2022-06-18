using MediatR;
using Domain.Comments;
using Domain.Shared;
using Domain.Interfaces;
using Infrastructure.DataModels;

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
        private readonly IRepository<Comment, CommentDataModel> _commentRepository;

        public Handler(IRepository<Comment, CommentDataModel> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.CommentCommandDTO;

            if (request.Id != inputItem.Id)
                throw new DomainException(nameof(inputItem.Id), "IDが正しくありません");

            var comment = await _commentRepository.FindAsync(request.Id);
            if (comment == null)
                throw new NotFoundException();
            if (comment.OwnerUserId != request.UserId)
                throw new BadRequestException();

            comment.Edit(content: new(inputItem.Content!));

            var result = await _commentRepository.UpdateAsync(comment);

            return new CommentResultDTO(result);
        }
    }
}
