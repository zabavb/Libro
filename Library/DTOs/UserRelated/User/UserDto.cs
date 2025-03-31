using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Library.DTOs.UserRelated.User
{
    public class UserDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters.")]
        public string FirstName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Email address cannot exceed 15 characters.")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Date of birth must be a valid date.")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public RoleType Role { get; set; }

        [Required(ErrorMessage = "Image url is required.")]
        public string ImageUrl { get; set; }

        public UserDto()
        {
            Id = Guid.NewGuid();
            FirstName = string.Empty;
            Role = RoleType.USER;
            ImageUrl = string.Empty;
        }
    }
}