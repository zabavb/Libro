using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserAPI.Models.Auth
{
    public static class IdentifierValidator
    {
        public static ValidationResult? Validate(string identifier, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return new ValidationResult("Identifier is required.");

            if (new EmailAddressAttribute().IsValid(identifier) &&
                Regex.IsMatch(identifier, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
                return ValidationResult.Success;

            var phoneRegex = @"^\+?[1-9]\d{1,14}$";
            if (Regex.IsMatch(identifier, phoneRegex))
                return ValidationResult.Success;

            return new ValidationResult("Identifier must be a valid email address or phone number.");
        }
    }

}
