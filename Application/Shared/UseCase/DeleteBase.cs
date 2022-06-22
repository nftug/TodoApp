using MediatR;
using Domain.Interfaces;
using Domain.Shared;

namespace Application.Shared.UseCase;

public abstract class DeleteBase<TDomain>
    where TDomain : ModelBase
{
    public class Command : IRequest<Unit>
    {
        public Guid Id { get; init; }
        public string UserId { get; init; }

        public Command(Guid id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Command, Unit>
    {
        private readonly IRepository<TDomain> _repository;

        public HandlerBase(IRepository<TDomain> repository)
        {
            _repository = repository;
        }

        public virtual async Task<Unit> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindAsync(request.Id);

            if (item == null)
                throw new NotFoundException();
            if (item.OwnerUserId != request.UserId)
                throw new BadRequestException();

            await _repository.RemoveAsync(request.Id);
            return Unit.Value;
        }
    }
}