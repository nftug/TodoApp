using MediatR;
using Application.Shared.Interfaces;
using Application.Shared.Enums;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Shared.UseCases;

public abstract class EditBase<TDomain, TResultDTO, TCommandDTO>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
    where TCommandDTO : ICommandDTO<TDomain>
{
    public class Command : IRequest<TResultDTO>
    {
        public Guid Id { get; init; }
        public TCommandDTO Item { get; init; }
        public Guid UserId { get; init; }
        public EditMode EditMode { get; init; }

        public Command(Guid id, TCommandDTO item, Guid userId, EditMode editMode)
        {
            Id = id;
            Item = item;
            UserId = userId;
            EditMode = editMode;
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

            if (request.EditMode == EditMode.Put)
                Put(item, request.Item);
            else if (request.EditMode == EditMode.Patch)
                Patch(item, request.Item);

            if (!await _domain.CanEdit(item, request.UserId))
                throw new BadRequestException();

            var result = await _repository.UpdateAsync(item);

            if (result == null)
                throw new DomainException("保存に失敗しました");

            return CreateDTO(result);
        }

        protected abstract void Put(TDomain origin, TCommandDTO command);

        protected abstract void Patch(TDomain origin, TCommandDTO command);

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}