using Application.Shared.Interfaces;
using Domain.User;

namespace Application.User;

public class UserResultDTO
{
    public class Me : IResultDTO<UserModel>
    {
        public Guid Id { get; set; }
        public string Username { get; }
        public string Email { get; }
        public DateTime CreatedDateTime { get; }
        public DateTime UpdatedDateTime { get; }
        public Guid? OwnerUserId { get; }

        public Me(UserModel user)
        {
            Id = user.Id;
            Username = user.UserName.Value;
            Email = user.Email.Value;
            CreatedDateTime = user.CreatedDateTime;
            UpdatedDateTime = user.UpdatedDateTime;
            OwnerUserId = user.OwnerUserId;
        }
    }

    public class Public : IResultDTO<UserModel>
    {
        public Guid Id { get; set; }
        public string Username { get; }
        public DateTime CreatedDateTime { get; }
        public DateTime UpdatedDateTime { get; }
        public Guid? OwnerUserId { get; }

        public Public(UserModel user)
        {
            Id = user.Id;
            Username = user.UserName.Value;
            CreatedDateTime = user.CreatedDateTime;
            UpdatedDateTime = user.UpdatedDateTime;
            OwnerUserId = user.OwnerUserId;
        }
    }
}
