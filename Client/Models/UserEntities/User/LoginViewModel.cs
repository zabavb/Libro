using System.ComponentModel.DataAnnotations;

namespace Client.Models.UserEntities.User
{
    public class LoginViewModel : IValidatableObject
    {
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 32 characters.")]
        public string Password { get; set; }

        public LoginViewModel()
        {
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Password = string.Empty;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(PhoneNumber))
            {
                yield return new ValidationResult(
                    "Either Email or Phone Number is required.",
                    [nameof(Email), nameof(PhoneNumber)]);
            }
        }
    }
}
