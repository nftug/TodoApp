using Application.Shared.UseCase;
using Domain.Interfaces;
using Domain.User;

namespace Application.User;

public class DetailsPublic : DetailsBase<UserModel, UserResultDTO.Public>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<UserModel> repository) : base(repository)
        {
        }

        protected override UserResultDTO.Public CreateDTO(UserModel item)
            => new(item);
    }
}
