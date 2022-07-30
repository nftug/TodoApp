using MediatR;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Shared.UseCases;

public abstract class DeleteBase<TDomain>
    where TDomain : ModelBase
{
    public class Command : IRequest<Unit>
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }

        public Command(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Command, Unit>
    {
        protected readonly IRepository<TDomain> _repository;
        protected readonly IDomainService<TDomain> _domain;

        public HandlerBase(
            IRepository<TDomain> repository,
            IDomainService<TDomain> domain
        )
        {
            _repository = repository;
            _domain = domain;
        }

        public virtual async Task<Unit> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindAsync(request.Id);

            if (item == null)
                throw new NotFoundException();
            if (!await _domain.CanDelete(item, request.UserId))
                throw new BadRequestException();

            await _repository.RemoveAsync(request.Id);
            return Unit.Value;
        }
    }
}