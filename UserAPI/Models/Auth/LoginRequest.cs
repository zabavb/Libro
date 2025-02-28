using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Identifier is required.")]
        [CustomValidation(typeof(IdentifierValidator), nameof(IdentifierValidator.Validate))]
        public string Identifier { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string Password { get; set; }

        public LoginRequest()
        {
            Identifier = string.Empty; ;
            Password = string.Empty;
        }
    }
}
