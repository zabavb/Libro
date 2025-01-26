using Library.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace DeadAspClient.Models.UserEntities.User
{
    public class ManageUserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [PastDate(ErrorMessage = "Date of birth must be in the past.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits and can include an optional '+' prefix.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public RoleType Role { get; set; }

        public ManageUserViewModel()
        {
            FirstName = string.Empty;
            LastName = null;
            DateOfBirth = DateTime.Now.AddYears(-18);
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Role = RoleType.GUEST;
        }

        public class PastDateAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value is DateTime dateTime && dateTime >= DateTime.Now)
                    return new ValidationResult(ErrorMessage ?? "Date must be in the past.");
                return ValidationResult.Success;
            }
        }
    }
}
