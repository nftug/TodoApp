namespace Application.Users
{
    public class UserDTO
    {
        public class Me
        {
            public string? Id { get; set; }
            public string? Username { get; set; }
            public string? Email { get; set; }
        }
        public class Public
        {
            public string? Id { get; set; }
            public string? Username { get; set; }
        }
    }
}