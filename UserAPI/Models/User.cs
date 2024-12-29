using Library.UserEntities;

namespace UserAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public RoleType Role { get; set; }
        public Guid? PasswordId { get; set; }
        public Password? Password { get; set; }
        public Subscription? Subscription { get; set; }

        //==== Basket ====

        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Now;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Role = RoleType.GUEST;
        }
    }
}