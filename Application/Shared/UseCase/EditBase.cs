using MediatR;
using Domain.Interfaces;
using Domain.Shared;
using Application.Shared.Interfaces;

namespace Application.Shared.UseCase;

public abstract class EditBase<TDomain, TResultDTO, TCommandDTO>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
    where TCommandDTO : ICommandDTO<TDomain>
{
    public class Command : IRequest<TResultDTO>
    {
        public TCommandDTO Item { get; init; }
        public Guid Id { get; init; }
        public Guid UserId { get; init; }

        public Command(Guid id, TCommandDTO item, Guid userId)
        {
            Item = item;
            Id = id;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Command, TResultDTO>
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

        public virtual async Task<TResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindAsync(request.Id);
            if (item == null)
                throw new NotFoundException();

            Put(item, request);

            if (!await _domain.CanEdit(item, request.UserId))
                throw new BadRequestException();

            var result = await _repository.UpdateAsync(item);

            if (result == null)
                throw new DomainException("保存に失敗しました");

            return CreateDTO(result);
        }

        protected abstract void Put(TDomain item, Command request);
        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}