using Application.Shared.UseCase;
using Domain.Interfaces;
using Domain.User;

namespace Application.User;

public class DetailsMe : DetailsBase<UserModel, UserResultDTO.Me>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<UserModel> repository) : base(repository)
        {
        }

        protected override UserResultDTO.Me CreateDTO(UserModel item)
            => new(item);
    }
}
