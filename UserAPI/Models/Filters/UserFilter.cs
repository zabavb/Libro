using Library.DTOs.User;

namespace UserAPI.Models.Filters
{
    public class UserFilter
    {
        public DateTime? DateOfBirthStart { get; set; }
        public DateTime? DateOfBirthEnd { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public RoleType? Role { get; set; }
        public bool HasSubscription { get; set; }
    }
}
