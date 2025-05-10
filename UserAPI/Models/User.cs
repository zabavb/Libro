using Library.DTOs.UserRelated.User;
using UserAPI.Models.Subscription;

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
        public Password? Password { get; set; }
        public ICollection<Guid>? SubscriptionIds { get; set; }
        public ICollection<UserSubscription>? UserSubscriptions { get; set; }

        public User(string firstName, string? lastName, DateTime? dateOfBirth, string emailDomen,
            string? phoneNumber, RoleType role = RoleType.USER)
        {
            UserId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Email = $"{firstName}.{lastName}@{emailDomen}.com";
            PhoneNumber = phoneNumber;
            Role = role;
        }

        public User()
        {
            UserId = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
            ImageUrl = string.Empty;
            Role = RoleType.USER;
        }
    }
}