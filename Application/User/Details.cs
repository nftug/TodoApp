using Application.Shared.UseCase;
using Domain.Interfaces;
using Domain.User;

namespace Application.User;

public class Details
{
    public class Me : DetailsBase<UserModel, UserResultDTO.Me>
    {
        public class Handler : HandlerBase
        {
            public Handler(
                IRepository<UserModel> repository,
                IDomainService<UserModel> domain
            ) : base(repository, domain)
            {
            }

            protected override UserResultDTO.Me CreateDTO(UserModel item)
                => new(item);
        }
    }

    public class Public : DetailsBase<UserModel, UserResultDTO.Public>
    {
        public class Handler : HandlerBase
        {
            public Handler(
                IRepository<UserModel> repository,
                IDomainService<UserModel> domain
            ) : base(repository, domain)
            {
            }

            protected override UserResultDTO.Public CreateDTO(UserModel item)
                => new(item);
        }
    }
}
