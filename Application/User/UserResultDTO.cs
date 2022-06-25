using Application.Shared.Interfaces;
using Domain.User;

namespace Application.User;

public class UserResultDTO
{
    public class Me : IResultDTO<UserModel>
    {
        public Guid Id { get; }
        public string Username { get; }
        public string Email { get; }
        public DateTime CreatedOn { get; }
        public DateTime UpdatedOn { get; }
        public Guid? OwnerUserId { get; }

        public Me(UserModel user)
        {
            Id = user.Id;
            Username = user.UserName.Value;
            Email = user.Email.Value;
            CreatedOn = user.CreatedOn;
            UpdatedOn = user.UpdatedOn;
            OwnerUserId = user.OwnerUserId;
        }
    }

    public class Public : IResultDTO<UserModel>
    {
        public Guid Id { get; }
        public string Username { get; }
        public DateTime CreatedOn { get; }
        public DateTime UpdatedOn { get; }
        public Guid? OwnerUserId { get; }

        public Public(UserModel user)
        {
            Id = user.Id;
            Username = user.UserName.Value;
            CreatedOn = user.CreatedOn;
            UpdatedOn = user.UpdatedOn;
            OwnerUserId = user.OwnerUserId;
        }
    }
}
