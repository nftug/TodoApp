using MediatR;
using Domain.Interfaces;
using Domain.Shared;
using Application.Shared.Interfaces;

namespace Application.Shared.UseCase;

public abstract class EditBase<TDomain, TEntity, TResultDTO, TCommandDTO>
    where TDomain : ModelBase
    where TEntity : IEntity
    where TResultDTO : IResultDTO<TDomain>
    where TCommandDTO : ICommandDTO<TDomain>
{
    public class Command : IRequest<TResultDTO>
    {
        public TCommandDTO Item { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, TCommandDTO item, string userId)
        {
            Item = item;
            Id = id;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Command, TResultDTO>
    {
        private readonly IRepository<TDomain, TEntity> _repository;

        public HandlerBase(IRepository<TDomain, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<TResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            if (request.Id != request.Item.Id)
                throw new DomainException(nameof(request.Item.Id), "IDが正しくありません");

            var item = await _repository.FindAsync(request.Id);
            if (item == null)
                throw new NotFoundException();
            if (item.OwnerUserId != request.UserId)
                throw new BadRequestException();

            Put(item, request);

            var result = await _repository.UpdateAsync(item);

            return CreateDTO(result);
        }

        protected abstract void Put(TDomain item, Command request);
        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}