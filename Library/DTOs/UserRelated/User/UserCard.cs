namespace Library.DTOs.UserRelated.User
{
    public class UserCard
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public RoleType Role { get; set; }

        public OrderForUserCard Order { get; set; }

        public UserCard() => FullName = string.Empty;
    }

    public class OrderForUserCard
    {
        public int OrdersCount { get; set; }
        public string LastOrder { get; set; }

        public OrderForUserCard() => LastOrder = string.Empty;
    }
}