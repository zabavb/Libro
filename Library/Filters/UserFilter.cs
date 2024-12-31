using Library.DTOs.User;

namespace Library.Filters
{
    public class UserFilter
    {
        public DateTime? DateOfBirthStart { get; set; }
        public DateTime? DateOfBirthEnd { get; set; }
        public RoleType? Role { get; set; }
        public bool HasSubscription { get; set; }
    }
}
