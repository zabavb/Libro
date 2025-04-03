using Library.DTOs.User;
using Library.Interfaces;

namespace Library.DTOs.UserRelated.User
{
    public class UserCardDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public RoleType Role { get; set; }

        public SingleSnippet<OrderCardSnippet> Order { get; set; }
        public int OrdersCount { get; set; }
        public string LastOrder { get; set; }

        public UserCardDto()
        {
            FullName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            LastOrder = string.Empty;
        }
    }

    public class OrderCardSnippet
    {
        public int OrdersCount { get; set; }
        public string LastOrder { get; set; }

        public OrderCardSnippet() => LastOrder = string.Empty;
    }
}