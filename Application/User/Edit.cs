using Domain.User;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.User;

public class Edit : EditBase<UserModel, UserResultDTO.Me, UserCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<UserModel> repository) : base(repository)
        {
        }

        protected override UserResultDTO.Me CreateDTO(UserModel item)
            => new(item);

        protected override void Put(UserModel item, Command request)
        {
            item.Edit(
                    new(request.Item.Username!),
                    new(request.Item.Email!)
                );
        }
    }
}
