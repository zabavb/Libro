using System.Collections;
using Library.DTOs.User;
using Library.DTOs.UserRelated.User;

namespace UserAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public RoleType Role { get; set; }
        public string ImageUrl { get; set; }
        public Guid PasswordId { get; set; }
        public Password Password { get; set; }
        public ICollection<Guid>? SubscriptionIds { get; set; }
        public ICollection<Subscription>? Subscriptions { get; set; }

        public User()
        {
            UserId = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
            ImageUrl = string.Empty;
            Role = RoleType.USER;
            Password = null!;
        }
    }
}