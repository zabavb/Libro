namespace Library.DTOs.UserRelated.User
{
    public class UserWithSubscriptionsDto
    {
        public Guid Id { get; set; }
        public string? LastName { get; set; }
        public string FirstName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public RoleType Role { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<SubscriptionForUserDetails> Subscriptions { get; set; }

        public UserWithSubscriptionsDto()
        {
            Id = Guid.NewGuid();
            FirstName = string.Empty;
            Role = RoleType.USER;
            ImageUrl = string.Empty;
        }
    }
}