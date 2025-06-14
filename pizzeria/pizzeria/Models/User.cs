namespace pizzeria.Models
{
    public class User
    {
        public required string Username { get; init; }
        public required string Name { get; init; }
        public required string Password { get; init; }

        public User() { }
    }
}
