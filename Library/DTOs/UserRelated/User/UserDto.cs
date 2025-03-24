using Library.DTOs.User;

namespace Library.DTOs.UserRelated.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? LastName { get; set; }
        public string FirstName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public RoleType Role { get; set; }
        public string ImageUrl { get; set; }

        public UserDto()
        {
            Id = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
            ImageUrl = string.Empty;
            Role = RoleType.USER;
        }
    }
}
